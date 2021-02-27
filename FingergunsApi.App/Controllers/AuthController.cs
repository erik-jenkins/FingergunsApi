using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FingergunsApi.App.Data.Contracts.Requests;
using FingergunsApi.App.Data.Models;
using FingergunsApi.App.Data.Repositories;
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
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;

        public AuthController(IUserRepository userRepository, IPasswordHelper passwordHelper)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var isEmailTaken = await _userRepository.IsEmailTakenAsync(registerRequest.Email);
            var isDisplayNameTaken = await _userRepository.IsDisplayNameTakenAsync(registerRequest.DisplayName);

            if (registerRequest.Confirm != registerRequest.Password)
                return BadRequest("Password confirmation must match the entered password.");

            if (isEmailTaken)
                return Conflict("Email is already in use.");

            if (isDisplayNameTaken)
                return Conflict("Display name is already in use.");

            var (hash, salt) = _passwordHelper.HashPassword(registerRequest.Password);

            var user = new User
            {
                Email = registerRequest.Email,
                DisplayName = registerRequest.DisplayName,
                Hash = hash,
                Salt = salt
            };

            await _userRepository.InsertUserAsync(user);
            await _userRepository.Save();

            return Ok();
        }
    }
}