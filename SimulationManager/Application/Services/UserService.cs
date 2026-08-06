namespace SimulationManager.Application.Services;

using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.User;
using SimulationManager.Application.Interfaces;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Exceptions;
using SimulationManager.Domain.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<PagedResponseDto<UserReponseDto>> GetPagedUsersAsync(
        int pageNumber,
        int pageSize)
    {
        var (users, totalRecords) = await _userRepository.GetPagedAsync(pageNumber, pageSize);

        var data = users.Select(MapToResponseDto);

        return new PagedResponseDto<UserReponseDto>(data, pageNumber, pageSize, totalRecords);
    }

    public async Task<UserReponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        return user is null ? null : MapToResponseDto(user);
    }

    public async Task<UserReponseDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = new User
        {
            UserName = dto.UserName,
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        _logger.LogInformation("User {Id} created successfully", user.Id);

        return MapToResponseDto(user);
    }

    public async Task<UserReponseDto> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        user.UserName = dto.UserName;
        user.Role = dto.Role;

        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("User {Id} updated successfully", id);

        return MapToResponseDto(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        await _userRepository.DeleteAsync(user);

        _logger.LogInformation("User {Id} deleted successfully", id);
    }

    private static UserReponseDto MapToResponseDto(User user)
       => new UserReponseDto(user.Id, user.UserName!, user.Role, user.CreatedAt);
}
