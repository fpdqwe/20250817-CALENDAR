using BLL.Abstractions;
using BLL.Dto;
using BLL.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Controllers
{
    [ApiController, Route("api/v1/events"), Authorize]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly ILogger _logger;
        public EventController(IEventService service, ILogger<EventController> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CallbackDto<Event>>> Get(Guid id)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Get(id);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Get()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<Event>>(result);
        }
        [HttpPost("add")]
        public async Task<ActionResult<CallbackDto<bool>>> Add([FromBody] CreateEventDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Add(dto.ToEntity());
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Add()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpPost("update")]
        public async Task<ActionResult<CallbackDto<bool>>> Update([FromBody] Event entity)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Update(entity);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Update()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpDelete("delete")]
        public async Task<ActionResult<CallbackDto<bool>>> Delete([FromBody] Event entity)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Delete(entity);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Get()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
    }
}
