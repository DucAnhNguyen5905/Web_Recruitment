using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Recuitment_DataAccess.Dapper;
using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using Recuitment_DataAccess.Recuitment_Unitofwork;
using Recuitment_DataAccess.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddDbContext<Recruitment_DBContext>(options =>

    options.UseSqlServer(configuration.GetConnectionString("Ten_ConnStr")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,

        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,

        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))

    };

});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration["RedisCacheUrl"];

});

builder.Services.AddSingleton<Microsoft.Extensions.Logging.ILoggerProvider, Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<IUnitofWork, UnitofWork>();
builder.Services.AddScoped<IApplicationDBConnection, ApplicationDBConnection>();
builder.Services.AddScoped<IEmployerRepositoryDapper, EmployerRepositoryDapper>();
builder.Services.AddScoped<IJobpostingRepositoryDapper, JobPostingRepositoryDapper>();
builder.Services.AddScoped<IFunctionRepository, FunctionRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<ICandidateRepositoryDapper, CandidateRepositoryDapper>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
