using Microsoft.AspNetCore.Identity;
using MyFileSystem.Core.DTOs.Account;
using MyFileSystem.Services.Interfaces.Account;
using System;
using System.Threading.Tasks;

namespace MyFileSystem.Services.Account
{
    public class AdminstrationServices : IAdministrationServices
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminstrationServices(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public object Get()
        {
            var Roles = _roleManager.Roles;
            return Roles;
        }

        public async Task<object> Get(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new Exception("Role Doesn't Exist! ");
            return role;
        }

        public async Task<object> CreateRole(CreateRoleDto createRoleDto)
        {
            if (createRoleDto.RoleName != null)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRoleDto.RoleName
                };
                var Result = await _roleManager.CreateAsync(identityRole);
                return Result;
            }
            else throw new Exception("Creating Role Failed! ");
        }

        public async Task<object> Put(string id, CreateRoleDto createRoleDto)
        {
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role == null)
                throw new Exception("Role Doesn't Exist! ");

            Role.Name = createRoleDto.RoleName;
            var result = await _roleManager.UpdateAsync(Role);
            return result;
        }

        public async Task<object> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);
            return result;
        }
    }
}