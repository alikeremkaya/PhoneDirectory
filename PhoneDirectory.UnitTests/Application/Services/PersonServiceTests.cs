using Moq;
using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Application.Services.PersonService;
using PhoneDirectory.Application.Services.PersonServices;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Infrastructure.Repositories.PersonRepositories;

namespace PhoneDirectory.UnitTests.Application.Services;

[TestFixture]
public class PersonServiceTests
{
    private Mock<IPersonRepository> _mockRepository;
    private IPersonService _service;
    private Person _testPerson;
    private PersonCreateDTO _createDto;
    private PersonUpdateDTO _updateDto;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IPersonRepository>();
        _service = new PersonService(_mockRepository.Object);

        _testPerson = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Ali",
            LastName = "Kaya",
            CompanyName = "Test Company",
            CreatedDate = DateTime.UtcNow
        };

        _createDto = new PersonCreateDTO
        {
            FirstName = "Ali",
            LastName = "Kaya",
            CompanyName = "Test Company"
        };

        _updateDto = new PersonUpdateDTO
        {
            Id = _testPerson.Id,
            FirstName = "Updated Ali",
            LastName = "Updated Kaya",
            CompanyName = "Updated Company"
        };
    }

    [Test]
    public async Task GetById_WithValidId_ShouldReturnSuccessDataResult()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetByIdAsync(_testPerson.Id, true))
            .ReturnsAsync(_testPerson);

        // Act
        var result = await _service.GetByIdAsync(_testPerson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Id, Is.EqualTo(_testPerson.Id));
    }

    [Test]
    public async Task GetById_WithInvalidId_ShouldReturnErrorResult()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        _mockRepository.Setup(x => x.GetByIdAsync(invalidId, true))
            .ReturnsAsync((Person)null);

        // Act
        var result = await _service.GetByIdAsync(invalidId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Data, Is.Null);
    }

    [Test]
    public async Task Update_WithNonExistingPerson_ShouldReturnErrorResult()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetByIdAsync(_updateDto.Id, true))
            .ReturnsAsync((Person)null);

        // Act
        var result = await _service.UpdateAsync(_updateDto);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
    }
}
