using Microsoft.EntityFrameworkCore;
using PJ01.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<PJ01Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PJ01Connect"), b => b.MigrationsAssembly("PJ01.Database")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
