using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Application.Services.PersonService;
using PhoneDirectory.Domain.Utilities.Concretes;

namespace PhoneDirectory.UnitTests.API.Controllers;

[TestFixture]
public class PersonControllerTests
{
    private Mock<IPersonService> _mockService;
    private PersonController _controller;
    private PersonDTO _testPersonDto;
    private PersonCreateDTO _createDto;
    private PersonUpdateDTO _updateDto;
    private Guid _testId;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IPersonService>();
        _controller = new PersonController(_mockService.Object);

        _testId = Guid.NewGuid();
        _testPersonDto = new PersonDTO
        {
            Id = _testId,
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
            Id = _testId,
            FirstName = "Updated Ali",
            LastName = "Updated Kaya",
            CompanyName = "Updated Company"
        };
    }

    [Test]
    public async Task GetAll_ShouldReturnOkResult()
    {
        // Arrange
        var personList = new List<PersonListDTO>
    {
        new PersonListDTO
        {
            Id = _testId,
            FirstName = "Ali",
            LastName = "Kaya",
            CompanyName = "Test Company"
        }
    };

        _mockService.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new SuccessDataResult<List<PersonListDTO>>(personList, "People successfully listed"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessDataResult<List<PersonListDTO>>;
        Assert.That(value.Data.Count, Is.EqualTo(1));
        Assert.That(value.Messages, Is.EqualTo("People successfully listed"));
    }

    [Test]
    public async Task GetById_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        _mockService.Setup(x => x.GetByIdAsync(_testId))
            .ReturnsAsync(new SuccessDataResult<PersonDTO>(_testPersonDto, "Person successfully found"));

        // Act
        var result = await _controller.GetById(_testId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessDataResult<PersonDTO>;
        Assert.That(value.Data.Id, Is.EqualTo(_testId));
        Assert.That(value.Messages, Is.EqualTo("Person successfully found"));
    }

    [Test]
    public async Task GetById_WithInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        _mockService.Setup(x => x.GetByIdAsync(invalidId))
            .ReturnsAsync(new ErrorDataResult<PersonDTO>("Person not found"));

        // Act
        var result = await _controller.GetById(invalidId);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Person not found"));
    }

    [Test]
    public async Task Create_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        _mockService.Setup(x => x.CreateAsync(_createDto))
            .ReturnsAsync(new SuccessResult("Person successfully created"));

        // Act
        var result = await _controller.Create(_createDto);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessResult;
        Assert.That(value.Messages, Is.EqualTo("Person successfully created"));
    }

    [Test]
    public async Task Create_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        _mockService.Setup(x => x.CreateAsync(_createDto))
            .ReturnsAsync(new ErrorResult("Invalid data"));

        // Act
        var result = await _controller.Create(_createDto);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Invalid data"));
    }

    [Test]
    public async Task Update_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        _mockService.Setup(x => x.UpdateAsync(_updateDto))
            .ReturnsAsync(new SuccessResult("Person successfully updated"));

        // Act
        var result = await _controller.Update(_testId, _updateDto);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessResult;
        Assert.That(value.Messages, Is.EqualTo("Person successfully updated"));
    }

    [Test]
    public async Task Update_WithIdMismatch_ShouldReturnBadRequest()
    {
        // Arrange
        var differentId = Guid.NewGuid();

        // Act
        var result = await _controller.Update(differentId, _updateDto);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("ID uyuşmazlığı"));
    }

    [Test]
    public async Task Delete_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        _mockService.Setup(x => x.DeleteAsync(_testId))
            .ReturnsAsync(new SuccessResult("Person successfully deleted"));

        // Act
        var result = await _controller.Delete(_testId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessResult;
        Assert.That(value.Messages, Is.EqualTo("Person successfully deleted"));
    }

    [Test]
    public async Task Delete_WithInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        _mockService.Setup(x => x.DeleteAsync(invalidId))
            .ReturnsAsync(new ErrorResult("Person not found"));

        // Act
        var result = await _controller.Delete(invalidId);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Person not found"));
    }
}
