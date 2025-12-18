using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Application.Models
{
    public class LoginModelDto
    {
        [Required(ErrorMessage = "Username kötelező")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Jelszó kötelező")]
        public string? Password { get; set; }
    }
}