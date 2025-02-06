using ApiDotNet_WithReact.Models;
using ApiDotNet_WithReact.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ApiDotNet_WithReact.Controllers;

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

            if (alunos is null || !alunos.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            SetCache(CacheAlunosKey, alunos);
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
        var CacheAlunoKey = GetAlunosCacheKeyName(nome);

        if (!_memoryCache.TryGetValue(CacheAlunoKey, out IEnumerable<Aluno>? alunoByName))
        {
            alunoByName = await _alunoService.GetAlunosByName(nome);

            if (alunoByName is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Nenhum aluno(a) localizado com o nome={nome}.");
            }

            SetCache(CacheAlunoKey, alunoByName);
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
        var CacheAlunoKey = GetAlunosCacheKey(id);

        if (!_memoryCache.TryGetValue(CacheAlunoKey, out Aluno? alunoById))
        {
            alunoById = await _alunoService.GetAluno(id);

            if (alunoById is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Aluno com o id={id} não foi localizado.");
            }

            SetCache(CacheAlunoKey, alunoById);
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
        if (aluno is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Dados inválidos");
        }

        await _alunoService.CreateAluno(aluno);

        InvalidateCacheAfterChange(aluno.Id, aluno);

        return StatusCode(StatusCodes.Status201Created, aluno);
    }
    #endregion

    #region PutAluno
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Aluno>> PutAluno(int id, [FromBody] Aluno aluno)
    {
        if (aluno == null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Dados inválidos.");
        }

        if (id != aluno.Id)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Id do aluno não corresponde.");
        }

        var existingAluno = await _alunoService.GetAluno(id);
        if (existingAluno == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, $"Aluno com id={id} não foi localizado.");
        }

        await _alunoService.UpdateAluno(aluno);

        InvalidateCacheAfterChange(id, aluno);

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

        InvalidateCacheAfterChange(id);

        return StatusCode(StatusCodes.Status200OK, $"Aluno deletado com sucesso.");
    }
    #endregion

    private string GetAlunosCacheKey(int id) => $"CacheAluno_{id}";

    private string GetAlunosCacheKeyName(string nome) => $"CacheAluno_{nome}";

    private void SetCache<T>(string key, T data)
    {
        var cacheOptions = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
            SlidingExpiration = TimeSpan.FromSeconds(15),
            Priority = CacheItemPriority.High,
        };
        _memoryCache.Set(key, data, cacheOptions);
    }

    private void InvalidateCacheAfterChange(int id, Aluno? aluno = null)
    {
        _memoryCache.Remove(CacheAlunosKey);
        _memoryCache.Remove(GetAlunosCacheKey(id));

        if (aluno != null)
        {
            SetCache(GetAlunosCacheKey(id), aluno);
        }
    }
}

