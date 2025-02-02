using System.ComponentModel.DataAnnotations;

namespace ApiDotNet_WithReact.ViewModels;

public class LoginModel
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(20, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres", MinimumLength = 3)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
