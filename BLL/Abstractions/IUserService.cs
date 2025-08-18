using BLL.Dto;

namespace BLL.Abstractions
{
    public interface IUserService
    {
        public Task<CallbackDto<bool>> DeleteUser(DeleteDto dto);
        public Task<CallbackDto<bool>> UpdateUser(UpdateUserDto dto);
        public Task<CallbackDto<FullUserDto>> GetUserByLogin(string login);
        public Task<CallbackDto<FullUserDto>> GetUser(Guid id);
        public Task<CallbackDto<string>> AuthUser(UserDto dto);
        public Task<CallbackDto<bool>> AddUser(UserDto dto);
    }
}
