using DataAccess.Abstractions;
using DataAccess.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DataAccess.Repositories
{
    public class ParticipantRepository : BaseRepository<Participant>
    {
        public ParticipantRepository(IContextManager contextManager,
            ILogger<ParticipantRepository> logger) : base(contextManager, logger)
        {

        }

        public override async Task<IResult<Guid>> Add(Participant entity)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    var eventExists = await context.Events.AnyAsync(e => e.Id == entity.EventId);
                    if (!eventExists)
                    {
                        sw.Stop();
                        _logger.LogWarning("Attempt to add participant for non-existent event: {EventId}", entity.EventId);
                        return new GuidResult(new ArgumentException("Event does not exist"), sw.ElapsedMilliseconds);
                    }

                    var userExists = await context.Users.AnyAsync(u => u.Id == entity.UserId);
                    if (!userExists)
                    {
                        sw.Stop();
                        _logger.LogWarning("Attempt to add non-existent user as participant: {UserId}", entity.UserId);
                        return new GuidResult(new ArgumentException("User does not exist"), sw.ElapsedMilliseconds);
                    }

                    // Проверка на дубликат (один пользователь не может быть дважды в одном событии)
                    var duplicateExists = await context.Participants
                        .AnyAsync(p => p.EventId == entity.EventId && p.UserId == entity.UserId);

                    if (duplicateExists)
                    {
                        sw.Stop();
                        _logger.LogWarning("Attempt to add duplicate participant: User {UserId} already in Event {EventId}",
                            entity.UserId, entity.EventId);
                        return new GuidResult(new ArgumentException("User is already a participant in this event"), sw.ElapsedMilliseconds);
                    }

                    await context.Participants.AddAsync(entity);
                    await context.SaveChangesAsync();

                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfully added participant {ParticipantId} for event {EventId} in {ElapsedMs}ms",
                        GetType().Name, entity.Id, entity.EventId, sw.ElapsedMilliseconds);
                    return new GuidResult(entity.Id, sw.ElapsedMilliseconds);
                }
            }
            catch (DbUpdateException dbEx)
            {
                sw.Stop();
                _logger.LogError(dbEx, "{TypeName} database error on add: {Message}",
                    GetType().Name, dbEx.InnerException?.Message ?? dbEx.Message);
                return new GuidResult(dbEx, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on add}", GetType().Name);
                return new GuidResult(ex, sw.ElapsedMilliseconds);
            }
        }

        public override async Task<IResult<Participant>> Get(Guid entityId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Participant? result;
            using (var context = ContextManager.CreateDatabaseContext())
            {
                result = await context.Participants
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == entityId);
            }
            sw.Stop();
            _logger.LogDebug($"{GetType().Name} handled get in {sw.ElapsedMilliseconds}ms.");
            return new EntityResult<Participant>(result, sw.ElapsedMilliseconds);
        }

        public override async Task<IResult<bool>> Update(Participant entity)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    // Получаем существующего участника
                    var existingParticipant = await context.Participants
                        .FirstOrDefaultAsync(x => x.Id == entity.Id);

                    if (existingParticipant == null)
                    {
                        sw.Stop();
                        _logger.LogInformation("{TypeName} participant with id {ParticipantId} not found",
                            GetType().Name, entity.Id);
                        return new BoolResult(false, sw.ElapsedMilliseconds);
                    }

                    // БЛОКИРУЕМ изменение EventId и UserId
                    // Если попытались изменить - возвращаем ошибку
                    if (entity.EventId != Guid.Empty && entity.EventId != existingParticipant.EventId)
                    {
                        sw.Stop();
                        _logger.LogWarning("Attempt to change EventId for participant: {ParticipantId}", entity.Id);
                        return new BoolResult(new Exception("EventId cannot be changed"), sw.ElapsedMilliseconds);
                    }

                    if (entity.UserId != Guid.Empty && entity.UserId != existingParticipant.UserId)
                    {
                        sw.Stop();
                        _logger.LogWarning("Attempt to change UserId for participant: {ParticipantId}", entity.Id);
                        return new BoolResult(new Exception("UserId cannot be changed"), sw.ElapsedMilliseconds);
                    }

                    // Обновляем только разрешенные поля
                    if (!string.IsNullOrEmpty(entity.Role)) existingParticipant.Role = entity.Role;
                    if (entity.WarningTimeOffset >= 0) existingParticipant.WarningTimeOffset = entity.WarningTimeOffset;
                    if (entity.Color != null) existingParticipant.Color = entity.Color;

                    await context.SaveChangesAsync();

                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfully updated participant {ParticipantId} in {ElapsedMs}ms",
                        GetType().Name, entity.Id, sw.ElapsedMilliseconds);
                    return new BoolResult(true, sw.ElapsedMilliseconds);
                }
            }
            catch (DbUpdateException dbEx)
            {
                sw.Stop();
                _logger.LogError(dbEx, "{TypeName} database error on update: {Message}",
                    GetType().Name, dbEx.InnerException?.Message ?? dbEx.Message);
                return new BoolResult(dbEx, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on update}", GetType().Name);
                return new BoolResult(ex, sw.ElapsedMilliseconds);
            }
        }
    }
}
