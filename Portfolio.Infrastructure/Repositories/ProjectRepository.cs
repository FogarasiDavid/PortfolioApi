using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entity;
using Portfolio.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PortfolioDbContext _context;
        public ProjectRepository(PortfolioDbContext context)
        {
            _context = context;
        }
        //olvasas
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context
                .Project
                .Include(p => p.Technologys)
                .AsNoTracking()
                .ToListAsync();
        }   
        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Project.FirstOrDefaultAsync(p => p.Id == id);
        }

        //iras

        public async Task<int> AddAsync(Project project)
        {
            await _context.Project.AddAsync(project);
            return project.Id;
        }

        public Task UpdateAsync(Project project)
        {
            _context.Project.Update(project);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var projectDelete = await _context.Project.FindAsync(id);
            if (projectDelete != null)
            {
                _context.Project.Remove(projectDelete);
            }
        }

        public async Task<Technology?> GetTechnologyByNameAsync(string name)
        {
            return await _context.Technology
                .FirstOrDefaultAsync(t => t.TechnologyName == name);
        }


    }
}
