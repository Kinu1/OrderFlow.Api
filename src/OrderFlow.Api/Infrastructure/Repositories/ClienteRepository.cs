using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Domain.Entities;

namespace OrderFlow.Api.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private static readonly List<Cliente> Clientes = [];
    private static int _proximoId = 1;

    public Task<int> CriarAsync(Cliente cliente)
    {
        cliente.Id = _proximoId++;
        cliente.DateTime = DateTime.Now;

        Clientes.Add(cliente);

        return Task.FromResult(cliente.Id);
    }

    public Task<List<Cliente>> ListarAsync()
    {
        return Task.FromResult(Clientes.ToList());
    }

    public Task<Cliente?> ObterPorIdAsync(int id)
    {
        var cliente = Clientes.FirstOrDefault(cliente => cliente.Id == id);

        return Task.FromResult(cliente);
    }

    public Task<bool> AtualizarAsync(Cliente cliente)
    {
        var clienteExistente = Clientes.FirstOrDefault(c => c.Id == cliente.Id);

        if (clienteExistente is null)
            return Task.FromResult(false);

        clienteExistente.Nome = cliente.Nome;
        clienteExistente.Email = cliente.Email;
        clienteExistente.Telefone = cliente.Telefone;

        return Task.FromResult(true);
    }

    public Task<bool> ExcluirAsync(int id)
    {
        var cliente = Clientes.FirstOrDefault(cliente => cliente.Id == id);

        if (cliente is null)
            return Task.FromResult(false);

        Clientes.Remove(cliente);

        return Task.FromResult(true);
    }
}