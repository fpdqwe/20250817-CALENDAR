using BLL.Dto;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace CalendarTests
{
    [TestFixture]
    public class ParticipantControllerTests
    {
        #region Configuration
        private static RestClient _client;
        private static Guid _participantId;
        private static Guid _userId;
        private static Guid _eventId;
        private const string BaseUrl = "http://localhost:5054/api/v1/participants";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _participantId = Guid.NewGuid();
            _userId = Guid.Parse("54cedbf1-0bb0-40ff-9cec-70bd4c54780f");
            _eventId = Guid.Parse("286ab442-0d17-4038-ba98-da00e152b635");
        }

        [SetUp]
        public void SetUp()
        {
            _client = new RestClient(BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }
        #endregion
        [Test, Order(1)]
        public async Task Add_ValidParticipant_ReturnsSuccess()
        {
            // Arrange
            var dto = new CreateParticipantDto
            {
                EventId = _eventId,
                UserId = _userId,
                Role = "Guest",
                WarningTimeOffset = 4,
                Color = "FF5733"
            };

            var request = new RestRequest("add", Method.Post);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<Guid>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TestContext.WriteLine("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            TestContext.WriteLine("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            TestContext.WriteLine("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.Not.EqualTo(Guid.Empty));
            TestContext.WriteLine($"Response value: {response.Data.Value}");
            _participantId = response.Data.Value;
        }
        [Test, Order(2), Timeout(1000)]
        public async Task Add_InvalidParticipant_ReturnsError()
        {
            // Arrange
            var dto = new CreateParticipantDto
            {
                EventId = Guid.Empty,
                UserId = Guid.Empty,
                Role = "InvalidRole",
                WarningTimeOffset = -1 // Invalid value
            };

            var request = new RestRequest("add", Method.Post);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<Guid>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            TestContext.WriteLine("Response status code: BadRequest");
            TestContext.WriteLine($"Response content:\n{response.Content}");
            //Assert.That(response.Data, Is.Not.Null);
            //TestContext.WriteLine("Response data is not null");
            //Assert.That(response.Data.IsDataReceived, Is.False);
            //TestContext.WriteLine("Response data received: {0}", response.Data.IsDataReceived);
            //Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            //TestContext.WriteLine($"Response error message: {response.Data.ErrorMessage}");
        }
        [Test, Order(3), Timeout(1000)]
        public async Task Get_ValidParticipant_ReturnsSuccess()
        {
            // Arrange
            var request = new RestRequest($"{_participantId}", Method.Get);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<ParticipantDto>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TestContext.WriteLine("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            TestContext.WriteLine("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            TestContext.WriteLine("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.Not.Null);
            TestContext.WriteLine(
                $"Response value: {JsonSerializer.Serialize(response.Data.Value,
                new JsonSerializerOptions { WriteIndented = true })}");
        }
        [Test, Order(4), Timeout(1000)]
        public async Task Get_InvalidParticipant_ReturnsError()
        {
            // Arrange
            var request = new RestRequest($"{Guid.Empty}", Method.Get);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<ParticipantDto>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TestContext.WriteLine("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            TestContext.WriteLine("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.False);
            TestContext.WriteLine("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            TestContext.WriteLine($"Response error: {response.Data.ErrorMessage}");
        }
        [Test, Order(5), Timeout(1000)]
        public async Task Update_ValidParticipant_ReturnsSuccess()
        {
            // Arrange
            var dto = new UpdateParticipantDto
            {
                Id = _participantId,
                Role = "Member",
                WarningTimeOffset = 30,
                Color = "3366FF"
            };

            var request = new RestRequest("update", Method.Post);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TestContext.WriteLine("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            TestContext.WriteLine("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            TestContext.WriteLine("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.True);
            TestContext.WriteLine($"Response value: {response.Data.Value}");
        }
        [Test, Order(6), Timeout(1000)]
        public async Task Update_InvalidParticipant_ReturnsError()
        {
            // Arrange
            var dto = new UpdateParticipantDto
            {
                Id = Guid.Empty,
                Role = "InvalidRole",
                WarningTimeOffset = -1
            };

            var request = new RestRequest("update", Method.Post);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TestContext.WriteLine("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            TestContext.WriteLine("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            TestContext.WriteLine("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.Value, Is.False);
            TestContext.WriteLine($"Response value: {response.Data.Value}");
        }

        [Test, Order(7), Timeout(1000)]
        public async Task Delete_InvalidParticipant_ReturnsError()
        {
            // Arrange
            var dto = new DeleteDto { Id = Guid.NewGuid() };

            var request = new RestRequest("delete", Method.Delete);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TestContext.WriteLine("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            TestContext.WriteLine("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.False);
            TestContext.WriteLine("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            TestContext.WriteLine($"Response error: {response.Data.ErrorMessage}");
        }
        [Test, Order(8), Timeout(1000)]
        public async Task Delete_ValidParticipant_ReturnsSuccess()
        {
            // Arrange
            var dto = new DeleteDto { Id = _participantId };

            var request = new RestRequest("delete", Method.Delete);
            request.AddJsonBody(dto);

            // Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TestContext.WriteLine("Response status code: OK");
            Assert.That(response.Data, Is.Not.Null);
            TestContext.WriteLine("Response data is not null");
            Assert.That(response.Data.IsDataReceived, Is.True);
            TestContext.WriteLine("Response data received: {0}", response.Data.IsDataReceived);
            Assert.That(response.Data.ErrorMessage, Is.Empty);
            TestContext.WriteLine($"Response error message: {response.Data.ErrorMessage}");
        }
    }
}
