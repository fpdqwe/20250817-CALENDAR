using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class ParticipantRepository : BaseRepository<Participant>
    {
        public ParticipantRepository(IContextManager contextManager,
            ILogger<ParticipantRepository> logger) : base(contextManager, logger)
        {

        }
    }
}
