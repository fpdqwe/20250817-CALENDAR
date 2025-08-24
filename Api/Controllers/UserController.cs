using BLL.Abstractions;
using BLL.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Controllers
{
    [ApiController, Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger _logger;
        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }

        [HttpGet("login={login}")]
        public async Task<ActionResult<CallbackDto<FullUserDto>>> GetByLogin(string login)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.GetUserByLogin(login);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"UserController handled \"GetByLogin()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<FullUserDto>>(result);
        }
        [HttpGet("id={id}")]
        public async Task<ActionResult<CallbackDto<FullUserDto>>> Get(Guid id)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.GetUser(id);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"UserController handled \"Get()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<FullUserDto>>(result);
        }
        [HttpPost("auth")]
        public async Task<ActionResult<CallbackDto<string>>> Auth([FromBody] UserDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.AuthUser(dto);
            if (result == null) throw new NullReferenceException(nameof(result));
            Response.Cookies.Append("jwt", result.Value);
            sw.Stop();
            _logger.LogInformation($"UserController handled \"Add()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<string>>(result);
        }
        [HttpPost("add")]
        public async Task<ActionResult<CallbackDto<bool>>> Add([FromBody] UserDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.AddUser(dto);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"UserController handled \"Add()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpPost("update")]
        public async Task<ActionResult<CallbackDto<bool>>> Update([FromBody] UpdateUserDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.UpdateUser(dto);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"UserController handled \"Add()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
        [HttpDelete("delete")]
        public async Task<ActionResult<CallbackDto<bool>>> Delete([FromBody] DeleteDto dto)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.DeleteUser(dto);
            if (result == null) throw new NullReferenceException(nameof(result));
            sw.Stop();
            _logger.LogInformation($"UserController handled \"Delete()\" in {sw.ElapsedMilliseconds}");
            return new ActionResult<CallbackDto<bool>>(result);
        }
    }
}
