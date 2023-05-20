namespace TestWebAPI.Data.Dtos;

// Об'єкт відповіді
public class Response<T> // Дженерік тип тому що у відповіді можуть бути і цифри і строки
{
    public string Key { get; set; } = null!; // Ключ
    public T Value { get; set; } = default!; // Значення
}
