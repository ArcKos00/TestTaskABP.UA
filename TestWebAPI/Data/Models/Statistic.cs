namespace TestWebAPI.Data.Models;

// Статистика
public class Statistic
{
    public List<ExperimentStatistic> Experiments { get; set; } = new List<ExperimentStatistic>();
}
