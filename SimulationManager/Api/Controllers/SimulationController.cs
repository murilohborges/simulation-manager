using Microsoft.AspNetCore.Mvc;
using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.Simulation;
using SimulationManager.Application.Interfaces;
using SimulationManager.Application.Services;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Exceptions;
using SimulationManager.Domain.Interfaces;
using System.Text.Json;

namespace SimulationManager.Api.Controllers;

[Route("api/simulation")]
[ApiController]
public class SimulationController : ControllerBase
{
    private readonly ISimulationService _simulationService;
    private readonly ILogger<SimulationController> _logger;

    public SimulationController(
        ISimulationService simulationService,
        ILogger<SimulationController> logger)
    {
        _simulationService = simulationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<SimulationResponseDto>>> Get(
        [FromQuery] PaginationRequestDto pagination)
    {
        var result = await _simulationService.GetPagedSimulationsAsync(
            pagination.PageNumber,
            pagination.PageSize);

        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetSimulation")]
    public async Task<ActionResult<SimulationResponseDto>> Get(int id)
    {
        var simulation = await _simulationService.GetSimulationByIdAsync(id);
        if (simulation is null)
            return NotFound($"Simulation {id} not found.");

        return Ok(simulation);
    }

    [HttpPost]
    public async Task<ActionResult<SimulationResponseDto>> Post(
        [FromBody] CreateSimulationDto dto)
    {
        try
        {
            var result = await _simulationService.CreateSimulationAsync(dto);

            return CreatedAtRoute("GetSimulation", new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating simulation");
            return StatusCode(500, new { error = "An error occurred while creating simulation" });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _simulationService.DeleteSimulationAsync(id);

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting simulation");
            return StatusCode(500, new { error = "An error occurred while deleting simulation" });
        }
    }

    
}
