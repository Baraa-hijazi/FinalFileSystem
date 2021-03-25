using MyFileSystem.Core.DTOs.Account;
using System.Threading.Tasks;

namespace MyFileSystem.Services.Interfaces.Account
{
    public interface IAdministrationServices
    {
        Task<object> CreateRole(CreateRoleDto createRoleDto);
        Task<object> Delete(string id);
        object Get();
        Task<object> Get(string id);
        Task<object> Put(string id, CreateRoleDto createRoleDto);
    }
}
