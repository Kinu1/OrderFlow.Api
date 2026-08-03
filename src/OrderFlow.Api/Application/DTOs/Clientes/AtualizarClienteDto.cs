using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Api.Application.DTOs.Clientes;

public class AtualizarClienteDto
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do cliente deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail informado é inválido.")]
    [StringLength(150 , ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [StringLength(20 , ErrorMessage = "O telefone deve ter no máximo 20 carecteres.")]
    public string Telefone { get; set; } = string.Empty;
}