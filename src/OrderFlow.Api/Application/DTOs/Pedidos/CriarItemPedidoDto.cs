using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Api.Application.DTOs.Pedidos;

public class CriarItemPedidoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "O ID do produto deve ser maior que zero.")]
    public int ProdutoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade do item deve ser maior que zero.")]
    public int Quantidade { get; set; }
}