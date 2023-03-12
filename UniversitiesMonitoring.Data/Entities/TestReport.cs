namespace UniversityMonitoring.Data.Entities;

public class TestReport
{
    public TestReport(string testFrom, long? headTime, long? pingTime)
    {
        TestFrom = testFrom;
        HeadTime = headTime;
        PingTime = pingTime;
    }

    /// <summary>
    /// Откуда была выполнена проверка
    /// </summary>
    public string TestFrom { get; }

    /// <summary>
    /// Время, за которое был получен ответ на HEAD запрос. Null, если запрос был не успешен. Измеряется в миллисекундах
    /// </summary>
    public long? HeadTime { get; }

    /// <summary>
    /// Время пинга. Null, если не успешно. Измеряется в миллисекундах
    /// </summary>
    public long? PingTime { get; }
}