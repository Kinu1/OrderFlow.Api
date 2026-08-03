using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Api.Application.DTOs.Produtos;

public class AtualizarProdutoDto
{

    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(300)]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue)]
    public int Estoque { get; set; }
}