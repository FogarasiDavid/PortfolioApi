using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Models;
using Portfolio.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portfolio.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        // A konfigurációt (appsettings.json) itt injektáljuk be
        public AuthService(IConfiguration configuration, RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<(int,string)> Registration(RegistrationModelDto model,string role)
        {
            var userExist = await userManager.FindByNameAsync(model.Username);
            if(userExist != null)
            {
                return (0, "User already exist");
            }
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            var createUserResult = await userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
            {
                return (0, "A regisztrálás létrehozása sikertelen volt!");
            }
            if(!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
            await userManager.AddToRoleAsync(user, role);
            return (0, "Regisztráció sikeres!");
        }
        public async Task<(int, string)> Login(LoginModelDto model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return (0, "Nem létező Username");
            if (!await userManager.CheckPasswordAsync(user, model.Password))
                return (0, "Hibás jelszó");

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            { 
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);
            return (1, token);
        }
            

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            // A kulcsot byte tömbbé alakítjuk
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // Az érvényességi időt kiolvassuk (ha nincs a configban, alapértelmezett 3 órát adunk)
            _ = long.TryParse(_configuration["JWT:TokenExpiryTimeInHour"], out long tokenExpiryTimeInHour);
            if (tokenExpiryTimeInHour == 0) tokenExpiryTimeInHour = 3;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                // A lejárati idő beállítása
                Expires = DateTime.UtcNow.AddHours(tokenExpiryTimeInHour),
                // A titkosítási algoritmus és a kulcs megadása
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                // A felhasználó adatai (claim-ek)
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}