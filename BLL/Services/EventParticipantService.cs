using BLL.Abstractions;
using BLL.Dto;
using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class EventParticipantService : IEventParticipantService
    {
        private readonly IRepository<EventParticipant> _repository;
        private readonly ILogger _logger;
        public EventParticipantService(IRepository<EventParticipant> repository,
            ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _logger = logger;
            _logger.LogDebug($"New instance of {nameof(EventParticipantService)} was initialized");
        }
        public async Task<CallbackDto<EventParticipant>> Get(Guid id)
        {
            _logger.LogDebug($"Trying to get participant id: {id}");
            var callback = new CallbackDto<EventParticipant>();
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
                callback.AddObject(result);
            }
            return callback;
        }
        public async Task<CallbackDto<bool>> Add(EventParticipant entity)
        {
            _logger.LogDebug($"Trying to add new participant, id {entity.Id}");
            var callback = new CallbackDto<bool>();
            if (entity == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Add(entity);
            callback.AddObject(result);
            _logger.LogDebug($"Add participant id: {entity.Id} operation result: {result}");
            return callback;
        }
        public async Task<CallbackDto<bool>> Update(EventParticipant entity)
        {
            _logger.LogDebug($"Trying to update participant, id {entity.Id}");
            var callback = new CallbackDto<bool>();
            if (entity == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Update(entity);
            callback.AddObject(result);
            _logger.LogDebug($"Update participant id: {entity.Id} operation result: {result}");
            return callback;
        }
        public async Task<CallbackDto<bool>> Delete(EventParticipant entity)
        {
            _logger.LogDebug($"Trying to delete participant, id {entity.Id}");
            var callback = new CallbackDto<bool>();
            if (entity == null)
            {
                var error = "Entity was null";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
                return callback;
            }
            var result = await _repository.Delete(entity);
            callback.AddObject(result);
            _logger.LogDebug($"Delete participant id: {entity.Id} operation result: {result}");
            return callback;
        }
    }
}
