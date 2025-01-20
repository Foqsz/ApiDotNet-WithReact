using ApiDotNet_WithReact.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDotNet_WithReact.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Aluno> Alunos { get; set; }
}                             