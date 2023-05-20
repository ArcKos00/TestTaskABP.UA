using TestWebAPI.Data.Models;

namespace TestWebAPI.Service.Interfaces
{
    // Інтерфейс для роботи з таблицею з девайсами
    public interface IDeviceRepository
    {
        // Метод додавання нового значення до таблиці
        Task<string> Add(string token, int experiment, string value);
        // Метод отримання деваййсу за токеном
        Task<DevicesForExperiment?> Get(string token);
        // Метод оновлення значення айді експерименту
        Task<bool> UpdateExperiment(string token, int experiment);
        // Метод оновлення значення експерименту
        Task<bool> UpdateValue (string token, string value);
        // Метод видаоення обєкта за його токеном
        Task<bool> Delete(string token);
    }
}
