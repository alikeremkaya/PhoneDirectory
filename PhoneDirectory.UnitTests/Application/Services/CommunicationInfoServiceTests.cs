using Mapster;
using Moq;
using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;
using PhoneDirectory.Application.Services.CommunicationInfoServices;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Enums;
using PhoneDirectory.Infrastructure.Repositories.CommunicationInfoRepositories;
using System.Linq.Expressions;

namespace PhoneDirectory.UnitTests.Application.Services
{
    [TestFixture]
    public class CommunicationInfoServiceTests
    {
        private Mock<ICommunicationInfoRepository> _mockRepository;
        private ICommunicationInfoService _service;
        private CommunicationInfo _testInfo;
        private Guid _personId;
        private CommunicationInfoCreateDTO _createDto;
        private CommunicationInfoUpdateDTO _updateDto;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
          
            TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Exact);

            TypeAdapterConfig<CommunicationInfo, CommunicationInfoDTO>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.InfoType, src => src.InfoType)
                .Map(dest => dest.InfoContent, src => src.InfoContent)
                .Map(dest => dest.CreatedDate, src => src.CreatedDate)
                .Map(dest => dest.UpdatedDate, src => src.UpdatedDate);

            TypeAdapterConfig<CommunicationInfoCreateDTO, CommunicationInfo>.NewConfig()
                .Map(dest => dest.InfoType, src => src.InfoType)
                .Map(dest => dest.InfoContent, src => src.InfoContent);

            TypeAdapterConfig<CommunicationInfoUpdateDTO, CommunicationInfo>.NewConfig()
                .Map(dest => dest.InfoType, src => src.InfoType)
                .Map(dest => dest.InfoContent, src => src.InfoContent);

            TypeAdapterConfig.GlobalSettings.Compile();
        }

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICommunicationInfoRepository>();
            _service = new CommunicationInfoService(_mockRepository.Object);

            _personId = Guid.NewGuid();
            _testInfo = new CommunicationInfo
            {
                Id = Guid.NewGuid(),
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
                Id = _testInfo.Id,
                InfoType = ContactInfoType.Email,
                InfoContent = "test@email.com"
            };
        }

        [Test]
        public async Task GetCommunicationInfoById_WithValidIds_ShouldReturnSuccessDataResult()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetByIdAsync(_testInfo.Id, It.IsAny<bool>()))
                           .ReturnsAsync(_testInfo);

            // Act
            var result = await _service.GetCommunicationInfoByIdAsync(_personId, _testInfo.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.IsSuccess, "Result should indicate success.");
                Assert.IsNotNull(result.Data, "Data should not be null.");
                Assert.AreEqual(_testInfo.Id, result.Data.Id, "Ids should match.");
                Assert.AreEqual(_personId, result.Data.PersonId, "PersonIds should match.");
                Assert.AreEqual(_testInfo.InfoType, result.Data.InfoType, "InfoTypes should match.");
                Assert.AreEqual(_testInfo.InfoContent, result.Data.InfoContent, "InfoContents should match.");
            });

            
            _mockRepository.Verify(x => x.GetByIdAsync(
                It.Is<Guid>(id => id == _testInfo.Id),
                It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public async Task GetCommunicationInfoById_WithInvalidIds_ShouldReturnErrorResult()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetAsync(
                It.Is<Expression<Func<CommunicationInfo, bool>>>(e =>
                    ExpressionMatches(e, x => x.Id == _testInfo.Id && x.PersonId == _personId)),
                true))
                .ReturnsAsync((CommunicationInfo?)null);

            // Act
            var result = await _service.GetCommunicationInfoByIdAsync(_personId, _testInfo.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.IsSuccess, "Result should indicate failure.");
                Assert.IsNull(result.Data, "Data should be null.");
            });

            
            _mockRepository.Verify(x => x.GetAsync(
                It.Is<Expression<Func<CommunicationInfo, bool>>>(e =>
                    ExpressionMatches(e, x => x.Id == _testInfo.Id && x.PersonId == _personId)),
                true), Times.Once);
        }

        [Test]
        public async Task UpdateCommunicationInfo_WithNonExistingInfo_ShouldReturnErrorResult()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetAsync(
                It.Is<Expression<Func<CommunicationInfo, bool>>>(e =>
                    ExpressionMatches(e, x => x.Id == _updateDto.Id && x.PersonId == _personId)),
                true))
                .ReturnsAsync((CommunicationInfo?)null);

            // Act
            var result = await _service.UpdateCommunicationInfoAsync(_personId, _updateDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.IsSuccess, "Result should indicate failure.");
            });

            
            _mockRepository.Verify(x => x.GetAsync(
                It.Is<Expression<Func<CommunicationInfo, bool>>>(e =>
                    ExpressionMatches(e, x => x.Id == _updateDto.Id && x.PersonId == _personId)),
                true), Times.Once);
        }

      
        private bool ExpressionMatches<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) where T : new()
        {
           
            var func1 = expr1.Compile();
            var func2 = expr2.Compile();

          
            var sampleEntity = new T();

            try
            {
                return func1(sampleEntity) == func2(sampleEntity);
            }
            catch
            {
               
                return false;
            }
        }
    }
}