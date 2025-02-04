using ApiDotNet_WithReact.Services;
using ApiDotNet_WithReact.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiDotNet_WithReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticateService _authenticateService;

        public AccountController(IConfiguration configuration, IAuthenticateService authenticateService)
        {
            _configuration = configuration ?? 
                throw new ArgumentNullException(nameof(configuration));

            _authenticateService = authenticateService ?? 
                throw new ArgumentNullException(nameof(authenticateService));
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterModel register)
        {
            if(register.Password != register.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Senhas não conferem");
                return BadRequest(ModelState);
            }

            var result = await _authenticateService.RegisterUser(register.Email, register.Password);    

            if (result)
            {
                return Ok($"Usuário {register.Email} criado com sucesso");
            } 
            else
            {
                ModelState.AddModelError("CreateUser", "Registro inválido. Deve ter no mínimo 1 letra maiuscula.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel login)
        {
            var result = await _authenticateService.Authenticate(login.Email, login.Password);

            if (result)
            { 
                return GenerateToken(login);
            }
            else
            {
                ModelState.AddModelError("LoginUser", "Login inválido.");
                return BadRequest(ModelState);
            }
        } 

        private ActionResult<UserToken> GenerateToken(LoginModel userInfo)
        {
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuToken", "foqsToken"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(30);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
