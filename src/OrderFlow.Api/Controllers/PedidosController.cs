using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OrderFlow.Api.Application.DTOs.Pedidos;
using OrderFlow.Api.Application.Services;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _pedidoservice;

    public PedidosController(PedidoService pedidoService)
    {
        _pedidoservice = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarPedidoDto dto)
    {
        try
        {
            var id = await _pedidoservice.CriarAsync(dto);

            return Created($"/api/pedidos/{id}", new { id });
        }

        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (SqlException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}