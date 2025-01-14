global using FastEndpoints;
global using FluentValidation;
using Core.Domain.Entities;
using Core.DomainService.Interfaces.Repository;
using Core.DomainService.Interfaces.Services;
using FastEndpoints.Swagger;
using Infrastructure.AppService.Services;
using Infrastructure.Infrastructure.Persistence.Contexts;
using Infrastructure.Infrastructure.Persistence.Repositories;
using Infrastructure.Infrastructure.Persistence.UnitWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Config = Infrastructure.Infrastructure.Persistence.ConfigIdentityServer.Config;

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

// Configura IdentityServer
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddAspNetIdentity<User>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UnitWork>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddFastEndpoints();

builder.Services.SwaggerDocument();

builder.Services.AddControllers();


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseFastEndpoints();
app.UseSwaggerGen();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
