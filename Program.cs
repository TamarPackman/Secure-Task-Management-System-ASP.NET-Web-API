using project.interfaces;
using project.Services;
using project.Middlewares;
 using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
.WriteTo.File(
    path:  $"Logs/{DateTime.UtcNow:yyyy/MM/dd}/log.txt",  // השתמש בפורמט תאריך בקובץ עצמו
    rollingInterval: RollingInterval.Day , // יצירת קובץ חדש כל יום
     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
)
    
    .CreateLogger();

// חיבור Serilog למערכת הלוגים של ASP.NET Core
 builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 builder.Services.AddJewelService();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseLog();
app.UseErrorHandling();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
 Log.CloseAndFlush();
