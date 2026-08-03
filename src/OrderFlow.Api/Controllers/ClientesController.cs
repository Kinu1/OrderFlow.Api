using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Application.DTOs.Clientes;
using OrderFlow.Api.Application.Services;
using OrderFlow.Api.Application.DTOs.Common;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClientesController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    /// <summary>
    /// Cria um novo cliente.
    /// </summary>
    /// <param name="dto">Dados do cliente.</param>
    /// <returns>ID do cliente criado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar(CriarClienteDto dto)
    {
        var id = await _clienteService.CriarAsync(dto);

        return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
    }

    /// <summary>
    /// Lista todos os clientes cadastrados.
    /// </summary>
    /// <returns>Lista de clientes.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var clientes = await _clienteService.ListarAsync();

        return Ok(clientes);
    }

    /// <summary>
    /// Obtém um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente.</param>
    /// <returns>Dados do cliente.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var cliente = await _clienteService.ObterPorIdAsync(id);

        if (cliente is null)
        {
            return NotFound(new ErroResponseDto
            {
                StatusCode = 404,
                Mensagem = "Cliente não encontrado."
            });
        }

        return Ok(cliente);
    }

    /// <summary>
    /// Atualiza os dados de um cliente.
    /// </summary>
    /// <param name="id">ID do cliente.</param>
    /// <param name="dto">Novos dados do cliente.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, AtualizarClienteDto dto)
    {
        var atualizado = await _clienteService.AtualizarAsync(id, dto);

        if (!atualizado)
        {
            return NotFound(new ErroResponseDto
            {
                StatusCode = 404,
                Mensagem = "Cliente não encontrado."
            });
        }

        return NoContent();
    }

    /// <summary>
    /// Exclui um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int id)
    {
        var excluido = await _clienteService.ExcluirAsync(id);

        if (!excluido)
        {
            return NotFound(new ErroResponseDto
            {
                StatusCode = 404,
                Mensagem = "Cliente não encontrado."
            });
        }

        return NoContent();
    }
}