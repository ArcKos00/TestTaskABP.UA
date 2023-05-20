using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace TestWebAPI.Middleware;

// Глобальний фільтр для відлову випадкових виключень які не оброблені
public class GlobalExceptionFilter : IExceptionFilter
{
    // При виникненні будьякого виключення яке не опрацьоване
    public void OnException(ExceptionContext context)
    {
        var problemDetailt = new ValidationProblemDetails // Створення звіту про помилку
        {
            Instance = context.HttpContext.Request.Path, // Записує за яким шляхом виникло виключення
            Status = StatusCodes.Status400BadRequest, // Встановлює статус код 400 
            Detail = context.Exception.Message // Записує текст виключення
        };

        context.Result = new BadRequestObjectResult(problemDetailt); // Створює об'єкт відповіді badrequest
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; // Встановлює статус відповіді

        context.ExceptionHandled = true; // Говорить що виключення опрацьовано
    }
}
