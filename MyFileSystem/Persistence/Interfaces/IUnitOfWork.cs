using System.Threading.Tasks;
using MyFileSystem.Core.Entities;

namespace MyFileSystem.Persistence.Interfaces
{
    public interface IUnitOfWork
    {
        IBaseRepository<File> FileRepository { get; }
        IBaseRepository<Folder> FoldersRepository { get; }
        IBaseRepository<ApplicationUser> AccountRepository { get; }
        Task CompleteAsync();
    }
}