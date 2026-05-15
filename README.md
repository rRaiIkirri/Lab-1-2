# Messaging System API

ASP.NET Core Web API для мінімального месенджера з підтримкою відстеження статусів через підтвердження клієнта (Client Acknowledgements):

`Created -> Sent -> Delivered -> Read`

API зберігає відправлені повідомлення зі статусом `Sent`. Клієнтські додатки оновлюють подальші статуси життєвого циклу через ендпоінт `PATCH /api/messages/{messageId}/status`.

## Стек технологій

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core + SQLite
- xUnit (інтеграційні тести)

## Структура проєкту

- `Messaging.Api/Models` — сутності бази даних (`User`, `Conversation`, `Message` та `MessageStatus`).
- `Messaging.Api/Data` — налаштування бази (EF Core `DbContext`) та міграції.
- `Messaging.Api/Services` — бізнес-логіка та валідація.
- `Messaging.Api/Controllers` — контролери для обробки HTTP-запитів.
- `Messaging.Api.Tests/Integration` — інтеграційний тест повного циклу роботи.
- `MessagingSystem.postman_collection.json` — колекція Postman для тестування API.

## Реалізовані функції

- Створення користувачів.
- Відправка повідомлень між користувачами.
- Збереження даних (користувачі, чати, повідомлення) у базі SQLite.
- Отримання історії листування користувача.
- Відстеження статусів повідомлень (`Sent -> Delivered -> Read`).
- Обробка помилок (неіснуючі користувачі, порожні повідомлення, дублювання імен, некоректні переходи статусів).

## Як запустити локально

Переконайтеся, що у вас встановлено ASP.NET Core Runtime версії 8.x:

```bash
dotnet --list-runtimes
