using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.FuelComposition;
using SimulationManager.Application.DTOs.User;
using SimulationManager.Application.Interfaces;
using SimulationManager.Application.Services;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Exceptions;
using SimulationManager.Domain.Interfaces;

namespace SimulationManager.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<UserReponseDto>>> Get(
            [FromQuery] PaginationRequestDto pagination)
        {
            var result = await _userService.GetPagedUsersAsync(
                pagination.PageNumber,
                pagination.PageSize);

            return Ok(result);
        }

        [HttpGet("{id:int}", Name ="GetUser")]
        public async Task<ActionResult<UserReponseDto>> Get(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if(user is null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserReponseDto>> Post([FromBody] CreateUserDto dto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(dto);

                return CreatedAtRoute("GetUser", new {id = result.Id}, result );
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new { error = "An error occurred while creating user" });
            }

        }

        [HttpPut]
        public async Task<ActionResult<UserReponseDto>> Update(
            int id,
            [FromBody] UpdateUserDto dto)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, dto);

                return Ok(result);
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return StatusCode(500, new { error = "An error occurred while updating user" });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return StatusCode(500, new { error = "An error occurred while deleting user" });
            }
        }

    }
}
