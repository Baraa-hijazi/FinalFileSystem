using Microsoft.AspNetCore.Identity;
using MyFileSystem.Core.DTOs.Account;
using MyFileSystem.Services.Interfaces.Account;
using System;
using System.Threading.Tasks;

namespace MyFileSystem.Services.Account
{
    public class AdministrationServices : IAdministrationServices
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationServices(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public object Get() => _roleManager.Roles;

        public async Task<object> Get(string id) => await _roleManager.FindByIdAsync(id);

        public async Task<object> CreateRole(CreateRoleDto createRoleDto)
        {
            var identityRole = new IdentityRole
            {
                Name = createRoleDto.RoleName
            };

            if (createRoleDto.RoleName != null) return await _roleManager.CreateAsync(identityRole);

            throw new Exception("Creating Role Failed! ");
        }

        public async Task<object> Put(string id, CreateRoleDto createRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null) throw new Exception("Role Doesn't Exist! ");

            role.Name = createRoleDto.RoleName;

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<object> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            
            return await _roleManager.DeleteAsync(role);
        }
    }
}