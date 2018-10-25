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
    public class CRMInventoryScanningServices
    {
        public static List<CRMScanHeaderModel> CRMGetMyScansService(string UserID, string TeamID)
        {
            List<CRMScanHeaderModel> myScans = new List<CRMScanHeaderModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                Guid TeamGuid = Guid.Empty;
                if (Guid.TryParse(TeamID, out TeamGuid))
                {
                    // string can be converted to GUID
                    // no additional processing needed
                }
                else
                {
                    // can not parse into a guid
                    throw new Exception("Can not convert TeamID into GUID");
                }

                Guid UserGuid = Guid.Empty;
                if (Guid.TryParse(UserID, out UserGuid))
                {
                    // string can be converted to GUID
                    // no additional processing needed
                }
                else
                {
                    // can not parse into a guid
                    throw new Exception("Can not convert UserID into GUID");
                }

                QueryExpression query = new QueryExpression
                {
                    EntityName = "po_inventoryscan",
                    ColumnSet = new ColumnSet("po_name", "createdby", "createdon", "po_account", "ownerid", "po_scantype"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.Or,
                                Conditions =
                                {
                                    new ConditionExpression("ownerid", ConditionOperator.Equal, UserGuid),
                                    new ConditionExpression("po_territoryteam", ConditionOperator.Equal, TeamGuid)
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
                    CRMScanHeaderModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMScanHeaderModel();

                        if (entityRecords[i].Contains("po_name") && entityRecords[i]["po_name"] != null)
                        {
                            entityModel.ScanName = entityRecords[i]["po_name"].ToString();
                        }
                        if (entityRecords[i].Contains("createdon") && entityRecords[i]["createdon"] != null)
                        {
                            entityModel.CreatedDate = Convert.ToDateTime(entityRecords[i]["createdon"]);
                        }
                        if (entityRecords[i].Contains("po_inventoryscanid") && entityRecords[i]["po_inventoryscanid"] != null)
                        {
                            entityModel.ScanID = entityRecords[i]["po_inventoryscanid"].ToString();
                        }
                        if (entityRecords[i].Contains("po_scantype") && entityRecords[i]["po_scantype"] != null)
                        {
                            entityModel.ScanTypeName = entityRecords[i].FormattedValues["po_scantype"].ToString(); 
                        }
                        if (entityRecords[i].Contains("ownerid") && entityRecords[i]["ownerid"] != null)
                        {
                            //EntityReference ownerRef = entityRecords[i].GetAttributeValue<EntityReference>("ownerid");
                            //entityModel.OwnerID = ownerRef.Id.ToString();
                            //entityModel.OwnerID = entityRecords[i]["ownerid"].ToString();
                        }
                        if (entityRecords[i].Contains("po_account") && entityRecords[i]["po_account"] != null)
                        {

                            EntityReference accountRef = entityRecords[i].GetAttributeValue<EntityReference>("po_account");
                            entityModel.AccountID = accountRef.Id.ToString();
                            entityModel.AccountName = accountRef.Name.ToString();
                            //entityModel = entityRecords[i]["new_dsm2"].ToString();
                        }
                        entityModel.ScanStatus = i % 4;

                        // add row to collection
                        myScans.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return myScans;
        }

        public static CRMScanHeaderModel CRMGetScanInfo(string ScanID)
        {
            CRMScanHeaderModel myScan = new CRMScanHeaderModel();

            return myScan;
        }


    }
}