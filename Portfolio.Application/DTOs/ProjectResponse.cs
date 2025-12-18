using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.DTOs
{
    public class ProjectResponse
    {
        //mit ad vissza az api

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string GitHubUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<string> Technologies { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }
}
