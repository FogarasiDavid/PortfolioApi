using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Models;
using Portfolio.Domain.Entity;

namespace Portfolio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthenticationController> logger;
        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModelDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Érvénytelen");
                }
                var (status, message) = await authService.Login(model);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration(RegistrationModelDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Érvénytelen");
                }
                var (status, message) = await authService.Registration(model, UserRoles.User);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(Registration), model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
