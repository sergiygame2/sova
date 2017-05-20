# sova

Sport4Everyone — OpenSource проект для пошуку спортивних залів для занять кросфітом. Проект написаний на c#, .net core framework. Використовується СУБД MySql.
	
  # Що потрібно встановити?
1. .net core 1.04, netcoreapp1.1 -> https://www.microsoft.com/net/download/core
2. MySQL Community Server -> https://dev.mysql.com/downloads/mysql/ 
3. Для зручності розробки з використанням потрібних інструментів можна використовувати:
 - Visual Studio 2017
 - JetBrains Rider
- Visual Studio Code
4. Для тестування REST Api - Postman
	# Інструкція налаштувань
  Потрібно виконати такі команди з папки, що містить файл проекту (.csproj)
1. dotnet restore — для встановлення усіх необхідних пакетів, потрібних для проекту
2. dotnet run — команда для запуску. Усі необіхдні міграції для БД будуть застосовані автоматично.
  
