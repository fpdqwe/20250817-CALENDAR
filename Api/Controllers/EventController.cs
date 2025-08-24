using BLL.Abstractions;
using BLL.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Controllers
{
    [ApiController, Route("[controller]")]
    public class EventController
    {
        private readonly IEventService _service;
        private readonly ILogger _logger;
        public EventController(IEventService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }
        [HttpGet]
        public async Task<ActionResult<CallbackDto<Event>>> Get(Guid id)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Get(id);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Get()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<Event>>(result);
        }
        [HttpPost]
        public async Task<ActionResult<CallbackDto<bool>>> Add([FromBody] Event entity)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Add(entity);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Add()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpPost]
        public async Task<ActionResult<CallbackDto<bool>>> Update([FromBody] Event entity)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Update(entity);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventController handled \"Update()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpDelete]
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
