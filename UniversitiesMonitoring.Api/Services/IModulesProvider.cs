using UniversitiesMonitoring.Api.Entities;
using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

public interface IModulesProvider
{
    /// <summary>
    /// Тестирует сервис 
    /// </summary>
    /// <param name="service">Инстанс сервиса</param>
    /// <returns>Массив с результатами теста из разных городов</returns>
    Task<IEnumerable<TestReport>> TestServiceAsync(UniversityService service);
    
    /// <summary>
    /// Регистрирует модуль
    /// </summary>
    /// <param name="url">URL, куда можно делать запросы</param>
    Task<ulong> CreateModuleAsync(string url);
    
    /// <summary>
    /// Удаляет модуль
    /// </summary>
    /// <param name="id">ID модуля</param>
    Task DeleteModuleAsync(ulong id);
    
    /// <summary>
    /// Получает модули
    /// </summary>
    /// <returns>Массив модулей</returns>
    IAsyncEnumerable<ModuleEntity> GetModulesAsync();
}
