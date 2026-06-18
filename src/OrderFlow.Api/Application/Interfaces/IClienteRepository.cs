using OrderFlow.Api.Domain.Entities;

namespace OrderFlow.Api.Application.Interfaces;

public interface IClienteRepository
{
    Task<int> CriarAsync(Cliente cliente);
    Task<List<Cliente>> ListarAsync();
    Task<Cliente?> ObterPorIdAsync(int id);
    Task<bool> AtualizarAsync(Cliente cliente);
    Task<bool> ExcluirAsync(int id);
}