using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PJ01.Domain;
using PJ01.Domain.Entities.Identity;
using PJ01.Core.Extensions;
using PJ01.Core.Helpers.Mappers;
using PJ01.Infrastructure.Context;
using PJ01.Core.Interfaces.Repositories;
using PJ01.Infrastructure.Repositories;
using PJ01.Domain.Entities;
using PJ01.Infrastructure.Repositorises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();
builder.Services
    .AddDbContext<PJ01Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PJ01Connect") ?? throw new InvalidOperationException("Connection string 'PJ01Context' not found.")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 1;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddSignInManager<SignInManager<User>>()
    .AddEntityFrameworkStores<PJ01Context>();
builder.Services.AddApplicationServices();
builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Accounts/Login");
builder.Services.AddAutoMapper(typeof(AccountProfile));
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Login}/{id?}");

app.Run();
