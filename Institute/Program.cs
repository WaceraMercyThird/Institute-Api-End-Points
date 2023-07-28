using Microsoft.EntityFrameworkCore;
using Institute.Datas.Services;
using Institute;
using Serilog;
using Serilog.Events;
using Institute.Helper;
using Akka.Actor;
using Institute.Actor;
using Akka.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


//serilog
Log.Logger = new LoggerConfiguration()
               .WriteTo.File
               (path: "C:\\Temp\\INSTITUTELAPILOGS-.txt",
                             outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm,ss.fff zzz}[{Level:u3}] {Message:lj}{NewLine}{Exception}",
                            rollingInterval: RollingInterval.Day,
                             restrictedToMinimumLevel: LogEventLevel.Information).CreateLogger();

// Add services to the container.
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddAutoMapper(typeof(ApplicationMapper));

// Set up the ActorSystem and create an actor
var actorSystem = ActorSystem.Create("MyActorSystem");
builder.Services.AddSingleton(actorSystem);
builder.Services.AddSingleton<IActorRefFactory>(actorSystem);
builder.Services.AddSingleton<EmailActor>();



builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InstituteDbContext")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
