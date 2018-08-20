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
    public class CRMTeamsServices
    {
        public static string GetCRMTeamNameService(string teamID)
        {
            string teamName = string.Empty;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "team",
                    ColumnSet = new ColumnSet("teamid", "name"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("teamid", ConditionOperator.Equal, teamID.Trim()),
                                    new ConditionExpression("csi_activeterritory", ConditionOperator.Equal, "862530000")
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
                        if (entityRecords[i].Contains("name") && entityRecords[i]["name"] != null)
                        {
                            teamName = entityRecords[i]["name"].ToString();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return teamName;
        }

        public static bool CRMIsTeamActiveService(string teamID)
        {
            bool teamActive = true;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "team",
                    ColumnSet = new ColumnSet("teamid", "csi_activeterritory"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("teamid", ConditionOperator.Equal, teamID.Trim())
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
                        if (entityRecords[i].Contains("csi_activeterritory") && entityRecords[i]["csi_activeterritory"] != null)
                        {
                            string terrActive = entityRecords[i].GetAttributeValue<OptionSetValue>("csi_activeterritory").Value.ToString();

                            if (terrActive != "862530000")
                            {
                                // not an active team with this value
                                teamActive = false;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return teamActive;
        }
    }
}