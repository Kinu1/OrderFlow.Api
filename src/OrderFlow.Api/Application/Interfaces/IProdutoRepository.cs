using OrderFlow.Api.Domain.Entities;

namespace OrderFlow.Api.Application.Interfaces;

public interface IProdutoRepository
{
    Task<int> CriarAsync(Produto produto);
    Task<List<Produto>> ListarAsync();
    Task<Produto?> ObterPorIdAsync(int id);
    Task<bool> AtualizarAsync(Produto produto);
    Task<bool> DesativarAsync(int Id);
}