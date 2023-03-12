using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.MonitoringService.Services.Inspecting;

internal class UniversityServiceInspector
{
    private readonly IServiceInspector _serviceInspector;
    private bool _isOnline;

    public ulong ServiceId => Service.ServiceId;

    public UniversityServiceEntity Service
    {
        get => _service;
        set
        {
            _isOnline = value.IsOnline;
            _service = value;
        }
    }

    private UniversityServiceEntity _service;

    public UniversityServiceInspector(IServiceInspector inspector, UniversityServiceEntity serviceEntity)
    {
        _serviceInspector = inspector;
        _service = serviceEntity;
        _isOnline = Service.IsOnline;
    }

    public async Task UpdateStateAsync(UpdateBuilder reportBuilder)
    {
        var nowStatus = await _serviceInspector.InspectServiceAsync(new Uri(Service.Url));

        if (nowStatus != _isOnline)
        {
            reportBuilder.AddChangeState(ServiceId, nowStatus);
            _isOnline = nowStatus;
        }
    }
}