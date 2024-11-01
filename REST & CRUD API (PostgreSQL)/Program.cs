using Microsoft.OpenApi.Models;
using RestApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDbContext>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
        Description = "API для управления пользователями"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
        c.RoutePrefix = string.Empty; // чтобы Swagger UI был доступен по корню URL
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
