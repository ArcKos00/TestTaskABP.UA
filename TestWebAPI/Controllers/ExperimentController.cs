using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestWebAPI.Data.Dtos;
using TestWebAPI.Service.Interfaces;

namespace TestWebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
// Клас Контролера який направляє запити на обробники подій
public class ExperimentController : ControllerBase
{
    private readonly IService _service; // Екземпляр сервісу для обробки
    public ExperimentController(IService service)
    {
        _service = service;
    }

    // Метод контролера який викликає обробку запиту отримання кольору для кнопки
    [HttpGet]
    [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)] // Запис, яку відповідь очікувати
    public async Task<IActionResult> Button_Color(string token) // Метод-контроллер для запуску обробки девайсу для експерименту button_color
    {
        var result = await _service.ButtonColor(token); // Виклик метода обробника
        return Ok(result); // Повернення результату та статус коду 200(ОК)
    }

    // Метод контролера який викликає обробку запиту отримання ціни 
    [HttpGet]
    [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)] // Запис, яку відповідь очікувати
    public async Task<IActionResult> Price(string token)// Метод-контроллер для запуску обробки девайсу для експерименту price
    {
        var result = await _service.Price(token); // Виклик метода обробника
        return Ok(result); // Повернення результату та статус коду 200(ОК)
    }

    // Метод контролера який викликає обробку запиту отримання статистики у вигляді строки jsonі
    [HttpGet]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetStatistic() // Отримання статистики
    {
        var result = await _service.GetStatistic(); // Виклик метода обробника
        return Ok(result); // Повернення результату та статус коду 200(ОК)
    }
}
