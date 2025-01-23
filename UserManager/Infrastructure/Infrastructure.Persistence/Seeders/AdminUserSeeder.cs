using Core.Domain.Entities;
using Core.DomainService.Interfaces.Seeders;
using Duende.IdentityServer.Models;
using Infrastructure.Infrastructure.Persistence.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Infrastructure.Infrastructure.Persistence.Seeders
{
    public class AdminUserSeeder : ISeedData
    {

        readonly UserManager<User> _userManager;
        readonly RoleManager<IdentityRole<int>> _roleManager;
        readonly AdminUserOption _adminUser;
        public AdminUserSeeder(UserManager<User> userManager,RoleManager<IdentityRole<int>> roleManager, IOptions<AdminUserOption> options)
        {
            _userManager = userManager;
            _adminUser = options.Value;
            _roleManager = roleManager;
        }
        public async Task SeedDataAsync()
        {

            string[] roleNames = { "ADMIN", "USER" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Crear el rol y guardarlo en la base de datos
                    roleResult = await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
            var user = await _userManager.FindByEmailAsync(_adminUser.Email);
            if (user is not null)
            {
                try
                {
                    await _userManager.AddToRoleAsync(user, "ADMIN");
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return;
            }

            var appUser = new User
            {

                UserName = _adminUser.Username,
                Email = _adminUser.Email,
            };


            var createAdminUser = await _userManager.CreateAsync(appUser, _adminUser.Password);
            // Asignar el rol de administrador al usuario
            await _userManager.AddToRoleAsync(appUser, "ADMIN");

        }
    }
}
