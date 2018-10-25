using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CRMSystemUserModel
    {
        public string SystemUserId { get; set; }
        public bool IsLicensed { get; set; }
        public bool IsDisabled { get; set; }
        public string InternalEmailAddress { get; set; }
        public string YomiFullName { get; set; }
        public string Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmployeeId { get; set; }
        public string DomainName { get; set; }
        public string FullName { get; set; }
    }

    public class CRMTeamMembershipModel
    {
        public string TeamMembershipId { get; set; }
        public string TeamId { get; set; }
        public string SystemUserId { get; set; }
        public string TeamName { get; set; }
    }

    public class CRMAccountModel
    {
        public string Id { get; set; }
        public string CSI_FacilityName { get; set; }
        public string OwnerID { get; set; }
        public string New_DSM2 { get; set; }
        public string Name { get; set; }
    }

    public class CRMNewRelationshipModel
    {
        public string Id { get; set; }
        public string New_Account { get; set; }
        public string New_Contact { get; set; }
    }

    public class CRMContactModel
    {
        public string Id { get; set; }
        public string ContactName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StateID { get; set; }
        public string SpecialtyID { get; set; }
        public string EMAIL { get; set; }
        public string NPI { get; set; }
        public string AccountID { get; set; }

    }

    public class CRMStateModel
    {
        public string StateID { get; set; }
        public string StateName { get; set; }
    }
}