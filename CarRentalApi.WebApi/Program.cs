using CarRentalApi.Business.DataProtection;
using CarRentalApi.Business.Operations.Car;
using CarRentalApi.Business.Operations.Feature;
using CarRentalApi.Business.Operations.Setting;
using CarRentalApi.Business.Operations.User;
using CarRentalApi.Data.Context;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Data.UnitOfWork;
using CarRentalApi.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer Token on Texbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme, Array.Empty<string>() }
    });
});


// Data Protection
builder.Services.AddScoped<IDataProtection, DataProtection>();

var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys"));

builder.Services.AddDataProtection()
    .SetApplicationName("CarRentalAPI")
    .PersistKeysToFileSystem(keysDirectory);

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // ayarlar
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // yetki isteyen token'ýn Issuer'i appsettings'deki ile uyuþuyor mu
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // benim için geçerli ýssuer kim

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true, // süresi dolan token'ý kabul etme

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };

        // Yetkilendirme hatalarýný yakalayýp özel mesaj döndürmek için
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                // Varsayýlan iþlemi engelle
                context.HandleResponse();

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new
                {
                    success = false,
                    statusCode = 401,
                    message = "Oturum açmanýz gerekiyor"
                });
                await context.Response.WriteAsync(result);
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new
                {
                    success = false,
                    statusCode = 403,
                    message = "Bu iþlem için yetkiniz bulunmuyor"
                });
                await context.Response.WriteAsync(result);
            }
        };
    });

// DB Connection
var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<CarRentalDbContext>(options => options.UseSqlServer(connectionString));

// Add Services - Dependcy Injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Generic olduðu için typeof kullandýk
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IFeatureService, FeatureManager>();
builder.Services.AddScoped<ICarService, CarManager>();
builder.Services.AddScoped<ISettingService, SettingManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseGlobalExceptionMiddleware(); // Global Exception Middleware added
app.UseMaintenenceMode(); // Maintenence middleware added

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
