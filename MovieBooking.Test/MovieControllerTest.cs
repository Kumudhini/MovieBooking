using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using MovieBooking.Constants;
using MovieBooking.Controllers;
using MovieBooking.Model;
using MovieBooking.Services;

namespace MovieBooking.Test
{
    public class Tests
    {
        [TestFixture]
        public class MovieBookingControllerTests
        {
            private MovieBookingController _controller;
            private Mock<IMovieService> _movieServiceMock;
            private Mock<ITicketService> _ticketServiceMock;
            private Mock<ILogger<MovieBookingController>> _loggerMock;

            [SetUp]
            public void Setup()
            {
                _movieServiceMock = new Mock<IMovieService>();
                _ticketServiceMock = new Mock<ITicketService>();
                _loggerMock = new Mock<ILogger<MovieBookingController>>();

                _controller = new MovieBookingController(_movieServiceMock.Object, _ticketServiceMock.Object, _loggerMock.Object);
            }

            [Test]
            public async Task GetAllMovie_ValidRequest_ReturnsOkResultWithData()
            {
                // Arrange
                List<Movies> movies = new List<Movies>();
                _movieServiceMock.Setup(m => m.GetAllMovie()).ReturnsAsync(movies);

                // Act
                var result = await _controller.GetAllMovie();

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(movies));
            }

            [Test]
            public async Task GetAllMovie_ExceptionThrown_ReturnsBadRequest()
            {
                // Arrange
                _movieServiceMock.Setup(m => m.GetAllMovie()).ThrowsAsync(new Exception("Test exception"));

                // Act
                var result = await _controller.GetAllMovie();

                // Assert
                Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
                var badRequestResult = result.Result as BadRequestObjectResult;
                Assert.That(badRequestResult?.StatusCode, Is.EqualTo(400));
            }

            [Test]
            public async Task Search_ValidRequest_ReturnsOkResultWithData()
            {
                // Arrange
                List<Movies> movies = new List<Movies>();
                _movieServiceMock.Setup(m => m.GetAllMovie()).ReturnsAsync(movies);

                // Act
                var result = await _controller.Search("Avengers");

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.StatusCode, Is.EqualTo(200));
            }

            [Test]
            public async Task Search_ExceptionThrown_ReturnsBadRequest()
            {
                // Arrange
                _movieServiceMock.Setup(m => m.Search(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));

                // Act
                var result = await _controller.Search("Inception");

                // Assert
                Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
                var badRequestResult = result.Result as BadRequestObjectResult;
                Assert.That(badRequestResult?.StatusCode, Is.EqualTo(400));
            }


            [Test]
            public async Task AddTicket_ExceptionThrown_ReturnsBadRequest()
            {
                // Arrange
                string movieName = "Inception";
                int ticketCount = 2;
                _ticketServiceMock.Setup(t => t.AddTickets(movieName, ticketCount)).ThrowsAsync(new Exception("Test exception"));

                // Act
                var result = await _controller.AddTicket(movieName, ticketCount);

                // Assert
                Assert.IsInstanceOf<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.That(badRequestResult?.StatusCode, Is.EqualTo(400));
            }
            [Test]
            public async Task AddMovie_ValidRequest_ReturnsOkResultWithData()
            {
                // Arrange
                var movies = new Movies();
                _ticketServiceMock.Setup(t => t.AddMovie(It.IsAny<Movies>())).ReturnsAsync(movies);

                // Act
                var result = await _controller.AddMovie(movies);

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(movies));
            }
        }
    }
}