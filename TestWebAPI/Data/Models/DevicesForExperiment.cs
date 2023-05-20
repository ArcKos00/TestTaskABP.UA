namespace TestWebAPI.Data.Models;

// Сутність Девайсу
public class DevicesForExperiment
{
    public string Id { get; set; } = null!; // Його айді
    public string Token { get; set; } = null!; // Токен за яким здійсняється запит
    public int ExperimentId { get; set; } // ForeignKey для посилання на Експеримент
    public Experiment? Experiment { get; set; } // Об'єкт експерименту (для витягування даних про експеримент у якому приймає участь)
    public string Value { get; set; } = null!; // саме значення яке призначено екпериментом
}
