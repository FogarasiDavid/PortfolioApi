using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portfolio.Domain.Exceptions;
using System.Threading.Tasks;

namespace Portfolio.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //olvasas
        public async Task<IEnumerable<Project>> GetAllProjectAsync()
        {
            //elmenti unitofwork-ön keresztül
            return await _unitOfWork.ProjectRepository.GetAllAsync();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            //expectation kuldése
            if (project == null)
            {
                throw new NotFoundException($"Nem található projekt ezzel az ID-val: {id}");
            }
            return project;
        }


        //iras
        public async Task CreateProjectAsync(Project newProject)
        {
            await _unitOfWork.ProjectRepository.AddAsync(newProject);
            await _unitOfWork.CommitAsync();
        }
        public async Task UpdateProjectAsync(Project project)
        {
            await _unitOfWork.ProjectRepository.UpdateAsync(project);
            await _unitOfWork.CommitAsync();
        }
        public async Task DeleteProjectAsync(int id)
        {
            await _unitOfWork.ProjectRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Technology?> GetTechnologyByNameAsync(string name)
        {
            return await _unitOfWork.ProjectRepository.GetTechnologyByNameAsync(name);
        }

    }
}
