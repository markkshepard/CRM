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
}