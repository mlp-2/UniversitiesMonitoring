namespace UniversitiesMonitoring.Api;

/// <summary>
/// Сводка статистики ответов на HEAD заросы с сервиса
/// </summary>
public class ResponseStatistics
{
   private const int QueueCapacity = 20;

   private readonly List<long> _responsesTime = new(QueueCapacity);

   public void AddResponseData(long responseTime) => _responsesTime.Add(responseTime);
   
   /// <summary>
   /// Проверяет по времени ответа потенциальную атаку
   /// </summary>
   /// <param name="responseTime">Время ответа</param>
   /// <returns>true, если сервис атакован</returns>
   public bool? IsPotentialAttack(long responseTime) =>
      _responsesTime.Count > 4 ? responseTime >= _responsesTime.Average() * 2 : null;
}