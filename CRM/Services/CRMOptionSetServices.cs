using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.Services
{
    public class CRMOptionSetServices
    {
        public static List<CRMSpecialitiesModel> CRMSpecialitiesListServices()
        {
            List<CRMSpecialitiesModel> theSpec = new List<CRMSpecialitiesModel>();
            CRMSpecialitiesModel aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000000";
            aSpec.SpecialtyName = "IC";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000001";
            aSpec.SpecialtyName = "IR";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000002";
            aSpec.SpecialtyName = "VS";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000003";
            aSpec.SpecialtyName = "CT";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000004";
            aSpec.SpecialtyName = "Podiatrist";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000005";
            aSpec.SpecialtyName = "Physician Other";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000010";
            aSpec.SpecialtyName = "Faculty Admin";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000006";
            aSpec.SpecialtyName = "Registered Nurse";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000007";
            aSpec.SpecialtyName = "Registered Tech";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000008";
            aSpec.SpecialtyName = "Lab Staff Other";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            aSpec.SpecialtyID = "100000009";
            aSpec.SpecialtyName = "CSI Employee";
            theSpec.Add(aSpec);
            aSpec = new CRMSpecialitiesModel();

            return theSpec;
        }
    }
}