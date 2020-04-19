using PMSDemo.Deliverables.Dtos.Exporting;
using PMSDemo.Dto;

namespace PMSDemo.Deliverables.Exporting
{
    public interface IDeliverableExcelExporter
    {
        FileDto ExportToFile(MdaDeliverableExportDto mdaDeliverables);
    }
}