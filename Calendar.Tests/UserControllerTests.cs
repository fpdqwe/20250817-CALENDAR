using Calendar.Services.Dto.Common;
using Calendar.Services.Dto.User;
using RestSharp;
using System.Net;

namespace Calendar.Tests
{
    [TestFixture]
    public class UserControllerTests : TestBase
    {
        #region Configuration
        private static RestClient _client;
        private static Guid _newUserGuid;
        private static string _newUserLogin;
        private static string _newUserPassword;
        private const string BaseUrl = "http://localhost:5054/api/v1/users";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            _newUserGuid = Guid.NewGuid();
            _newUserLogin = $"testuser_{_newUserGuid}";
            _newUserPassword = "password123";
        }
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _client = new RestClient(BaseUrl);
        }
        [TearDown]
        public override void TearDown()
        {
            _client?.Dispose();
            base.TearDown();
        }
        #endregion
        [Test, Order(1), Timeout(2000)]
        public async Task Add_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var userDto = new UserDto
            {
                Login = _newUserLogin,
                Password = _newUserPassword,
            };
            Logger.Debug("Login: {NewUserLogin}", _newUserLogin);

            var request = new RestRequest("add", Method.Post);
            request.AddJsonBody(userDto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<Guid>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: {StatusCode}", response.StatusCode);
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response is not null: {bool}", response.Data != null);
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response is data received: {bool}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.Not.EqualTo(Guid.Empty));
            Logger.Information("New user guid: {guid}", response.Data.Value);
            _newUserGuid = response.Data.Value;
        }
        [Test, Order(2), Timeout(1000)]
        public async Task Add_DuplicateUser_ReturnsError()
        {
            // Arrange
            var userDto = new UserDto
            {
                Login = _newUserLogin,
                Password = _newUserPassword,
            };
            Logger.Information($"Login: {_newUserLogin}");

            var request = new RestRequest("add", Method.Post);
            request.AddJsonBody(userDto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<Guid>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: {StatusCode}", response.StatusCode);
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response is not null: {bool}", response.Data != null);
            Assert.That(response.Data.IsDataReceived, Is.False);
            Logger.Information("Response is data received: {bool}", response.Data.IsDataReceived);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            Logger.Information("Response error message: {Message}", response.Data.ErrorMessage);
        }
        [Test, Order(3), Timeout(1000)]
        public async Task GetByLogin_ValidLogin_ReturnsUser()
        {
            // Arrange
            var login = _newUserLogin;
            var request = new RestRequest($"ln\\{login}", Method.Get);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<FullUserDto>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: {StatusCode}", response.StatusCode);
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response is not null: {bool}", response.Data != null);
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response is data received: {bool}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.Not.Null);
            Logger.Information("Response data is not null: {bool}", response.Data.Value != null);
            Assert.That(response.Data.Value.Login, Is.EqualTo(login));
            _newUserGuid = response.Data.Value.Id;
        }
        [Test, Order(4), Timeout(1000)]
        public async Task GetByLogin_InvalidLogin_ReturnsError()
        {
            // Arrange
            var login = "nonexistentuser";
            var request = new RestRequest($"ln\\{login}", Method.Get);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<FullUserDto>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.False);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            Logger.Information(response.Data.ErrorMessage);
        }
        [Test, Order(5), Timeout(1000)]
        public async Task Get_ValidId_ReturnsUser()
        {
            // Arrange
            var userId = _newUserGuid; // Существующий ID
            var request = new RestRequest($"id\\{userId}", Method.Get);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<FullUserDto>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.True);
            Assert.That(response.Data.Value, Is.Not.Null);
            Assert.That(response.Data.Value.Id, Is.EqualTo(userId));
        }
        [Test, Order(6), Timeout(1000)]
        public async Task Get_InvalidId_ReturnsError()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Несуществующий ID
            var request = new RestRequest($"id\\{userId}", Method.Get);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<FullUserDto>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.False);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
        }
        [Test, Order(7), Timeout(1000)]
        public async Task Auth_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var userDto = new UserDto
            {
                Login = _newUserLogin,
                Password = _newUserPassword
            };

            var request = new RestRequest("auth", Method.Post);
            request.AddJsonBody(userDto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<string>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.True);
            Assert.That(response.Data.Value, Is.Not.Empty);
        }
        [Test, Order(8), Timeout(1000)]
        public async Task Auth_InvalidCredentials_ReturnsError()
        {
            // Arrange
            var userDto = new UserDto
            {
                Login = _newUserLogin,
                Password = "wrongpassword"
            };

            var request = new RestRequest("auth", Method.Post);
            request.AddJsonBody(userDto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<string>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.False);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            Logger.Information(response.Data.ErrorMessage);
        }
        [Test, Order(9), Timeout(1000)]
        public async Task Update_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var updateDto = new UpdateUserDto
            {
                Id = _newUserGuid,
                Password = "newpassword123",
                Name = "John",
                Surname = "Johnson"
            };

            var request = new RestRequest("update", Method.Post);
            request.AddJsonBody(updateDto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.True);
            Assert.That(response.Data.Value, Is.True);
        }
        [Test, Order(10), Timeout(1000)]
        public async Task Delete_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var deleteDto = new DeleteDto
            {
                Id = _newUserGuid
            };

            var request = new RestRequest("delete", Method.Delete);
            request.AddJsonBody(deleteDto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.True);
            Assert.That(response.Data.Value, Is.True);
        }
        [Test, Order(11), Timeout(1000)]
        public async Task Delete_InvalidUser_ReturnsError()
        {
            // Arrange
            var deleteDto = new DeleteDto
            {
                Id = Guid.NewGuid() // Несуществующий ID
            };
            Logger.Information($"Test guid: {deleteDto.Id}");

            var request = new RestRequest("delete", Method.Delete);
            request.AddJsonBody(deleteDto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.True);
            Assert.That(response.Data.Value, Is.False);
            Logger.Information(response.Data.ErrorMessage);
        }
    }
}