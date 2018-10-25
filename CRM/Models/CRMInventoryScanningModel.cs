using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CRMScanHeaderModel
    {
        public bool ScanCompleted { get; set; } = false;
        public int ScanStatus { get; set; } = 0;
        public string ScanStatusName { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string ScanID { get; set; }
        public string ScanName { get; set; }
        public string TerritoryTeam { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string ScanTypeID { get; set; }
        public string ScanTypeName { get; set; }
        public string StatusName { get; set; }
    }

    public class CRMScanLineModel
    {

    }
}