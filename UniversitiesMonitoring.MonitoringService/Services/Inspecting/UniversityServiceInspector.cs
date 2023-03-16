using System.Diagnostics;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.MonitoringService.Services.Inspecting;

internal class UniversityServiceInspector
{
    private readonly IServiceInspector _serviceInspector;

    public ulong ServiceId => Service.ServiceId;

    public UniversityServiceEntity Service
    {
        get => _service;
        set => _service = value;
    }

    private UniversityServiceEntity _service;

    public UniversityServiceInspector(IServiceInspector inspector, UniversityServiceEntity serviceEntity)
    {
        _serviceInspector = inspector;
        _service = serviceEntity;
    }

    public async Task UpdateStateAsync(UpdateBuilder reportBuilder)
    {
        var timer = new Stopwatch();
        
        timer.Start();
        var nowStatus = await _serviceInspector.InspectServiceAsync(new Uri(Service.Url));
        timer.Stop();
        reportBuilder.AddChangeState(ServiceId, nowStatus, nowStatus ? timer.ElapsedMilliseconds : null);
    }
}