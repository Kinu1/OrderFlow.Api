namespace OrderFlow.Api.Application.DTOs.Pedidos;

public class CriarPedidoDto
{
    public int ClienteId { get; set; }
    public List<CriarItemPedidoDto> Itens { get; set; } = new();
}