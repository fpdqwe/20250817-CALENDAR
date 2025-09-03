using BLL.Abstractions;
using BLL.Dto;
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
        [HttpGet("li/id={id},year={year}")]
        public async Task<ActionResult<CallbackDto<List<EventDto>>>> GetByUserId(Guid id, int year)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.GetByUserId(id, year);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"GetByUserId()\" in {sw.ElapsedMilliseconds}ms.");
            return new ActionResult<CallbackDto<List<EventDto>>>(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CallbackDto<EventDto>>> Get(Guid id)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Get(id);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Get()\" in {sw.ElapsedMilliseconds}ms.");
            return new ActionResult<CallbackDto<EventDto>>(result);
        }
        [HttpPost("add")]
        public async Task<ActionResult<CallbackDto<bool>>> Add([FromBody] CreateEventDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Add(dto);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Add()\" in {sw.ElapsedMilliseconds}ms.");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpPost("update")]
        public async Task<ActionResult<CallbackDto<bool>>> Update([FromBody] UpdateEventDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Update(dto);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Update()\" in {sw.ElapsedMilliseconds}ms.");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpDelete("delete")]
        public async Task<ActionResult<CallbackDto<bool>>> Delete([FromBody] DeleteDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Delete(dto);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Get()\" in {sw.ElapsedMilliseconds}ms.");
            return new ActionResult<CallbackDto<bool>>(result);
        }
    }
}
