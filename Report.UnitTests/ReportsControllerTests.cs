using Microsoft.AspNetCore.Mvc;
using Moq;
using Report.API.Controllers;
using Report.Application.DTOs;
using Report.Application.Services;
using Report.Domain.Utilities.Concretes;

namespace Report.UnitTests;

public class Tests
{
    [TestFixture]
    public class ReportsControllerTests
    {
        private Mock<IReportApplicationService> _mockService;
        private ReportsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IReportApplicationService>();
            _controller = new ReportsController(_mockService.Object);
        }

        [Test]
        public async Task GetAll_WhenSuccess_ReturnsOkResult()
        {
            // Arrange
            var reports = new List<ReportListDTO>
    {
        new() { Id = Guid.NewGuid(), Location = "Istanbul" },
        new() { Id = Guid.NewGuid(), Location = "Ankara" }
    };

            _mockService.Setup(s => s.GetAllReportsAsync())
                .ReturnsAsync(new SuccessDataResult<IEnumerable<ReportListDTO>>(reports, "Success"));

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var returnValue = okResult.Value as SuccessDataResult<IEnumerable<ReportListDTO>>;
            Assert.That(returnValue.Data.Count(), Is.EqualTo(2), "Expected 2 reports to be returned.");
        }

        [Test]
        public async Task GetById_WhenReportExists_ReturnsOkResult()
        {
            // Arrange
            var reportId = Guid.NewGuid();
            var report = new ReportDTO { Id = reportId, Location = "Istanbul" };

            _mockService.Setup(s => s.GetReportByIdAsync(reportId))
                .ReturnsAsync(new SuccessDataResult<ReportDTO>(report, "Success"));

            // Act
            var result = await _controller.GetById(reportId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var returnValue = okResult.Value as SuccessDataResult<ReportDTO>;
            Assert.That(returnValue.Data.Id, Is.EqualTo(reportId));
        }

        [Test]
        public async Task GetById_WhenReportNotFound_ReturnsNotFound()
        {
            // Arrange
            var reportId = Guid.NewGuid();

            _mockService.Setup(s => s.GetReportByIdAsync(reportId))
                .ReturnsAsync(new ErrorDataResult<ReportDTO>("Not found"));

            // Act
            var result = await _controller.GetById(reportId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task Create_WhenValidInput_ReturnsCreatedResult()
        {
            // Arrange
            var createDto = new CreateReportDTO { Location = "Istanbul" };
            var createdReport = new ReportDTO { Id = Guid.NewGuid(), Location = "Istanbul" };

            _mockService.Setup(s => s.CreateReportAsync(createDto))
                .ReturnsAsync(new SuccessDataResult<ReportDTO>(createdReport, "Created"));

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            var returnValue = createdResult.Value as SuccessDataResult<ReportDTO>;
            Assert.That(returnValue.Data.Location, Is.EqualTo("Istanbul"));
        }

        [TearDown]
        public void Teardown()
        {
            _mockService = null;
            _controller = null;
        }
    }
}