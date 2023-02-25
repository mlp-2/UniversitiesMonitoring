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
}
