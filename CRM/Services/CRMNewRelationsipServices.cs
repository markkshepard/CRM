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
    public class CRMNewRelationsipServices
    {
        public static List<CRMNewRelationshipModel> GetCRMRelationshipsForAccountServices(string accountID)
        {
            List<CRMNewRelationshipModel> theRelationships = new List<CRMNewRelationshipModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "new_relationship",
                    ColumnSet = new ColumnSet("new_contact", "new_account"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("new_account", ConditionOperator.Equal, accountID.Trim()),
                                    new ConditionExpression("statuscode", ConditionOperator.Equal, 1)
                                }
                            }
                        }
                    }

                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMNewRelationshipModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMNewRelationshipModel();

                        if (entityRecords[i].Contains("new_contact") && entityRecords[i]["new_contact"] != null)
                        {
                            EntityReference contractRef = entityRecords[i].GetAttributeValue<EntityReference>("new_contact");
                            entityModel.New_Contact = contractRef.Id.ToString();
                            
                        }
                        if (entityRecords[i].Contains("new_account") && entityRecords[i]["new_account"] != null)
                        {
                            EntityReference acctRef = entityRecords[i].GetAttributeValue<EntityReference>("new_account");
                            entityModel.New_Account = acctRef.Id.ToString();
                        }

                        // add row to collection
                        theRelationships.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theRelationships;
        }
    }
}