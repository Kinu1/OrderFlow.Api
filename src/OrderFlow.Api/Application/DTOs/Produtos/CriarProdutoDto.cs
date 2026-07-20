using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Api.Application.DTOs.Produtos;

public class CriarProdutoDto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "A descrição do produto deve ter no máximo 300 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço do produto deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque do produto não pode ser negativo.")]
    public int Estoque { get; set; }
}