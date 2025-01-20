using System.ComponentModel.DataAnnotations;

namespace ApiDotNet_WithReact.Models;

public class Aluno
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(80, ErrorMessage = "Nome com até 80 caracteres.")]
    public string Nome { get; set; }
    [Required]
    [EmailAddress]
    [StringLength(80, ErrorMessage = "E-mail com até 80 caracteres.")]
    public string Email { get; set; }
    [Required]
    public int Idade { get; set; }
}
