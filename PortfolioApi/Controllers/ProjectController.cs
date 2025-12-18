using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Services;
using Portfolio.Domain.Entity;
using Portfolio.Infrastructure.Database;
using Portfolio.Infrastructure.Repositories;

namespace Portfolio.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        //projectservice injektálása
        private readonly IProjectService projectService;
        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        //id alapjan kereses projectet
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await projectService.GetByIdAsync(id);
            return Ok(project);
        }
        //Mindet kilistaz
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var projects = await projectService.GetAllProjectAsync();
            return Ok(projects);
        }
        //Uj project
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateProject(CreateProjectDto request)
        {
            //technologia nezese, uj listaval
            var projectTechonolgies = new List<Technology>();
            if (request.Technologies != null)
            {
                foreach (var techname in request.Technologies) 
                {
                    var existingTechnology = await projectService.GetTechnologyByNameAsync(techname);
                    if (existingTechnology != null)
                    {
                        projectTechonolgies.Add(existingTechnology);
                    }
                    else
                    {
                        projectTechonolgies.Add(new Technology { TechnologyName = techname });
                    }
                }

            }
            //uj project berakasa
            var newProject = new Project
            {
                Name = request.Name,
                GitHubUrl = request.GitHubUrl,
                Description = request.Description,
                Status = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Technologys = projectTechonolgies,
            };
            await projectService.CreateProjectAsync(newProject);
            return CreatedAtAction(nameof(GetById),new {id = newProject.Id}, newProject);
        }
        //update
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateProject(Project project, int id)
        {
            if (id != project.Id)
            {
                return BadRequest("Az URL ID és a Body ID nem egyezik!");
            }
            await projectService.UpdateProjectAsync(project);
            return Ok(project);
        }

        //torol
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        { 
            await projectService.DeleteProjectAsync(id);
            return Ok();
        }

    }
}
