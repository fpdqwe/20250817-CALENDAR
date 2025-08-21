using BLL.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController, Route("[controller]")]
    public class UserController
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
    }
}
