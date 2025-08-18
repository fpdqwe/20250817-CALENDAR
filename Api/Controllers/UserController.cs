using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController, Route("[controller]")]
    public class UserController
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }
    }
}
