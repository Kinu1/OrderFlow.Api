USE OrderFlowDb;
GO

CREATE PROCEDURE Pedido_Cancelar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM Pedidos
            WHERE Id = @Id
        )
        BEGIN
            THROW 50010, 'Pedido não encontrado.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM Pedidos
            WHERE Id = @Id
              AND Status = 3
        )
        BEGIN
            THROW 50011, 'Pedido já está cancelado.', 1;
        END;

        UPDATE Produto
        SET Produto.Estoque = Produto.Estoque + Item.Quantidade
        FROM Produtos AS Produto
        INNER JOIN ItensPedido AS Item
            ON Item.ProdutoId = Produto.Id
        WHERE Item.PedidoId = @Id;

        UPDATE Pedidos
        SET Status = 3
        WHERE Id = @Id;

        COMMIT TRANSACTION;

        SELECT 1 AS Sucesso;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO