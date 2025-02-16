using BlogSystem.Core;
using BlogSystem.Repository.Data;
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
        public UnitOfWork(BlogDbContext context)
        {
            _context = context;
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
    }
}
