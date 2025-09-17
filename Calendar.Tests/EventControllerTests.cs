using Calendar.Domain.Enums;
using Calendar.Services.Dto.Common;
using Calendar.Services.Dto.Event;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace Calendar.Tests
{
    [TestFixture]
    public class EventControllerTests : TestBase
    {
        #region Configuration
        private static RestClient _client;
        private static Guid _userId;
        private static Guid _id;
        private const string BaseUrl = "http://localhost:5054/api/v1/events";
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            base.OneTimeSetUp();
            _id = Guid.NewGuid();
            _userId = Guid.Parse("293bc13f-5455-47a2-b602-d15879f3a28e");
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
        [Test, Order(1)]
        public async Task Add_ValidEvent_ReturnsSuccess()
        {
            // Arrange
            var dto = new CreateEventDto
            {
                Name = "TestEvent: " + _id,
                DateCreated = DateTime.UtcNow,
                Date = DateTime.UtcNow.AddDays(2),
                Duration = 4,
                Description = "Add_Event_ReturnsSuccess() test entity. Should be deleted",
                IterationTime = IterationTime.Weekly,
                CreatorId = _userId
            };

            var request = new RestRequest("add", method: Method.Post);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<Guid>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response data received: {IsDataReceived}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.Not.EqualTo(Guid.Empty));
            Logger.Information("Response value: {Value}", response.Data.Value);
            _id = response.Data.Value;
            Logger.Information("Stored EventId: {EventId}", _id);
        }
        [Test, Order(2), Timeout(1000)]
        public async Task Add_InvalidEvent_ReuturnsError()
        {
            // Arrange
            var dto = new CreateEventDto
            {
                Name = "TestEvent: " + _id,
                DateCreated = DateTime.UtcNow,
                Date = DateTime.UtcNow.AddDays(2),
                Duration = 4,
                Description = "Add_Event_ReturnsSuccess() test entity. Should be deleted",
                IterationTime = IterationTime.Weekly,
                CreatorId = Guid.Empty
            };
            var request = new RestRequest("add", method: Method.Post);
            request.AddJsonBody(dto);
            // Act
            var response = await _client.ExecuteAsync<CallbackDto<Guid>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.False);
            Logger.Information("Response data received: {IsDataReceived}", response.Data.IsDataReceived);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            Logger.Information("Response error message: {ErrorMessage}", response.Data.ErrorMessage);
        }
        [Test, Order(3), Timeout(1000)]
        public async Task Get_ValidEvent_ReturnsSuccess()
        {
            // Arrange
            var request = new RestRequest($"{_id}", Method.Get);
            // Act
            var response = await _client.ExecuteAsync<CallbackDto<EventDto>>(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response data received: {IsDataReceived}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.Not.Null);

            var jsonResponse = JsonSerializer.Serialize(response.Data.Value,
                new JsonSerializerOptions { WriteIndented = true });
            Logger.Information("Response value: \n{JsonResponse}", jsonResponse);
        }
        [Test, Order(4), Timeout(1000)]
        public async Task Get_InvalidEvent_ReturnsError()
        {
            // Arrange
            var request = new RestRequest($"{Guid.Empty}", Method.Get);
            // Act
            var response = await _client.ExecuteAsync<CallbackDto<EventDto>>(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.False);
            Logger.Information("Response data received: {IsDataReceived}", response.Data.IsDataReceived);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            Logger.Information("Response error message: {ErrorMessage}", response.Data.ErrorMessage);
        }
        [Test, Order(5), Timeout(1000)]
        public async Task Update_ValidEvent_ReturnsSuccess()
        {
            // Arrange
            Logger.Information($"New date {DateTime.UtcNow.AddDays(20)}");
            var dto = new UpdateEventDto
            {
                Id = _id,
                Date = DateTime.UtcNow.AddDays(20),
                Description = "new description",
            };
            var request = new RestRequest("update", Method.Post);
            request.AddJsonBody(dto);
            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.True);
            Logger.Information($"Response value: {response.Data.Value}");
        }
        [Test, Order(6), Timeout(1000)]
        public async Task Update_InvalidEvent_ReturnsError()
        {
            // Arrange
            var dto = new UpdateEventDto
            {
                Id = Guid.Empty,
                Date = DateTime.UtcNow.AddDays(20),
                Description = "new description",
            };
            var request = new RestRequest("update", Method.Post);
            request.AddJsonBody(dto);
            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.False);
            Logger.Information($"Response value: {response.Data.Value}");
        }
        [Test, Order(7), Timeout(1000)]
        public async Task GetByUserId_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var request = new RestRequest($"li/id={_userId},year={DateTime.Now.Year}",
                Method.Get);
            // Act
            var response = await _client.ExecuteAsync<CallbackDto<List<EventDto>>>(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.Not.Null);
            Logger.Information(
                $"Response value: \n{JsonSerializer.Serialize(response.Data.Value,
                new JsonSerializerOptions { WriteIndented = true })}");
        }
        [Test, Order(8), Timeout(1000)]
        public async Task GetByUserId_InvalidUserId_ReturnsError()
        {
            // Arrange
            var request = new RestRequest($"li/id={Guid.Empty},year={DateTime.Now.Year}",
                Method.Get);
            // Act
            var response = await _client.ExecuteAsync<CallbackDto<List<EventDto>>>(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.False);
            Logger.Information("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            Logger.Information($"Response error message: {response.Data.ErrorMessage}");
        }
        [Test, Order(9), Timeout(1000)]
        public async Task Delete_InvalidEvent_ReturnsError()
        {
            // Arrange
            var dto = new DeleteDto { Id = Guid.NewGuid() };

            var request = new RestRequest("delete", Method.Delete);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.False);
            Logger.Information($"Response value: {response.Data.ErrorMessage}");
        }
        [Test, Order(10), Timeout(1000)]
        public async Task Delete_ValidEvent_ReturnsSuccess()
        {
            // Arrange
            var dto = new DeleteDto { Id = _id };

            var request = new RestRequest("delete", Method.Delete);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Logger.Information("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            Logger.Information("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            Logger.Information("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.ErrorMessage, Is.Empty);
            Logger.Information($"Response error message: {response.Data.ErrorMessage}");
        }
    }
}
