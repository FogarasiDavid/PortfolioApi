using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entity;
using Portfolio.Domain.Enums; 
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Portfolio.Application.Services
{
    public class GitHubSyncService
    {
        //githubservice / projectservice injektalas
        private readonly IGitHubService _gitHubService;
        private readonly IProjectService _projectService;

        public GitHubSyncService(IGitHubService gitHubService, IProjectService projectService)
        {
            _gitHubService = gitHubService;
            _projectService = projectService;
        }

        //github által küldött json átírása új projectbe
        public async Task SyncRepositoriesAsync(string username)
        {
            var repos = await _gitHubService.GetUserRepositoriesAsync(username);
            var existingProjects = await _projectService.GetAllProjectAsync();
            foreach (var repo in repos)
            {
                //van e már ilyen project elmentve
                if (existingProjects.Any(p => p.GitHubUrl == repo.HtmlUrl))
                {
                    continue;
                }
                //repo nev-description ellenörzés-vágás
                string safeName = repo.Name ?? "Névtelen";
                if (safeName.Length > 100) safeName = safeName.Substring(0, 100);
                string safeDesc = repo.Description ?? "Nincs leírás";
                if (safeDesc.Length > 500) safeDesc = safeDesc.Substring(0, 500);

                //technologiak listája
                var Technologys = new List<Technology>();
                if (!string.IsNullOrEmpty(repo.Language))
                {
                    var existingTech = await _projectService.GetTechnologyByNameAsync(repo.Language);

                    if (existingTech != null)
                    {
                        Technologys.Add(existingTech);
                    }
                    else
                    {
                        Technologys.Add(new Technology
                        {
                            TechnologyName = repo.Language,
                        });
                    }
                    //projectbe mentés 
                    var newProject = new Project
                    {
                        Name = safeName,
                        Description = repo.Description,
                        GitHubUrl = repo.HtmlUrl ?? "https://missing-url.com",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Status = true,
                        Technologys = Technologys,
                    };

                    await _projectService.CreateProjectAsync(newProject);
                }
            }
        }
    }
}