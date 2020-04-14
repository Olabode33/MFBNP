using System.Collections.Generic;
using PMSDemo.Authorization.Users.Importing.Dto;
using PMSDemo.Dto;

namespace PMSDemo.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
