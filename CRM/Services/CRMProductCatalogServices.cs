using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace CRM.Services
{
    public class CRMProductCatalogServices
    {
        public static List<CRMProductCatalogModel> CRMGetEntireProductCatalogService()
        {
            List<CRMProductCatalogModel> theProducts = new List<CRMProductCatalogModel>();
            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "csi_productcatalog",
                    ColumnSet = new ColumnSet("csi_gtin", "csi_gtinbox", "csi_modelnbr", "csi_partnbr", "csi_sellingdescription")
                };

                EntityCollection entityRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (entityRecords != null && entityRecords.Entities.Count > 0)
                {
                    CRMProductCatalogModel entityModel;
                    for (int i = 0; i < entityRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        entityModel = new CRMProductCatalogModel();

                        if (entityRecords[i].Contains("csi_gtin") && entityRecords[i]["csi_gtin"] != null)
                        {
                            entityModel.CSI_GTIN = entityRecords[i]["csi_gtin"].ToString();
                        }
                        if (entityRecords[i].Contains("csi_gtinbox") && entityRecords[i]["csi_gtinbox"] != null)
                        {
                            entityModel.CSI_GTINBox = entityRecords[i]["csi_gtinbox"].ToString();
                        }
                        if (entityRecords[i].Contains("csi_modelnbr") && entityRecords[i]["csi_modelnbr"] != null)
                        {
                            entityModel.CSI_ModelNbr = entityRecords[i]["csi_modelnbr"].ToString();
                        }
                        if (entityRecords[i].Contains("csi_partnbr") && entityRecords[i]["csi_partnbr"] != null)
                        {
                            entityModel.CSI_PartNbr = entityRecords[i]["csi_partnbr"].ToString();
                        }
                        if (entityRecords[i].Contains("csi_sellingdescription") && entityRecords[i]["csi_sellingdescription"] != null)
                        {
                            entityModel.CSI_SellingDescription = entityRecords[i]["csi_sellingdescription"].ToString();
                        }

                        // add row to collection
                        theProducts.Add(entityModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theProducts;
        }
    }
}