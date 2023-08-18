using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblInvoiceBankDetailsTO
    {
        #region Declarations
        Int32 idBank;
        Int32 orgId;
        Int32 talukaId;
        Int32 districtId;
        Int32 stateId;
        Int32 countryId;
        Int32 pincode;
        Int32 createdBy;
        DateTime createdOn;
        String bankName;
        String accountNo;
        String ifscCode;
        String branch;
        String areaName;
        String villageName;
        #endregion

        #region Constructor
        public TblInvoiceBankDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBank
        {
            get { return idBank; }
            set { idBank = value; }
        }
        public Int32 OrgId
        {
            get { return orgId; }
            set { orgId = value; }
        }
        public Int32 TalukaId
        {
            get { return talukaId; }
            set { talukaId = value; }
        }
        public Int32 DistrictId
        {
            get { return districtId; }
            set { districtId = value; }
        }
        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public Int32 CountryId
        {
            get { return countryId; }
            set { countryId = value; }
        }
        public Int32 Pincode
        {
            get { return pincode; }
            set { pincode = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String BankName
        {
            get { return bankName; }
            set { bankName = value; }
        }
        public String AccountNo
        {
            get { return accountNo; }
            set { accountNo = value; }
        }
        public String IfscCode
        {
            get { return ifscCode; }
            set { ifscCode = value; }
        }
        public String Branch
        {
            get { return branch; }
            set { branch = value; }
        }
        public String AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }
        public String VillageName
        {
            get { return villageName; }
            set { villageName = value; }
        }
        #endregion
    }
}
