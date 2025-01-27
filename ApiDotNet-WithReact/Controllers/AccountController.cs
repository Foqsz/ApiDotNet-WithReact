using ApiDotNet_WithReact.Services;
using ApiDotNet_WithReact.ViewModels;
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
                ModelState.AddModelError("CreateUser", "Registro inválido");
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
                ModelState.AddModelError("Login", "Login inválido");
                return BadRequest(ModelState);
            }
        }

        }
    }
}
