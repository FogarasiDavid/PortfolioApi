using Portfolio.Domain.Entity;
using Portfolio.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Portfolio.Domain.Enums.ProjectStatus;

namespace Portfolio.Application.DTOs
{
    public class UpdateProjectRequest
    {
        //update project
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;

    }
}
