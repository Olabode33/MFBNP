using System.Collections.Generic;
using PMSDemo.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace PMSDemo.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
