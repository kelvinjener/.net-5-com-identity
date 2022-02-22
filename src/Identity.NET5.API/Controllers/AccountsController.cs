using Identity.NET5.Business.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.NET5.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;

        public AccountsController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!User.IsInRole("ADMIN"))
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"Usuário logado não tem permissão para executar esta ação. Contate o administrador do sistema." });

            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Usuário já existe!" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.ToList().Select(s => s.Description));
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"Falha ao criar usuário! Por favor verifique as informações e tente novamente. Erro(s): {errors}" });
            }

            return Ok(new Response { Status = "Success", Message = "Usuário criado com Sucesso!" });
        }

        [Authorize]
        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!User.IsInRole("ADMIN"))
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"Usuário logado não tem permissão para executar esta ação. Contate o administrador do sistema." });

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Usuário não localizado!" });

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, code, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.ToList().Select(s => s.Description));
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"Não foi possível realizar a troca da senha. Erro(s): {errors}" });
            }

            return Ok(new Response { Status = "Success", Message = "Senha alterada com Sucesso!" });
        }

        [Authorize]
        [HttpPost]
        [Route("revokeaccess")]
        public async Task<IActionResult> RevokeAccess([FromBody] RevokeAccessModel model)
        {
            if (!User.IsInRole("ADMIN"))
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"Usuário logado não tem permissão para executar esta ação. Contate o administrador do sistema." });

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Usuário não localizado!" });

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, code, "N0Ac*ES2");

            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.ToList().Select(s => s.Description));
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"Não foi possível realizar a revogação de acesso deste usuário. Erro(s): {errors}" });
            }

            return Ok(new Response { Status = "Success", Message = "Acesso do usuário revogado com Sucesso!" });
        }
    }
}
