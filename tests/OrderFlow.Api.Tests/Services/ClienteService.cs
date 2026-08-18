using Moq;
using OrderFlow.Api.Application.DTOs.Clientes;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Application.Services;
using OrderFlow.Api.Domain.Entities;
using Xunit;

namespace OrderFlow.Api.Tests.Services;

public class ClienteServiceTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    public async Task CriarAsync_DeveLancarExcecao_QuandoEmailForInvalido(string emailInvalido)
    {
        var repositoryMock = new Mock<IClienteRepository>();
        var service = new ClienteService(repositoryMock.Object);

        var dto = new CriarClienteDto
        {
            Nome = "Pedro",
            Email = emailInvalido,
            Telefone = "21999999999"
        };

        var acao = async () => await service.CriarAsync(dto);

        var excecao =
            await Assert.ThrowsAsync<ArgumentException>(acao);

        Assert.Equal(
            "O e-mail do cliente é obrigatório.",
            excecao.Message);

        repositoryMock.Verify(
            repository =>
                repository.CriarAsync(It.IsAny<Cliente>()),
            Times.Never);

    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoNomeEstiverVazio()
    {
        var repositoryMock = new Mock<IClienteRepository>();
        var service = new ClienteService(repositoryMock.Object);

        var dto = new CriarClienteDto
        {
            Nome = "",
            Email = "pedro@email.com",
            Telefone = "21999999999"
        };

        var acao = async () => await service.CriarAsync(dto);

        var excecao = await Assert.ThrowsAsync<ArgumentException>(acao);

        Assert.Equal("O nome do cliente é obrigatório.", excecao.Message);

        repositoryMock.Verify(
            repository => repository.CriarAsync(It.IsAny<Cliente>()),
            Times.Never);
    }

    [Fact]
    public async Task CriarAsync_DeveRetornarId_QuandoClienteForValido()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.CriarAsync(It.IsAny<Cliente>()))
            .ReturnsAsync(1);

        var service = new ClienteService(repositoryMock.Object);

        var dto = new CriarClienteDto
        {
            Nome = "Pedro",
            Email = "pedro@email.com",
            Telefone = "21999999999"
        };

        var IdCriado = await service.CriarAsync(dto);

        Assert.Equal(1, IdCriado);
    }

    [Fact]
    public async Task CriarAsync_DeveEnviarDadosSemEspacosAoRepositorio_QuandoClienteForValido()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
               .Setup(repository => repository.CriarAsync(It.IsAny<Cliente>()))
               .ReturnsAsync(1);

        var service = new ClienteService(repositoryMock.Object);

        var dto = new CriarClienteDto
        {
            Nome = "  Pedro   ",
            Email = "   pedro@email.com   ",
            Telefone = "    2199999999  "
        };

        await service.CriarAsync(dto);

        repositoryMock.Verify(
            repositoryMock => repositoryMock.CriarAsync(
                It.Is<Cliente>(Cliente =>
                    Cliente.Nome == "Pedro" &&
                    Cliente.Email == "pedro@email.com" &&
                    Cliente.Telefone == "2199999999")),
                Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ObterPorIdAsync_DeveLancarExcecao_QuandoIdNaoForPositivo(int id)
    {
        var repositoryMock = new Mock<IClienteRepository>();
        var service = new ClienteService(repositoryMock.Object);

        var acao = async () => await service.ObterPorIdAsync(id);

        var excecao = await Assert.ThrowsAsync<ArgumentException>(acao);

        Assert.Equal("O ID do cliente deve ser maior que zero.", excecao.Message);

        repositoryMock.Verify(
        repository => repository.ObterPorIdAsync(It.IsAny<int>()),
        Times.Never);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoClienteNãoForEncontrado()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.ObterPorIdAsync(10))
            .ReturnsAsync((Cliente?)null);

        var service = new ClienteService(repositoryMock.Object);

        var resultado = await service.ObterPorIdAsync(10);

        Assert.Null(resultado);

        repositoryMock.Verify(
            repository => repository.ObterPorIdAsync(10),
            Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarClienteResponseDto_QuandoClienteForEncontrado()
    {
        var clienteDoRepositorio = new Cliente
        {
            Id = 10,
            Nome = "Pedro",
            Email = "pedro@email.com",
            Telefone = "21999999999",
            DateTime = new DateTime(2026, 8, 11)
        };

        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.ObterPorIdAsync(10))
            .ReturnsAsync(clienteDoRepositorio);

        var service = new ClienteService(repositoryMock.Object);

        var resultado = await service.ObterPorIdAsync(10);

        Assert.NotNull(resultado);
        Assert.Equal(10, resultado.Id);
        Assert.Equal("Pedro", resultado.Nome);
        Assert.Equal("pedro@email.com", resultado.Email);
        Assert.Equal("21999999999", resultado.Telefone);
        Assert.Equal(new DateTime(2026, 8, 11), resultado.DateTime);

        repositoryMock.Verify(
            repository => repository.ObterPorIdAsync(10),
            Times.Once);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarClienteMapeados_QuandoRepositorioTiverClientes()
    {
        var clienteDoRepositorio = new List<Cliente>
        {
            new Cliente
            {
                Id = 1,
                Nome = "Pedro",
                Email = "pedro@email.com",
                Telefone = "21999999999",
            },

            new Cliente
            {
                Id = 2,
                Nome = "Ana",
                Email = "ana@email.com",
                Telefone = "21888888888",
            }
        };

        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.ListarAsync())
            .ReturnsAsync(clienteDoRepositorio);

        var service = new ClienteService(repositoryMock.Object);

        var resultado = await service.ListarAsync();

        Assert.Equal(2, resultado.Count);

        Assert.Equal(1, resultado[0].Id);
        Assert.Equal("Pedro", resultado[0].Nome);
        Assert.Equal("pedro@email.com", resultado[0].Email);
        Assert.Equal("21999999999", resultado[0].Telefone);

        Assert.Equal(2, resultado[1].Id);
        Assert.Equal("Ana", resultado[1].Nome);
        Assert.Equal("ana@email.com", resultado[1].Email);
        Assert.Equal("21888888888", resultado[1].Telefone);

        repositoryMock.Verify(
            repositoryMock => repositoryMock.ListarAsync(),
            Times.Once);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarListaVazia_QuandoNaoHouverClientes()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.ListarAsync())
            .ReturnsAsync(new List<Cliente>());

        var service = new ClienteService(repositoryMock.Object);

        var resultado = await service.ListarAsync();

        Assert.Empty(resultado);

        repositoryMock.Verify(
            repository => repository.ListarAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveRetornarFalse_QuandoClienteNaoForEncontrado()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.ObterPorIdAsync(10))
            .ReturnsAsync((Cliente?)null);

        var service = new ClienteService(repositoryMock.Object);

        var dto = new AtualizarClienteDto
        {
            Nome = "Pedro atualizado",
            Email = "pedro.atualizado@email.com",
            Telefone = "21999999999"
        };

        var resultado = await service.AtualizarAsync(10, dto);

        Assert.False(resultado);

        repositoryMock.Verify(
            repository => repository.AtualizarAsync(It.IsAny<Cliente>()),
            Times.Never);

    }

    [Fact]
    public async Task AtualizarAsync_DeveRetornarTrueEEnviarDadosLimpos_QuandoClienteExistir()
    {
        var clienteExistente = new Cliente
        {
            Id = 10,
            Nome = "Nome antigo",
            Email = "antigo@email.com",
            Telefone = "21900000000"
        };

        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.ObterPorIdAsync(10))
            .ReturnsAsync(clienteExistente);

        repositoryMock
            .Setup(repository => repository.AtualizarAsync(It.IsAny<Cliente>()))
            .ReturnsAsync(true);

        var service = new ClienteService(repositoryMock.Object);

        var dto = new AtualizarClienteDto
        {
            Nome = "  Pedro atualizado  ",
            Email = "  pedro.atualizado@email.com  ",
            Telefone = "  21999999999  "
        };

        var resultado = await service.AtualizarAsync(10, dto);

        Assert.True(resultado);

        repositoryMock.Verify(
            repository => repository.ObterPorIdAsync(10),
            Times.Once);

        repositoryMock.Verify(
            repository => repository.AtualizarAsync(
                It.Is<Cliente>(cliente =>
                    cliente.Id == 10 &&
                    cliente.Nome == "Pedro atualizado" &&
                    cliente.Email == "pedro.atualizado@email.com" &&
                    cliente.Telefone == "21999999999")),
            Times.Once);

    }

    [Fact]
    public async Task ExcluirAsync_DeveRetornarTrueEExcluirCliente_QuandoClienteExistir()
    {
        var clienteExistente = new Cliente
        {
            Id = 10,
            Nome = "Pedro",
            Email = "pedro@email.com",
            Telefone = "21999999999"
        };

        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.ObterPorIdAsync(10))
            .ReturnsAsync(clienteExistente);

        repositoryMock
            .Setup(repository => repository.ExcluirAsync(10))
            .ReturnsAsync(true);

        var service = new ClienteService(repositoryMock.Object);

        var resultado = await service.ExcluirAsync(10);

        Assert.True(resultado);

        repositoryMock.Verify(
            repository => repository.ObterPorIdAsync(10),
            Times.Once);

        repositoryMock.Verify(
            repository => repository.ExcluirAsync(10),
            Times.Once);

    }

    [Fact]
    public async Task ExcluirAsync_DeveRetornarFalseENaoExcluir_QuandoClienteNaoForEncontrado()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(repository => repository.ObterPorIdAsync(10))
            .ReturnsAsync((Cliente?)null);

        var service = new ClienteService(repositoryMock.Object);

        var resultado = await service.ExcluirAsync(10);

        Assert.False(resultado);

        repositoryMock.Verify(
            repository => repository.ExcluirAsync(It.IsAny<int>()),
            Times.Never);

    }
}

