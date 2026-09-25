using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationManager.Application.DTOs.FuelComposition;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Interfaces;
using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.Interfaces;
using SimulationManager.Domain.Exceptions;

namespace SimulationManager.Api.Controllers
{
    [Route("api/fuel-compositions")]
    [ApiController]
    public class FuelCompositionController : ControllerBase
    {
        private readonly IFuelCompositionService _fuelCompositionService;
        private readonly ILogger<FuelCompositionController> _logger;

        public FuelCompositionController(
            IFuelCompositionService fuelCompositionService,
            ILogger<FuelCompositionController> logger)
        {
            _fuelCompositionService = fuelCompositionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<FuelCompositionResponseDto>>> Get(
            [FromQuery] PaginationRequestDto pagination)
        {
            var result = await _fuelCompositionService.GetPagedFuelCompositionsAsync(
                pagination.PageNumber,
                pagination.PageSize);

            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetFuelComposition")]
        public async Task<ActionResult<FuelCompositionResponseDto>> Get(int id)
        {
            var fuelComposition = await _fuelCompositionService.GetFuelCompositionByIdAsync(id);

            if (fuelComposition is null)
            {
                return NotFound(new { error = "Fuel Composition not found" });
            }

            return Ok(fuelComposition);
        }

        [HttpPost]
        public async Task<ActionResult<FuelCompositionResponseDto>> Post([FromBody] CreateFuelCompositionDto dto)
        {
            try
            {
                var result = await _fuelCompositionService.CreateFuelCompositionAsync(dto);

                return CreatedAtRoute("GetFuelComposition", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating fuel composition");
                return StatusCode(500, new { error = "An error occurred while creating fuel composition" });
            }
        }

        [HttpPut]
        public async Task<ActionResult<FuelCompositionResponseDto>> Update(
            int id,
            [FromBody] UpdateFuelCompositionDto dto)
        {
            try
            {
                var result = await _fuelCompositionService.UpdateFuelCompositionAsync(id, dto);

                return Ok(result);
            } 
            catch(NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating fuel composition");
                return StatusCode(500, new { error = "An error occurred while updating fuel composition" });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _fuelCompositionService.DeleteFuelCompositionAsync(id);

                return NoContent();
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting fuel composition");
                return StatusCode(500, new { error = "An error occurred while deleting fuel composition" });
            }
        }
    }
}
