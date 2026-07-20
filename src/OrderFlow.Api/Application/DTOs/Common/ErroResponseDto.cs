namespace OrderFlow.Api.Application.DTOs.Common;

public class ErroResponseDto
{
    public int StatusCode { get; set; }

    public String Mensagem { get; set; } = string.Empty;
}