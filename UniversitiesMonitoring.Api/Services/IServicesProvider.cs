using UniversitiesMonitoring.Api.Entities;
using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

public interface IServicesProvider
{
    /// <summary>
    /// Получает статистику ответов
    /// </summary>
    /// <returns>Статистика ответов</returns>
    ResponseStatistics GetResponseStatistic(UniversityService service);
    
    /// <summary>
    /// Получает сервис по ID
    /// </summary>
    /// <param name="serviceId">ID сервиса</param>
    /// <returns>Сервис. Если не найден, то null</returns>
    Task<UniversityService?> GetServiceAsync(ulong serviceId);
    
    /// <summary>
    /// Получает Uptime сервиса в процентах
    /// </summary>
    /// <param name="serviceId">ID сервиса</param>
    /// <returns>Проценты от общего времени, когда сервис был онлайн</returns>
    double? GetServiceUptime(ulong serviceId);
    
    /// <summary>
    /// Получает все сервисы
    /// </summary>
    /// <param name="universityId">Опционально фильтрует по университету</param>
    Task<IEnumerable<UniversityService>> GetAllServicesAsync(ulong? universityId = null);
    
    /// <summary>
    /// Подписывает пользователя на сервис
    /// </summary>
    /// <param name="user">Инстанс пользователя</param>
    /// <param name="service">Инстанс сервиса</param>
    Task SubscribeUserAsync(User user, UniversityService service);
    
    /// <summary>
    /// Отписывает пользователя от сервиса
    /// </summary>
    /// <param name="user">Инстанс пользователя</param>
    /// <param name="service">Инстанс сервиса</param>
    /// <returns></returns>
    Task UnsubscribeUserAsync(User user, UniversityService service);
    
    /// <summary>
    /// Получает университет по ID
    /// </summary>
    /// <param name="universityId">ID университета</param>
    /// <returns>Возвращает университет, если найден</returns>
    Task<University?> GetUniversityAsync(ulong universityId);
    
    /// <summary>
    /// Получает все университеты в коллекции, позволяющей выполнять SQL запросы
    /// </summary>
    IQueryable<University> GetAllUniversities();

    /// <summary>
    /// Обновляет состояние сервиса 
    /// </summary>
    /// <param name="service">Инстанс сервиса</param>
    /// <param name="isOnline">True, если онлайн</param>
    /// <param name="forceSafe">True, если надо сохранить изменения</param>
    /// <param name="updateTime">Время обноавления опционально</param>
    /// <returns>true, если сервис изменил свое состояние</returns>
    Task<bool> UpdateServiceStateAsync(UniversityService service,
        bool isOnline,
        bool forceSafe,
        DateTime? updateTime = null);

    /// <summary>
    /// Добавляет статистику сервиса 
    /// </summary>
    /// <param name="service">Инстанс сервиса</param>
    /// <param name="serviceStats">Его статистика</param>
    Task AddServiceStatisticsAsync(UniversityService service,
        ServiceStatisticsEntity serviceStats,
        bool forceSave = false);

    /// <summary>
    /// Создает комментарий для сервиса
    /// </summary>
    /// <param name="service">Инстанс сервиса</param>
    /// <param name="author">Инстанс автора</param>
    /// <param name="comment">Объект, описывающий комментарий</param>
    Task LeaveCommentAsync(UniversityService service, User author, Comment comment);
    
    /// <summary>
    /// Создает репорт
    /// </summary>
    /// <param name="service">Инстанс сервиса</param>
    /// <param name="issuer">Инстанс автора</param>
    /// <param name="report">Объект, описывающий репорт</param>
    Task CreateReportAsync(UniversityService service, User issuer, Report report);
    
    /// <summary>
    /// Получает репорт сервиса
    /// </summary>
    /// <param name="reportId">ID репорта</param>
    Task<UniversityServiceReport?> GetReportAsync(ulong reportId);
    
    /// <summary>
    /// Создает статистику Excel для сервиса
    /// </summary>
    /// <param name="serviceId">ID сервиса</param>
    /// <param name="offset">Сдвиг в минутах относительно UTC</param>
    /// <returns>Массив байтов, описывающий Excel файл</returns>
    Task<byte[]> CreateExcelReportAsync(ulong serviceId, int offset);
    
    /// <summary>
    /// Получение всех репортов
    /// </summary>
    IEnumerable<UniversityServiceReport> GetAllReports();
    
    /// <summary>
    /// Получение репортов, которые были созданы во время офлайна сервиса
    /// </summary>
    /// <param name="service">Сервис</param>
    IEnumerable<UniversityServiceReport> GetReportsByOffline(UniversityService service);
    
    /// <summary>
    /// Разрешение репорта
    /// </summary>
    /// <param name="report">Инстанс репорта</param>
    Task SolveReportAsync(UniversityServiceReport report);
}