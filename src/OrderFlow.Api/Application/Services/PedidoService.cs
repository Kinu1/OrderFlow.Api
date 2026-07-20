using OrderFlow.Api.Application.DTOs.Common;
using OrderFlow.Api.Application.DTOs.Pedidos;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Domain.Entities;
using OrderFlow.Api.Domain.Enums;

namespace OrderFlow.Api.Application.Services;

public class PedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<PagedRespondeDto<PedidoResumoDto>> ListarAsync(
    int? clienteId,
    StatusPedido? status,
    int page,
    int pageSize)
    {
        if (clienteId.HasValue && clienteId.Value <= 0)
            throw new ArgumentException("O ID do cliente deve ser maior que zero.");

        if (page <= 0)
            throw new ArgumentException("A página deve ser maior que zero.");

        if (pageSize <= 0)
            throw new ArgumentException("O tamanho da página deve ser maior que zero.");

        if (pageSize > 100)
            throw new ArgumentException("O tamanho da página não pode ser maior que 100.");

        return await _pedidoRepository.ListarAsync(
            clienteId,
            status,
            page,
            pageSize
        );

    }

    public async Task<int> CriarAsync(CriarPedidoDto dto)
    {
        if (dto.ClienteId <= 0)
            throw new ArgumentException("O ID do cliente deve ser maior que zero.");

        if (dto.Itens is null || dto.Itens.Count == 0)
            throw new ArgumentException("O pedido deve possuir pelo menos um item.");

        if (dto.Itens.Any(item => item.ProdutoId <= 0))
            throw new ArgumentException("Todos os produtos devem possuir ID válido.");

        if (dto.Itens.Any(item => item.Quantidade <= 0))
            throw new ArgumentException("Todos os itens devem possuir quantidade maior que zero.");

        var possuiProdutoDuplicado = dto.Itens
            .GroupBy(item => item.ProdutoId)
            .Any(grupo => grupo.Count() > 1);

        if (possuiProdutoDuplicado)
            throw new ArgumentException("O mesmo produto não pode aparecer mais de uma vez no pedido.");

        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            Itens = dto.Itens.Select(item => new ItemPedido
            {
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade
            }).ToList()
        };

        return await _pedidoRepository.CriarAsync(pedido);
    }

    public async Task<PedidoResponseDto?> ObterPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do pedido deve ser maior que zero.");

        return await _pedidoRepository.ObterPorIdAsync(id);
    }

    public async Task<bool> CancelarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do pedido deve ser maior que zero.");

        return await _pedidoRepository.CancelarAsync(id);
    }

    public async Task<bool> ConfirmarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do pedido deve ser maior que zero.");

        return await _pedidoRepository.ConfirmarAsync(id);

    }  
}