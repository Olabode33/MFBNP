using PMSDemo.PerformanceActivities.Dtos;
using PMSDemo.PerformanceIndicators.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.Deliverables.Dtos.Exporting
{
    public class DeliverableExportDto
    {
        public GetDeliverableForEditOutput Deliverable { get; set; }
        public List<GetPerformanceIndicatorForEditOutput> Indicators { get; set; }
        public List<GetPerformanceActivityForEditOutput> Activities { get; set; }
        public List<GetPerformanceReviewForEditOutput> Reviews { get; set; }
    }

    public class MdaDeliverableExportDto
    {
        public string MdaName { get; set; }
        public string ResponsiblePerson { get; set; }
        public List<DeliverableExportDto> deliverables { get; set; }
    }
}
