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
    public class CRMStateServices
    {
        public static List<CRMStateModel> CRMGetStateListService()
        {
            List<CRMStateModel> theStates = new List<CRMStateModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "new_state",
                    ColumnSet = new ColumnSet("new_stateid", "new_statename")
                };
                // return records sorted by name
                //query.AddOrder("csi_facilityname", OrderType.Ascending);
                query.AddOrder("new_statename", OrderType.Ascending);

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMStateModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMStateModel();

                        if (entityRecords[i].Contains("new_stateid") && entityRecords[i]["new_stateid"] != null)
                        {
                            entityModel.StateID = entityRecords[i]["new_stateid"].ToString();
                        }
                        if (entityRecords[i].Contains("new_statename") && entityRecords[i]["new_statename"] != null)
                        {
                            entityModel.StateName = entityRecords[i]["new_statename"].ToString();
                        }
                       
                        // add row to collection
                        theStates.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theStates;
        }
    }
}