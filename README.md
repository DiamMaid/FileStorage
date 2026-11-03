# FileStorage API

Тестовое задание: кроссплатформенный API-сервис для загрузки, хранения и скачивания файлов с JWT-авторизацией.

## Стек

- ASP.NET Core .NET 8
- Clean Architecture (Domain → Application → Api)
- JWT Bearer аутентификация
- PBKDF2 хеширование паролей (System.Security.Cryptography)
- Локальное файловое хранилище
- Serilog (консоль + файл)
- Docker + Docker Compose

## Запуск

```bash
docker compose up --build
```

API: `http://localhost:8080`
Swagger: `http://localhost:8080/swagger`

## Эндпоинты

| Метод | Путь | Описание |
|---|---|---|
| POST | `/api/auth/register` | Регистрация |
| POST | `/api/auth/login` | Вход (возвращает JWT) |
| POST | `/api/files/upload` | Загрузка файла |
| GET | `/api/files` | Список файлов |
| GET | `/api/files/download/{id}` | Скачать файл |
| DELETE | `/api/files/{id}` | Удалить файл |

## Проверка через Swagger

1. `POST /api/auth/register` → регистрация, получить токен
2. Нажать **Authorize** → ввести `Bearer {токен}`
3. `POST /api/files/upload` → загрузить файл
4. `GET /api/files` → посмотреть список
5. `GET /api/files/download/{id}` → скачать
6. `DELETE /api/files/{id}` → удалить