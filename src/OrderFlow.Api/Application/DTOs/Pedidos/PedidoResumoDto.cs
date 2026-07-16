using OrderFlow.Api.Domain.Enums;

namespace OrderFlow.Api.Application.DTOs.Pedidos;

public class PedidoResumoDto
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public string ClienteNome { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }

    public decimal Total { get; set; }

    public StatusPedido Status { get; set;}
}

