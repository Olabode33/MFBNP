
using System;
using Abp.Application.Services.Dto;
using PMSDemo.Enums;

namespace PMSDemo.PerformanceActivities.Dtos
{
    public class PerformanceActivityDto : EntityDto<long>
    {
        public long OrganizationUnitId { get; set; }
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
    }
}