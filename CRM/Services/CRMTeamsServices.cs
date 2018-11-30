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

        public static List<CRMTeamMembershipModel> CRMGetAllMyTeamsService(string id)
        {
            List<CRMTeamMembershipModel> myTeams = new List<CRMTeamMembershipModel>();

            // first read in own team
            myTeams = GetTeamsIAmOnService(id);
            // next add on any team for RSM
            myTeams.AddRange(GetTeamsIAmAnRSMService(id));
            // finally look at AVP
            myTeams.AddRange(GetTeamsIAmAnAVPService(id));
            // Ensure that only one team id is in list
            // Otherwise you could end up with duplicate goal records
            List<CRMTeamMembershipModel> dupFree = myTeams.GroupBy(x => x.TeamName)
                                                    .Select(g => g.First()).ToList();

            // return dup results
            return dupFree;
        }

        private static List<CRMTeamMembershipModel> GetTeamsIAmOnService(string id)
        {
            List<CRMTeamMembershipModel> crmTeams = new List<CRMTeamMembershipModel>();
            crmTeams = CRMTeamMembershipServices.GetCRMTeamMembershipsService(id);

            return crmTeams;
        }

        private static List<CRMTeamMembershipModel> GetTeamsIAmAnRSMService(string id)
        {
            List<CRMTeamMembershipModel> myTeams = new List<CRMTeamMembershipModel>();
            CRMTeamMembershipModel aTeam = new CRMTeamMembershipModel();

            List<CRMSalesHierarchyModel> theHier = new List<CRMSalesHierarchyModel>();
            theHier = CRMSalesHierarchyServices.CRMGetSalesHierarchyForRSMService(id);

            foreach (var row in theHier)
            {
                aTeam.TeamId = row.SalesTeamID;
                aTeam.TeamName = row.SalesTeamName;

                myTeams.Add(aTeam);
                aTeam = new CRMTeamMembershipModel();
            }

            return myTeams;
        }

        private static List<CRMTeamMembershipModel> GetTeamsIAmAnAVPService(string id)
        {
            List<CRMTeamMembershipModel> myTeams = new List<CRMTeamMembershipModel>();
            CRMTeamMembershipModel aTeam = new CRMTeamMembershipModel();

            List<CRMSalesHierarchyModel> theHier = new List<CRMSalesHierarchyModel>();
            theHier = CRMSalesHierarchyServices.CRMGetSalesHierarchyForAVPService(id);

            foreach (var row in theHier)
            {
                aTeam.TeamId = row.SalesTeamID;
                aTeam.TeamName = row.SalesTeamName;

                myTeams.Add(aTeam);
                aTeam = new CRMTeamMembershipModel();
            }

            return myTeams;
        }
    }
}