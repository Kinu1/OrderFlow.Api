USE OrderFlowDb;
GO

CREATE PROCEDURE Pedido_Confirmar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

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
        THROW 50012, 'Pedido cancelado não pode ser confirmado.', 1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Pedidos
        WHERE Id = @Id
          AND Status = 2
    )
    BEGIN
        THROW 50013, 'Pedido já está confirmado.', 1;
    END;

    UPDATE Pedidos
    SET Status = 2
    WHERE Id = @Id;

    SELECT 1 AS Sucesso;
END;
GO