﻿// <auto-generated />
using ApiDotNet_WithReact.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiDotNet_WithReact.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250120132647_populandoTabela")]
    partial class populandoTabela
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ApiDotNet_WithReact.Models.Aluno", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("Idade")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Alunos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "joao.silva@example.com",
                            Idade = 20,
                            Nome = "João Silva"
                        },
                        new
                        {
                            Id = 2,
                            Email = "maria.oliveira@example.com",
                            Idade = 22,
                            Nome = "Maria Oliveira"
                        },
                        new
                        {
                            Id = 3,
                            Email = "carlos.souza@example.com",
                            Idade = 21,
                            Nome = "Carlos Souza"
                        },
                        new
                        {
                            Id = 4,
                            Email = "ana.pereira@example.com",
                            Idade = 23,
                            Nome = "Ana Pereira"
                        },
                        new
                        {
                            Id = 5,
                            Email = "pedro.lima@example.com",
                            Idade = 24,
                            Nome = "Pedro Lima"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
