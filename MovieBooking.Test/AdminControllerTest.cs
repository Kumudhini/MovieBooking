using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieBooking.Controllers;
using MovieBooking.Model;
using MovieBooking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieBooking.Test
{
    public class AdminControllerTests
    {
        private readonly Mock<ITicketService> _ticketServiceMock;
        private readonly Mock<ILogger<AdminController>> _loggerMock;
        private readonly AdminController _adminController;

        public AdminControllerTests()
        {
            _ticketServiceMock = new Mock<ITicketService>();
            _loggerMock = new Mock<ILogger<AdminController>>();
            _adminController = new AdminController(_ticketServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task UpdateTicketStatus_ValidInput_ReturnsOkResult()
        {
            // Arrange
            string movieName = "MovieName";
            int ticket = 10;
            var expectedResult = "Success";

            _ticketServiceMock.Setup(x => x.UpdateTicketStatus(movieName, ticket))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _adminController.UpdateTicketStatus(movieName, ticket) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetBookedTickets_ReturnsOkResult()
        {
            // Arrange
            var expectedTickets = new List<Tickets> { new Tickets(), new Tickets() };

            _ticketServiceMock.Setup(x => x.GetBookedTickets())
                .ReturnsAsync(expectedTickets);

            // Act
            var result = await _adminController.GetBookedTickets() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteMovie_ValidInput_ReturnsOkResult()
        {
            // Arrange
            string movieName = "MovieName";
            string id = "123";
            var expectedResult = "Success";

            _ticketServiceMock.Setup(x => x.DeleteMovie(movieName, id))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _adminController.DeleteMovie(movieName, id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
