using Portfolio.Application.Interfaces;
using Portfolio.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PortfolioDbContext _context;

        public IProjectRepository ProjectRepository { get; private set; }
        public UnitOfWork(PortfolioDbContext context) {
            _context = context; 
            ProjectRepository = new ProjectRepository(_context);
        }

        //elmentés az adatbázisba
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
