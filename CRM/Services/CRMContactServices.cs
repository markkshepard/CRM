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
                            entityModel.ContactName = CRMUtilityServices.GetTitleCaseService(entityRecords[i]["fullname"].ToString());
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

        public static int CRMSaveNewContactService(CRMContactModel theCnt)
        {
            int rc = 0;
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                Entity contact = new Entity("contact");
                contact["fullname"] = theCnt.ContactName;
                contact["firstname"] = theCnt.FirstName;
                contact["lastname"] = theCnt.LastName;
               
                contact["new_specialty"] = new OptionSetValue(Convert.ToInt32(theCnt.SpecialtyID));
                contact["new_npi"] = theCnt.NPI;
                contact["emailaddress1"] = theCnt.EMAIL;

                Guid objGuid = Guid.Empty;
                if (Guid.TryParse(theCnt.StateID, out objGuid))
                {
                    // string can be converted to GUID
                    contact["new_state"] = new EntityReference("new_state", objGuid);
                }
                else
                {
                    // can not parse state into a guid
                    rc = 100;           // bad state guid
                }
                
                // try to convert Account ID as a string into GUID
                Guid acctGuid = Guid.Empty;
                if (Guid.TryParse(theCnt.AccountID, out acctGuid))
                {
                    // success--not immediate action needed
                }
                else
                {
                    // can not parse
                    rc = 200;           // bad Account ID
                }

                // only execute if no conversion errors
                if (rc == 0)
                {
                    // create the contact record
                    // returns ID of new record
                    Guid contactID = _orgService.Create(contact);

                    // Next create the relationship record with the Account entity
                    Entity relationship = new Entity("new_relationship");
                    relationship["new_contact"] = new EntityReference("contact", contactID);
                    relationship["statuscode"] = 1;
                    relationship["new_account"] = new EntityReference("account", acctGuid);
                    relationship["new_firstname"] = theCnt.FirstName;
                    relationship["new_lastname"] = theCnt.LastName;
                    relationship["new_name"] = theCnt.ContactName;

                    Guid relationshipID = _orgService.Create(relationship);
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                Console.WriteLine(ex.Message);
                rc = 300;       // try-catch exception
            }

            return rc;
        }
    }
}