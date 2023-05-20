namespace TestWebAPI.Data.Models;

// Сутність Експерименту
public class Experiment
{
    public int Id { get; set; } // Його айді
    public string Name { get; set; } = null!; // Назва Експерименту
    public List<DevicesForExperiment> Devices { get; set; } = new List<DevicesForExperiment>(); // Девайси які приймають участь у даному екперименту
}
