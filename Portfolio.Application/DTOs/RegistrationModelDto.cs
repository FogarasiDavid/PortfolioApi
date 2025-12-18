using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Application.Models
    {
     public class RegistrationModelDto
     {
         [Required]
         public string? Username { get; set; }
         [Required]
         public string? Email { get; set; }
         [Required]
         public string? Password { get; set; }
         public string? FirstName { get; set; }
         public string? LastName { get; set; }
     }
}