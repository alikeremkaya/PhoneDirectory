using Mapster;
using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Utilities.Concretes;
using PhoneDirectory.Domain.Utilities.Interfaces;
using PhoneDirectory.Infrastructure.Repositories.CommunicationInfoRepositories;

namespace PhoneDirectory.Application.Services.CommunicationInfoServices;

public class CommunicationInfoService : ICommunicationInfoService
{
    private readonly ICommunicationInfoRepository _communicationInfoRepository;

    public CommunicationInfoService(ICommunicationInfoRepository communicationInfoRepository)
    {
        _communicationInfoRepository = communicationInfoRepository;
    }

    public async Task<IDataResult<CommunicationInfoDTO>> CreateCommunicationInfoAsync(Guid personId, CommunicationInfoCreateDTO dto)
    {
        var communicationInfo = dto.Adapt<CommunicationInfo>();
        communicationInfo.Id = Guid.NewGuid();
        communicationInfo.PersonId = personId;

        await _communicationInfoRepository.AddAsync(communicationInfo);
        await _communicationInfoRepository.SaveChangesAsync();

        var resultDto = communicationInfo.Adapt<CommunicationInfoDTO>();
        return new SuccessDataResult<CommunicationInfoDTO>(resultDto, "Communication info created successfully");
    }

    public async Task<IDataResult<CommunicationInfoDTO>> GetCommunicationInfoByIdAsync(Guid personId, Guid communicationInfoId)
    {
        var communicationInfo = await _communicationInfoRepository.GetByIdAsync(communicationInfoId);
        if (communicationInfo == null || communicationInfo.PersonId != personId)
            return new ErrorDataResult<CommunicationInfoDTO>("Communication info not found");

        var dto = communicationInfo.Adapt<CommunicationInfoDTO>();
        return new SuccessDataResult<CommunicationInfoDTO>(dto, "Communication info retrieved successfully");
    }

    public async Task<IDataResult<List<CommunicationInfoDTO>>> GetCommunicationInfosByPersonIdAsync(Guid personId)
    {
        var communicationInfos = await _communicationInfoRepository.GetAllAsync(ci => ci.PersonId == personId);
        var dtos = communicationInfos.Select(ci => ci.Adapt<CommunicationInfoDTO>()).ToList();
        return new SuccessDataResult<List<CommunicationInfoDTO>>(dtos, "Communication infos retrieved successfully");
    }

    public async Task<IResult> UpdateCommunicationInfoAsync(Guid personId, CommunicationInfoUpdateDTO dto)
    {
        var communicationInfo = await _communicationInfoRepository.GetByIdAsync(dto.Id);
        if (communicationInfo == null || communicationInfo.PersonId != personId)
            return new ErrorResult("Communication info not found");

        dto.Adapt(communicationInfo);
        await _communicationInfoRepository.UpdateAsync(communicationInfo);
        await _communicationInfoRepository.SaveChangesAsync();

        return new SuccessResult("Communication info updated successfully");
    }

    public async Task<IResult> DeleteCommunicationInfoAsync(Guid personId, Guid communicationInfoId)
    {
        var communicationInfo = await _communicationInfoRepository.GetByIdAsync(communicationInfoId);
        if (communicationInfo == null || communicationInfo.PersonId != personId)
            return new ErrorResult("Communication info not found");

        await _communicationInfoRepository.DeleteAsync(communicationInfo);
        await _communicationInfoRepository.SaveChangesAsync();

        return new SuccessResult("Communication info deleted successfully");
    }
}


