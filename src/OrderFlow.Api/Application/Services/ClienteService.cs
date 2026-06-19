using OrderFlow.Api.Application.DTOs.Clientes;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Domain.Entities;

namespace OrderFlow.Api.Application.Services;

public class ClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<int> CriarAsync(CriarClienteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("O e-mail do cliente é obrigatório.");

        var cliente = new Cliente
        {
            Nome = dto.Nome.Trim(),
            Email = dto.Email.Trim(),
            Telefone = dto.Telefone.Trim(),
        };

        return await _clienteRepository.CriarAsync(cliente);
    }

    public async Task<List<ClienteResponseDto>> ListarAsync()
    {
        var clientes = await _clienteRepository.ListarAsync();

        return clientes.Select(cliente => new ClienteResponseDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            DateTime = cliente.DateTime

        }).ToList();
    }

    public async Task<ClienteResponseDto?> ObterPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do cliente deve ser maior que zero.");

        var cliente = await _clienteRepository.ObterPorIdAsync(id);

        if (cliente is null)
            return null;

        return new ClienteResponseDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            DateTime = cliente.DateTime
        };
    }

    public async Task<bool> AtualizarAsync(int id, CriarClienteDto dto)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do cliente deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("O e-mail do cliente é obrigatório.");

        var clienteExistente = await _clienteRepository.ObterPorIdAsync(id);

        if (clienteExistente is null)
            return false;

        clienteExistente.Nome = dto.Nome.Trim();
        clienteExistente.Email = dto.Email.Trim();
        clienteExistente.Telefone = dto.Telefone.Trim();

        return await _clienteRepository.AtualizarAsync(clienteExistente);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID do cliente deve ser maior que zero.");

        var clienteExistente = await _clienteRepository.ObterPorIdAsync(id);

        if (clienteExistente is null)
            return false;

        return await _clienteRepository.ExcluirAsync(id);
    }
}