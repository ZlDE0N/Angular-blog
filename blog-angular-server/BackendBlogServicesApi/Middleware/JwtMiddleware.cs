using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string[] _rutasAutenticacion;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IOptions<RutasConfiguracion> opciones)
    {
        _next = next;
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
        _rutasAutenticacion = opciones.Value.RutasAutenticacion;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }

        //if (IsAuthenticationPath(context.Request.Path))
        //{
        //    await _next(context);
        //    return;
        //}

        if (EsRutaAutenticacion(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var token = GetTokenFromHeader(context);
        if (string.IsNullOrEmpty(token))
        {
            await RespondWithUnauthorized(context, "No token provided.");
            return;
        }

        await AttachUserToContext(context, token);
        await _next(context);
    }
    private bool EsRutaAutenticacion(string path)
    {
        return _rutasAutenticacion.Contains(path, StringComparer.OrdinalIgnoreCase);
    }

    private bool IsAuthenticationPath(string path)
    {
        return path.Equals("/api/auth/login", StringComparison.OrdinalIgnoreCase) ||
               path.Equals("/api/auth/register", StringComparison.OrdinalIgnoreCase) ||
               path.Equals("/api/auth/validate-token", StringComparison.OrdinalIgnoreCase) ||
                path.Equals("/", StringComparison.OrdinalIgnoreCase);
    }

    private string GetTokenFromHeader(HttpContext context)
    {
        return context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    }

    private async Task AttachUserToContext(HttpContext context, string token)
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
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x =>x.Type == "nameid");
            if (userIdClaim == null)
            {
                await RespondWithUnauthorized(context, "Token does not contain a valid user identifier.");
            }

            var userId = int.Parse(userIdClaim.Value);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<UserService>();
                context.Items["User"] = await userService.GetByIdAsync(userId);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            await RespondWithUnauthorized(context, "Token has expired.");
        }
        catch (SecurityTokenException)
        {
            await RespondWithUnauthorized(context, "Invalid token.");
        }
        catch (Exception ex)
        {
            await RespondWithUnauthorized(context, $"Authentication failed: {ex.Message}");
        }
    }

    private async Task RespondWithUnauthorized(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var responseObj = new
        {
            status = false,
            httpCode = 401,
            message = $"Unauthorized: {message}"
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(responseObj);
        await context.Response.WriteAsync(jsonResponse);
    }

}
