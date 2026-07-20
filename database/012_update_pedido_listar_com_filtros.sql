USE OrderFlowDb;
GO

ALTER PROCEDURE Pedido_Listar
    @ClienteId INT = NULL,
    @Status INT = NULL
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
    ORDER BY Pedido.Id DESC;
END;
GO