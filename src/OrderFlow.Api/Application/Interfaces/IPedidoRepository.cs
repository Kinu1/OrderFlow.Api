using OrderFlow.Api.Application.DTOs.Pedidos;
using OrderFlow.Api.Domain.Entities;

namespace OrderFlow.Api.Application.Interfaces;

public interface IPedidoRepository
{
    Task<int> CriarAsync(Pedido pedido);

    Task<List<PedidoResumoDto>> ListarAsync();

    Task<PedidoResponseDto?> ObterPorIdAsync(int id);

    Task<bool> CancelarAsync(int id);

    Task<bool> ConfirmarAsync(int id);
}