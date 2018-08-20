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
    }
}