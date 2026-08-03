USE OrderFlowDb;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM Clientes
    WHERE Email = 'pedro.silva@email.com'
)
BEGIN
    INSERT INTO Clientes
    (
        Nome,
        Email,
        Telefone
    )
    VALUES
    (
        'Pedro Silva',
        'pedro.silva@email.com',
        '11999999999'
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM Clientes
    WHERE Email = 'maria.souza@email.com'
)
BEGIN
    INSERT INTO Clientes
    (
        Nome,
        Email,
        Telefone
    )
    VALUES
    (
        'Maria Souza',
        'maria.souza@email.com',
        '21988888888'
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM Produtos
    WHERE Nome = 'Mouse Gamer'
)
BEGIN
    INSERT INTO Produtos
    (
        Nome,
        Descricao,
        Preco,
        Estoque,
        Ativo
    )
    VALUES
    (
        'Mouse Gamer',
        'Mouse gamer RGB com alta precisão.',
        149.90,
        20,
        1
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM Produtos
    WHERE Nome = 'Teclado Mecânico'
)
BEGIN
    INSERT INTO Produtos
    (
        Nome,
        Descricao,
        Preco,
        Estoque,
        Ativo
    )
    VALUES
    (
        'Teclado Mecânico',
        'Teclado mecânico ABNT2 com switches blue.',
        299.90,
        15,
        1
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM Produtos
    WHERE Nome = 'Headset Gamer'
)
BEGIN
    INSERT INTO Produtos
    (
        Nome,
        Descricao,
        Preco,
        Estoque,
        Ativo
    )
    VALUES
    (
        'Headset Gamer',
        'Headset gamer com microfone e som surround.',
        199.90,
        10,
        1
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM Produtos
    WHERE Nome = 'Monitor Full HD'
)
BEGIN
    INSERT INTO Produtos
    (
        Nome,
        Descricao,
        Preco,
        Estoque,
        Ativo
    )
    VALUES
    (
        'Monitor Full HD',
        'Monitor 24 polegadas Full HD.',
        899.90,
        8,
        1
    );
END;
GO

SELECT
    Id,
    Nome,
    Email,
    Telefone,
    CriadoEm
FROM Clientes;
GO

SELECT
    Id,
    Nome,
    Descricao,
    Preco,
    Estoque,
    Ativo,
    CriadoEm
FROM Produtos;
GO