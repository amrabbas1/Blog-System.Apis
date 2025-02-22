using BlogSystem.Core;
using BlogSystem.Core.Repositories.Interfaces;
using BlogSystem.Repository.Data;
using BlogSystem.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogDbContext _context;
        private Hashtable _repositories;
        public UnitOfWork(BlogDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message;
                throw new Exception($"Database update failed: {innerException}", ex);
            }
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRepository<TEntity>;
        }
    }
}
