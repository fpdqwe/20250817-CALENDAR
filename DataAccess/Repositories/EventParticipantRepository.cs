using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class EventParticipantRepository : BaseRepository<EventParticipant>
    {
        public EventParticipantRepository(IContextManager contextManager,
            ILogger<EventParticipantRepository> logger) : base(contextManager, logger)
        {
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }
    }
}
