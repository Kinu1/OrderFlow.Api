using OrderFlow.Api.Domain;

namespace OrderFlow.Api.Application.DTOs.Pedidos;

public class PedidoRespondeDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Total { get; set; }
    public StatusPedido Status { get; set; }
    public List<ItemPedidoResponseDto> Itens { get; set; } = new();

}