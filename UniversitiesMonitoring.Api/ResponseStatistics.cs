namespace UniversitiesMonitoring.Api;

/// <summary>
/// Сводка статистики ответов на HEAD заросы с сервиса
/// </summary>
public class ResponseStatistics
{
   private const int QueueCapacity = 20;

   private readonly List<long> _responsesTime = new(QueueCapacity);

   public ResponseStatistics(IEnumerable<long> initData)
   { 
      _responsesTime.AddRange(initData);
   }
   
   public void AddResponseData(long responseTime) => _responsesTime.Add(responseTime);

   /// <summary>
   /// Проверяет по времени ответа потенциальную атаку
   /// </summary>
   /// <param name="responseTime">Время ответа</param>
   /// <returns>true, если сервис атакован</returns>
   public bool? IsPotentialAttack()
   {
      if (_responsesTime.Count < 6) return null;

      var doubleAvg = _responsesTime.ToArray()[2..].Average() * 2;
      var isUnderAttack = true;

      for (var i = 0; i < 3; i++) isUnderAttack &= _responsesTime[i] > doubleAvg;

      return isUnderAttack;
   }
}