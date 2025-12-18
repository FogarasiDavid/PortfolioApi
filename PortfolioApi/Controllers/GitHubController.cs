using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services; 
using System.Threading.Tasks;

namespace Portfolio.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User")]
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        //service injektalas
        private readonly GitHubSyncService _gitHubSyncService;

        public GitHubController(GitHubSyncService gitHubSyncService)
        {
            _gitHubSyncService = gitHubSyncService;
        }
        
        //service hasznalata, kiiras
        [HttpPost("sync/{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Sync(string username)
        {
            await _gitHubSyncService.SyncRepositoriesAsync(username);

            return Ok("Szinkronizáció kész!");
        }
    }
}