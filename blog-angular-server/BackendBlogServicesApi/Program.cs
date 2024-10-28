using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.Repositories.Interfaces;
using BackendBlogServicesApi.Repositories;
using BackendBlogServicesApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Configuración de URLs
builder.WebHost.UseUrls("https://localhost:44358/", "https://localhost:5001", "https://localhost:5002");
builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.WithOrigins("https://localhost:4200", "http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
    );
});

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend Blog Services API", Version = "v1" });

    // Añadir el esquema de autenticación JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer {token}' (without quotes) in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    // Use the custom operation filter to apply JWT security
    c.OperationFilter<AddAuthTokenHeaderOperationFilter>();
});

// Configuración de Entity Framework
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Repositories y Services
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped<IEntriesBlogRepository, EntriesBlogRepository>();
builder.Services.AddScoped<EntriesBlogService>();

builder.Services.AddScoped<IEntriesBlogCategoryRepository, EntriesBlogCategoryRepository>();
builder.Services.AddScoped<EntriesBlogCategoryService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();

builder.Services.Configure<RutasConfiguracion>(builder.Configuration.GetSection("RutasConfiguracion"));

// Configuración de autenticación
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

//// Configurar el pipeline de HTTP
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Blog Services API v1"));
//}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseMiddleware<JwtMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

public class AddAuthTokenHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if the action has the [AllowAnonymous] attribute
        var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

        // Exclude routes that start with 'api/auth'
        var isAuthRoute = context.ApiDescription.RelativePath.StartsWith("api/auth", StringComparison.OrdinalIgnoreCase);

        // Only apply the security requirement if the action does NOT have the AllowAnonymous attribute
        // and is NOT an 'api/auth' route
        if (!hasAllowAnonymous && !isAuthRoute)
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } }
                }
            };
        }
    }
}
