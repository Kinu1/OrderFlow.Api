namespace OrderFlow.Api.Domain.Entities;

public class Produto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public decimal Preco { get; set; }

    public int Estoque { get; set; }

    public bool Ativo { get; set; }

    public DateTime DateTime { get; set; }
}