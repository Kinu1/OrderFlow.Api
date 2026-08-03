USE OrderFlowDb;
GO

CREATE OR ALTER PROCEDURE Pedido_Listar
    @ClienteId INT = NULL,
    @Status INT = NULL,
    @Page INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Pedido.Id,
        Pedido.ClienteId,
        Cliente.Nome AS ClienteNome,
        Pedido.CriadoEm,
        Pedido.Total,
        Pedido.Status
    FROM Pedidos AS Pedido
    INNER JOIN Clientes AS Cliente
        ON Cliente.Id = Pedido.ClienteId
    WHERE
        (@ClienteId IS NULL OR Pedido.ClienteId = @ClienteId)
        AND (@Status IS NULL OR Pedido.Status = @Status)
    ORDER BY Pedido.Id DESC
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT
        COUNT(*) AS TotalItems
    FROM Pedidos AS Pedido
    INNER JOIN Clientes AS Cliente
        ON Cliente.Id = Pedido.ClienteId
    WHERE
        (@ClienteId IS NULL OR Pedido.ClienteId = @ClienteId)
        AND (@Status IS NULL OR Pedido.Status = @Status);
END;
GO