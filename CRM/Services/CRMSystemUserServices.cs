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
    public class CRMSystemUserServices
    {
        /// <summary>
        /// Returns information about the user
        /// When given a UserID (i.e. login ID)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>CRM User Model</returns>
        public static List<CRMSystemUserModel> GetCRMSystemUserService(string userID)
        {
            List<CRMSystemUserModel> theUser = new List<CRMSystemUserModel>();

            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "systemuser",
                    ColumnSet = new ColumnSet("systemuserid", "islicensed", "isdisabled", "internalemailaddress",
                                                "yomifullname", "title", "lastname", "firstname", "employeeid", "domainname", "fullname"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("islicensed", ConditionOperator.Equal, true),
                                    new ConditionExpression("internalemailaddress", ConditionOperator.BeginsWith, userID.Trim())
                                }
                            }
                        }
                    }

                };

                EntityCollection systemUserRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (systemUserRecords != null && systemUserRecords.Entities.Count > 0)
                {
                    CRMSystemUserModel sysUserModel;
                    for (int i = 0; i < systemUserRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        sysUserModel = new CRMSystemUserModel();

                        if (systemUserRecords[i].Contains("systemuserid") && systemUserRecords[i]["systemuserid"] != null)
                        {
                            sysUserModel.SystemUserId = systemUserRecords[i]["systemuserid"].ToString();
                        }
                        if (systemUserRecords[i].Contains("islicensed") && systemUserRecords[i]["islicensed"] != null)
                        {
                            sysUserModel.IsLicensed = Convert.ToBoolean(systemUserRecords[i]["islicensed"]);
                        }
                        if (systemUserRecords[i].Contains("isdisabled") && systemUserRecords[i]["isdisabled"] != null)
                        {
                            sysUserModel.IsDisabled = Convert.ToBoolean(systemUserRecords[i]["isdisabled"]);
                        }
                        if (systemUserRecords[i].Contains("internalemailaddress") && systemUserRecords[i]["internalemailaddress"] != null)
                        {
                            sysUserModel.InternalEmailAddress = systemUserRecords[i]["internalemailaddress"].ToString();
                        }
                        if (systemUserRecords[i].Contains("yomifullname") && systemUserRecords[i]["yomifullname"] != null)
                        {
                            sysUserModel.YomiFullName = systemUserRecords[i]["yomifullname"].ToString();
                        }
                        if (systemUserRecords[i].Contains("title") && systemUserRecords[i]["title"] != null)
                        {
                            sysUserModel.Title = systemUserRecords[i]["title"].ToString();
                        }
                        if (systemUserRecords[i].Contains("lastname") && systemUserRecords[i]["lastname"] != null)
                        {
                            sysUserModel.LastName = systemUserRecords[i]["lastname"].ToString();
                        }
                        if (systemUserRecords[i].Contains("firstname") && systemUserRecords[i]["firstname"] != null)
                        {
                            sysUserModel.FirstName = systemUserRecords[i]["firstname"].ToString();
                        }
                        if (systemUserRecords[i].Contains("employeeid") && systemUserRecords[i]["employeeid"] != null)
                        {
                            sysUserModel.EmployeeId = systemUserRecords[i]["employeeid"].ToString();
                        }
                        if (systemUserRecords[i].Contains("domainname") && systemUserRecords[i]["domainname"] != null)
                        {
                            sysUserModel.DomainName = systemUserRecords[i]["domainname"].ToString();
                        }
                        if (systemUserRecords[i].Contains("fullname") && systemUserRecords[i]["fullname"] != null)
                        {
                            sysUserModel.FullName = systemUserRecords[i]["fullname"].ToString();
                        }
                        // add row to collection
                        theUser.Add(sysUserModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return theUser;
        }

        /// <summary>
        /// Returns a complete list of all active System Users in CRM
        /// </summary>
        /// <returns>List of CRM System Users</returns>
        public static List<CRMSystemUserModel> GetCRMSystemUserListService()
        {
            List<CRMSystemUserModel> theUsers = new List<CRMSystemUserModel>();

            try
            {
                CrmServiceClient conn = new CrmServiceClient(DatabaseServices.GetCRMDBConnectionString());

                IOrganizationService _orgService;
                _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                QueryExpression query = new QueryExpression
                {
                    EntityName = "systemuser",
                    ColumnSet = new ColumnSet("systemuserid", "islicensed", "isdisabled", "internalemailaddress",
                                                "yomifullname", "title", "lastname", "firstname", "employeeid", "domainname", "fullname"),
                    Criteria =
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("islicensed", ConditionOperator.Equal, true)
                                }
                            }
                        }
                    }

                };

                EntityCollection systemUserRecords = _orgService.RetrieveMultiple(query);

                // see if data returned
                if (systemUserRecords != null && systemUserRecords.Entities.Count > 0)
                {
                    CRMSystemUserModel sysUserModel;
                    for (int i = 0; i < systemUserRecords.Entities.Count; i++)
                    {
                        // new blank copy of the C# model
                        sysUserModel = new CRMSystemUserModel();

                        if (systemUserRecords[i].Contains("systemuserid") && systemUserRecords[i]["systemuserid"] != null)
                        {
                            sysUserModel.SystemUserId = systemUserRecords[i]["systemuserid"].ToString();
                        }
                        if (systemUserRecords[i].Contains("islicensed") && systemUserRecords[i]["islicensed"] != null)
                        {
                            sysUserModel.IsLicensed = Convert.ToBoolean(systemUserRecords[i]["islicensed"]);
                        }
                        if (systemUserRecords[i].Contains("isdisabled") && systemUserRecords[i]["isdisabled"] != null)
                        {
                            sysUserModel.IsDisabled = Convert.ToBoolean(systemUserRecords[i]["isdisabled"]);
                        }
                        if (systemUserRecords[i].Contains("internalemailaddress") && systemUserRecords[i]["internalemailaddress"] != null)
                        {
                            sysUserModel.InternalEmailAddress = systemUserRecords[i]["internalemailaddress"].ToString();
                        }
                        if (systemUserRecords[i].Contains("yomifullname") && systemUserRecords[i]["yomifullname"] != null)
                        {
                            sysUserModel.YomiFullName = systemUserRecords[i]["yomifullname"].ToString();
                        }
                        if (systemUserRecords[i].Contains("title") && systemUserRecords[i]["title"] != null)
                        {
                            sysUserModel.Title = systemUserRecords[i]["title"].ToString();
                        }
                        if (systemUserRecords[i].Contains("lastname") && systemUserRecords[i]["lastname"] != null)
                        {
                            sysUserModel.LastName = systemUserRecords[i]["lastname"].ToString();
                        }
                        if (systemUserRecords[i].Contains("firstname") && systemUserRecords[i]["firstname"] != null)
                        {
                            sysUserModel.FirstName = systemUserRecords[i]["firstname"].ToString();
                        }
                        if (systemUserRecords[i].Contains("employeeid") && systemUserRecords[i]["employeeid"] != null)
                        {
                            sysUserModel.EmployeeId = systemUserRecords[i]["employeeid"].ToString();
                        }
                        if (systemUserRecords[i].Contains("domainname") && systemUserRecords[i]["domainname"] != null)
                        {
                            sysUserModel.DomainName = systemUserRecords[i]["domainname"].ToString();
                        }
                        if (systemUserRecords[i].Contains("fullname") && systemUserRecords[i]["fullname"] != null)
                        {
                            sysUserModel.FullName = systemUserRecords[i]["fullname"].ToString();
                        }
                        // add row to collection
                        theUsers.Add(sysUserModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return theUsers;
        }
    }
}