using System.Threading.Tasks;
using Abp.Application.Services;
using PMSDemo.Install.Dto;

namespace PMSDemo.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}