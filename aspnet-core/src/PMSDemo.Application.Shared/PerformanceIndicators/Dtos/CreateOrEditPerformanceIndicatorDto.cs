
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using PMSDemo.Enums;
using System.Collections.Generic;

namespace PMSDemo.PerformanceIndicators.Dtos
{
    public class CreateOrEditPerformanceIndicatorDto : EntityDto<long?>
    {

        [Required]
        public long OrganizationUnitId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string BaselineValue { get; set; }
        public string BaselineComment { get; set; }
        public int Year { get; set; }
        public string Target { get; set; }
        public string Actual { get; set; }
        public DataTypeEnum DataType { get; set; }
        public UnitsEnum Unit { get; set; }
        public ComparisonMethodEnum ComparisonMethod { get; set; }
        public string MeansOfVerification { get; set; }
        public string Note { get; set; }
        public bool CanCascade { get; set; }
        public int PercentageAchieved { get; set; }
    }
}