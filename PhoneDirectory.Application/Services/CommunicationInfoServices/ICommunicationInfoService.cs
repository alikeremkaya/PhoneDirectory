using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;
using PhoneDirectory.Domain.Utilities.Interfaces;

namespace PhoneDirectory.Application.Services.CommunicationInfoServices;

public interface ICommunicationInfoService
{
    Task<IDataResult<CommunicationInfoDTO>> CreateCommunicationInfoAsync(Guid personId, CommunicationInfoCreateDTO dto);
    Task<IDataResult<CommunicationInfoDTO>> GetCommunicationInfoByIdAsync(Guid personId, Guid communicationInfoId);
    Task<IDataResult<List<CommunicationInfoDTO>>> GetCommunicationInfosByPersonIdAsync(Guid personId);
    Task<IResult> UpdateCommunicationInfoAsync(Guid personId, CommunicationInfoUpdateDTO dto);
    Task<IResult> DeleteCommunicationInfoAsync(Guid personId, Guid communicationInfoId);
}
