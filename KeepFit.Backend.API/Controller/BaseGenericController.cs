using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
public abstract class BaseGenericController<TService, TResponse, TDto>(TService service) : ControllerBase
    where TService : IContract<TResponse, TDto>
{
    private readonly TService _service = service;

    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync(
        [FromQuery] PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.GetAllAsync(filter, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new PageApiResponse<List<TResponse>?>(null, 0, 0, 0));
        }
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(
        [FromQuery] PaginationFilter filter,
        Guid id,  
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.GetAsync(filter, id, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new PageApiResponse<List<TResponse>?>(null, 0, 0, 0));
        }
    }
    
    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync(
        [FromBody] TDto dto, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.CreateAsync(dto, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new PageApiResponse<List<TResponse>?>(null, 0, 0, 0));
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (NotFoundException message)
        {
            return NotFound(new ApiResponse<bool>(true, false, "Erreur: " + message.Message));
        }
    }
}