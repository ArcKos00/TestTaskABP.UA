using TestWebAPI.Service.Interfaces;
using TestWebAPI.Exceptions;
using Newtonsoft.Json;
using TestWebAPI.Data.Models;
using TestWebAPI.Data.Dtos;

namespace TestWebAPI.Service;

// Клас сервіса для обробки бізнеслогіки
// Класи були розділені на сервіс та репозиторії задля того щоб було зручно робити тестування. Для того щобб не робити mock цілої бази даних та не робіити інтеграційні тести
public class Service : IService
{
    private readonly IDeviceRepository _deviceRepository; // Для роботи з таблицею девайсів
    private readonly IExperimentRepository _experimentRepository; // для роботи з таблицею експериментів
    private readonly Random _random; // Клас рандом для можливості отримати псевдовипадкове значення для розподілу по ціні
    public Service(IDeviceRepository deviceRepository, IExperimentRepository experimentRepository, Random random) // Отримання та присвоєння обєктів для праці
    {
        _deviceRepository = deviceRepository;
        _experimentRepository = experimentRepository;
        _random = random;
    }

    // Метод обробки події при спробі отримати значення експерименту button_color
    public async Task<Response<string>> ButtonColor(string token)
    {
        // Навіщо створювати у двох місцях якщо можна в одному?
        var response = new Response<string>(); // Створюю об'єкт відповіді у який потім додам відповідь
        var experiment = "button_color"; // Назва експерименту для обробника подій
        try // Потенційно небезпечний блок коду
        {
            var result = await _deviceRepository.Get(token); // Спроба отримати девайс за токеном
            if (result == null) // Якщо девайс не знайдений 
            {
                throw new BusinessException("NotFound"); // Кидаємо своє виключення для переходу в блок catch
            }
            if (result.Experiment?.Name != experiment) // Якщо цей девайс вже приймає участь у іншому експерименті
            {
                throw new Exception("The device is already being used in another exam"); // Кинути виключення для завершення запиту та переходу до глобального фільтру виключень
            }

            response.Key = result.Experiment?.Name!;
            response.Value = result.Value; // Якщо усе успішно то призначити отримані значення об'єкту відповіді
        }
        catch (BusinessException) // Обробка власного виключення
        {
            var experimentId = await _experimentRepository.GetExpByName(experiment); // Шукаємо чи є взагалі такий експеримент у базі
            var value = string.Empty; // Для того щоб не створювати значення у конструкціі switch/case створено тут

            switch ((_deviceRepository as ICounter)!.Count(experiment) % 3) // Отримання кількості девайсів у експерименті
            {                                                               // Для рівномірного розподілу використана конструкція switch/case яка приймає залишок від ділення кількості девайсів
                case (0): // для залишку 0 значення #FF0000
                    value = "#FF0000";
                    break;
                case (1): // для залишку 1 значення #00FF00
                    value = "#00FF00";
                    break;
                case (2): // для залишку 2 значення #0000FF
                    value = "#0000FF";
                    break;
            }
            await _deviceRepository.Add(token, experimentId, value); // Направлення на створення девайсу у бд з такими параметрами 
            response.Key = experiment;
            response.Value = value; // Якщо усе успішно то призначити отримані значення об'єкту відповіді
        }

        return response; // Повертає обєкт відповіді
    }

    // Метод обробки події при спробі отримати значення експерименту price
    public async Task<Response<int>> Price(string token)
    {
        // Навіщо створювати у двох місцях якщо можна в одному?
        var response = new Response<int>(); // Створюю об'єкт відповіді у який потім додам відповідь
        var experiment = "price"; // Назва експерименту для обробника подій
        try // Потенційно небезпечний блок коду
        {
            var result = await _deviceRepository.Get(token); // Спроба отримати девайс за токеном
            if (result == null) // Якщо девайс не знайдений 
            {
                throw new BusinessException("Not Found"); // Кидаємо своє виключення для переходу в блок catch
            }
            if (result.Experiment?.Name != experiment) // Якщо цей девайс вже приймає участь у іншому експерименті
            {
                throw new Exception("The device is already being used in another exam"); // Кинути виключення для завершення запиту та переходу до глобального фільтру виключень
            }

            response.Key = result.Experiment!.Name;
            response.Value = int.Parse(result.Value); // Якщо усе успішно то призначити отримані значення об'єкту відповіді
        }
        catch (BusinessException) // Обробка власного виключення
        {
            var experimentId = await _experimentRepository.GetExpByName(experiment); // Шукаємо чи є взагалі такий експеримент у базі
            var value = _random.Next(100); // Беремо псевдовипадкове значення в межах від 0 до 100 без вклчення 100
            if (value < 75)
            {
                value = 10; // 75% отримують ціну 10
            }
            else if (value >= 75 && value < 85)
            {
                value = 20; // ще 10% отримують 20
            }
            else if (value >= 85 && value < 90)
            {
                value = 50; // ще 5% отримують 50
            }
            else
            {
                value = 5; // ще 10% отримують 5
            }

            await _deviceRepository.Add(token, experimentId, value.ToString()); // Направлення на створення девайсу у бд з такими параметрами 
            response.Key = experiment;
            response.Value = value; // Якщо усе успішно то призначити отримані значення об'єкту відповіді
        }

        return response; // Повертає обєкт відповіді
    }

    // Метод для повернення статистики У вигляді Json обєкту
    public async Task<string> GetStatistic()
    {
        var experiments = await _experimentRepository.GetExperiments() ?? throw new Exception("Experiments not found"); // Отримання даних про експерименти
        var statistic = new Statistic();
        var options = new Dictionary<string, int>();

        // Пробіг по кожному експерименту
        foreach (var experiment in experiments)
        {
            var counter = 0; // Лічильник загальної кількості девайсів

            // Пробіг по кожному девайсу в еспермиенті
            foreach (var item in experiment.Devices)
            {
                if (options.TryGetValue(item.Value, out int value))
                {
                    options[item.Value] = ++value;
                }
                else
                {
                    options.Add(item.Value, 1);
                }
                counter++;
            }

            statistic.Experiments.Add(new ExperimentStatistic
            {
                Experiment = experiment.Name,
                CountDevices = counter,
                Options = options
            });
            options = new Dictionary<string, int>();
        };

        var result = JsonConvert.SerializeObject(statistic, Formatting.Indented); // Перетворення оюєкту на строку Json

        return result; // Повернення
    }
}
