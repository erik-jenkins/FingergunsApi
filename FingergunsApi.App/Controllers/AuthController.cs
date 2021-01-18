using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FingergunsApi.App.Data.Contracts.Requests;
using FingergunsApi.App.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FingergunsApi.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordHelper _passwordHelper;

        public AuthController(ApplicationDbContext dbContext, IPasswordHelper passwordHelper)
        {
            _dbContext = dbContext;
            _passwordHelper = passwordHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email);

            if (user == null)
                return Unauthorized();

            if (!_passwordHelper.IsPasswordValid(loginRequest.Password, user.Hash, user.Salt))
                return Unauthorized();

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.NameIdentifier, $"{user.UserId}")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = loginRequest.RememberMe,
                IssuedUtc = DateTimeOffset.UtcNow,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authenticationProperties);

            return Ok();
        }
    }
}