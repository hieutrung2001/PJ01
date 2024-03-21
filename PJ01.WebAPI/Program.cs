using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PJ01.Core.Extensions;
using PJ01.Infrastructure.Context;
using PJ01.Domain.Entities.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PJ01.Core.Helpers.Mappers;
using PJ01.Core.Interfaces.Repositories;
using PJ01.Infrastructure.Repositorises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services
    .AddDbContext<PJ01Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PJ01Connect")));

builder.Services.AddApplicationServices();
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
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PJ01Context>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAutoMapper(typeof(StudentProfile));
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();


// cors
app.UseCors(options => options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
