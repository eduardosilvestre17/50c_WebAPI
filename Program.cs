using Sage50c.WebAPI.Services;
using Sage50c.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sage50c Web API",
        Version = "v1",
        Description = "API para integração com Sage50c - Documentos de Venda"
    });
});

// Add Sage50c API service
builder.Services.AddSingleton<Sage50cApiService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionMiddleware();

// Sempre mostrar Swagger (mesmo em produção para facilitar testes)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sage50c Web API v1");
    c.RoutePrefix = string.Empty; // Para abrir o Swagger na raiz
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();