using TestWebAPI.Data.Dtos;

namespace TestWebAPI.Service.Interfaces;

// Інтерфейс сервіса для обробки бізнеслогіки
public interface IService
{
    // Метод обробки події при спробі отримати значення експерименту button_color
    public Task<Response<string>> ButtonColor(string token);
    // Метод обробки події при спробі отримати значення експерименту price
    public Task<Response<int>> Price(string token);
    // Метод для повернення статистики У вигляді Json обєкту
    public Task<string> GetStatistic();
}
