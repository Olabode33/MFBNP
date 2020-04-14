using Abp.Application.Services.Dto;
using PMSDemo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceIndicators.Dtos
{
    public class IndicatorYearlyTargetDto: EntityDto
    {
        public int IndicatorId { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public ComparisonMethodEnum ComparisonMethod { get; set; }
        public string MeansOfVerification { get; set; }
        public string Target { get; set; }
        public string Actual { get; set; }
        public string DataSource { get; set; }
        public string Note { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
