using DataAccess.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository<Event>
    {
        public EventRepository(IContextManager contextManager,
            ILogger<EventRepository> logger) : base(contextManager, logger) { }
        public async Task<IResult<ICollection<Event>>> GetByUserId(Guid userId, int year)
        {
            return await ExecuteOperation(async context =>
            {
                ICollection<Event> result = await context.Events
                    .Where(x => x.CreatorId == userId)
                    .Where(x => x.IterationTime != IterationTime.Single || x.Date.Year == year)
                    .Select(x => new Event()
                    {
                        Id = x.Id,
                        CreatorId = x.CreatorId,
                        Date = x.Date,
                        Duration = x.Duration,
                        Description = x.Description,
                        IterationTime = x.IterationTime,
                        DateCreated = x.DateCreated,
                        Color = x.Color,
                        Ico = x.Ico,
                        Name = x.Name,
                    })
                    .AsNoTracking()
                    .ToListAsync();
                return result;
            }, nameof(GetByUserId));
        }
        public virtual async Task<IResult<Guid>> Add(Event entity)
        {
            return await ExecuteOperation(async context =>
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await context.Events.AddAsync(entity);

                    if (entity.Participants != null)
                    {
                        foreach (var participant in entity.Participants)
                        {
                            // Говорим EF, что пользователь уже существует
                            context.Entry(participant.User).State = EntityState.Unchanged;
                            await context.Participants.AddAsync(participant);
                        }
                    }

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return entity.Id;
                }
            }, nameof(Add));
        }
        public async Task<IResult<Event?>> Get(Guid entityId)
        {
            return await ExecuteOperation(async context =>
            {
                var result = await context.Events
                    .Include(x => x.Participants)
                    .Include(x => x.Creator)
                    .FirstOrDefaultAsync(x => x.Id == entityId);
                return result;
            }, nameof(Get));
        }

        public async Task<IResult<bool>> Update(Event entity)
        {
            return await ExecuteOperation(async context =>
            {
                var existingEvent = await context.Events
                        .FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (existingEvent == null) return (false, ValidationResult.Invalid("Entity does not exists"));

                if (entity.Date != default) existingEvent.Date = entity.Date;
                if (entity.Duration > 0) existingEvent.Duration = entity.Duration;
                if (!string.IsNullOrEmpty(entity.Name)) existingEvent.Name = entity.Name;
                if (entity.Description != null) existingEvent.Description = entity.Description;
                if (entity.Color != null) existingEvent.Color = entity.Color;
                if (entity.Ico != null) existingEvent.Ico = entity.Ico;
                if (entity.IterationTime != default) existingEvent.IterationTime = entity.IterationTime;
                if (entity.CreatorId != default) existingEvent.CreatorId = entity.CreatorId;
                await context.SaveChangesAsync();

                return (true, ValidationResult.Valid());
            }, nameof(Update));
        }

        public async Task<IResult<bool>> Delete(Event entity)
        {
            return await ExecuteOperation(async context =>
            {
                context.Events.Attach(entity);
                context.Events.Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }, nameof(Delete));
        }
    }
}
