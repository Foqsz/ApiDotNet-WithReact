using ApiDotNet_WithReact.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiDotNet_WithReact.Context;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    public DbSet<Aluno> Alunos { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //modelBuilder.Entity<Aluno>().HasData(
    //        new Aluno
    //        {
    //            Id = 1,
    //            Nome = "João Silva",
    //            Email = "joao.silva@example.com",
    //            Idade = 20
    //        },
    //        new Aluno
    //        {
    //            Id = 2,
    //            Nome = "Maria Oliveira",
    //            Email = "maria.oliveira@example.com",
    //            Idade = 22
    //        },
    //        new Aluno
    //        {
    //            Id = 3,
    //            Nome = "Carlos Souza",
    //            Email = "carlos.souza@example.com",
    //            Idade = 21
    //        },
    //        new Aluno
    //        {
    //            Id = 4,
    //            Nome = "Ana Pereira",
    //            Email = "ana.pereira@example.com",
    //            Idade = 23
    //        },
    //        new Aluno
    //        {
    //            Id = 5,
    //            Nome = "Pedro Lima",
    //            Email = "pedro.lima@example.com",
    //            Idade = 24
    //        }
    //    );
}

