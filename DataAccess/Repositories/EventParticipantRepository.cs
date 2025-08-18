using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class EventParticipantRepository : BaseRepository<EventParticipant>
    {
        public EventParticipantRepository(IContextManager contextManager, ILogger logger) : base(contextManager, logger)
        {
            
        }
    }
}
