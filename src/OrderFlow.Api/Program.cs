using OrderFlow.Api.Application.Interfaces;
using OrderFlow.Api.Application.Services;
using OrderFlow.Api.Infrastructure.Data;
using OrderFlow.Api.Infrastructure.Repositories;
using OrderFlow.Api.Middlewares;
using System.Text.Json.Serialization;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Application.DTOs.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var erros = context.ModelState
           .Where(item => item.Value?.Errors.Count > 0)
           .SelectMany(item => item.Value!.Errors)
           .Select(erro => erro.ErrorMessage)
           .ToList();

        var resposta = new ErroValidacaoResponseDto
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Mensagem = "Um ou mais erros de validação ocorreram.",
            Erros = erros
        };

        return new BadRequestObjectResult(resposta);
    };
});


builder.Services.AddSingleton<SqlConnectionFactory>();

builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();