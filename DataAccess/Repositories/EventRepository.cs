using DataAccess.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }
        public virtual async Task<List<Event>> GetByUserId(Guid userId, int year)
        {
            //_logger.LogDebug("");
            using (var context = ContextManager.CreateDatabaseContext())
            {
                return await context.Events
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
            }
        }
        public virtual async Task<bool> Add(Event entity)
        {
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
                                await context.EventsParticipants.AddAsync(participant);
                            }
                        }
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Event transaction failed ex: {ex.Message}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<Event?> Get(Guid entityId)
        {
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    return await context.Events
                        .Include(x => x.Participants)
                        .FirstOrDefaultAsync(x => x.Id == entityId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return null;
            }
        }

        public async Task<bool> Update(Event entity)
        {
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    context.Events.Update(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Event entity)
        {
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    context.Events.Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }
    }
}
