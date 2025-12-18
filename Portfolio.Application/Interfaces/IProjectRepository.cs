using Nest;
using Portfolio.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Interfaces
{
   public interface IProjectRepository 
   {
        //CRUD interfacek repositorynak a projecthez
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<int> AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(int id);
        Task <Technology?> GetTechnologyByNameAsync(string name);
   }
}
