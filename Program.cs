using Longfunctie.api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Configuration section
// --------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtKey = builder.Configuration["Jwt:Key"];

// Safety checks (fail fast)
if (string.IsNullOrWhiteSpace(connectionString))
    throw new Exception("Missing DB connection string.");

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("Missing JWT key.");

// --------------------
// Services section
// --------------------
builder.Services.AddControllers();

// EF Core (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();