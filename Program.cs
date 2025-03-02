using Project.interfaces;
using Project.Services;
using Project.Middlewares;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .CreateLogger();
// חיבור Serilog למערכת הלוגים של ASP.NET Core
builder.Host.UseSerilog();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<ITokenService>((options, TokenService) =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = TokenService.GetTokenValidationParameters();
    });
builder.Services.AddAuthorization(cfg =>
    {
        cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
        cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User"));
    });    
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Jewelry", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                },
            new string[] {}
        }
        });
    });
builder.Services.AddJewelService();
builder.Services.AddUserService();
builder.Services.AddAuthorizationService();
builder.Services.AddTokenService();
var app = builder.Build();
// רישום ה-Middleware
app.UseLog();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseErrorHandlingMiddleware();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
