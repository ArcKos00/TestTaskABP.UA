Тестове завдання від компанії ABP.UA
Перед першим ввімкненням потрибно знайти файл appsettings.json, який зберігається у корені проекту. Після чого замінити строку підключення та дані користувача для входу у mssql на свої
Після вводу даних підключення до бази даних можна запускати проект
Проект сам додасть деякі початкові значення до бази даних  
Через swagger є доступ до трьох http-методів
1) Отримання даних для девайсу у першому експерименті, якщо вони відсутні то сворює нові результати експерименту
2) Отримання даних для девайсу у другому експерименті, якщо вони відсутні то сворює нові результати експерименту
3) Метод для отримання даних статистики у форматі Json

Коли проект зпущено, у корені репозиторію, можна запустити файл index.html який відобразить на сторінці браузера статистику по експериментах у вигляді json-об'єкту
Також у корені репозиторія є картинка яка відабражає схему бази даних