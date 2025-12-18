using Portfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(int, string)> Registration(RegistrationModelDto model, string role);
        Task<(int, string)> Login(LoginModelDto model);
    }
}
