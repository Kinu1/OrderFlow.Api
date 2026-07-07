using OrderFlow.Api.Application.DTOs.Pedidos;
using OrderFlow.Api.Domain.Entities;

namespace OrderFlow.Api.Application.Interfaces;

public interface IPedidoRepository
{
    Task<int> CriarAsync(Pedido pedido);

    Task<PedidoResponseDto?> ObterPorIdAsync(int id);
}