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
    public class CRMSalesHierarchyServices
    {
        /// <summary>
        /// Get full sales hierarchy list
        /// </summary>
        /// <returns>List of Sales Hierarchy class</returns>
        public static List<CRMSalesHierarchyModel> CRMGetEntireSalesHierarchyService()
        {
            List<CRMSalesHierarchyModel> theHier = new List<CRMSalesHierarchyModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
                
                QueryExpression query = new QueryExpression
                {
                    EntityName = "team",
                    ColumnSet = new ColumnSet("name", "new_territory", "teamid"),
                    LinkEntities =
                    {
                        new LinkEntity
                        {
                            Columns = new ColumnSet("name", "csi_rsmuserid", "csi_asduserid"),
                            EntityAlias = "region",
                            JoinOperator = JoinOperator.LeftOuter,
                            LinkFromAttributeName = "new_territory",
                            LinkFromEntityName = "team",
                            LinkToAttributeName = "teamid",
                            LinkToEntityName = "team"
                        }
                    },
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_activeterritory", ConditionOperator.Equal, 862530000)
                                }
                            }
                        }
                    }

                };
                // return records sorted by name
                //query.AddOrder("csi_facilityname", OrderType.Ascending);
                query.AddOrder("name", OrderType.Ascending);

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMSalesHierarchyModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMSalesHierarchyModel();

                       
                        if (entityRecords[i].Contains("name") && entityRecords[i]["name"] != null)
                        {
                            entityModel.SalesTeamName = entityRecords[i]["name"].ToString();
                        }
                        if (entityRecords[i].Contains("teamid") && entityRecords[i]["teamid"] != null)
                        {
                            entityModel.SalesTeamID = entityRecords[i]["teamid"].ToString();
                        }
                        if (entityRecords[i].Contains("new_territory") && entityRecords[i]["new_territory"] != null)
                        {
                            EntityReference regionRef = entityRecords[i].GetAttributeValue<EntityReference>("new_territory");
                            entityModel.RegionID = regionRef.Id.ToString();
                        }
                        if (entityRecords[i].Contains("region.name") && entityRecords[i]["region.name"] != null)
                        {
                            AliasedValue rName = entityRecords[i].GetAttributeValue<AliasedValue>("region.name");
                            entityModel.RegionName = rName.Value.ToString();
                        }
                        if (entityRecords[i].Contains("region.csi_rsmuserid") && entityRecords[i]["region.csi_rsmuserid"] != null)
                        {
                            AliasedValue rmsID = entityRecords[i].GetAttributeValue<AliasedValue>("region.csi_rsmuserid");
                            EntityReference rid = (EntityReference)rmsID.Value;
                            entityModel.RSMID = rid.Id.ToString();
                        }
                        if (entityRecords[i].Contains("region.csi_asduserid") && entityRecords[i]["region.csi_asduserid"] != null)
                        {
                            AliasedValue avpID = entityRecords[i].GetAttributeValue<AliasedValue>("region.csi_asduserid");
                            EntityReference aid = (EntityReference)avpID.Value;
                            entityModel.AVPID = aid.Id.ToString();
                        }

                        // add row to collection
                        theHier.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theHier;
        }

        public static List<CRMSalesHierarchyModel> CRMGetSalesHierarchyForAVPService(string mgrID)
        {
            List<CRMSalesHierarchyModel> theHier = new List<CRMSalesHierarchyModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                mgrID = mgrID.ToLower();

                QueryExpression query = new QueryExpression
                {
                    EntityName = "team",
                    ColumnSet = new ColumnSet("name", "new_territory", "teamid"),
                    LinkEntities =
                    {
                        new LinkEntity
                        {
                            Columns = new ColumnSet("name", "csi_rsmuserid", "csi_asduserid"),
                            EntityAlias = "region",
                            JoinOperator = JoinOperator.LeftOuter,
                            LinkFromAttributeName = "new_territory",
                            LinkFromEntityName = "team",
                            LinkToAttributeName = "teamid",
                            LinkToEntityName = "team"
                        }
                    },
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_activeterritory", ConditionOperator.Equal, 862530000)
                                }
                            }
                        }
                    }

                };
                // return records sorted by name
                //query.AddOrder("csi_facilityname", OrderType.Ascending);
                query.AddOrder("name", OrderType.Ascending);

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);
                bool addTeam = false;

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMSalesHierarchyModel entityModel;
                    
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMSalesHierarchyModel();

                        addTeam = false;

                        if (entityRecords[i].Contains("name") && entityRecords[i]["name"] != null)
                        {
                            entityModel.SalesTeamName = entityRecords[i]["name"].ToString();
                        }
                        if (entityRecords[i].Contains("teamid") && entityRecords[i]["teamid"] != null)
                        {
                            entityModel.SalesTeamID = entityRecords[i]["teamid"].ToString();
                        }
                        if (entityRecords[i].Contains("new_territory") && entityRecords[i]["new_territory"] != null)
                        {
                            EntityReference regionRef = entityRecords[i].GetAttributeValue<EntityReference>("new_territory");
                            entityModel.RegionID = regionRef.Id.ToString();
                        }
                        if (entityRecords[i].Contains("region.name") && entityRecords[i]["region.name"] != null)
                        {
                            AliasedValue rName = entityRecords[i].GetAttributeValue<AliasedValue>("region.name");
                            entityModel.RegionName = rName.Value.ToString();
                        }
                        if (entityRecords[i].Contains("region.csi_rsmuserid") && entityRecords[i]["region.csi_rsmuserid"] != null)
                        {
                            AliasedValue rmsID = entityRecords[i].GetAttributeValue<AliasedValue>("region.csi_rsmuserid");
                            EntityReference rid = (EntityReference)rmsID.Value;
                            entityModel.RSMID = rid.Id.ToString();
                        }
                        if (entityRecords[i].Contains("region.csi_asduserid") && entityRecords[i]["region.csi_asduserid"] != null)
                        {
                            AliasedValue avpID = entityRecords[i].GetAttributeValue<AliasedValue>("region.csi_asduserid");
                            EntityReference aid = (EntityReference)avpID.Value;
                            entityModel.AVPID = aid.Id.ToString();
                            // check for match
                            if (entityModel.AVPID.ToLower() == mgrID)
                            {
                                // match found--set flag
                                addTeam = true;
                            }
                        }

                        // add row to collection
                        if (addTeam == true)
                        {
                            // only add if match
                            theHier.Add(entityModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theHier;
        }

        public static List<CRMSalesHierarchyModel> CRMGetSalesHierarchyForRSMService(string mgrID)
        {
            List<CRMSalesHierarchyModel> theHier = new List<CRMSalesHierarchyModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                mgrID = mgrID.ToLower();

                QueryExpression query = new QueryExpression
                {
                    EntityName = "team",
                    ColumnSet = new ColumnSet("name", "new_territory", "teamid"),
                    LinkEntities =
                    {
                        new LinkEntity
                        {
                            Columns = new ColumnSet("name", "csi_rsmuserid", "csi_asduserid"),
                            EntityAlias = "region",
                            JoinOperator = JoinOperator.LeftOuter,
                            LinkFromAttributeName = "new_territory",
                            LinkFromEntityName = "team",
                            LinkToAttributeName = "teamid",
                            LinkToEntityName = "team"
                        }
                    },
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("csi_activeterritory", ConditionOperator.Equal, 862530000)
                                }
                            }
                        }
                    }

                };
                // return records sorted by name
                //query.AddOrder("csi_facilityname", OrderType.Ascending);
                query.AddOrder("name", OrderType.Ascending);

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);
                bool addTeam = false;

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMSalesHierarchyModel entityModel;

                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMSalesHierarchyModel();

                        addTeam = false;

                        if (entityRecords[i].Contains("name") && entityRecords[i]["name"] != null)
                        {
                            entityModel.SalesTeamName = entityRecords[i]["name"].ToString();
                        }
                        if (entityRecords[i].Contains("teamid") && entityRecords[i]["teamid"] != null)
                        {
                            entityModel.SalesTeamID = entityRecords[i]["teamid"].ToString();
                        }
                        if (entityRecords[i].Contains("new_territory") && entityRecords[i]["new_territory"] != null)
                        {
                            EntityReference regionRef = entityRecords[i].GetAttributeValue<EntityReference>("new_territory");
                            entityModel.RegionID = regionRef.Id.ToString();
                        }
                        if (entityRecords[i].Contains("region.name") && entityRecords[i]["region.name"] != null)
                        {
                            AliasedValue rName = entityRecords[i].GetAttributeValue<AliasedValue>("region.name");
                            entityModel.RegionName = rName.Value.ToString();
                        }
                        if (entityRecords[i].Contains("region.csi_rsmuserid") && entityRecords[i]["region.csi_rsmuserid"] != null)
                        {
                            AliasedValue rmsID = entityRecords[i].GetAttributeValue<AliasedValue>("region.csi_rsmuserid");
                            EntityReference rid = (EntityReference)rmsID.Value;
                            entityModel.RSMID = rid.Id.ToString();
                            // check for match
                            if (entityModel.RSMID.ToLower() == mgrID)
                            {
                                // match found--set flag
                                addTeam = true;
                            }
                        }
                        if (entityRecords[i].Contains("region.csi_asduserid") && entityRecords[i]["region.csi_asduserid"] != null)
                        {
                            AliasedValue avpID = entityRecords[i].GetAttributeValue<AliasedValue>("region.csi_asduserid");
                            EntityReference aid = (EntityReference)avpID.Value;
                            entityModel.AVPID = aid.Id.ToString();
                        }
                        // add row to collection
                        if (addTeam == true)
                        {
                            // only add if match
                            theHier.Add(entityModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theHier;
        }
    }
}