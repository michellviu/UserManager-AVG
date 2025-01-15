global using FastEndpoints;
global using FluentValidation;
using Core.Domain.Entities;
using Core.DomainService.Interfaces.Repository;
using Core.DomainService.Interfaces.Services;
using Duende.IdentityServer.Models;
using FastEndpoints.Swagger;
using Infrastructure.AppService.Services;
using Infrastructure.Infrastructure.Persistence.Contexts;
using Infrastructure.Infrastructure.Persistence.Repositories;
using Infrastructure.Infrastructure.Persistence.UnitWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configura el contexto de la base de datos para usar MySQL
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireNonAlphanumeric = false;

})
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UnitWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();


builder.Services.AddFastEndpoints();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SigningKey"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.SwaggerDocument();

builder.Services.AddControllers();


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Crear roles y usuarios por defecto
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    string[] roleNames = { "Admin", "User" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Crear el rol y guardarlo en la base de datos
            roleResult = await roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }
    }

    // Crear un usuario administrador por defecto
    var adminUser = new User
    {
        UserName = "admin",
        Email = "admin@admin.com"
    };
    string adminPassword = "Admin@123";
    var user = await userManager.FindByEmailAsync("admin@admin.com");

    if (user == null)
    {
        var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
        if (createAdminUser.Succeeded)
        {
            // Asignar el rol de administrador al usuario
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseFastEndpoints();
app.UseSwaggerGen();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
