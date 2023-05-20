using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestWebAPI.Data.Models;

namespace TestWebAPI.Data.Configurations;

// Клас налаштування для сутності DevicesForExperiment
public class DeviceConfig : IEntityTypeConfiguration<DevicesForExperiment>
{
    // Метод конфігурації
    public void Configure(EntityTypeBuilder<DevicesForExperiment> builder)
    {
        builder.HasKey(k => k.Id); // Задати Primarykey
        builder.ToTable("Devices"); // До таблиці

        builder.HasOne(d => d.Experiment).WithMany().HasForeignKey(k => k.ExperimentId); // відображення звязку таблиці "один до багатьох"
    }
}
