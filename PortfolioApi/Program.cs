using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Services;
using Portfolio.Infrastructure.Database;
using Portfolio.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Portfolio.Api.Validators;
using Portfolio.Infrastructure.External.Github;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Portfolio.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Portfolio.Domain.Entity;
using Portfolio.Core.Entities;
using Portfolio.Api.BackgroundServices;

//logger keszitese, consolba irasa
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
//try catch block
try
{
    var builder = WebApplication.CreateBuilder(args);
    //useserilog bevezetese
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    // database appsettingsbol
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string"
            + "'DefaultConnection' not found.");

    builder.Services.AddDbContext<PortfolioDbContext>(options =>
        options.UseSqlServer(connectionString));

    // 1. Identity beállítása
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<PortfolioDbContext>()
        .AddDefaultTokenProviders();

    // 2. Authentication és JWT beállítása
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            //Itt olvassa ki a titkos kulcsot az appsettings.json-ból
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });
    // validaciok
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectRequestValidator>();

    // Swagger beállítása a JWT gombbal
    builder.Services.AddSwaggerGen(option =>
    {
        option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Kérlek írd be a tokent így: Bearer {token}",
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type=Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });

    // github client
    builder.Services.AddHttpClient<GitHubClient>(client =>
    {
        client.BaseAddress = new Uri("https://api.github.com/");
        client.DefaultRequestHeaders.Add("User-Agent", "PortfolioApp");
        var token = builder.Configuration["GitHub:Token"];
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    });

    // scopeok 
    builder.Services.AddScoped<IGitHubService, GitHubService>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<GitHubSyncService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    //background services
    builder.Services.AddHostedService<ProjectStatsWorker>();
    // exceptionhandle
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
    builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<PortfolioDbContext>();
            // Ez a parancs ellenõrzi, van-e pendig migration, és lefuttatja (létrehozza a DB-t)
            dbContext.Database.Migrate();
            Log.Information("Az adatbázis migráció sikeresen lefutott.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Hiba történt az adatbázis migráció közben.");
        }
    }
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();
    app.UseHttpsRedirection();

    // Cors -> Auth -> Authorization
    app.UseCors("Open");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
// catch resz
catch (Exception ex)
{
    Log.Fatal(ex, "Az applikáció váratlanul leált");
}
// mindenképp csinálja
finally
{
    Log.CloseAndFlush();
}
public partial class Program { }