using Moq;
using Report.Application.DTOs;
using Report.Application.Services;
using Report.Domain.Enums;
using NUnit.Framework;
using Report.Infrastructure.Repositories;

namespace Report.UnitTests
{
    [TestFixture]
    public class ReportApplicationServiceTests
    {
        private Mock<IReportRepository> _mockRepository;
        private ReportApplicationService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IReportRepository>();
            _service = new ReportApplicationService(_mockRepository.Object);
        }

        [Test]
        public async Task CreateReportAsync_WhenValidRequest_ShouldReturnSuccessResult()
        {
            // Arrange
            var createDto = new CreateReportDTO { Location = "Istanbul" };
            var report = new Report.Domain.Entities.Report
            {
                Id = Guid.NewGuid(),
                Location = "Istanbul",
                CreatedDate = DateTime.UtcNow,
                ReportStatus = ReportStatus.Preparing
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Report.Domain.Entities.Report>()))
                .Returns(Task.FromResult(report));
            _mockRepository.Setup(r => r.SaveChangesAsync())
                .Returns(Task.FromResult(1)); // SaveChangesAsync returns Task<int>

            // Act
            var result = await _service.CreateReportAsync(createDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True, "Result should be successful");
                Assert.That(result.Data, Is.Not.Null, "Data should not be null");
                Assert.That(result.Data.Location, Is.EqualTo("Istanbul"), "Location should match");
                Assert.That(result.Messages, Is.EqualTo("Report created successfully"), "Success message should match");
            });

            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Report.Domain.Entities.Report>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllReportsAsync_WhenReportsExist_ShouldReturnSuccessResult()
        {
            // Arrange
            var reports = new List<Report.Domain.Entities.Report>
    {
        new() { Id = Guid.NewGuid(), Location = "Istanbul" },
        new() { Id = Guid.NewGuid(), Location = "Ankara" }
    };

            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<bool>()))
         .ReturnsAsync(reports as IEnumerable<Report.Domain.Entities.Report>);


            // Act
            var result = await _service.GetAllReportsAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True, "Result should be successful");
                var reportsList = result.Data.ToList();
                Assert.That(reportsList, Has.Count.EqualTo(2), "Should return 2 reports");
                Assert.That(result.Messages, Is.EqualTo("Reports retrieved successfully"), "Success message should match");
            });
        }

       

        [Test]
        public async Task GetReportByIdAsync_WhenReportNotFound_ShouldReturnErrorResult()
        {
            // Arrange
            var reportId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(reportId, false))
                .ReturnsAsync((Report.Domain.Entities.Report)null);

            // Act
            var result = await _service.GetReportByIdAsync(reportId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False, "Result should be unsuccessful");
                Assert.That(result.Data, Is.Null, "Data should be null");
                Assert.That(result.Messages, Is.EqualTo("Report not found"), "Error message should match");
            });
        }

      

        [Test]
        public async Task UpdateReportStatusAsync_WhenUpdateFails_ShouldReturnErrorResult()
        {
            // Arrange
            var reportId = Guid.NewGuid();
            var updateDto = new UpdateReportDTO
            {
                Status = ReportStatus.Completed,
                PersonCount = 10,
                PhoneNumberCount = 20
            };

            _mockRepository.Setup(r => r.UpdateReportStatusAsync(reportId, updateDto.Status, updateDto.PersonCount, updateDto.PhoneNumberCount))
                .ThrowsAsync(new Exception("Update failed"));

            // Act
            var result = await _service.UpdateReportStatusAsync(reportId, updateDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False, "Result should be unsuccessful");
                Assert.That(result.Data, Is.Null, "Data should be null");
                Assert.That(result.Messages, Does.Contain("Update failed"), "Error message should contain exception message");
            });
        }

        [TearDown]
        public void Teardown()
        {
            _mockRepository = null;
            _service = null;
        }
    }
}
