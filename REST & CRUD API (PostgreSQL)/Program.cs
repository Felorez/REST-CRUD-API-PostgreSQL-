using Microsoft.OpenApi.Models;
using RestApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDbContext>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Разрешить любые источники
              .AllowAnyHeader() // Разрешить любые заголовки
              .AllowAnyMethod(); // Разрешить любые HTTP-методы
    });
});

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
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
