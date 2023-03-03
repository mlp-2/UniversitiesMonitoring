using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.Module.Networking;

public class TestReportBuilder
{
    private string? _testFrom;
    private long? _headResponseTime;
    private long? _pingResponseTime;

    public void WithTestLocation(string testFrom) => _testFrom = testFrom;

    public void WithHeadResult(long headResponseTime) => _headResponseTime = headResponseTime;

    public void WithPingResult(long pingResponseTime) => _pingResponseTime = pingResponseTime;

    public TestReport Build()
    {
        if (_testFrom == null) throw new InvalidOperationException("Location is required");

        return new TestReport(_testFrom, _headResponseTime, _pingResponseTime);
    }
}