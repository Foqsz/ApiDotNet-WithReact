using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiDotNet_WithReact.Migrations
{
    /// <inheritdoc />
    public partial class populandoTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Alunos",
                columns: new[] { "Id", "Email", "Idade", "Nome" },
                values: new object[,]
                {
                    { 1, "joao.silva@example.com", 20, "João Silva" },
                    { 2, "maria.oliveira@example.com", 22, "Maria Oliveira" },
                    { 3, "carlos.souza@example.com", 21, "Carlos Souza" },
                    { 4, "ana.pereira@example.com", 23, "Ana Pereira" },
                    { 5, "pedro.lima@example.com", 24, "Pedro Lima" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Alunos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Alunos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Alunos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Alunos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Alunos",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
