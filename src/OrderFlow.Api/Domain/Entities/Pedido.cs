namespace OrderFlow.Api.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public DateTime DateTime { get; set; }

    public decimal Total { get; set; }

    public StatusPedido Status { get; set; }

    public List<ItemPedido> Itens { get; set; } = new();
}