using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Data.Models;
using TestWebAPI.Exceptions;
using TestWebAPI.Service.Interfaces;

namespace TestWebAPI.Service;

// Клас для роботи з таблицею Девайсів
public class DeviceRepository : IDeviceRepository, ICounter
{
    private readonly AppDbContext _context; // Контекст бази даних
    public DeviceRepository(AppDbContext context)
    {
        _context = context; // Передаємо сінглтон контексту, бо він один на мсс
    }

    // Метод додавання нового значення до таблиці 
    public async Task<string> Add(string token, int experimentId, string value)
    {
        var item = await _context.Devices   // Створюємо об'єкт на основі значень які отримали та одразу намагаємось додати його до бд
            .AddAsync(new DevicesForExperiment
            {
                Id = Guid.NewGuid().ToString(),
                Token = token,
                ExperimentId = experimentId,
                Value = value
            });

        await _context.SaveChangesAsync(); // Зберігаємо зміни
        return item.Entity.Id; // Повертаємо айді створеного об'єкта
    }

    // Метод видаоення обєкта за його токеном
    public async Task<bool> Delete(string token)
    {
        var item = await Get(token) ?? throw new BusinessException("Not Found"); // Спроба отримати девайс за токеном, якшо нічого не знайдено то кидається виключення
        
        _context.Entry(item).State = EntityState.Deleted; // Встановлення стану даного поля на видалений, після збереження змін запис стане недоступним
        await _context.SaveChangesAsync(); // Зберігаємо зміни 
        return true; // Повертаємо відповідь що видалення пройшло успішно
    }

    // Метод отримання деваййсу за токеном
    public async Task<DevicesForExperiment?> Get(string token)
    {
        return await _context.Devices
            .Include(i => i.Experiment) // Витягує разом з даними про експеримент
            .FirstOrDefaultAsync(f => f.Token.Equals(token)); // Пошук по базі першого співпадіння токена з запитом. Повертає або знайдений обєкт або нулл
    }

    // Метод оновлення значення експерименту
    public async Task<bool> UpdateValue(string token, string value)
    {
        var item = await Get(token) ?? throw new BusinessException("Not Found"); // Спроба отримати девайс за токеном, якшо нічого не знайдено то кидається виключення

        item.Value = value; // Оновлення значення 
        _context.Entry(item).CurrentValues.SetValues(item); // Внесення змін до бд
        await _context.SaveChangesAsync(); // Збереження змін
        return true; // Повертає відповідь що оновлення пройшло успішно
    }

    // Метод оновлення значення айді експерименту
    public async Task<bool> UpdateExperiment(string token, int experimentId)
    {
        var item = await Get(token) ?? throw new BusinessException("Not Found"); // Спроба отримати девайс за токеном, якшо нічого не знайдено то кидається виключення

        item.ExperimentId = experimentId; // Оновлення значення 
        _context.Entry(item).CurrentValues.SetValues(item); // Внесення змін до бд
        await _context.SaveChangesAsync(); // Збереження змін
        return true; // Повертає відповідь що оновлення пройшло успішно
    }

    // Метод інтерфейсу ICounter. Зроблений для зручності тестування та зручності підрахунку кількості девайсів
    public int Count(string experiment)
    {
        return _context.Devices.Where(w => w.Experiment!.Name.Equals(experiment)).Count(); // Підрахунок кількості девайсів у експерименті
    }
}
