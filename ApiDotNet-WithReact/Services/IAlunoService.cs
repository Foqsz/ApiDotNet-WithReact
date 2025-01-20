using ApiDotNet_WithReact.Models;

namespace ApiDotNet_WithReact.Services;

public interface IAlunoService
{
    //Task = operação assincrona. 
    Task<IEnumerable<Aluno>> GetAlunos();
    Task<Aluno> GetAluno(int id);
    Task<IEnumerable<Aluno>> GetAlunosByName(string nome);
    Task CreateAluno(Aluno aluno);
    Task UpdateAluno(Aluno aluno);
    Task DeleteAluno(Aluno aluno);
}
