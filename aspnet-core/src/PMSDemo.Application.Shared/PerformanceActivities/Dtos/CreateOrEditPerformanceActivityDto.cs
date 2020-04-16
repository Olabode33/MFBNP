
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using PMSDemo.Enums;

namespace PMSDemo.PerformanceActivities.Dtos
{
    public class CreateOrEditPerformanceActivityDto : EntityDto<long?>
    {
        [Required]
        public long OrganizationUnitId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string MilestoneAchieved { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? PlannedCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public CompletionStatusEnum CompletionStatus { get; set; }
        public string Note { get; set; }
        public bool CanCascade { get; set; }
        public string DataSource { get; set; }
        public int? CompletionLevel { get; set; }
    }
}