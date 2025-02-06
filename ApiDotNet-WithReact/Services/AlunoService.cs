using ApiDotNet_WithReact.Context;
using ApiDotNet_WithReact.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDotNet_WithReact.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            try
            {
                //delay de 3s
                //é assincrono e bloqueia a thread
                System.Threading.Thread.Sleep(3000);
                return await _context.Alunos.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Aluno>> GetAlunosByName(string nome)
        {
            System.Threading.Thread.Sleep(3000);
            IEnumerable<Aluno> alunos;
            if (!string.IsNullOrEmpty(nome))
            {
                alunos = await _context.Alunos.Where(a => a.Nome.Contains(nome)).ToListAsync();
            }
            else
            {
                alunos = await GetAlunos();
            }
            return alunos;
        }

        public async Task<Aluno> GetAluno(int id)
        {
            System.Threading.Thread.Sleep(3000);
            var alunos = await _context.Alunos.FindAsync(id);
            return alunos;
        }

        public async Task CreateAluno(Aluno aluno)
        {
            await CheckEmailExist(aluno);

            await _context.Alunos.AddAsync(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAluno(Aluno aluno)
        {
            await CheckEmailExist(aluno);

            var existingAluno = await _context.Alunos.FindAsync(aluno.Id);
            if (existingAluno != null)
            {
                _context.Entry(existingAluno).CurrentValues.SetValues(aluno);
            }

            await _context.SaveChangesAsync();
        } 

        public async Task DeleteAluno(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
        }

        private async Task CheckEmailExist(Aluno aluno)
        {
            var alunoExiste = await _context.Alunos.FirstOrDefaultAsync(a => a.Email == aluno.Email && a.Id != aluno.Id);

            if (alunoExiste != null)
            {
                throw new Exception("E-mail já cadastrado.");
            }
        }

    }
}
