namespace OrderFlow.Api.Application.DTOs.Common;

public class ErroValidacaoResponseDto
{
    public int StatusCode { get; set; }

    public string Mensagem { get; set; } = string.Empty;

    public List<string> Erros { get; set; } = new();
}