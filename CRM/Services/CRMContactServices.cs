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
    public class CRMContactServices
    {
        public static List<CRMContactModel> GetCRMContactsForRelationshipService(string relationshipID)
        {
            List<CRMContactModel> theContacts = new List<CRMContactModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "contact",
                    ColumnSet = new ColumnSet("contactid", "fullname"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("contactid", ConditionOperator.Equal, relationshipID.Trim())                                    
                                }
                            }
                        }
                    }

                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMContactModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMContactModel();

                        if (entityRecords[i].Contains("contactid") && entityRecords[i]["contactid"] != null)
                        {
                            entityModel.Id = entityRecords[i]["contactid"].ToString();
                        }
                        if (entityRecords[i].Contains("fullname") && entityRecords[i]["fullname"] != null)
                        {
                            entityModel.ContactName = entityRecords[i]["fullname"].ToString();
                        }

                        // add row to collection
                        theContacts.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theContacts;
        }
    }
}