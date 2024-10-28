using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Repositories.Interfaces;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BackendBlogServicesApi.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, UserService userService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<Result<UserDto>> RegisterAsync(UserDto userRegisterDto)
        {
            // Guardar el usuario en la base de datos
            return await _userService.AddAsync(userRegisterDto);
        }

        public async Task<Result<UserLoginResponseDto>> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(userLoginDto.UsernameOrEmail);

            if (user == null || !user.Estado)
            {
                return Result<UserLoginResponseDto>.Failure(null, "Usuario no encontrado o deshabilitado.");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password);
            if (!isPasswordValid)
            {
                return Result<UserLoginResponseDto>.Failure(null, "Contraseña incorrecta.");
            }

            var tokenString = GenerateJwtToken(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                email = user.email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Estado = user.Estado
            };

            var responseDto = new UserLoginResponseDto
            {
                TokenDecode = new TokenInfo
                {
                    Token = tokenString,
                    Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"]))
                },
                User = userDto
            };

            return Result<UserLoginResponseDto>.Success(responseDto, "Inicio de sesión exitoso.");
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature), // Cambia a HS512 si es necesario
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public async Task<ValidationResultToken> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid");

                if (userIdClaim != null)
                {
                    return new ValidationResultToken
                    {
                        IsValid = true,
                        UserId = int.Parse(userIdClaim.Value),
                        Message = "Token is valid."
                    };
                }
                else
                {
                    return new ValidationResultToken
                    {
                        IsValid = false,
                        Message = "Token is missing user ID."
                    };
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return new ValidationResultToken
                {
                    IsValid = false,
                    Message = "Token has expired."
                };
            }
            catch (SecurityTokenException)
            {
                return new ValidationResultToken
                {
                    IsValid = false,
                    Message = "Invalid token."
                };
            }
            catch (Exception ex)
            {
                return new ValidationResultToken
                {
                    IsValid = false,
                    Message = $"Authentication failed: {ex.Message}"
                };
            }
        }
    }
}
