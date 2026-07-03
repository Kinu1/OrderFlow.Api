using System.Data;
using Microsoft.Data.SqlClient;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Domain.Entities;
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