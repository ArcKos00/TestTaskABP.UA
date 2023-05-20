namespace TestWebAPI.Service.Interfaces;

// Інтерфейс для можливості зробити mock для зручного тестування
public interface ICounter
{
    int Count(string experiment); // Повертає кількість девайсів у експерименті (як заплановано)
}
