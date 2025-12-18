using Portfolio.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Interfaces
{
    public interface IProjectService
    {
        //service interfacei
        public Task<IEnumerable<Project>> GetAllProjectAsync();
        public Task<Project?> GetByIdAsync(int id);
        public Task CreateProjectAsync(Project newProject);
        public Task UpdateProjectAsync(Project project);
        public Task DeleteProjectAsync(int id);
        Task<Technology?> GetTechnologyByNameAsync(string name);
    }
}
