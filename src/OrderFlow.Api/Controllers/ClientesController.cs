using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Application.DTOs.Clientes;
using OrderFlow.Api.Application.Services;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClientesController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarClienteDto dto)
    {
        try
        {
            var id = await _clienteService.CriarAsync(dto);

            return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }


    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var clientes = await _clienteService.ListarAsync();

        return Ok(clientes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        try
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);

            if (cliente is null)
                return NotFound(new { mensagem = "Cliente não encontrado" });

            return Ok(cliente);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, CriarClienteDto dto)
    {
        try
        {
            var atualizado = await _clienteService.AtualizarAsync(id, dto);

            if (!atualizado)
                return NotFound(new { message = "Cliente não encontrado." });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("(id:int)")]
    public async Task<IActionResult> Excluir(int id)
    {
        try
        {
            var excluido = await _clienteService.ExcluirAsync(id);

            if (!excluido)
                return NotFound(new { mensagem = "Cliente não encontrado" });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }

    }
}