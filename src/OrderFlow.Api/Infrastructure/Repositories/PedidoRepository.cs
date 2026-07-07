using System.Data;
using Microsoft.Data.SqlClient;
using OrderFlow.Api.Application.DTOs.Pedidos;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Domain.Entities;
using OrderFlow.Api.Domain.Enums;
using OrderFlow.Api.Infrastructure.Data;

namespace OrderFlow.Api.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public PedidoRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CriarAsync(Pedido pedido)
    {
        await using var conexao = _connectionFactory.CreateConnection();

        await using var comando = new SqlCommand("Pedido_Criar", conexao);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);

        var parametroItens = comando.Parameters.Add("@Itens", SqlDbType.Structured);
        parametroItens.TypeName = "dbo.ItemPedidoType";
        parametroItens.Value = CriarTabelaItens(pedido.Itens);

        await conexao.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        return Convert.ToInt32(resultado);
    }
    
    public async Task<PedidoResponseDto?> ObterPorIdAsync(int id)
{
    await using var conexao = _connectionFactory.CreateConnection();

    await using var comando = new SqlCommand("Pedido_ObterPorId", conexao);
    comando.CommandType = CommandType.StoredProcedure;

    comando.Parameters.AddWithValue("@Id", id);

    await conexao.OpenAsync();

    await using var reader = await comando.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
        return null;

    var pedido = new PedidoResponseDto
    {
        Id = reader.GetInt32(reader.GetOrdinal("Id")),
        ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
        CriadoEm = reader.GetDateTime(reader.GetOrdinal("CriadoEm")),
        Total = reader.GetDecimal(reader.GetOrdinal("Total")),
        Status = (StatusPedido)reader.GetInt32(reader.GetOrdinal("Status"))
    };

    await reader.NextResultAsync();

    while (await reader.ReadAsync())
    {
        pedido.Itens.Add(new ItemPedidoResponseDto
        {
            ProdutoId = reader.GetInt32(reader.GetOrdinal("ProdutoId")),
            ProdutoNome = reader.GetString(reader.GetOrdinal("ProdutoNome")),
            Quantidade = reader.GetInt32(reader.GetOrdinal("Quantidade")),
            PrecoUnitario = reader.GetDecimal(reader.GetOrdinal("PrecoUnitario")),
            SubTotal = reader.GetDecimal(reader.GetOrdinal("Subtotal"))
        });
    }

    return pedido;
}
    

    private static DataTable CriarTabelaItens(List<ItemPedido> itens)
    {
        var tabela = new DataTable();

        tabela.Columns.Add("ProdutoId", typeof(int));
        tabela.Columns.Add("Quantidade", typeof(int));

        foreach (var item in itens)
        {
            tabela.Rows.Add(item.ProdutoId, item.Quantidade);
        }

        return tabela;
    }
}