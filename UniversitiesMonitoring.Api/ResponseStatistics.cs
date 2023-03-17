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
    /// <returns>true, если сервис атакован</returns>
    public bool? IsPotentialAttack()
    {
        if (_responsesTime.Count < 6) return null;

        var responseTimesForAvg = _responsesTime.ToArray()[..^3];
        var doubleAvg = responseTimesForAvg.Average() * 2;
        var isUnderAttack = true;

        for (var i = _responsesTime.Count - 1; i > _responsesTime.Count - 4; i--)
        {
            isUnderAttack &= _responsesTime[i] > doubleAvg;
        }

        return isUnderAttack;
    }
}