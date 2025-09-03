using BLL.Abstractions;
using BLL.Dto;
using BLL.Extensions;
using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IRepository<Participant> _repository;
        private readonly ILogger _logger;
        public ParticipantService(IRepository<Participant> repository,
            ILogger<ParticipantService> logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _logger = logger;
            _logger.LogDebug($"New instance of {nameof(ParticipantService)} was initialized");
        }
        public async Task<CallbackDto<ParticipantDto>> Get(Guid id)
        {
            _logger.LogDebug($"Trying to get participant id: {id}");
            var callback = new CallbackDto<ParticipantDto>();
            var result = await _repository.Get(id);
            if (result == null)
            {
                var error = $"Failed to get participant, id: {id}";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
            }
            else
            {
                _logger.LogDebug($"Successfuly got participant from db, id: {id}");
                callback.AddObject(result.ToDto());
            }
            return callback;
        }
        public async Task<CallbackDto<bool>> Add(CreateParticipantDto dto)
        {
            _logger.LogDebug($"Trying to add new participant, id {dto.UserId}");
            var callback = new CallbackDto<bool>();
            if (dto == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Add(dto.ToEntity());
            callback.AddObject(result);
            _logger.LogDebug($"Add participant id: {dto.UserId} operation result: {result}");
            return callback;
        }
        public async Task<CallbackDto<bool>> Update(UpdateParticipantDto dto)
        {
            _logger.LogDebug($"Trying to update participant, id {dto.Id}");
            var callback = new CallbackDto<bool>();
            if (dto == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Update(dto.ToEntity());
            callback.AddObject(result);
            _logger.LogDebug($"Update participant id: {dto.Id} operation result: {result}");
            return callback;
        }
        public async Task<CallbackDto<bool>> Delete(DeleteDto dto)
        {
            _logger.LogDebug($"Trying to delete participant, id {dto.Id}");
            var callback = new CallbackDto<bool>();
            if (dto == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Delete(dto.ToParticipant());
            callback.AddObject(result);
            _logger.LogDebug($"Delete participant id: {dto.Id} operation result: {result}");
            return callback;
        }
    }
}
