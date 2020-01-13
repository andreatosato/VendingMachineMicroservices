﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VendingMachine.Service.Authentications.API.Configuration;
using VendingMachine.Service.Authentications.API.Data.Models;
using VendingMachine.Service.Authentications.API.ViewModels;

namespace VendingMachine.Service.Authentications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly JwtSettings jwtSettings;

        public AuthController(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher, IOptions<JwtSettings> jwtSettings)
        {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Sign-up a new user
        /// </summary>
        /// <param name="model">The information about the registered user</param>
        /// <response code="200">Registration completed successfully</response>
        /// <response code="400">Unable to register the news user because of an error of input data</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Register(RegisterRequest model)
        {
            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Perform a login
        /// </summary>
        /// <param name="model">Login information</param>
        /// <returns>An object containing the Authentication Bearer Token</returns>
        /// <response code="200">Login completed successfully</response>
        /// <response code="400">Unable to perform login because of an error of input data</response>
        /// <response code="401">Invalid passsword</response>
        [HttpPost("Token")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AuthResponse>> CreateToken(LoginRequest model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest();
            }

            if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
            {
                var userClaims = await userManager.GetClaimsAsync(user);
                var userRoles = await userManager.GetRolesAsync(user);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName ?? string.Empty)
                }
                .Union(userRoles.Select(role => new Claim(ClaimTypes.Role, role)))
                .Union(userClaims);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
                    signingCredentials: signingCredentials
                    );

                var result = new AuthResponse(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo);
                return Ok(result);
            }

            return Unauthorized();
        }
    }
}