using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Api.Application.DTOs.Pedidos;

public class CriarPedidoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "O ID do cliente deve ser maior que zero.")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "O pedido deve possuir pelo menos um item.")]
    [MinLength(1, ErrorMessage = "O pedido deve possuir pelo menos um item.")]
    public List<CriarItemPedidoDto> Itens { get; set; } = new();
}