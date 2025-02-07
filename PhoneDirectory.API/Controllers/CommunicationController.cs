using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;
using PhoneDirectory.Application.Services.CommunicationInfoServices;

namespace PhoneDirectory.API.Controllers
{
    [ApiController]
    [Route("api/persons/{personId}/communication-info")]
    public class CommunicationInfoController : ControllerBase
    {
        private readonly ICommunicationInfoService _communicationInfoService;

        public CommunicationInfoController(ICommunicationInfoService communicationInfoService)
        {
            _communicationInfoService = communicationInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByPerson(Guid personId)
        {
            var result = await _communicationInfoService.GetCommunicationInfosByPersonIdAsync(personId);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Messages);
        }

        [HttpGet("{communicationInfoId}")]
        public async Task<IActionResult> GetById(Guid personId, Guid communicationInfoId)
        {
            var result = await _communicationInfoService.GetCommunicationInfoByIdAsync(personId, communicationInfoId);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Messages);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid personId, [FromBody] CommunicationInfoCreateDTO createDTO)
        {
            var result = await _communicationInfoService.CreateCommunicationInfoAsync(personId, createDTO);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Messages);
        }

        [HttpPut("{communicationInfoId}")]
        public async Task<IActionResult> Update(Guid personId, [FromBody] CommunicationInfoUpdateDTO updateDTO)
        {
            var result = await _communicationInfoService.UpdateCommunicationInfoAsync(personId, updateDTO);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Messages);
        }

        [HttpDelete("{communicationInfoId}")]
        public async Task<IActionResult> Delete(Guid personId, Guid communicationInfoId)
        {
            var result = await _communicationInfoService.DeleteCommunicationInfoAsync(personId, communicationInfoId);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Messages);
        }
    }
}
