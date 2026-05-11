using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationManager.Application.DTOs.FuelComposition;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Interfaces;
using SimulationManager.Application.DTOs.Common;

namespace SimulationManager.Api.Controllers
{
    [Route("api/fuel-composition")]
    [ApiController]
    public class FuelCompositionController : ControllerBase
    {
        private readonly IFuelCompositionRepository _fuelCompositionRepository;

        public FuelCompositionController(IFuelCompositionRepository fuelCompositionRepository)
        {
            _fuelCompositionRepository = fuelCompositionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<FuelCompositionResponseDto>>> Get(
            [FromQuery] PaginationRequestDto pagination)
        {
            var (fuelCompositions, totalRecords) = await _fuelCompositionRepository.GetPagedAsync(
                pagination.PageNumber,
                pagination.PageSize);

            var data = fuelCompositions.Select(f => new FuelCompositionResponseDto(
                f.Id,
                f.Name!,
                f.Composition,
                f.CreatedAt,
                f.UserId
            ));

            return Ok(new PagedResponseDto<FuelCompositionResponseDto>(
                data,
                pagination.PageNumber,
                pagination.PageSize,
                totalRecords
            ));
        }

        [HttpGet("{id:int}", Name = "GetFuelComposition")]
        public async Task<ActionResult<FuelCompositionResponseDto>> Get(int id)
        {
            var fuelComposition = await _fuelCompositionRepository.GetByIdAsync(id);
            if (fuelComposition is null)
            {
                return NotFound("Fuel Composition is not Found...");
            }
            var response = new FuelCompositionResponseDto(
                fuelComposition.Id,
                fuelComposition.Name!,
                fuelComposition.Composition,
                fuelComposition.CreatedAt,
                fuelComposition.UserId
            );
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<FuelCompositionResponseDto>> Post([FromBody] CreateFuelCompositionDto dto)
        {
            var fuelComposition = new FuelComposition
            {
                Name = dto.Name,
                Composition = dto.Composition,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _fuelCompositionRepository.CreateAsync(fuelComposition);

            var response = new FuelCompositionResponseDto(
                fuelComposition.Id,
                fuelComposition.Name,
                fuelComposition.Composition,
                fuelComposition.CreatedAt,
                fuelComposition.UserId
            );
            return new CreatedAtRouteResult("GetFuelComposition",
                new { id = fuelComposition.Id }, response);
        }

        [HttpPut]
        public async Task<ActionResult<FuelCompositionResponseDto>> Update(
            int id,
            [FromBody] UpdateFuelCompositionDto dto)
        {
            var fuelComposition = await _fuelCompositionRepository.GetByIdAsync(id);
            if (fuelComposition is null)
                return NotFound($"Fuel Composition {id} not found.");

            fuelComposition.Name = dto.Name;
            fuelComposition.Composition = dto.Composition;

            await _fuelCompositionRepository.UpdateAsync(fuelComposition);

            return Ok(new FuelCompositionResponseDto(
                fuelComposition.Id,
                fuelComposition.Name!,
                fuelComposition.Composition,
                fuelComposition.CreatedAt,
                fuelComposition.UserId
            ));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var fuelComposition = await _fuelCompositionRepository.GetByIdAsync(id);

            if (fuelComposition is null)
            {
                return NotFound("Fuel Composition not found...");
            }

            await _fuelCompositionRepository.DeleteAsync(fuelComposition);

            return NoContent();
        }
    }
}
