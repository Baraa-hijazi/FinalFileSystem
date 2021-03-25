using MyFileSystem.Core.Entities;
using MyFileSystem.Persistence.Repositories;
using System.Threading.Tasks;
using MyFileSystem.Persistence.Contexts;
using MyFileSystem.Persistence.Interfaces;

namespace MyFileSystem.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FileSystemDbContext _context;
        public IBaseRepository<File> FileRepository { get; }
        public IBaseRepository<Folder> FoldersRepository { get; }
        public IBaseRepository<ApplicationUser> AccountRepository { get; }
        public UnitOfWork(FileSystemDbContext context)
        {
            FileRepository = new BaseRepository<File>(context);
            FoldersRepository = new BaseRepository<Folder>(context);
            AccountRepository = new BaseRepository<ApplicationUser>(context);
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
