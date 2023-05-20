using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestWebAPI.Data.Models;

namespace TestWebAPI.Data.Configurations;

// Клас конфігурації для Experiment
public class ExperimentConfig : IEntityTypeConfiguration<Experiment>
{
    // Метод конфігурації
    public void Configure(EntityTypeBuilder<Experiment> builder)
    {
        builder.HasKey(k => k.Id); // Задати PriaryKey
        builder.ToTable("Experiment"); // До таблиці

        builder.Property(p => p.Id).UseHiLo(); // Використовую для забезпечення унікальності

        builder.HasMany(m => m.Devices).WithOne(o => o.Experiment).HasForeignKey(o => o.ExperimentId); // Відображення звязку "багато до одного"
    }
}
