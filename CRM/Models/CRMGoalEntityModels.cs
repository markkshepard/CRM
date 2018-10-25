using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CRMGoalCategoriesModel
    {
        public string CSI_GoalCategoriesID { get; set; }
        public int CSI_CategoryID { get; set; }
        public string CSI_GoalCategoryName { get; set; }
        public string CSI_Name { get; set; }
        public int CSI_GoalGrowthDriverID { get; set; }
    }

    public class CRMGoalStatusModel
    {
        public string CSI_GoalStatusID { get; set; }
        public int CSI_StatusID { get; set; }
        public string CSI_StatusName { get; set; }
        public string CSI_Name { get; set; }
        public bool CSI_IsStatusActive { get; set; }
    }

    public class CRMGoalTimelineModel
    {
        public string CSI_GoalTimelineID { get; set; }
        public int CSI_TimelineID { get; set; }
        public string CSI_TimelineName { get; set; }
        public string CSI_Name { get; set; }
    }

    public class CRMGrowthDriverModel
    {
        public string CSI_GrowthDriverID { get; set; }
        public int CSI_GoalGrowthDriverID { get; set; }
        public string CSI_GrowthDriverName { get; set; }
        public string CSI_Name { get; set; }
        public int CSI_SortOrder { get; set; }
    }

    public class CRMGoalModel
    {
        public DateTime CSI_AdjEstimatedCompletion { get; set; }
        public string CSI_GoalCategoryGUID { get; set; }
        public int CSI_GoalCategoryID { get; set; }
        public string CSI_GoalGUID { get; set; }
        public int CSI_GoalIdentifier { get; set; }
        public string CSI_GoalsID { get; set; }
        public string CSI_GoalStatusGUID { get; set; }
        public int CSI_StatusID { get; set; }
        public string CSI_GoalTimelineGUID { get; set; }
        public int CSI_GoalTimelineID { get; set; }
        public string CSI_GrowthDriverGUID { get; set; }
        public int CSI_GrowthDriverID { get; set; }
        public string CSI_Name { get; set; }
        public DateTime CSI_OrigEstimatedCompletion { get; set; }
        public bool CSI_RegionalFocus { get; set; }
        public string CSI_SMARTGoal { get; set; }
        public string CSI_TeamIDGUID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}