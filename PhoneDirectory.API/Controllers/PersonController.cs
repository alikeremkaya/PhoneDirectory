using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Application.Services.PersonService;

namespace PhoneDirectory.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _personService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _personService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonCreateDTO createDTO)
    {
        var result = await _personService.CreateAsync(createDTO);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PersonUpdateDTO updateDTO)
    {
        if (id != updateDTO.Id)
            return BadRequest("ID uyuþmazlýðý");

        var result = await _personService.UpdateAsync(updateDTO);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _personService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }
}

