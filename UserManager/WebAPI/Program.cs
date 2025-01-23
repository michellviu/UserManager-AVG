global using FastEndpoints;
global using FluentValidation;
using Core.Domain.Entities;
using Core.DomainService.Interfaces.Repository;
using Core.DomainService.Interfaces.Seeders;
using Core.DomainService.Interfaces.Services;
using FastEndpoints.Swagger;
using Infrastructure.AppService.Services;
using Infrastructure.HostedService;
using Infrastructure.Infrastructure.Persistence.Contexts;
using Infrastructure.Infrastructure.Persistence.Options;
using Infrastructure.Infrastructure.Persistence.Repositories;
using Infrastructure.Infrastructure.Persistence.Seeders;
using Infrastructure.Infrastructure.Persistence.UnitWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure the database context
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

builder.Services.Configure<AdminUserOption>(builder.Configuration.GetSection("AdminUserOption"));
//builder.Services.Configure<RolOption>(builder.Configuration.GetSection("RolOption"));

builder.Services.AddScoped<ISeedData, AdminUserSeeder>();
builder.Services.AddHostedService<MigratorHostedService>();

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
