using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Application.DTOs.Produtos;
using OrderFlow.Api.Application.Services;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoService _produtoService;

    public ProdutosController(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    /// <summary>
    /// Cria um novo produto.
    /// </summary>
    /// <param name="dto">Dados do produto.</param>
    /// <returns>ID do produto criado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar(CriarProdutoDto dto)
    {
        var id = await _produtoService.CriarAsync(dto);

        return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
    }

    /// <summary>
    /// Lista todos os produtos ativos.
    /// </summary>
    /// <returns>Lista de produtos ativos.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var produtos = await _produtoService.ListarAsync();

        return Ok(produtos);
    }

    /// <summary>
    /// Obtém um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <returns>Dados do produto.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);

        if (produto is null)
        {
            return NotFound(new
            {
                statusCode = 404,
                mensagem = "Produto não encontrado."
            });
        }

        return Ok(produto);
    }

    /// <summary>
    /// Atualiza os dados de um produto.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <param name="dto">Novos dados do produto.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, CriarProdutoDto dto)
    {
        var atualizado = await _produtoService.AtualizarAsync(id, dto);

        if (!atualizado)
        {
            return NotFound(new
            {
                statusCode = 404,
                mensagem = "Produto não encontrado."
            });
        }

        return NoContent();
    }

    /// <summary>
    /// Desativa um produto pelo ID.
    /// </summary>
    /// <remarks>
    /// O produto não é excluído fisicamente. Ele apenas deixa de aparecer na listagem de produtos ativos.
    /// </remarks>
    /// <param name="id">ID do produto.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(int id)
    {
        var desativado = await _produtoService.DesativarAsync(id);

        if (!desativado)
        {
            return NotFound(new
            {
                statusCode = 404,
                mensagem = "Produto não encontrado."
            });
        }

        return NoContent();
    }
}