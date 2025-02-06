using ApiDotNet_WithReact.Models;
using ApiDotNet_WithReact.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ApiDotNet_WithReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AlunosController : ControllerBase
    {
        #region Fields
        private readonly IAlunoService _alunoService;
        private readonly IMemoryCache _memoryCache;
        private const string CacheAlunosKey = "cacheAlunos";
        #endregion

        #region Constructor
        public AlunosController(IAlunoService alunoService, IMemoryCache memoryCache)
        {
            _alunoService = alunoService;
            _memoryCache = memoryCache;
        }
        #endregion

        #region GetAlunos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Aluno>>> GetAlunos()
        {
            if (!_memoryCache.TryGetValue(CacheAlunosKey, out IEnumerable<Aluno>? alunos))
            {
                alunos = await _alunoService.GetAlunos();

                if (alunos is not null && alunos.Any())
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                        SlidingExpiration = TimeSpan.FromSeconds(15),
                        Priority = CacheItemPriority.High,
                    };
                    _memoryCache.Set(CacheAlunosKey, alunos, cacheOptions);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            return StatusCode(StatusCodes.Status200OK, alunos);
        }
        #endregion

        #region GetAlunoByName
        [HttpGet("ByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Aluno>>> GetAlunoByName(string nome)
        {
            var CacheAlunoKey = $"CacheAluno_{nome}";

            if (!_memoryCache.TryGetValue(CacheAlunoKey, out IEnumerable<Aluno>? alunoByName))
            {
                alunoByName = await _alunoService.GetAlunosByName(nome);

                if (alunoByName is not null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                        SlidingExpiration = TimeSpan.FromSeconds(15),
                        Priority = CacheItemPriority.High,
                    };
                    _memoryCache.Set(CacheAlunoKey, alunoByName, cacheOptions); 
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Nenhum aluno(a) localizado com o nome={nome}.");
                }
            }  
            return StatusCode(StatusCodes.Status200OK, alunoByName);
        }
        #endregion

        #region GetAlunoById
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Aluno>> GetAlunoById(int id)
        {
            var CacheAlunoKey = $"CacheAluno_{id}";

            if (!_memoryCache.TryGetValue(CacheAlunoKey, out Aluno? alunoById))
            {
                alunoById = await _alunoService.GetAluno(id);

                if (alunoById is not null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                        SlidingExpiration = TimeSpan.FromSeconds(15),
                        Priority = CacheItemPriority.High,
                    };
                    _memoryCache.Set(CacheAlunoKey, alunoById, cacheOptions);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Aluno com o id={id} não foi localizado.");
                }
            }
            return StatusCode(StatusCodes.Status200OK, alunoById);
        }
        #endregion

        #region CreateAluno
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

            _memoryCache.Remove(CacheAlunosKey);



            return StatusCode(StatusCodes.Status201Created, aluno);
        }
        #endregion

        #region PutAluno
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
        #endregion

        #region DeleteAluno
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
        #endregion
    }
}
