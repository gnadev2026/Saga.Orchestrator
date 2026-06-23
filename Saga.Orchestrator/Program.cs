using Saga.Orchestrator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("Order", c => c.BaseAddress = new Uri("https://localhost:7114"));
builder.Services.AddHttpClient("Notifier", c => c.BaseAddress = new Uri("https://localhost:7118"));
builder.Services.AddHttpClient("Inventory", c => c.BaseAddress = new Uri("https://localhost:7028"));

builder.Services.AddSingleton<IOrderProxy, OrderProxy>();
builder.Services.AddSingleton<INotifierProxy, NotifierProxy>();
builder.Services.AddSingleton<IInventoryProxy, InventoryProxy>();
builder.Services.AddSingleton<IOrderManager, OrderManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
