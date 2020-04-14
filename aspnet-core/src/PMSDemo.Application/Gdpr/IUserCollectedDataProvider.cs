using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using PMSDemo.Dto;

namespace PMSDemo.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
