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
            if (result.IsSuccess && result.Result != null)
            {
                _logger.LogDebug("Successfuly got participant from db, id: {id}", id);
                callback.AddObject(result.Result.ToDto());
            }
            else
            {
                var error = $"Failed to get participant, id: {id}";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
            }
            return callback;
        }
        public async Task<CallbackDto<Guid>> Add(CreateParticipantDto dto)
        {
            _logger.LogDebug("Trying to add new participant id: {Id} of event: {EventId}",
                dto.UserId, dto.EventId);
            var callback = new CallbackDto<Guid>();
            if (dto == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Add(dto.ToEntity());
            if (result.IsSuccess && result.Result != Guid.Empty) callback.AddObject(result.Result);
            else if (result.Message != null) callback.SetErrorMessage(result.Message);
            else callback.SetErrorMessage("Failed to add participant");
            _logger.LogDebug("Add participant id: {UserId} operation result: {Result}",
                dto.UserId, result.Result);
            return callback;
        }
        public async Task<CallbackDto<bool>> Update(UpdateParticipantDto dto)
        {
            _logger.LogDebug("Trying to update participant, id {Id}", dto.Id);
            var callback = new CallbackDto<bool>();
            if (dto == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Update(dto.ToEntity());
            if (result.IsSuccess) callback.AddObject(result.Result);
            else if (result.Message != null) callback.SetErrorMessage(result.Message);
            else callback.SetErrorMessage("Failed to update participant");
            _logger.LogDebug("Update participant id: {Id} operation result: {Result}",
                dto.Id, result.Result);
            return callback;
        }
        public async Task<CallbackDto<bool>> Delete(DeleteDto dto)
        {
            _logger.LogDebug("Trying to delete participant, id: {Id}", dto.Id);
            var callback = new CallbackDto<bool>();
            if (dto == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Delete(dto.ToParticipant());
            if (result.IsSuccess) callback.AddObject(result.Result);
            else if (result.Message != null) callback.SetErrorMessage(result.Message);
            else callback.SetErrorMessage("Failed to delete participant");

            _logger.LogDebug("Delete participant id: {Id} operation result: {Result}",
                dto.Id, result.Result);
            return callback;
        }
    }
}
