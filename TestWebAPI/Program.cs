using TestWebAPI.Middleware;
using TestWebAPI.Service.Interfaces;
using TestWebAPI.Service;
using TestWebAPI.Data;
using TestWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()             // Отримання налаштувань для проекту
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(typeof(GlobalExceptionFilter));
}).AddJsonOptions(opts => opts.JsonSerializerOptions.WriteIndented = true); // ��������� ����������� ������� ��� ������� ���������

builder.Services.AddSwaggerGen(); 

builder.Services.AddTransient<Random>();

builder.Services.AddTransient<IService, Service>();
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddTransient<IExperimentRepository, ExperimentRepository>();

builder.Services.AddDbContextFactory<AppDbContext>(opts => opts.UseSqlServer(config.GetConnectionString("DefaultConnection"))); // ϳ��������� �� �� �� ������� ���������� � �������������
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowAll", opti =>
    {
        opti.AllowAnyHeader();
        opti.AllowAnyMethod();
        opti.AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseRouting();

app.MapDefaultControllerRoute();
app.MapControllers();

InitializeDB(app);

app.Run();

// ����� ��� ���-��������� ������������ � ��� �����
void InitializeDB(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreatedAsync().Wait();
            if (!context.Experiment.Any())
            {
                context.Experiment.Add(new Experiment
                {
                    Name = "button_color"
                });
                context.Experiment.Add(new Experiment
                {
                    Name = "price"
                });
                context.SaveChangesAsync().Wait();
            }
            var service = services.GetRequiredService<IService>();
            for (int i = 0; i < 43; i++)
            {
                service.ButtonColor("device" + i);
            }
            for (int i = 0; i < 100; i++)
            {
                service.Price("devicee" + i);
            }
            
        }
        catch
        {
        }
    }
}