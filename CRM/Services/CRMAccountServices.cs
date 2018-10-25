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
    public class CRMAccountServices
    {
        /// <summary>
        /// Gets the Accounts associated with a Team and/or DSM2
        /// </summary>
        /// <param name="TeamID">Team or DSM2 ID</param>
        /// <param name="DSM2">Not Used</param>
        /// <returns>List of Account Model class</returns>
        public static List<CRMAccountModel> GetCRMAccountsForTeamService(string TeamID, string DSM2)
        {
            List<CRMAccountModel> theAccounts = new List<CRMAccountModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                Guid objGuid = Guid.Empty;
                if (Guid.TryParse(TeamID, out objGuid))
                {
                    // string can be converted to GUID
                    // no additional processing needed
                }
                else
                {
                    // can not parse into a guid
                    throw new Exception("Can not convert TeamID into GUID");
                }

                QueryExpression query = new QueryExpression
                {
                    EntityName = "account",
                    ColumnSet = new ColumnSet("accountid", "name", "csi_facilityname", "ownerid", "new_dsm2"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.Or,
                                Conditions =
                                {
                                    //new ConditionExpression("ownerid", ConditionOperator.Equal, TeamID.Trim()),
                                    new ConditionExpression("ownerid", ConditionOperator.Equal, objGuid),
                                    //new ConditionExpression("new_dsm2", ConditionOperator.Equal, TeamID.Trim())
                                    new ConditionExpression("new_dsm2", ConditionOperator.Equal, objGuid)
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
                    CRMAccountModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMAccountModel();

                        if (entityRecords[i].Contains("accountid") && entityRecords[i]["accountid"] != null)
                        {
                            entityModel.Id = entityRecords[i]["accountid"].ToString();
                        }
                        if (entityRecords[i].Contains("csi_facilityname") && entityRecords[i]["csi_facilityname"] != null)
                        {
                            entityModel.CSI_FacilityName = entityRecords[i]["csi_facilityname"].ToString();
                        }
                        if (entityRecords[i].Contains("name") && entityRecords[i]["name"] != null)
                        {
                            entityModel.Name = CRMUtilityServices.GetTitleCaseService(entityRecords[i]["name"].ToString());
                        }
                        if (entityRecords[i].Contains("ownerid") && entityRecords[i]["ownerid"] != null)
                        {
                            EntityReference ownerRef = entityRecords[i].GetAttributeValue<EntityReference>("ownerid");
                            entityModel.OwnerID = ownerRef.Id.ToString();
                            //entityModel.OwnerID = entityRecords[i]["ownerid"].ToString();
                        }
                        if (entityRecords[i].Contains("new_dsm2") && entityRecords[i]["new_dsm2"] != null)
                        {
                            EntityReference ownerRef = entityRecords[i].GetAttributeValue<EntityReference>("new_dsm2");
                            entityModel.OwnerID = ownerRef.Id.ToString();
                            //entityModel.New_DSM2 = entityRecords[i]["new_dsm2"].ToString();
                        }
                        
                        // add row to collection
                        theAccounts.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theAccounts;
        }

        public static List<CRMAccountModel> CRMGetSharedAccountsService(string userID)
        {
            List<CRMAccountModel> theAccts = new List<CRMAccountModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                Guid objGuid = Guid.Empty;
                if (Guid.TryParse(userID, out objGuid))
                {
                    // string can be converted to GUID
                    // no additional processing needed
                }
                else
                {
                    // can not parse into a guid
                    throw new Exception("Can not convert User ID into GUID");
                }

                QueryExpression query = new QueryExpression
                {
                    EntityName = "principalobjectaccess",
                    ColumnSet = new ColumnSet("objectid"),
                    
                    LinkEntities =
                    {
                        new LinkEntity
                        {
                            Columns = new ColumnSet("accountid", "name"),
                            EntityAlias = "acct",
                            JoinOperator = JoinOperator.LeftOuter,
                            LinkFromAttributeName = "objectid",
                            LinkFromEntityName = "principalobjectaccess",
                            LinkToAttributeName = "accountid",
                            LinkToEntityName = "account"
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
                                    new ConditionExpression("objecttypecode", ConditionOperator.Equal, "account"),
                                    new ConditionExpression("principaltypecode", ConditionOperator.Equal, "systemuser"),
                                    new ConditionExpression("principalid", ConditionOperator.Equal, objGuid)
                                }
                            }
                        }
                    }
                };
                // return records sorted by name
                //query.AddOrder("csi_facilityname", OrderType.Ascending);
                //query.AddOrder("name", OrderType.Ascending);

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMAccountModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMAccountModel();

                        if (entityRecords[i].Contains("acct.accountid") && entityRecords[i]["acct.accountid"] != null)
                        {
                            AliasedValue acctID = entityRecords[i].GetAttributeValue<AliasedValue>("acct.accountid");
                            entityModel.Id = acctID.Value.ToString();
                        }
                        if (entityRecords[i].Contains("acct.name") && entityRecords[i]["acct.name"] != null)
                        {
                            AliasedValue acctN = entityRecords[i].GetAttributeValue<AliasedValue>("acct.name");
                            entityModel.Name = CRMUtilityServices.GetTitleCaseService(acctN.Value.ToString());
                            entityModel.CSI_FacilityName = entityModel.Name;
                        }
                        // add row to collection
                        theAccts.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theAccts;
        }
    }
}