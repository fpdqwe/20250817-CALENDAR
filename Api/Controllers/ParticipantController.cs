using BLL.Abstractions;
using BLL.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Controllers
{
    [ApiController, Route("api/v1/participants")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _service;
        private readonly ILogger _logger;
        public ParticipantController(IParticipantService service,
            ILogger<ParticipantController> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CallbackDto<ParticipantDto>>> Get(Guid id)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Get(id);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventParticipantController handled \"Get()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<ParticipantDto>>(result);
        }
        [HttpPost("add")]
        public async Task<ActionResult<CallbackDto<Guid>>> Add([FromBody] CreateParticipantDto participant)
        {
            if (participant.WarningTimeOffset < 0) return BadRequest("WarningTimeOffset can't be negative");
            if (participant.UserId == Guid.Empty) return BadRequest("User id can't be empty");
            if (participant.EventId == Guid.Empty) return BadRequest("Event id can't be empty");
            var sw = Stopwatch.StartNew();
            var result = await _service.Add(participant);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventParticipantController handled \"Add()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<Guid>>(result);
        }
        [HttpPost("update")]
        public async Task<ActionResult<CallbackDto<bool>>> Update([FromBody] UpdateParticipantDto participant)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Update(participant);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"EventParticipantController handled \"Update()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpDelete("delete")]
        public async Task<ActionResult<CallbackDto<bool>>> Delete([FromBody] DeleteDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Delete(dto);
            sw.Stop();
            _logger.LogInformation($"EventParticipantController handled \"Delete()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
    }
}
