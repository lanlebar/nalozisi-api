global using Microsoft.EntityFrameworkCore;
global using API.Models;
global using API.Models.Main;
global using API.DTOs.Auth;
global using API.Enums;
global using API.Formats.Exceptions;
global using API.Formats.Return;
global using Microsoft.AspNetCore.Mvc;
global using System.ComponentModel.DataAnnotations;

using API.Services.AuthService;
using System.Text;
using API.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Key").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
// Authorization
builder.Services.AddAuthorization();

// Database connection
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Program services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// CORS
builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));



var app = builder.Build();

// Middleware
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NgOrigins");
app.UseHttpsRedirection();

// auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
