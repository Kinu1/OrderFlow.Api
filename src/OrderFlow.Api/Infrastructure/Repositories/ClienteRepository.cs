using System.Data;
using Microsoft.Data.SqlClient;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Domain.Entities;
using OrderFlow.Api.Infrastructure.Data;

namespace OrderFlow.Api.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public ClienteRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CriarAsync(Cliente cliente)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Cliente_Criar", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@Nome", cliente.Nome);
        comando.Parameters.AddWithValue("@Email", cliente.Email);
        comando.Parameters.AddWithValue("@Telefone", cliente.Telefone);

        await conexao.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        return Convert.ToInt32(resultado);
    }

    public async Task<List<Cliente>> ListarAsync()
    {
        var clientes = new List<Cliente>();

        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Cliente_Listar", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        await conexao.OpenAsync();

        await using var reader = await comando.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            clientes.Add(new Cliente
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Telefone = reader.IsDBNull(reader.GetOrdinal("Telefone"))
                    ? string.Empty
                    : reader.GetString(reader.GetOrdinal("Telefone")),
                DateTime = reader.GetDateTime(reader.GetOrdinal("CriadoEm"))
            });
        }

        return clientes;
    }

    public async Task<Cliente?> ObterPorIdAsync(int id)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Cliente_ObterPorId", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@Id", id);

        await conexao.OpenAsync();

        await using var reader = await comando.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new Cliente
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Nome = reader.GetString(reader.GetOrdinal("Nome")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Telefone = reader.IsDBNull(reader.GetOrdinal("Telefone"))
                ? string.Empty
                : reader.GetString(reader.GetOrdinal("Telefone")),
            DateTime = reader.GetDateTime(reader.GetOrdinal("CriadoEm"))
        };
    }

    public async Task<bool> AtualizarAsync(Cliente cliente)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Cliente_Atualizar", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@Id", cliente.Id);
        comando.Parameters.AddWithValue("@Nome", cliente.Nome);
        comando.Parameters.AddWithValue("@Email", cliente.Email);
        comando.Parameters.AddWithValue("@Telefone", cliente.Telefone);

        await conexao.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        var linhasAfetadas = Convert.ToInt32(resultado);

        return linhasAfetadas > 0;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Cliente_Excluir", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@Id", id);

        await conexao.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        var linhasAfetadas = Convert.ToInt32(resultado);

        return linhasAfetadas > 0;
    }
}