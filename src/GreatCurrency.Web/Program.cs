using GreatCurrency.DAL.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("GreatCurrencyPostgreSQL");

builder.Services.AddDbContext<GreatCurrencyContext>(options =>
 options.UseNpgsql(connection));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();
