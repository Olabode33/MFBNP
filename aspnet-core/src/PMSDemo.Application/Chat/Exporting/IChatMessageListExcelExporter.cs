using System.Collections.Generic;
using Abp;
using PMSDemo.Chat.Dto;
using PMSDemo.Dto;

namespace PMSDemo.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
