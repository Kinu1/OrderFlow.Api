USE OrderFlowDb;
GO

CREATE TABLE Pedidos
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    CriadoEm DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Total DECIMAL(18,2) NOT NULL,
    Status INT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Pedidos_Clientes
        FOREIGN KEY (ClienteId)
        REFERENCES Clientes(Id),

    CONSTRAINT CK_Pedidos_Total
        CHECK (Total >= 0),

    CONSTRAINT CK_Pedidos_Status
        CHECK (Status IN (1, 2, 3))
);
GO

CREATE TABLE ItensPedido
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_ItensPedido_Pedidos
        FOREIGN KEY (PedidoId)
        REFERENCES Pedidos(Id),

    CONSTRAINT FK_ItensPedido_Produtos
        FOREIGN KEY (ProdutoId)
        REFERENCES Produtos(Id),

    CONSTRAINT CK_ItensPedido_Quantidade
        CHECK (Quantidade > 0),

    CONSTRAINT CK_ItensPedido_PrecoUnitario
        CHECK (PrecoUnitario > 0),

    CONSTRAINT CK_ItensPedido_Subtotal
        CHECK (Subtotal >= 0),

    CONSTRAINT UQ_ItensPedido_Pedido_Produto
        UNIQUE (PedidoId, ProdutoId)
);
GO

CREATE INDEX IX_Pedidos_ClienteId
ON Pedidos(ClienteId);
GO

CREATE INDEX IX_ItensPedido_ProdutoId
ON ItensPedido(ProdutoId);
GO