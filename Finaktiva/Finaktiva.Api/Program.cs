using Finaktiva.Api.Extensions;
using Finaktiva.Api.Middleware;
using Finaktiva.Application;
using Finaktiva.Infrastructure;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Desactiva el filtro de validación automática
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//PARA ACCEDER A LA URL DE APLICACION
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowSpecificOrigin",
    builder => builder
    .WithOrigins("http://localhost:4200")
    .WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

var app = builder.Build();

//HABILITAR LOS CORS PARA ACCEDER A LA API
app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.ApplyMigration();
//app.SeedData();

app.UseAuthorization();
app.MapControllers();
app.Run();
