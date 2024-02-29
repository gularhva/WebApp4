using Microsoft.EntityFrameworkCore;
using WebApp4.Abstractions;
using WebApp4.Abstractions.IRepositories;
using WebApp4.Abstractions.IRepositories.IEntityRepositories;
using WebApp4.Abstractions.IUnitOfWorks;
using WebApp4.Contexts;
using WebApp4.Extentions;
using WebApp4.Implementations.Repositories;
using WebApp4.Implementations.Repositories.EntityRepositories;
using WebApp4.Implementations.Services;
using WebApp4.Implementations.UnitOfWorks;
using WebApp4.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration.GetConnectionString("ApplicationDbContext");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config));

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();

builder.Services.AddScoped<IStudentRepository,StudentRepository>();
builder.Services.AddScoped<ISchoolRepository,SchoolRepository>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.ConfigureExtentionHandler();
app.UseAuthorization();

app.MapControllers();

app.Run();
