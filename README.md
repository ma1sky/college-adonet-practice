# Практика ADO.NET и SQL Server в Codespaces

В проекте используются .NET 10, Microsoft.Data.SqlClient 7.1.0 и SQL Server 2022 Developer.
Полная [инструкция для преподавателя и студентов](ИНСТРУКЦИЯ.md) описывает создание Codespace,
подключение к базе, бесплатные квоты и выполнение примеров.

## Материалы курса

- [Четыре лекции по ADO.NET](lectures)
- [Исходный код C#](practice/Program.cs)
- [Подготовка учебной базы](sql/01-init.sql)
- [Проверочные SQL-запросы](sql/02-check.sql)

## Быстрый запуск

1. Создайте собственный репозиторий из шаблона преподавателя. Если кнопка **Use this template**
   пока отсутствует, нажмите **Fork** и создайте копию под своим личным аккаунтом.
2. Откройте Code → Codespaces → New with options и выберите машину с 2 ядрами.
3. Дождитесь окончания подготовки окружения.
4. В терминале из корня репозитория выполните:

```bash
dotnet run --project practice -- count
dotnet run --project practice -- list
dotnet run --project practice -- list "ИСП-21"
dotnet run --project practice -- sql sql/02-check.sql
```

На новой базе ожидается 3 студента, из них 2 в группе ИСП-21.

Для повторного выполнения подготовки без удаления существующих таблиц:

```bash
dotnet run --project practice -- init
```

В примерах лекций используйте `string connectionString = LabDb.ConnectionString;`.
Сохраните файл practice/LabDb.cs, когда будете заменять practice/Program.cs своим кодом.
После замены Program.cs команды init, list, count и sql перестанут существовать,
если вы не сохраните их обработку. Для возврата к учебному запускателю сначала
сохраните свою работу и восстановите исходный Program.cs средствами Git.

Пароль в docker-compose.yml — демонстрационный, только для изолированной учебной среды.
Порт SQL Server не опубликован наружу. Не используйте здесь реальные данные и пароли.

В конце занятия сделайте commit и push, затем остановите Codespace.
SQL Server хранит базу в Docker volume этой среды. База не включается в Git;
при удалении Codespace она теряется. Сохраняйте SQL-скрипты и код.
