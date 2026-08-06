namespace SimulationManager.Application.Interfaces;

using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.User;

public interface IUserService
{
    Task<PagedResponseDto<UserReponseDto>> GetPagedUsersAsync(int pageNumber, int pageSize);

    Task<UserReponseDto?> GetUserByIdAsync(int id);

    Task<UserReponseDto> CreateUserAsync(CreateUserDto dto);

    Task<UserReponseDto> UpdateUserAsync(int id, UpdateUserDto dto);

    Task DeleteUserAsync(int id);
}
