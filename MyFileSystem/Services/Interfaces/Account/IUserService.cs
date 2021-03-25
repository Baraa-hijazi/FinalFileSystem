using MyFileSystem.Core.DTOs;
using MyFileSystem.Core.DTOs.Account;
using System.Threading.Tasks;

namespace MyFileSystem.Services.Interfaces.Account
{
    public interface IUserService
    {
        Task<PagedResultDto<LoginDto>> GetPagedUsers(int? pageIndex, int? pageSize);
        Task<object> Login(LoginDto loginDto);
        Task<object> Register(RegisterDto signUpDto);
        Task<object> Logout();
        Task<LoginDto> GetUser(string id);
        Task<object> AssignRoles(AssignRoleDto assignRoleDto);
    }
}
