using DotNetEnv;
using Loja.Infra.Ioc;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Usar o método de extensão para adicionar dependências
builder.Services.AddProjectManagerDependencies(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructionSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Aplique a política que você configurou, ou "AllowAll" para desenvolvimento

app.UseAuthorization();

app.MapControllers();

app.Run();
