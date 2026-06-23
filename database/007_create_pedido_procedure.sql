USE OrderFlowDb;
GO

IF TYPE_ID(N'dbo.ItemPedidoType') IS NULL
BEGIN
    EXEC(N'
        CREATE TYPE dbo.ItemPedidoType AS TABLE
        (
            ProdutoId INT NOT NULL PRIMARY KEY,
            Quantidade INT NOT NULL CHECK (Quantidade > 0)
        );
    ');
END;
GO

CREATE PROCEDURE Pedido_Criar
    @ClienteId INT,
    @Itens dbo.ItemPedidoType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM Clientes
            WHERE Id = @ClienteId
        )
        BEGIN
            THROW 50001, 'Cliente não encontrado.', 1;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM @Itens
        )
        BEGIN
            THROW 50002, 'O pedido deve possuir pelo menos um item.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM @Itens AS Item
            LEFT JOIN Produtos AS Produto WITH (UPDLOCK, HOLDLOCK)
                ON Produto.Id = Item.ProdutoId
            WHERE Produto.Id IS NULL
        )
        BEGIN
            THROW 50003, 'Um ou mais produtos não foram encontrados.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM @Itens AS Item
            INNER JOIN Produtos AS Produto WITH (UPDLOCK, HOLDLOCK)
                ON Produto.Id = Item.ProdutoId
            WHERE Produto.Ativo = 0
        )
        BEGIN
            THROW 50004, 'Um ou mais produtos estão inativos.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM @Itens AS Item
            INNER JOIN Produtos AS Produto WITH (UPDLOCK, HOLDLOCK)
                ON Produto.Id = Item.ProdutoId
            WHERE Produto.Estoque < Item.Quantidade
        )
        BEGIN
            THROW 50005, 'Estoque insuficiente para um ou mais produtos.', 1;
        END;

        DECLARE @Total DECIMAL(18,2);

        SELECT
            @Total = SUM(
                CAST(Item.Quantidade * Produto.Preco AS DECIMAL(18,2))
            )
        FROM @Itens AS Item
        INNER JOIN Produtos AS Produto
            ON Produto.Id = Item.ProdutoId;

        INSERT INTO Pedidos
        (
            ClienteId,
            Total,
            Status
        )
        VALUES
        (
            @ClienteId,
            @Total,
            1
        );

        DECLARE @PedidoId INT = CAST(SCOPE_IDENTITY() AS INT);

        INSERT INTO ItensPedido
        (
            PedidoId,
            ProdutoId,
            Quantidade,
            PrecoUnitario,
            Subtotal
        )
        SELECT
            @PedidoId,
            Item.ProdutoId,
            Item.Quantidade,
            Produto.Preco,
            CAST(Item.Quantidade * Produto.Preco AS DECIMAL(18,2))
        FROM @Itens AS Item
        INNER JOIN Produtos AS Produto
            ON Produto.Id = Item.ProdutoId;

        UPDATE Produto
        SET Produto.Estoque = Produto.Estoque - Item.Quantidade
        FROM Produtos AS Produto
        INNER JOIN @Itens AS Item
            ON Item.ProdutoId = Produto.Id;

        COMMIT TRANSACTION;

        SELECT @PedidoId AS Id;
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