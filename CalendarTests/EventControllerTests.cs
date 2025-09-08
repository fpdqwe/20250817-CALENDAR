using BLL.Dto;
using RestSharp;
using System.Net;

namespace CalendarTests
{
    [TestFixture]
    public class EventControllerTests
    {
        private static RestClient _client;
        private static Guid _id;
        private const string BaseUrl = "http://localhost:5054/api/v1/events";
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _id = Guid.NewGuid();
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
        [Test, Order(0)]
        public async Task Add_Event_ReturnsSuccess()
        {
            // Arrange
            var dto = new CreateEventDto
            {
                Name = "TestEvent" + _id,
                DateCreated = DateTime.UtcNow,
                Date = DateTime.UtcNow.AddDays(2),
                Duration = 4,
                Description = "Add_Event_ReturnsSuccess() test entity. Should be deleted",
                IterationTime = Domain.Enums.IterationTime.Weekly,
            };

            var request = new RestRequest("add", method: Method.Post);
            request.AddJsonBody(dto);
            //  Act
            var response = await _client.ExecuteAsync<CallbackDto<bool>>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.True);
            Assert.That(response.Data.Value, Is.True);
        }
        [Test, Order(1), Timeout(1000)]
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
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.False);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            TestContext.WriteLine(response.Data.ErrorMessage);
        }
        [Test, Order(2), Timeout(1000)]
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
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IsDataReceived, Is.False);
            Assert.That(response.Data.ErrorMessage, Is.Not.Empty);
            TestContext.WriteLine(response.Data.ErrorMessage);
        }
    }
}
