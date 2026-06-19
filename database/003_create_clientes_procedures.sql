USE OrderFlowDb;
GO

CREATE OR ALTER PROCEDURE Cliente_Criar
    @Nome NVARCHAR(100),
    @Email NVARCHAR(150),
    @Telefone NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Clientes
    (
        Nome,
        Email,
        Telefone
    )
    VALUES
    (
        @Nome,
        @Email,
        @Telefone
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END;
GO

CREATE PROCEDURE Cliente_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Nome,
        Email,
        Telefone,
        CriadoEm
    FROM Clientes
    ORDER BY Id DESC;
END;
GO

CREATE PROCEDURE Cliente_ObterPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Nome,
        Email,
        Telefone,
        CriadoEm
    FROM Clientes
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE Cliente_Atualizar
    @Id INT,
    @Nome NVARCHAR(100),
    @Email NVARCHAR(150),
    @Telefone NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Clientes
    SET
        Nome = @Nome,
        Email = @Email,
        Telefone = @Telefone
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS LinhasAfetadas;
END;
GO

CREATE PROCEDURE Cliente_Excluir
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Clientes
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS LinhasAfetadas;
END;
GO