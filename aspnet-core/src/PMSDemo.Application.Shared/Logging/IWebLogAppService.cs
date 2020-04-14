using Abp.Application.Services;
using PMSDemo.Dto;
using PMSDemo.Logging.Dto;

namespace PMSDemo.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
