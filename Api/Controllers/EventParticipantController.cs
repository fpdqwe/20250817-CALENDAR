using BLL.Abstractions;
using BLL.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Controllers
{
    [ApiController, Route("api/v1/participants")]
    public class EventParticipantController
    {
        private readonly IEventParticipantService _service;
        private readonly ILogger _logger;
        public EventParticipantController(IEventParticipantService service,
            ILogger<EventParticipantController> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CallbackDto<EventParticipant>>> Get(Guid id)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Get(id);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventParticipantController handled \"Get()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<EventParticipant>>(result);
        }
        [HttpPost("add")]
        public async Task<ActionResult<CallbackDto<bool>>> Add([FromBody] EventParticipant participant)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Add(participant);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventParticipantController handled \"Add()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpPost("update")]
        public async Task<ActionResult<CallbackDto<bool>>> Update([FromBody] EventParticipant participant)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Update(participant);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventParticipantController handled \"Update()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpDelete("delete")]
        public async Task<ActionResult<CallbackDto<bool>>> Delete(Guid id)
        {
            var sw = Stopwatch.StartNew();
            var entity = new EventParticipant { Id = id };
            var result = await _service.Delete(entity);
            sw.Stop();
            _logger.LogInformation($"EventParticipantController handled \"Delete()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
    }
}
