using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationManager.Domain.Entities;
using SimulationManager.Application.DTOs.User;
using SimulationManager.Application.DTOs.FuelComposition;
using SimulationManager.Domain.Interfaces;
using SimulationManager.Application.DTOs.Common;

namespace SimulationManager.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("fuel-compositions")]
        public async Task<ActionResult<IEnumerable<UserReponseDto>>> GetUsersFuelCompositions()
        {
            var users = await _userRepository.GetUsersWithFuelCompositionsAsync();

            var response = users.Select(u => new UserReponseDto(
                u.Id,
                u.UserName!,
                u.Role!,
                u.CreatedAt,
                u.FuelCompositions?.Select(f => new FuelCompositionResponseDto(
                    f.Id,
                    f.Name!,
                    f.Composition,
                    f.CreatedAt,
                    f.UserId
                )).ToList()

            ));
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<UserReponseDto>>> Get(
            [FromQuery] PaginationRequestDto pagination)
        {
            var (users, totalRecords) = await _userRepository.GetPagedAsync(
                pagination.PageNumber,
                pagination.PageSize);

            var data = users.Select(u => new UserReponseDto(
                u.Id,
                u.UserName!,
                u.Role!,
                u.CreatedAt
            ));

            return Ok(new PagedResponseDto<UserReponseDto>(
                data,
                pagination.PageNumber,
                pagination.PageSize,
                totalRecords
            ));
        }

        [HttpGet("{id:int}", Name ="GetUser")]
        public async Task<ActionResult<UserReponseDto>> Get(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if(user is null)
            {
                return NotFound("User is not Found...");
            }
            return Ok(new UserReponseDto(user.Id, user.UserName!, user.Role!, user.CreatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<UserReponseDto>> Post([FromBody] CreateUserDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            var response = new UserReponseDto(user.Id, user.UserName!, user.Role!, user.CreatedAt);
            return new CreatedAtRouteResult("GetUser", 
                new { id = user.Id}, response);
        }

        [HttpPut]
        public async Task<ActionResult<UserReponseDto>> Update(
            int id,
            [FromBody] UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return NotFound($"User {id} not found.");

            user.UserName = dto.UserName;
            user.Role = dto.Role;

            await _userRepository.UpdateAsync(user);

            return Ok(new UserReponseDto(
                user.Id,
                user.UserName!,
                user.Role!,
                user.CreatedAt
            ));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if(user is null)
            {
                return NotFound("User not found...");
            }

            await _userRepository.DeleteAsync(user);
            return NoContent();
        }

    }
}
