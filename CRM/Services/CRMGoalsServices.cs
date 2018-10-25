using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using System.ServiceModel;
using CRM.Models;

namespace CRM.Services
{
    public class CRMGoalsServices
    {
        #region ********************************  STATUS FUNCTIONS  ***************************************

        /// <summary>
        /// Retieves the Goal Status list From CRM
        /// Only returns active status rowsd
        /// </summary>
        /// <returns>Class containing Goal Status</returns>
        public static List<CRMGoalStatusModel> CRMGetGoalStatusListService()
        {
            List<CRMGoalStatusModel> theStatus = new List<CRMGoalStatusModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goalstatus",
                    ColumnSet = new ColumnSet("csi_statusid", "csi_statusname"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.Or,
                                Conditions =
                                {
                                    new ConditionExpression("csi_isstatusactive", ConditionOperator.Equal, true)
                                    
                                }
                            }
                        }
                    }

                };
                
                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMGoalStatusModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMGoalStatusModel();

                        if (entityRecords[i].Contains("csi_statusid") && entityRecords[i]["csi_statusid"] != null)
                        {
                            entityModel.CSI_StatusID = Convert.ToInt32(entityRecords[i]["csi_statusid"]);
                        }
                        if (entityRecords[i].Contains("csi_statusname") && entityRecords[i]["csi_statusname"] != null)
                        {
                            entityModel.CSI_StatusName = entityRecords[i]["csi_statusname"].ToString();
                        }
                        
                        // add row to collection
                        theStatus.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theStatus;
        }

        public static string CRMGetStatusNameService(int statusID)
        {
            string statusName = String.Empty;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goalstatus",
                    ColumnSet = new ColumnSet("csi_statusid", "csi_statusname"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_statusid", ConditionOperator.Equal, statusID)

                                }
                            }
                        }
                    }
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        if (entityRecords[i].Contains("csi_statusname") && entityRecords[i]["csi_statusname"] != null)
                        {
                            statusName = entityRecords[i]["csi_statusname"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return statusName;
        }

        /// <summary>
        /// Get the Status GUID for the Status ID
        /// </summary>
        /// <param name="StatusID">Status ID</param>
        /// <returns>String containing Status GUID</returns>
        public static string CRMGetStatusGUID(int StatusID)
        {
            string statusGUID = String.Empty;
            
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goalstatus",
                    ColumnSet = new ColumnSet("csi_goalstatusid"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_statusid", ConditionOperator.Equal, StatusID)
                                }
                            }
                        }
                    }
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        if (entityRecords[i].Contains("csi_goalstatusid") && entityRecords[i]["csi_goalstatusid"] != null)
                        {
                            statusGUID = entityRecords[i]["csi_goalstatusid"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return statusGUID;
        }

        #endregion ************************** END OF STATUS  **********************************************

        #region **********************************  TIMELINE FUNCTIONS  ******************************************

        /// <summary>
        /// Get the Timeline values
        /// </summary>
        /// <returns>List of Timeline values</returns>
        public static List<CRMGoalTimelineModel> GetCRMGetGoalTimelineListService()
        {
            List<CRMGoalTimelineModel> theTimeline = new List<CRMGoalTimelineModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goaltimeline",
                    ColumnSet = new ColumnSet("csi_timelineid", "csi_timelinename")
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMGoalTimelineModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMGoalTimelineModel();

                        if (entityRecords[i].Contains("csi_timelineid") && entityRecords[i]["csi_timelineid"] != null)
                        {
                            entityModel.CSI_TimelineID = Convert.ToInt32(entityRecords[i]["csi_timelineid"]);
                        }
                        if (entityRecords[i].Contains("csi_timelinename") && entityRecords[i]["csi_timelinename"] != null)
                        {
                            entityModel.CSI_TimelineName = entityRecords[i]["csi_timelinename"].ToString();
                        }

                        // add row to collection
                        theTimeline.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theTimeline;
        }

        /// <summary>
        /// Get status name for an ID
        /// </summary>
        /// <param name="timelineID">Status ID</param>
        /// <returns>Status name</returns>
        public static string CRMGetTimelineNameService(int timelineID)
        {
            string timelineName = String.Empty;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goaltimeline",
                    ColumnSet = new ColumnSet("csi_timelineid", "csi_timelinename"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_timelineid", ConditionOperator.Equal, timelineID)
                                }
                            }
                        }
                    }
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        if (entityRecords[i].Contains("csi_timelinename") && entityRecords[i]["csi_timelinename"] != null)
                        {
                            timelineName = entityRecords[i]["csi_timelinename"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return timelineName;
        }

        /// <summary>
        /// Gets the Timeline GUID for the ID
        /// </summary>
        /// <param name="timelineID"></param>
        /// <returns></returns>
        public static string CRMGetTimelineGUIDService(int timelineID)
        {
            string timelineGUID = String.Empty;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goaltimeline",
                    ColumnSet = new ColumnSet("csi_goaltimelineid"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_timelineid", ConditionOperator.Equal, timelineID)
                                }
                            }
                        }
                    }
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        if (entityRecords[i].Contains("csi_goaltimelineid") && entityRecords[i]["csi_goaltimelineid"] != null)
                        {
                            timelineGUID = entityRecords[i]["csi_goaltimelineid"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return timelineGUID;
        }

        #endregion *********************************  END TIMELINES  *******************************************************

        #region ****************************************  GROWTH DRIVERS  *********************************************************

        public static List<CRMGrowthDriverModel> CRMGetGrowthDriverListService()
        {
            List<CRMGrowthDriverModel> theDrivers = new List<CRMGrowthDriverModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_growthdriver",
                    ColumnSet = new ColumnSet("csi_goalgrowthdriverid", "csi_growthdrivername")
                };
                // return records sorted
                query.AddOrder("csi_sortorder", OrderType.Ascending);

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMGrowthDriverModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMGrowthDriverModel();

                        if (entityRecords[i].Contains("csi_goalgrowthdriverid") && entityRecords[i]["csi_goalgrowthdriverid"] != null)
                        {
                            entityModel.CSI_GoalGrowthDriverID = Convert.ToInt32(entityRecords[i]["csi_goalgrowthdriverid"]);
                        }
                        if (entityRecords[i].Contains("csi_growthdrivername") && entityRecords[i]["csi_growthdrivername"] != null)
                        {
                            entityModel.CSI_GrowthDriverName = entityRecords[i]["csi_growthdrivername"].ToString();
                        }
                        
                        // add row to collection
                        theDrivers.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theDrivers;
        }

        /// <summary>
        /// Get the Growth Driver Name for the passed in ID
        /// </summary>
        /// <param name="growthDriverID">Growth Driver ID (not primary key GUID)</param>
        /// <returns>String of name</returns>
        public static string CRMGetGrowthDriverNameService(int growthDriverID)
        {
            string gdName = String.Empty;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_growthdriver",
                    ColumnSet = new ColumnSet("csi_goalgrowthdriverid", "csi_growthdrivername"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.Or,
                                Conditions =
                                {
                                    new ConditionExpression("csi_goalgrowthdriverid", ConditionOperator.Equal, growthDriverID)
                                }
                            }
                        }
                    }
                };
                // return records sorted
                query.AddOrder("csi_sortorder", OrderType.Ascending);

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        if (entityRecords[i].Contains("csi_growthdrivername") && entityRecords[i]["csi_growthdrivername"] != null)
                        {
                            gdName = entityRecords[i]["csi_growthdrivername"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return gdName;
        }

        public static string CRMGetGrowthDriverGUIDService(int growthDriverID)
        {
            string growthDriverGUID = String.Empty;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_growthdriver",
                    ColumnSet = new ColumnSet("csi_growthdriverid"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_goalgrowthdriverid", ConditionOperator.Equal, growthDriverID)
                                }
                            }
                        }
                    }
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        if (entityRecords[i].Contains("csi_growthdriverid") && entityRecords[i]["csi_growthdriverid"] != null)
                        {
                            growthDriverGUID = entityRecords[i]["csi_growthdriverid"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return growthDriverGUID;
        }

        #endregion ******************************************  GROWTH DRIVERS  *************************************************************

        #region ****************************************  GOAL CATEGORIES  ***************************************************

        /// <summary>
        /// Get the Goal Categories for a given Growth Driver
        /// From CRM Entities
        /// </summary>
        /// <param name="growthDriver">Growth Driver to filter on</param>
        /// <returns>List of Goal Categories</returns>
        public static List<CRMGoalCategoriesModel> CRMGetGoalCategoriesForGrowthDriverService(int growthDriver)
        {
            List<CRMGoalCategoriesModel> theCats = new List<CRMGoalCategoriesModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goalcategories",
                    ColumnSet = new ColumnSet("csi_categoryid", "csi_goalcategoryname"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.Or,
                                Conditions =
                                {
                                    new ConditionExpression("csi_goalgrowthdriverid", ConditionOperator.Equal, growthDriver)
                                }
                            }
                        }
                    }
                };
                
                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMGoalCategoriesModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMGoalCategoriesModel();

                        if (entityRecords[i].Contains("csi_categoryid") && entityRecords[i]["csi_categoryid"] != null)
                        {
                            entityModel.CSI_CategoryID = Convert.ToInt32(entityRecords[i]["csi_categoryid"]);
                        }
                        if (entityRecords[i].Contains("csi_goalcategoryname") && entityRecords[i]["csi_goalcategoryname"] != null)
                        {
                            entityModel.CSI_GoalCategoryName = entityRecords[i]["csi_goalcategoryname"].ToString();
                        }
                        
                        // add row to collection
                        theCats.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theCats;
        }

        /// <summary>
        /// Get the Goal Category for a specified Category ID
        /// </summary>
        /// <param name="GoalCategoryID">Category ID--Not Primary Key GUID</param>
        /// <returns>string of Category Name</returns>
        public static string CRMGetGoalCategoryNameService(int GoalCategoryID)
        {
            string catName = String.Empty;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goalcategories",
                    ColumnSet = new ColumnSet("csi_categoryid", "csi_goalcategoryname"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_categoryid", ConditionOperator.Equal, GoalCategoryID)
                                }
                            }
                        }
                    }
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        if (entityRecords[i].Contains("csi_goalcategoryname") && entityRecords[i]["csi_goalcategoryname"] != null)
                        {
                            catName = entityRecords[i]["csi_goalcategoryname"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return catName;
        }

        public static string CRMGetGoalCategoryGUIDService(int goalCategoryID)
        {
            string goalCategoryGUID = String.Empty;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goalcategories",
                    ColumnSet = new ColumnSet("csi_goalcategoriesid"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_categoryid", ConditionOperator.Equal, goalCategoryID)
                                }
                            }
                        }
                    }
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        if (entityRecords[i].Contains("csi_goalcategoriesid") && entityRecords[i]["csi_goalcategoriesid"] != null)
                        {
                            goalCategoryGUID = entityRecords[i]["csi_goalcategoriesid"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return goalCategoryGUID;
        }

        #endregion ************************************  END GOAL CATEGORIES  ***************************************

        #region ************************************  GOAL ENTITY FUNCTIONS  ******************************************

        public static int CRMSaveNewGoalService(CRMGoalModel crmGoal)
        {
            int rc = 0;

            return rc;
        }

        /// <summary>
        /// Queries the Goals Entity
        /// Returning the highest integer value from
        /// the Goal Identifier field
        /// Used to give user easy way to reference a goal
        /// Represents a seqntial number
        /// But not used for any other purpose
        /// </summary>
        /// <returns>Maximum value in the Goals Entity for Goal Identifier field</returns>
        public static int CRMGetMaxGoalIDService()
        {
            int maxVal = 0;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_goals",
                    ColumnSet = new ColumnSet("csi_goalidentifier"),
                };
                // return records sorted
                query.AddOrder("csi_goalidentifier", OrderType.Ascending);

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    // only interest in last value in that records are sorted in ascending order
                    int lastRecord = entityRecords.Entities.Count - 1;   
                    if (entityRecords[lastRecord].Contains("csi_goalidentifier") && entityRecords[lastRecord]["csi_goalidentifier"] != null)
                    {
                        // last value--highest number
                        maxVal = Convert.ToInt32(entityRecords[lastRecord]["csi_goalidentifier"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return maxVal;
        }
        #endregion ***********************************  END OF GOAL FUNCTIONS  ****************************************
    }
}