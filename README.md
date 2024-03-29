# BookOrganiser
Приложение для каталогизации вашей домашней библиотеки.
Базовые принципы программы и интерфейс в некоторой степени похожи на программу LibaBook, однако, в отличие от последней, программа  значительно упрощена и ускорена.

# Для работы требуется установленный PostgreSQL!
PostgreSQL - свободная и бесплатная система управления базами данных.
Вы можете скачать PostgreSQL по ссылке https://www.postgresql.org/download/ (Бесплатно)

### Основные возможности
* Создание, изменение, удаление карточек книг
* Удобное добавление книг, возможность "заморозить" значения атрибутов при добавлении нескольких книг сразу, нажав правой кнопкой мыши на название атрибута в окне "Add Book"
* Поиск книг в интернете (на сайте ozon.ru) и автоматическое добавление сведений со станицы книги
* Поиск книг в БД, как быстрый (по всем атрибутам), так и продвинутый (с возможностью задать значение в каждом атрибуте)
* Создание или поиск базы данных pgSQL при первом запуске
* Замена значений в БД
 
### Планы на краткосрочную перспективу
* Сортировка и группировка по аттрибутам пользователя
* Возможность менять местами и скрывать столбцы
* Улучшения UI

### Планы на среднесрочную перспективу
* Добавление дополнительных источников для поиска книг в интернете
* Возможность изменить атрибуты нескольких книг сразу
* Возможность удалить несколько книг сразу
* Возможность перевода приложения на русский и другие языки

### Планы на долгосрочную перспективу
* Отвязать программу от PostgreSQL (возможность работать без установленной postgreSQL)
* Импорт из других баз данных, excel и т.д.
* Экспорт в другие БД, html и т.д.
* Поиск книг по штрих-коду

### Предложения, пожелания, комментарии
* Принимаются по адресу shepilov.dmitrii+BO@gmail.com

### Сторонние библиотеки
* Npgsql.4.1.1 - https://github.com/npgsql

