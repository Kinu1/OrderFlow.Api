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

    [HttpPost]
    public async Task<IActionResult> Criar(CriarProdutoDto dto)
    {
        var id = await _produtoService.CriarAsync(dto);

        return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var produtos = await _produtoService.ListarAsync();

        return Ok(produtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);

        if (produto is null)
            return NotFound(new
            {
                statusCode = 404,
                mensagem = "Produto não encontrado."

            });

        return Ok(produto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, CriarProdutoDto dto)
    {
        var atualizado = await _produtoService.AtualizarAsync(id, dto);

        if (!atualizado)
            return NotFound(new
            {
                statusCode = 404,
                mensagem = "Produto não encontrado."

            });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desativar(int id)
    {
        var desativado = await _produtoService.DesativarAsync(id);

        if (!desativado)
            return NotFound(new
            {
                statusCode = 404,
                mensagem = "Produto não encontrado."

            });

        return NoContent();
    }
}