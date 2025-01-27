using ApiDotNet_WithReact.Models;
using ApiDotNet_WithReact.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDotNet_WithReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Aluno>>> GetAlunos()
        {
            var alunos = await _alunoService.GetAlunos();

            if (alunos == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status200OK, alunos);
        }

        [HttpGet("ByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Aluno>>> GetAlunoByName(string nome)
        {
            var alunoByName = await _alunoService.GetAlunosByName(nome);

            if (alunoByName == null || !alunoByName.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Nenhum aluno(a) localizado com o nome={nome}.");
            }

            return StatusCode(StatusCodes.Status200OK, alunoByName);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Aluno>> GetAlunoById(int id)
        {
            var alunoById = await _alunoService.GetAluno(id);

            if (alunoById == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Aluno com o id={id} não foi localizado.");
            }

            return StatusCode(StatusCodes.Status200OK, alunoById);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Aluno>> CreateAluno(Aluno aluno)
        {
            if (aluno == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            await _alunoService.CreateAluno(aluno);
            return StatusCode(StatusCodes.Status201Created, aluno);
        }

        // O atributo [FromBody] indica que o parâmetro 'aluno' deve ser lido do corpo da requisição HTTP.
        // Isso é útil quando os dados são enviados no formato JSON
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Aluno>> PutAluno(int id, [FromBody] Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Id do aluno não corresponde.");
            }

            if (aluno == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Não pode ser nulo.");
            }

            await _alunoService.UpdateAluno(aluno);

            return StatusCode(StatusCodes.Status200OK, aluno);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Aluno>> DeleteAluno(int id)
        {
            var buscarAluno = await _alunoService.GetAluno(id);

            if (buscarAluno == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Aluno com id= {id} não localizado.");
            }

            await _alunoService.DeleteAluno(buscarAluno);
            return StatusCode(StatusCodes.Status200OK, "Aluno deletado com sucesso.");
        }
    }
}
