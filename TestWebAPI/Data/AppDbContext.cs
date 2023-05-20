using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data.Configurations;
using TestWebAPI.Data.Models;

namespace TestWebAPI.Data;

// Клас контексту бази даних з яким працює програма
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Таблиці які присутні у базі даних
    public DbSet<DevicesForExperiment> Devices { set; get; } = null!; // Девайси
    public DbSet<Experiment> Experiment { set; get; } = null!; // Експерименти

    // Прив'язка даних
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.UseHiLo();
        builder.ApplyConfiguration(new DeviceConfig());
        builder.ApplyConfiguration(new ExperimentConfig());
    }
}
