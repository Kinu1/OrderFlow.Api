using Xunit;
using Moq;
using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Application.Services;
using OrderFlow.Api.Domain.Entities;

namespace OrderFlow.Api.Tests.Services;

public class ClienteServiceTests
{
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarCliente_QuandoClienteExistir()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        var cliente = new Cliente
        {
            Id = 1,
            Nome = "Pedro",
            Email = "pedro@email.com",
            Telefone = "11999999999"
        };

        repositoryMock
            .Setup(r => r.ObterPorIdAsync(1))
            .ReturnsAsync(cliente);

        var service = new ClienteService(repositoryMock.Object);

        var resultado = await service.ObterPorIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal("Pedro", resultado.Nome);
        Assert.Equal("pedro@email.com", resultado.Email);

    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExecao_QuandoClienteNaoExistir()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        repositoryMock
            .Setup(r => r.ObterPorIdAsync(1))
            .ReturnsAsync((Cliente?)null);

        var service = new ClienteService(repositoryMock.Object);

        await Assert.ThrowsAsync<Exeception>(() =>
            service.ObterPorIdAsync(1));
    }

    [Fact]
    public async Task CriarAsync_DeveChamarRepository_QuandoDadosForemValidos()
    {
        var repositoryMock = new Mock<IClienteRepository>();

        var dto = new CriarClienteDto
        {
            Nome = "Pedro",
            Email = "pedro@email.com",
            Telefone = "11999999999"
        };

        var service = new ClienteService(repositoryMock.Object);

        await service.CriarAsync(dto);
        repositoryMock.Verify(
            r => r.CriarAsync(It.IsAny<Cliente>()),
            Times.Once);
    }   

}