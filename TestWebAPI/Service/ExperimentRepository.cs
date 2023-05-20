using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data.Models;
using TestWebAPI.Data;
using TestWebAPI.Service.Interfaces;
using TestWebAPI.Exceptions;

namespace TestWebAPI.Service;

// Клас для роботи з таблицею Експериментами
public class ExperimentRepository : IExperimentRepository
{
    private readonly AppDbContext _context; // Контекст бази даних
    public ExperimentRepository(AppDbContext context)
    {
        _context = context; // Передаємо сінглтон контексту, бо він один на мсс
    }

    // Метод додавання нового значення до таблиці 
    public async Task<int> Add(string experiment)
    {
        var item = await _context.Experiment
            .AddAsync(new Experiment // Створюємо об'єкт на основі значень які отримали та одразу намагаємось додати його до бд
            {
                Name = experiment
            });

        await _context.SaveChangesAsync(); // Зберігаємо зміни
        return item.Entity.Id; // Повертаємо айді створеного об'єкта
    }

    // Метод видаоення обєкта за його айді
    public async Task<bool> Delete(string id)
    {
        var item = await GetExp(id) ?? throw new BusinessException("Not Found"); // Спроба отримати девайс за токеном, якшо нічого не знайдено то кидається виключення

        _context.Entry(item).State = EntityState.Deleted; // Встановлення стану даного поля на видалений, після збереження змін запис стане недоступним
        await _context.SaveChangesAsync(); // Зберігаємо зміни
        return true; // Повертаємо відповідь що видалення пройшло успішно
    }

    // Метод отримання екперименту за айді
    public async Task<Experiment?> GetExp(string id)
    {
        return await _context.Experiment.FirstOrDefaultAsync(f => f.Id.Equals(id)); // Пошук по базі першого співпадіння токена з запитом. Повертає або знайдений обєкт або нулл
    }

    // Метод отримання екпериментів для статистики
    public async Task<List<Experiment>?> GetExperiments()
    {
        return await _context.Experiment.Include(i => i.Devices).ToListAsync(); // Повертає усі експерименти з девайсами
    }

    // Метод отримання екперименту за назвою
    public async Task<int> GetExpByName(string name)
    {
        var result = await _context.Experiment.FirstOrDefaultAsync(f => f.Name.Equals(name)); // Пошук по базі першого співпадіння токена з запитом. Повертає або знайдений обєкт або нулл
        return result == default ? 0 : result.Id; // повертає 0 якщо нічого не знайдено у базі
    }

    // Метод оновлення назви експерименту
    public async Task<bool> UpdateExperiment(string id, string experiment)
    {
        var item = await GetExp(id) ?? throw new BusinessException("Not Found"); // Спроба отримати девайс за токеном, якшо нічого не знайдено то кидається виключення

        item.Name = experiment; // Оновлення значення
        _context.Entry(item).CurrentValues.SetValues(item); // Внесення змін до бд
        await _context.SaveChangesAsync(); // Збереження змін
        return true; // Повертає відповідь що оновлення пройшло успішн
    }
}
