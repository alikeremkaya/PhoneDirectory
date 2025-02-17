using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;
using PhoneDirectory.Application.Services.CommunicationInfoServices;
using PhoneDirectory.Domain.Enums;
using PhoneDirectory.Domain.Utilities.Concretes;

namespace PhoneDirectory.UnitTests.API.Controllers;

[TestFixture]
public class CommunicationInfoControllerTests
{
    private Mock<ICommunicationInfoService> _mockService;
    private CommunicationInfoController _controller;
    private CommunicationInfoDTO _testInfoDto;
    private Guid _personId;
    private Guid _communicationInfoId;
    private CommunicationInfoCreateDTO _createDto;
    private CommunicationInfoUpdateDTO _updateDto;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<ICommunicationInfoService>();
        _controller = new CommunicationInfoController(_mockService.Object);

        _personId = Guid.NewGuid();
        _communicationInfoId = Guid.NewGuid();

        _testInfoDto = new CommunicationInfoDTO
        {
           
            PersonId = _personId,
            InfoType = ContactInfoType.PhoneNumber,
            InfoContent = "+905551234567",
            CreatedDate = DateTime.UtcNow
        };

        _createDto = new CommunicationInfoCreateDTO
        {
            InfoType = ContactInfoType.PhoneNumber,
            InfoContent = "+905551234567"
        };

        _updateDto = new CommunicationInfoUpdateDTO
        {
            Id = _communicationInfoId,
            InfoType = ContactInfoType.Email,
            InfoContent = "test@email.com"
        };
    }

    [Test]
    public async Task GetAllByPerson_ShouldReturnOkResult()
    {
        // Arrange
        var infoList = new List<CommunicationInfoDTO> { _testInfoDto };
        _mockService.Setup(x => x.GetCommunicationInfosByPersonIdAsync(_personId))
            .ReturnsAsync(new SuccessDataResult<List<CommunicationInfoDTO>>(infoList, "Communication infos successfully listed"));

        // Act
        var result = await _controller.GetAllByPerson(_personId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessDataResult<List<CommunicationInfoDTO>>;
        Assert.That(value.Data.Count, Is.EqualTo(1));
        Assert.That(value.Messages, Is.EqualTo("Communication infos successfully listed"));
        Assert.That(value.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetById_WithValidIds_ShouldReturnOkResult()
    {
        // Arrange
        _mockService.Setup(x => x.GetCommunicationInfoByIdAsync(_personId, _communicationInfoId))
            .ReturnsAsync(new SuccessDataResult<CommunicationInfoDTO>(_testInfoDto, "Communication info successfully found"));

        // Act
        var result = await _controller.GetById(_personId, _communicationInfoId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessDataResult<CommunicationInfoDTO>;
       
        Assert.That(value.Messages, Is.EqualTo("Communication info successfully found"));
        Assert.That(value.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetById_WithInvalidIds_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidInfoId = Guid.NewGuid();
        var errorMessage = "Communication info not found";
        _mockService.Setup(x => x.GetCommunicationInfoByIdAsync(_personId, invalidInfoId))
            .ReturnsAsync(new ErrorDataResult<CommunicationInfoDTO>(errorMessage));

        // Act
        var result = await _controller.GetById(_personId, invalidInfoId);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage)); 
    }

    [Test]
    public async Task Create_WithValidData_ShouldReturnOkResult()  
    {
        // Arrange
        _mockService.Setup(x => x.CreateCommunicationInfoAsync(_personId, _createDto))
            .ReturnsAsync(new SuccessDataResult<CommunicationInfoDTO>(_testInfoDto, "Communication info successfully created"));

        // Act
        var result = await _controller.Create(_personId, _createDto);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>()); 
        var okResult = result as OkObjectResult;  
        var value = okResult.Value as SuccessDataResult<CommunicationInfoDTO>;
       
        Assert.That(value.Messages, Is.EqualTo("Communication info successfully created")); 
        Assert.That(value.IsSuccess, Is.True);  
                                             
    }

    [Test]
    public async Task Update_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        _mockService.Setup(x => x.UpdateCommunicationInfoAsync(_personId, _updateDto))
            .ReturnsAsync(new SuccessResult("Communication info successfully updated"));

        // Act
        var result = await _controller.Update(_personId, _updateDto);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessResult;
        Assert.That(value.Messages, Is.EqualTo("Communication info successfully updated"));
        Assert.That(value.IsSuccess, Is.True);
    }

    [Test]
    public async Task Update_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Communication info not found";
        _mockService.Setup(x => x.UpdateCommunicationInfoAsync(_personId, _updateDto))
            .ReturnsAsync(new ErrorResult(errorMessage));

        // Act
        var result = await _controller.Update(_personId, _updateDto);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
    }

    [Test]
    public async Task Delete_WithValidIds_ShouldReturnOkResult()
    {
        // Arrange
        _mockService.Setup(x => x.DeleteCommunicationInfoAsync(_personId, _communicationInfoId))
            .ReturnsAsync(new SuccessResult("Communication info successfully deleted"));

        // Act
        var result = await _controller.Delete(_personId, _communicationInfoId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SuccessResult;
        Assert.That(value.Messages, Is.EqualTo("Communication info successfully deleted"));
        Assert.That(value.IsSuccess, Is.True);
    }

    [Test]
    public async Task Delete_WithInvalidIds_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidInfoId = Guid.NewGuid();
        var errorMessage = "Communication info not found";
        _mockService.Setup(x => x.DeleteCommunicationInfoAsync(_personId, invalidInfoId))
            .ReturnsAsync(new ErrorResult(errorMessage));

        // Act
        var result = await _controller.Delete(_personId, invalidInfoId);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage)); 
    }
}