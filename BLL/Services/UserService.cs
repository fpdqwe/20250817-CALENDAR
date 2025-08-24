using BLL.Abstractions;
using BLL.Dto;
using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<User> _repository;
        private readonly ILogger _logger;
        private readonly IHasher _hasher;
        private readonly ITokenProvider _tokenProvider;
        public UserService(IUserRepository<User> repository, IHasher hasher, ITokenProvider tokenProvider, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(hasher, nameof(hasher));
            ArgumentNullException.ThrowIfNull(tokenProvider, nameof(tokenProvider));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _hasher = hasher;
            _tokenProvider = tokenProvider;
            _logger = logger;
            _logger.LogDebug($"New instance of {nameof(EventService)} was initialized");
        }
        public async Task<CallbackDto<bool>> AddUser(UserDto dto)
        {
            _logger.LogDebug($"Trying to add new user");
            var entity = dto.ToEntity();
            entity.Password = _hasher.Hash(dto.Password);
            var result = new CallbackDto<bool>();
            var record = await _repository.Add(entity);
            if (record == true) result.AddObject(record);
            else result.SetErrorMessage("Failed to create new user");
            return result;
        }
        public async Task<CallbackDto<string>> AuthUser(UserDto dto)
        {
            _logger.LogDebug($"Trying to auth user {dto.Login}");
            var callback = new CallbackDto<string>();
            var password = await _repository.GetPasswordByLogin(dto.Login);
            if (password != null)
            {
                if (_hasher.Verify(dto.Password, password))
                {
                    var user = await _repository.GetByLogin(dto.Login);
                    if (user != null)
                        callback.AddObject(_tokenProvider.Generate(user));
                    else _logger.LogError("User disappeared from database during authentication");
                }
                else callback.SetErrorMessage("Wrong login or password");
            }
            else callback.SetErrorMessage("Failed to find user");

            return callback;
        }
        public async Task<CallbackDto<FullUserDto>> GetUser(Guid id)
        {
            var callback = new CallbackDto<FullUserDto>();
            var user = await _repository.Get(id);
            if (user != null)
            {
                callback.AddObject(new FullUserDto()
                {
                    Id = id,
                    Login = user.Login,
                    Name = user.Name,
                    Surname = user.Lastname,
                    DateRegistered = user.DateRegistered,
                    DateOfBirth = user.DateOfBirth,
                    Zodiac = user.Zodiac,
                });
            }
            else
            {
                callback.SetErrorMessage("Failed to find user");
            }
            return callback;
        }
        public async Task<CallbackDto<FullUserDto>> GetUserByLogin(string login)
        {
            var callback = new CallbackDto<FullUserDto>();
            var user = await _repository.GetByLogin(login);
            if (user != null)
            {
                callback.AddObject(new FullUserDto()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Name = user.Name,
                    Surname = user.Lastname,
                    DateRegistered = user.DateRegistered,
                    DateOfBirth = user.DateOfBirth,
                    Zodiac = user.Zodiac,
                });
            }
            else
            {
                callback.SetErrorMessage("Failed to find user");
            }
            return callback;
        }
        public async Task<CallbackDto<bool>> UpdateUser(UpdateUserDto dto)
        {
            dto.Password = _hasher.Hash(dto.Password);
            var callback = new CallbackDto<bool>();
            callback.AddObject(await _repository.Update(dto.ToEntity()));
            return callback;
        }
        public async Task<CallbackDto<bool>> DeleteUser(DeleteDto dto)
        {
            var callback = new CallbackDto<bool>();
            callback.AddObject(await _repository.Delete(new User() { Id = dto.Id }));
            return callback;
        }
    }
}
