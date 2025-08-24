using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class EventRepository : BaseRepository<Event>
    {
        public EventRepository(IContextManager contextManager,
            ILogger<EventRepository> logger) : base(contextManager, logger)
        {

        }
    }
}
