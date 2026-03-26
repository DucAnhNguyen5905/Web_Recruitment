using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Recuitment_DataAccess.EFCore;
using Recuitment_DataAccess.IRepository;
using Recuitment_DataAccess.Recuitment_Unitofwork;
using Recuitment_DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddDbContext<Recruitment_DBContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Ten_ConnStr")));
builder.Services.AddSingleton<Microsoft.Extensions.Logging.ILoggerProvider, Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<IUnitofWork, UnitofWork>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

