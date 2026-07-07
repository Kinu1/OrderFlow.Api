USE OrderFlowDb;
GO

CREATE PROCEDURE Pedido_ObterPorId
@Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       Id,
       ClienteId,
       CriadoEm,
       Total,
       Status
    FROM Pedidos
    WHERE Id = @Id;

    SELECT 
       Item.ProdutoId,
       Produto.Nome AS ProdutoNome,
       Item.Quantidade,
       Item.PrecoUnitario,
       Item.Subtotal
    FROM ItensPedido AS Item
    INNER JOIN Produtos AS Produto
       ON Produto.Id = Item.ProdutoId
    WHERE Item.PedidoId = @Id
    ORDER BY Item.Id;
END;
GO