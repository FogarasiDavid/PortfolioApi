using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entity
{
    public class Project
    {
        public int Id { get; set; }
        //min 1 betunek kell lennie -> nemlehet ""
        [Required(ErrorMessage = "Név requested")]
        [StringLength(100,MinimumLength =1,ErrorMessage ="A névnek muszáj lennie")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        //muszaj githuburl hogy mukodjon
        public required string GitHubUrl { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //technology kollekcioja
        public ICollection<Technology> Technologys { get; set; } = new List<Technology>();
    }

}
