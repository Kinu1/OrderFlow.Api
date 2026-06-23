namespace OrderFlow.Api.Application.DTOs.Pedidos;

public class ItemPedidoResponseDto
{
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal SubTotal { get; set; }
}