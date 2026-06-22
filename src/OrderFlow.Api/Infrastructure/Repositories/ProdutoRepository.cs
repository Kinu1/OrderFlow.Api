using System.Data;
using Microsoft.Data.SqlClient;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Domain.Entities;
using OrderFlow.Api.Infrastructure.Data;

namespace OrderFlow.Api.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public ProdutoRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CriarAsync(Produto produto)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Produto_Criar", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@Nome", produto.Nome);
        comando.Parameters.AddWithValue("@Descricao", produto.Descricao);
        comando.Parameters.AddWithValue("@Preco", produto.Preco);
        comando.Parameters.AddWithValue("@Estoque", produto.Estoque);

        await conexao.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        return Convert.ToInt32(resultado);
    }

    public async Task<List<Produto>> ListarAsync()
    {
        var produtos = new List<Produto>();

        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Produto_Listar", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        await conexao.OpenAsync();

        await using var reader = await comando.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            produtos.Add(MapearProduto(reader));
        }

        return produtos;
    }

      public async Task<Produto?> ObterPorIdAsync(int id)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Produto_ObterPorId", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@Id", id);

        await conexao.OpenAsync();

        await using var reader = await comando.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapearProduto(reader);
    }

    public async Task<bool> AtualizarAsync(Produto produto)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Produto_Atualizar", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@Id", produto.Id);
        comando.Parameters.AddWithValue("@Nome", produto.Nome);
        comando.Parameters.AddWithValue("@Descricao", produto.Descricao);
        comando.Parameters.AddWithValue("@Preco", produto.Preco);
        comando.Parameters.AddWithValue("@Estoque", produto.Estoque);

        await conexao.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        var LinhasAfetadas = Convert.ToInt32(resultado);

        return LinhasAfetadas > 0;
    }

    public async Task<bool> DesativarAsync(int Id)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Produto_Desativar", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@Id", Id);

        await conexao.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        var LinhasAfetadas = Convert.ToInt32(resultado);

        return LinhasAfetadas > 0;
    }

    private static Produto MapearProduto(SqlDataReader reader)
    {
        return new Produto
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Nome = reader.GetString(reader.GetOrdinal("Nome")),
            Descricao = reader.IsDBNull(reader.GetOrdinal("Descricao"))
                ? string.Empty
                : reader.GetString(reader.GetOrdinal("Descricao")),
            Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
            Estoque = reader.GetInt32(reader.GetOrdinal("Estoque")),
            Ativo = reader.GetBoolean(reader.GetOrdinal("Ativo")),
            DateTime = reader.GetDateTime(reader.GetOrdinal("CriadoEm"))
        };
    }
}