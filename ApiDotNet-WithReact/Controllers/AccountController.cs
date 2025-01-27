using ApiDotNet_WithReact.Services;
using ApiDotNet_WithReact.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
