using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Application.DTOs.Pedidos;
using OrderFlow.Api.Application.Services;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _pedidoService;

    public PedidosController(PedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    /// <summary>
    /// Cria um novo pedido para um cliente.
    /// </summary>
    /// <remarks>
    /// O pedido deve possuir um cliente válido e pelo menos um item.
    /// O preço, subtotal, total e baixa de estoque são calculados pelo banco de dados.
    /// </remarks>
    /// <param name="dto">Dados necessários para criação do pedido.</param>
    /// <returns>ID do pedido criado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Criar(CriarPedidoDto dto)
    {
        var id = await _pedidoService.CriarAsync(dto);

        return Created($"/api/pedidos/{id}", new { id });
    }

    /// <summary>
    /// Lista todos os pedidos cadastrados.
    /// </summary>
    /// <returns>Lista resumida de pedidos.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var pedidos = await _pedidoService.ListarAsync();

        return Ok(pedidos);
    }

    /// <summary>
    /// Obtém os detalhes de um pedido pelo ID.
    /// </summary>
    /// <param name="id">ID do pedido.</param>
    /// <returns>Pedido com seus respectivos itens.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);

        if (pedido is null)
        {
            return NotFound(new
            {
                statusCode = 404,
                mensagem = "Pedido não encontrado."
            });
        }

        return Ok(pedido);
    }

    /// <summary>
    /// Cancela um pedido existente.
    /// </summary>
    /// <remarks>
    /// Ao cancelar um pedido, os itens são devolvidos ao estoque e o status do pedido muda para Cancelado.
    /// </remarks>
    /// <param name="id">ID do pedido.</param>
    [HttpPut("{id:int}/cancelar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancelar(int id)
    {
        await _pedidoService.CancelarAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Confirma um pedido pendente.
    /// </summary>
    /// <remarks>
    /// Um pedido cancelado não pode ser confirmado.
    /// </remarks>
    /// <param name="id">ID do pedido.</param>
    [HttpPut("{id:int}/confirmar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirmar(int id)
    {
        await _pedidoService.ConfirmarAsync(id);

        return NoContent();
    }
}