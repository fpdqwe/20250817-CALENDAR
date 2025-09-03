using BLL.Dto;
using Domain.Entities;

namespace BLL.Extensions
{
    public static class ParticipantExtensions
    {
        /// <summary>
        /// Converts participant to dto.
        /// this.User.Id sholud not be null
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static ParticipantDto ToDto(this Participant participant)
        {
            ArgumentNullException.ThrowIfNull(participant.User, nameof(participant.User));
            return new ParticipantDto
            {
                Id = participant.Id,
                Color = participant.Color,
                Role = participant.Role,
                WarningTimeOffset = participant.WarningTimeOffset,
                UserId = participant.User.Id,
                Name = participant.User.Name,
                Lastname = participant.User.Lastname,
            };
        }
        public static Participant ToEntity(this ParticipantDto dto)
        {
            return new Participant
            {
                Id = dto.Id,
                Color = dto.Color,
                Role = dto.Role,
                WarningTimeOffset = dto.WarningTimeOffset,
                User = new User
                {
                    Id = dto.UserId,
                    Name = dto.Name,
                    Lastname = dto.Lastname,
                }
            };
        }
        public static Participant ToEntity(this CreateParticipantDto dto)
        {
            return new Participant
            {
                Id = Guid.NewGuid(),
                Color = dto.Color,
                Role = dto.Role,
                WarningTimeOffset = dto.WarningTimeOffset,
                Event = new Event { Id = dto.EventId },
                User = new User { Id = dto.UserId }
            };
        }
        public static Participant ToEntity(this UpdateParticipantDto dto)
        {
            return new Participant
            {
                Id = dto.Id,
                Role = dto.Role,
                Color = dto.Color,
                WarningTimeOffset = dto.WarningTimeOffset,
            };
        }
        public static Participant ToParticipant(this DeleteDto dto)
        {
            return new Participant { Id = dto.Id };
        }
    }
}
