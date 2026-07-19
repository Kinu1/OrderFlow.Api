using System.Net;
using Microsoft.Data.SqlClient;

namespace OrderFlow.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message
            );
        }
        catch (SqlException ex)
        {
            var statusCode = ObterStatusCodeSql(ex);

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(ex, "Erro inesperado no SQL Server.");

                await HandleExceptionAsync(
                    context,
                    statusCode,
                    "Ocorreu um erro ao acessar o banco de dados."
                );

                return;
            }

            await HandleExceptionAsync(
                context,
                statusCode,
                ex.Message
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado na aplicação.");

            await HandleExceptionAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno no servidor."
            );
        }
    }

    private static HttpStatusCode ObterStatusCodeSql(SqlException ex)
    {
        return ex.Number switch
        {
            50001 => HttpStatusCode.NotFound,
            50003 => HttpStatusCode.NotFound,
            50010 => HttpStatusCode.NotFound,

            50002 => HttpStatusCode.BadRequest,
            50004 => HttpStatusCode.BadRequest,
            50005 => HttpStatusCode.BadRequest,
            50011 => HttpStatusCode.BadRequest,
            50012 => HttpStatusCode.BadRequest,
            50013 => HttpStatusCode.BadRequest,

            _ => HttpStatusCode.InternalServerError
        };

    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string mensagem)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var resposta = new
        {
            statusCode = context.Response.StatusCode,
            mensagem
        };

        await context.Response.WriteAsJsonAsync(resposta);
    }
}