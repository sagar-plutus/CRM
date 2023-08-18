using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class OrgExportRptTO
    {
        #region Declaration
        Int32 idOrganization;
        String firmName;
        String cnfName;
        String regMobileNo;
        String district;
        String taluka;
        String village;
        String state;
        String foFirstName;
        String foMiddleName;
        String foLastName;
        String foMobileNo;
        String foAlternateMobileNo;
        String foPhoneNo;
        String foDob;
        String foEmailAddr;
        String foAlterEmailAddr;
        Double cdStructure;
        Double deliveryPeriod;
        String soFirstName;
        String soMiddleName;
        String soLastName;
        String soMobileNo;
        String soAlternateMobileNo;
        String soPhoneNo;
        String soDob;
        String soEmailAddr;
        String soAlterEmailAddr;   
        
        String plotNo;
        String streetName;
        String areaName;
        String pinCode;
        String phoneNo;
        String faxNo;
        String emailAddr;
        String website;

        String panNo;
        String provGstNo;
        String permGstNo;
        Int32 isSpecialCnf;

        #endregion

        #region Get Set
        public int IdOrganization { get => idOrganization; set => idOrganization = value; }
        public string FirmName { get => firmName; set => firmName = value; }
        public string CnfName { get => cnfName; set => cnfName = value; }
        public string RegMobileNo { get => regMobileNo; set => regMobileNo = value; }
        public string District { get => district; set => district = value; }
        public string Taluka { get => taluka; set => taluka = value; }
        public string Village { get => village; set => village = value; }
        public string State { get => state; set => state = value; }
        public string FoFirstName { get => foFirstName; set => foFirstName = value; }
        public string FoMiddleName { get => foMiddleName; set => foMiddleName = value; }
        public string FoLastName { get => foLastName; set => foLastName = value; }
        public string FoMobileNo { get => foMobileNo; set => foMobileNo = value; }
        public string FoAlternateMobileNo { get => foAlternateMobileNo; set => foAlternateMobileNo = value; }
        public string FoDob { get => foDob; set => foDob = value; }
        public string FoEmailAddr { get => foEmailAddr; set => foEmailAddr = value; }
        public string FoAlterEmailAddr { get => foAlterEmailAddr; set => foAlterEmailAddr = value; }
        public double CdStructure { get => cdStructure; set => cdStructure = value; }
        public double DeliveryPeriod { get => deliveryPeriod; set => deliveryPeriod = value; }
        public string SoFirstName { get => soFirstName; set => soFirstName = value; }
        public string SoMiddleName { get => soMiddleName; set => soMiddleName = value; }
        public string SoLastName { get => soLastName; set => soLastName = value; }
        public string SoMobileNo { get => soMobileNo; set => soMobileNo = value; }
        public string SoAlternateMobileNo { get => soAlternateMobileNo; set => soAlternateMobileNo = value; }
        public string SoDob { get => soDob; set => soDob = value; }
        public string SoEmailAddr { get => soEmailAddr; set => soEmailAddr = value; }
        public string SoAlterEmailAddr { get => soAlterEmailAddr; set => soAlterEmailAddr = value; }
        public string PlotNo { get => plotNo; set => plotNo = value; }
        public string StreetName { get => streetName; set => streetName = value; }
        public string AreaName { get => areaName; set => areaName = value; }
        public string PinCode { get => pinCode; set => pinCode = value; }
        public string PhoneNo { get => phoneNo; set => phoneNo = value; }
        public string FaxNo { get => faxNo; set => faxNo = value; }
        public string EmailAddr { get => emailAddr; set => emailAddr = value; }
        public string Website { get => website; set => website = value; }
        public string PanNo { get => panNo; set => panNo = value; }
        public string ProvGstNo { get => provGstNo; set => provGstNo = value; }
        public string PermGstNo { get => permGstNo; set => permGstNo = value; }
        public int IsSpecialCnf { get => isSpecialCnf; set => isSpecialCnf = value; }
        public string FoPhoneNo { get => foPhoneNo; set => foPhoneNo = value; }
        public string SoPhoneNo { get => soPhoneNo; set => soPhoneNo = value; }

        #endregion
    }
}
