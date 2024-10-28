using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendBlogServicesApi.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return Result<UserDto>.Failure(new UserDto(), $"Usuario con ID '{id}' no encontrado.");
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                email = user.email,
                Estado = user.Estado,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllUsers();
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                email = user.email,
                Estado = user.Estado,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            }).ToList();

            return Result<IEnumerable<UserDto>>.Success(userDtos);
        }

        public async Task<Result<UserDto>> AddAsync(UserDto userDto)
        {
            userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var user = new User
            {
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                email = userDto.email,
                Password = userDto.Password,
                Estado = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            try
            {
                var existingNameUser = await _userRepository.ExistsByUsernameAsync(userDto.Username);
                if (existingNameUser)
                {
                    return Result<UserDto>.Failure(new UserDto(), $"El nombre de usuario '{userDto.Username}' ya está registrado.");
                }

                var existingEmailUser = await _userRepository.ExistsByEmailAsync(userDto.email);
                if (existingEmailUser)
                {
                    return Result<UserDto>.Failure(new UserDto(), $"El correo electrónico '{userDto.email}' ya está registrado.");
                }

                var createdUser = await _userRepository.CreateUserAsync(user);
                userDto.Id = createdUser.Id;
                return Result<UserDto>.Success(userDto, "Usuario añadido exitosamente.");
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure(new UserDto(), $"Error al añadir el usuario: {ex.Message}");
            }
        }

        public async Task<Result<UserDto>> UpdateAsync(int id, UserDto userDto)
        {
            var existingUser = await _userRepository.GetUserById(id);
            if (existingUser == null)
            {
                return Result<UserDto>.Failure(new UserDto(), "Usuario no encontrado.");
            }

            if (existingUser.Username != userDto.Username)
            {
                var existingNameUser = await _userRepository.ExistsByUsernameAsync(userDto.Username);
                if (existingNameUser)
                {
                    return Result<UserDto>.Failure(new UserDto(), $"El nombre de usuario '{userDto.Username}' ya está registrado.");
                }
            }

            if (existingUser.email != userDto.email)
            {
                var existingEmailUser = await _userRepository.ExistsByEmailAsync(userDto.email);
                if (existingEmailUser)
                {
                    return Result<UserDto>.Failure(new UserDto(), $"El correo electrónico '{userDto.email}' ya está registrado.");
                }
            }

            existingUser.Username = userDto.Username;
            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.email = userDto.email;
            existingUser.Estado = userDto.Estado;
            existingUser.UpdatedAt = DateTime.UtcNow;

            if (userDto.Password != null)
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            try
            {
                var userUpdated = await _userRepository.UpdateUser(existingUser);
                var userUpdatedDTO = new UserDto()
                {
                    Id = userUpdated.Id,
                    Username = userUpdated.Username,
                    Password = existingUser.Password,
                    FirstName = userUpdated.FirstName,
                    LastName = userUpdated.LastName,
                    email = userUpdated.email,
                    Estado = userUpdated.Estado,
                    CreatedAt = userUpdated.CreatedAt,
                    UpdatedAt = userUpdated.UpdatedAt
                };
                return Result<UserDto>.Success(userUpdatedDTO, "Usuario actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure(new UserDto(), $"Error al actualizar el usuario: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var success = await _userRepository.DeleteUser(id);
                if (!success)
                {
                    return Result<bool>.Failure(false, "Error al eliminar el usuario.");
                }
                return Result<bool>.Success(true, "Usuario eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(false, $"Error al eliminar el usuario: {ex.Message}");
            }
        }
    }
}
