using System.Security.Claims;
using Hr.Ozluk.Application.DTOs;
using Hr.Ozluk.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Ozluk.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PersonDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _personService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _personService.GetByIdAsync(id, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create([FromBody] PersonCreateDto dto, CancellationToken cancellationToken)
    {
        var createdBy = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? "system";
        var result = await _personService.CreateAsync(dto, createdBy, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PersonDto>> Update(string id, [FromBody] PersonUpdateDto dto, CancellationToken cancellationToken)
    {
        var updatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? "system";
        var result = await _personService.UpdateAsync(id, dto, updatedBy, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await _personService.DeleteAsync(id, cancellationToken);
        if (!result) return NotFound();
        return NoContent();
    }
}
