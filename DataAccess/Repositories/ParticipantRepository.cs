using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DataAccess.Repositories
{
    public class ParticipantRepository : BaseRepository<Participant>, IRepository<Participant>
    {
        public ParticipantRepository(IContextManager contextManager,
            ILogger<ParticipantRepository> logger) : base(contextManager, logger)
        {

        }
        public async Task<IResult<Guid>> Add(Participant entity)
        {
            return await ExecuteOperation(async context =>
            {
                var eventExists = await context.Events.AnyAsync(x => x.Id == entity.EventId);
                if (!eventExists)
                    return (Guid.Empty,
                    ValidationResult.Invalid($"Attempt to add participant for non-existent event: {entity.EventId}"));

                var userExists = await context.Users.AnyAsync(x => x.Id == entity.UserId);
                if (!userExists)
                    return (Guid.Empty,
                    ValidationResult.Invalid($"Attempt to add non-existent user as participant: {entity.UserId}"));

                var duplicateExists = await context.Participants
                    .AnyAsync(x => x.EventId == entity.EventId && x.UserId == entity.UserId);
                if (duplicateExists)
                    return (Guid.Empty,
                    ValidationResult.Invalid($"User is already a participant in this event"));

                await context.Participants.AddAsync(entity);
                await context.SaveChangesAsync();
                return (entity.Id, ValidationResult.Valid());
            }, nameof(Add));
        }
        public async Task<IResult<Participant>> Get(Guid entityId)
        {
            return await ExecuteOperation(async context =>
            {
                var result = await context.Participants
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == entityId);
                if (result == null) return (null, ValidationResult.Invalid($"User id: {entityId} does not exists"));
                else return (result, ValidationResult.Valid());
            }, nameof(Get));
        }
        public async Task<IResult<bool>> Update(Participant entity)
        {
            return await ExecuteOperation(async context =>
            {
                var existingParticipant = await context.Participants
                    .FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (existingParticipant == null)
                    return (false, ValidationResult.Invalid($"Participant with id {entity.Id} not found"));
                if (entity.EventId != Guid.Empty && entity.EventId != existingParticipant.EventId)
                    return (false, ValidationResult.Invalid($"Attempt to change EventId for participant {entity.Id}"));
                if (entity.UserId != Guid.Empty && entity.UserId != existingParticipant.UserId)
                    return (false, ValidationResult.Invalid($"Attempt to change UserId for participant {entity.Id}"));

                if (!string.IsNullOrEmpty(entity.Role)) existingParticipant.Role = entity.Role;
                if (entity.WarningTimeOffset >= 0) existingParticipant.WarningTimeOffset = entity.WarningTimeOffset;
                if (entity.Color != null) existingParticipant.Color = entity.Color;

                await context.SaveChangesAsync();
                return (true, ValidationResult.Valid());
            }, nameof(Update));
        }
        public async Task<IResult<bool>> Delete(Participant entity)
        {
            return await ExecuteOperation(async context =>
            {
                context.Participants.Attach(entity);
                context.Participants.Remove(entity);
                try
                {
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (DBConcurrencyException ex)
                {
                    return false;
                }
            }, nameof(Delete));
        }
    }
}
