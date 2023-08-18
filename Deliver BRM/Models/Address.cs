using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class Address
    {
        #region Declarations
        Int32 idAddr;
        Int32 talukaId;
        String talukaName;
        Int32 districtId;
        Int32 stateId;
        Int32 countryId;
        Int32 pincode;
        Int32 createdBy;
        DateTime createdOn;
        String plotNo;
        String streetName;
        String areaName;
        String villageName;
        String comments;
        #endregion

        #region Constructor
        public Address()
        {

        }

        #endregion

        #region GetSet
        public Int32 IdAddr
        {
            get { return idAddr; }
            set { idAddr = value; }
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
        public String PlotNo
        {
            get { return plotNo; }
            set { plotNo = value; }
        }
        public String StreetName
        {
            get { return streetName; }
            set { streetName = value; }
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
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        #endregion
    }
    
}
