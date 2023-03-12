# UniversitiesMonitoring

**Статус тестирования:** 

![Упс](https://github.com/predprof-1551/UniversitiesMonitoring/actions/workflows/unit-testing.yaml/badge.svg)

## Инструкция по установке

### Предварительные требования

Нужно иметь что-либо из перечисленного:
- [Docker Engine](https://docs.docker.com/get-docker/) и [Docker Compose](https://docs.docker.com/compose/install/) 
- Приложение [Docker Desktop](https://docs.docker.com/desktop/)

### Алгоритм установки
1. Скачайте последний релиз [по ссылке](https://github.com/predprof-1551/UniversitiesMonitoring/releases/latest/) в удобном Вам формате и распакуйте его
2. Перейдите в терминале в директорию, куда Вы распаковали архив и зайдите в папку `UniversitiesMonitoring-x.x.x`, где x.x.x - версия релиза
3. Создайте `.env` файл со следующей структурой
```
MYSQL_PASSWORD=<Пароль от БД>
JWT_SECRET=<JWT секрет для создания токенов>
EMAIL_ADDRESS=<Почта, с которой будут приходить уведмоления>
EMAIL_PASSWORD=<Пароль от почты>
SMTP_HOST=<SMTP сервер для почты>
SMTP_PORT=<Порт SMTP сервера (без SSL)> 
TELEGRAM_TOKEN=<Токен Telegram бота>
TELEGRAM_CHAT_ID=<Id Telegram чата>
RUNTIME_ENV=Production
```
4. Выполните команду:
```shell
docker compose up -d
```

После этого начнется сборка проекта и вследствие этого запуск

