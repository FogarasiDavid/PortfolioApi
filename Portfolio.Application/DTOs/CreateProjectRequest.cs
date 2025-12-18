using Portfolio.Domain.Entity;
using Portfolio.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Portfolio.Domain.Entity.Project;
using static Portfolio.Domain.Enums.ProjectStatus;

namespace Portfolio.Application.DTOs
{
    public class CreateProjectRequest
    {
        //uj project

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public required string GitHubUrl { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
        public List<int> TechnologyIds { get; set; } = new List<int>();

    }
}
