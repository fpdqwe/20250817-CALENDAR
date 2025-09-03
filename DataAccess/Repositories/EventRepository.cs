using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class EventRepository : BaseRepository<Event>
    {
        public EventRepository(IContextManager contextManager,
            ILogger<EventRepository> logger) : base(contextManager, logger)
        {
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }

        public override async Task<bool> Add(Event entity)
        {
            using (var context = ContextManager.GenerateDatabaseContext())
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
    }
}
