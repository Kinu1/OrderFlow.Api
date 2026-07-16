USE OrderFlowDb;
GO

CREATE PROCEDURE Pedido_Listar
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
    ORDER BY Pedido.Id DESC;
END;
GO