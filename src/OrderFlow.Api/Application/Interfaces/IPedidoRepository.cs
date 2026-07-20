using OrderFlow.Api.Application.DTOs.Pedidos;
using OrderFlow.Api.Domain.Entities;
using OrderFlow.Api.Domain.Enums;
using OrderFlow.Api.Application.DTOs.Common;

namespace OrderFlow.Api.Application.Interfaces;

public interface IPedidoRepository
{
    Task<int> CriarAsync(Pedido pedido);

    Task<PagedRespondeDto<PedidoResumoDto>> ListarAsync(
    int? clienteId,
    StatusPedido? status,
    int page,
    int pageSize);

    Task<PedidoResponseDto?> ObterPorIdAsync(int id);

    Task<bool> CancelarAsync(int id);

    Task<bool> ConfirmarAsync(int id);
}