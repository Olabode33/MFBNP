using System.Collections.Generic;
using PMSDemo.Authorization.Users.Dto;
using PMSDemo.Dto;

namespace PMSDemo.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}