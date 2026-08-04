using Moq;
using OrderFlow.Api.Application.DTOs.Clientes;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Application.Services;
using OrderFlow.Api.Domain.Entities;
using Xunit;

namespace OrderFlow.Api.Tests.Services;

public class ClienteServiceTests
{
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
            Telefone = "    21999999999  "
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
}