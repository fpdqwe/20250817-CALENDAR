using DataAccess.Abstractions;
using DataAccess.Results;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DataAccess.Repositories
{
    public class EventRepository : IEventRepository<Event>
    {
        public IContextManager ContextManager { get; private set; }
        private readonly ILogger _logger;
        public EventRepository(IContextManager contextManager,
            ILogger<EventRepository> logger)
        {
            ArgumentNullException.ThrowIfNull(contextManager, nameof(contextManager));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ContextManager = contextManager;
            _logger = logger;
            _logger.LogDebug("New instance of {TypeName} was initialized", GetType().Name);
        }
        public virtual async Task<IResult<ICollection<Event>>> GetByUserId(Guid userId, int year)
        {
            var sw = Stopwatch.StartNew();
            using (var context = ContextManager.CreateDatabaseContext())
            {
                var result = await context.Events
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
                sw.Stop();
                _logger.LogInformation("{TypeName} successfuly get events list by user id {EntityId} in {ElapsedMs}ms",
                    GetType().Name, userId, sw.ElapsedMilliseconds);
                return new EntityResult<ICollection<Event>>(result, sw.ElapsedMilliseconds);
            }
        }
        public virtual async Task<IResult<Guid>> Add(Event entity)
        {
            var sw = Stopwatch.StartNew();
            using (var context = ContextManager.CreateDatabaseContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await context.Events.AddAsync(entity);
                        if (entity.Participants != null)
                        {
                            foreach (var participant in entity.Participants)
                            {
                                // Telling ef that user already exists
                                context.Entry(participant.User).State = EntityState.Unchanged;
                                await context.Participants.AddAsync(participant);
                            }
                        }
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        sw.Stop();
                        _logger.LogInformation("{TypeName} successfuly added entity {EntityId} in {ElapsedMs}ms",
                            GetType().Name, entity.Id, sw.ElapsedMilliseconds);
                        return new GuidResult(entity.Id, sw.ElapsedMilliseconds);
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
            }
        }
        public async Task<IResult<Event>> Get(Guid entityId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    var result = await context.Events
                        .Include(x => x.Participants)
                        .Include(x => x.Creator)
                        .FirstOrDefaultAsync(x => x.Id == entityId);

                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly got entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, entityId, sw.ElapsedMilliseconds);
                    return new EntityResult<Event>(result, sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on get}", GetType().Name);
                return new EntityResult<Event>(ex, sw.ElapsedMilliseconds);
            }
        }

        public async Task<IResult<bool>> Update(Event entity)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    var existingEvent = await context.Events
                        .FirstOrDefaultAsync(x => x.Id == entity.Id);

                    if (existingEvent == null)
                    {
                        sw.Stop();
                        _logger.LogInformation("{TypeName} entity with id {EntityId} not found",
                            GetType().Name, entity.Id);
                        return new BoolResult(false, sw.ElapsedMilliseconds);
                    }

                    if (entity.Date != default) existingEvent.Date = entity.Date;
                    if (entity.Duration > 0) existingEvent.Duration = entity.Duration;
                    if (!string.IsNullOrEmpty(entity.Name)) existingEvent.Name = entity.Name;
                    if (entity.Description != null) existingEvent.Description = entity.Description;
                    if (entity.Color != null) existingEvent.Color = entity.Color;
                    if (entity.Ico != null) existingEvent.Ico = entity.Ico;
                    if (entity.IterationTime != default) existingEvent.IterationTime = entity.IterationTime;
                    if (entity.CreatorId != default) existingEvent.CreatorId = entity.CreatorId;
                    await context.SaveChangesAsync();
                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly updated entity {EntityId} in {ElapsedMs}ms",
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

        public async Task<IResult<bool>> Delete(Event entity)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    context.Events.Remove(entity);
                    await context.SaveChangesAsync();
                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly deleted entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, entity.Id, sw.ElapsedMilliseconds);
                    return new BoolResult(true, sw.ElapsedMilliseconds);
                }
            }
            catch (DbUpdateException dbEx)
            {
                sw.Stop();
                _logger.LogError(dbEx, "{TypeName} database error on delete: {Message}",
                    GetType().Name, dbEx.InnerException?.Message ?? dbEx.Message);
                return new BoolResult(dbEx, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on delete}", GetType().Name);
                return new BoolResult(ex, sw.ElapsedMilliseconds);
            }
        }
    }
}
