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
    public class CRMTeamMembershipServices
    {
        public static List<CRMTeamMembershipModel> GetCRMTeamMembershipsService(string systemUserID)
        {
            List<CRMTeamMembershipModel> myTeams = new List<CRMTeamMembershipModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "teammembership",
                    ColumnSet = new ColumnSet("systemuserid", "teamid", "teammembershipid"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("systemuserid", ConditionOperator.Equal, systemUserID.Trim())
                                }
                            }
                        }
                    }

                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMTeamMembershipModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMTeamMembershipModel();
                        // check for active teams--if record is not active--do not continue to add
                        if (CRMTeamsServices.CRMIsTeamActiveService(entityRecords[i]["teamid"].ToString()))
                        {
                            if (entityRecords[i].Contains("systemuserid") && entityRecords[i]["systemuserid"] != null)
                            {
                                entityModel.SystemUserId = entityRecords[i]["systemuserid"].ToString();
                            }
                            if (entityRecords[i].Contains("teamid") && entityRecords[i]["teamid"] != null)
                            {
                                entityModel.TeamId = entityRecords[i]["teamid"].ToString();
                                entityModel.TeamName = CRMTeamsServices.GetCRMTeamNameService(entityModel.TeamId);
                            }
                            if (entityRecords[i].Contains("teammembershipid") && entityRecords[i]["teammembershipid"] != null)
                            {
                                entityModel.TeamMembershipId = entityRecords[i]["teammembershipid"].ToString();
                            }
                            // add row to collection
                            
                                myTeams.Add(entityModel);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return myTeams;
        }
       
    }
}