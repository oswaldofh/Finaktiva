using Finaktiva.Infrastructure;
using Finaktiva.Application;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//PARA ACCEDER A LA URL DE APLICACION
builder.Services.AddHttpContextAccessor();

//SE HABILITAN LOS CORS
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowSpecificOrigin",
    builder => builder
    .WithOrigins("http://localhost:3000") // El origen de tu frontend
    .WithOrigins("http://localhost:26367")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
