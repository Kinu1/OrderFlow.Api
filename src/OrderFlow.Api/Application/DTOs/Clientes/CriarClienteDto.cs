using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Api.Application.DTOs.Clientes;

public class CriarClienteDto
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do cliente deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail do cliente é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
    [StringLength(150, ErrorMessage = "O e-mail do cliente deve ter no máximo 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "O telefone do cliente deve ter no máximo 20 caracteres.")]
    public string Telefone { get; set; } = string.Empty;
}