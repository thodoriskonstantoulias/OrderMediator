using Microsoft.EntityFrameworkCore;
using OrderMediator.Data.Data;
using OrderMediator.Data.Services;
using OrderMediator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPriceResolver, PriceResolver>();

builder.Services.AddHttpClient<IERPService, ERPService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ErpEndpoint"]);
});

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

    await dbInitializer.InitializeAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
