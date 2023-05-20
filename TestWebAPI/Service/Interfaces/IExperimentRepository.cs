using TestWebAPI.Data.Models;

namespace TestWebAPI.Service.Interfaces;

// Інтерфейс для роботи з таблицею з експериентами
public interface IExperimentRepository
{
    // Метод додавання нового значення до таблиці
    Task<int> Add(string experiment);
    // Метод отримання експерименту за айді
    Task<Experiment?> GetExp(string id);
    // Метод отримання екперименту за назвою
    Task<int> GetExpByName(string name);
    // Метод отримання екпериментів для статистики
    Task<List<Experiment>?> GetExperiments();
    // Метод оновлення назви експерименту
    Task<bool> UpdateExperiment(string token, string experiment);
    // Метод видаоення обєкта за його айді
    Task<bool> Delete(string token);
}
