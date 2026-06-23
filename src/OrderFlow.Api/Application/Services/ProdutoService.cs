using OrderFlow.Api.Application.DTOs.Produtos;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Domain.Entities;

namespace OrderFlow.Api.Application.Services;

public class ProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<int> CriarAsync(CriarProdutoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("O nome do produto é obrigatório.");

        if (dto.Preco <= 0)
            throw new ArgumentException("O preço do produto deve ser maior que zero.");

        if (dto.Estoque < 0)
            throw new ArgumentException("O estoque do produto não pode ser negativo.");

        var produto = new Produto
        {
            Nome = dto.Nome.Trim(),
            Descricao = dto.Descricao.Trim() ?? string.Empty,
            Preco = dto.Preco,
            Estoque = dto.Estoque,
            Ativo = true
        };

        return await _produtoRepository.CriarAsync(produto);
    }

    public async Task<List<ProdutoResponseDto>> ListarAsync()
    {
        var produtos = await _produtoRepository.ListarAsync();

        return produtos.Select(produto => new ProdutoResponseDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            Ativo = produto.Ativo,
            DateTime = produto.DateTime
        }).ToList();
    }

    public async Task<ProdutoResponseDto?> ObterPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do produto deve ser maior que zero.");

        var produto = await _produtoRepository.ObterPorIdAsync(id);

        if (produto is null)
            return null;

        return new ProdutoResponseDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            Ativo = produto.Ativo,
            DateTime = produto.DateTime
        };
    }

    public async Task<bool> AtualizarAsync(int id, CriarProdutoDto dto)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do produto deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("O nome do produto é obrigatório.");

        if (dto.Preco <= 0)
            throw new ArgumentException("O preço do produto deve ser maior que zero.");

        if (dto.Estoque < 0)
            throw new ArgumentException("O estoque do produto não pode ser negativo.");

        var produtoExistente = await _produtoRepository.ObterPorIdAsync(id);

        if (produtoExistente is null)
            return false;

        produtoExistente.Nome = dto.Nome.Trim();
        produtoExistente.Descricao = dto.Descricao.Trim() ?? string.Empty;
        produtoExistente.Preco = dto.Preco;
        produtoExistente.Estoque = dto.Estoque;

        return await _produtoRepository.AtualizarAsync(produtoExistente);
    }

    public async Task<bool> DesativarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do produto deve ser maior que zero.");

        var produtoExistente = await _produtoRepository.ObterPorIdAsync(id);

        if (produtoExistente is null)
            return false;

        return await _produtoRepository.DesativarAsync(id);
    }
}