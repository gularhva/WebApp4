using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;
using System.Text;
using WebApp4.Abstractions;
using WebApp4.Abstractions.IRepositories;
using WebApp4.Abstractions.IRepositories.IEntityRepositories;
using WebApp4.Abstractions.IUnitOfWorks;
using WebApp4.Abstractions.Services;
using WebApp4.Contexts;
using WebApp4.Entities.Identities;
using WebApp4.Extentions;
using WebApp4.Implementations.Repositories;
using WebApp4.Implementations.Repositories.EntityRepositories;
using WebApp4.Implementations.Services;
using WebApp4.Implementations.UnitOfWorks;
using WebApp4.Profiles;
using TokenHandler = WebApp4.Implementations.TokenHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer("Admin",options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,//tokunumuzu kim/hansi origin islede biler
        ValidateIssuer = true, //tokunu kim palylayir
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, //tokenin ozel keyi

        ValidAudience = builder.Configuration["JWT:Audience"],

        ValidIssuer = builder.Configuration["JWT:Issuer"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),

        //token omru qeder islemesi ucun
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Restaurant Final API",
        Description = "ASP.NET Core 6 Web API"
    });
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});
var config = builder.Configuration.GetConnectionString("ApplicationDbContext");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config));

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITokenHandler, TokenHandler>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

Logger? log = new LoggerConfiguration()
    .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
    .WriteTo.File("Logs/myJsonLogs.json")
    .WriteTo.File("Logs/mylogs.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}]{Message:lj{NewLine}{Exception}")
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"), sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
    {
        TableName = "MySeriLogg",
        AutoCreateSqlTable = true
    },
    null, null, LogEventLevel.Warning, null,
    columnOptions: new Serilog.Sinks.MSSqlServer.ColumnOptions
    {
        AdditionalColumns = new Collection<SqlColumn>
        {
            new SqlColumn(columnName:"User_Name",SqlDbType.NVarChar)
        }
    },
    null, null)
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();
Log.Logger = log;
builder.Host.UseSerilog(log);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.ConfigureExtentionHandler();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("User_Name", username);
    await next(context);
});
app.MapControllers();

app.Run();
