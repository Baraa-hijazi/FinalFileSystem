using AutoMapper;
using MyFileSystem.Core.Entities;
using MyFileSystem.Persistence.Contexts;
using MyFileSystem.Persistence.Repositories.Interfaces;

namespace MyFileSystem.Persistence.Repositories
{
    public class FoldersRepository : BaseRepository<Folder>, IFoldersRepository
    {
        private readonly FileSystemDbContext _context;
        private readonly IMapper _mapper;

        public FoldersRepository(FileSystemDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}