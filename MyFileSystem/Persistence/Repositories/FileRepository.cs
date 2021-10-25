﻿using AutoMapper;
using MyFileSystem.Persistence.Contexts;
using MyFileSystem.Persistence.Repositories.Interfaces;

namespace MyFileSystem.Persistence.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileSystemDbContext _context;
        private readonly IMapper _mapper;

        public FileRepository(FileSystemDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}