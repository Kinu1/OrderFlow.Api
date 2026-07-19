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

    [HttpPost]
    public async Task<IActionResult> Criar(CriarPedidoDto dto)
    {
        var id = await _pedidoService.CriarAsync(dto);

        return Created($"/api/pedidos/{id}", new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var pedidos = await _pedidoService.ListarAsync();

        return Ok(pedidos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);

        if (pedido is null)
            return NotFound(new
            {
                statusCode = 404,
                mensagem = "Pedido não encontrado."

            });

        return Ok(pedido);
    }

    [HttpPut("{id:int}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        await _pedidoService.CancelarAsync(id);

        return NoContent();
    }

    [HttpPut("{id:int}/confirmar")]
    public async Task<IActionResult> Confirmar(int id)
    {
        await _pedidoService.ConfirmarAsync(id);

        return NoContent();

    }
}