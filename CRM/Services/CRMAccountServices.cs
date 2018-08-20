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
        public static List<CRMAccountModel> GetCRMAccountsForTeamService(string TeamID, string DSM2)
        {
            List<CRMAccountModel> theAccounts = new List<CRMAccountModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "account",
                    ColumnSet = new ColumnSet("accountid", "csi_facilityname", "ownerid", "new_dsm2"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.Or,
                                Conditions =
                                {
                                    new ConditionExpression("ownerid", ConditionOperator.Equal, TeamID.Trim()),
                                    new ConditionExpression("new_dsm2", ConditionOperator.Equal, TeamID.Trim())
                                }
                            }
                        }
                    }

                };
                // return records sorted by name
                query.AddOrder("csi_facilityname", OrderType.Ascending);

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
                        if (entityRecords[i].Contains("ownerid") && entityRecords[i]["ownerid"] != null)
                        {
                            entityModel.OwnerID = entityRecords[i]["ownerid"].ToString();
                        }
                        if (entityRecords[i].Contains("new_dsm2") && entityRecords[i]["new_dsm2"] != null)
                        {
                            entityModel.New_DSM2 = entityRecords[i]["new_dsm2"].ToString();
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
    }
}