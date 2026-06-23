USE OrderFlowDb;
GO

CREATE OR ALTER PROCEDURE Produto_Criar
    @Nome NVARCHAR(100),
    @Descricao NVARCHAR(300),
    @Preco DECIMAL(18,2),
    @Estoque INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Produtos
    (
        Nome,
        Descricao,
        Preco,
        Estoque
    )
    VALUES
    (
        @Nome,
        @Descricao,
        @Preco,
        @Estoque
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END;
GO

CREATE PROCEDURE Produto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Nome,
        Descricao,
        Preco,
        Estoque,
        Ativo,
        CriadoEm
    FROM Produtos
    WHERE Ativo = 1
    ORDER BY Id DESC;
END;
GO

CREATE PROCEDURE Produto_ObterPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Nome,
        Descricao,
        Preco,
        Estoque,
        Ativo,
        CriadoEm
    FROM Produtos
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE Produto_Atualizar
    @Id INT,
    @Nome NVARCHAR(100),
    @Descricao NVARCHAR(300),
    @Preco DECIMAL(18,2),
    @Estoque INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Produtos
    SET
        Nome = @Nome,
        Descricao = @Descricao,
        Preco = @Preco,
        Estoque = @Estoque
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS LinhasAfetadas;
END;
GO

CREATE PROCEDURE Produto_Desativar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Produtos
    SET Ativo = 0
    WHERE Id = @Id;
        AND Ativo = 1;

    SELECT @@ROWCOUNT AS LinhasAfetadas;
END;
GO