using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    //*****************************************************************************************************************************
    //***
    //*** The classes in this file are intended to represent comnbination of entities from Dynamics
    //***
    //*****************************************************************************************************************************
    public class CRMSalesHierarchyModel
    {
        public string AVPName { get; set; }
        public string RSMName { get; set; }
        public string SalesTeamName { get; set; }
        public string SalesTeamID { get; set; }
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string RSMID { get; set; }
        public string AVPID { get; set; }
        public string RSMEmail { get; set; }
        public string AVPEmail { get; set; }
    }
}