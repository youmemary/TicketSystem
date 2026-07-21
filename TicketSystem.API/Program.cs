using TicketSystem.Infrastructure.Repositories;
using TicketSystem.Infrastructure.Services;
using TicketSystem.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер DI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRequestRepository, JsonRequestRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
