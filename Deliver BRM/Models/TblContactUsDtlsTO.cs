using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblContactUsDtls
    {
        #region Declarations

        Int32 idContactUsDtls;

        String departmentName;
        String designation;
        String personName;
        String contactNo;
        String emailId;

        Int32 isActive;

        Int32 supportTypeId;
        #endregion

        #region Constructor
        public TblContactUsDtls()
        {

        }
        #endregion

        #region GetSet

        public Int32 IdContactUsDtls
        {
            get { return idContactUsDtls; }
            set { idContactUsDtls = value; }
        }

        public String DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; }
        }

        public String Designation
        {
            get { return designation; }
            set { designation = value; }
        }

        public String PersonName
        {
            get { return personName; }
            set { personName = value; }
        }

        public String ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; }
        }

        public String EmailId
        {
            get { return emailId; }
            set { emailId = value; }
        }

        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 SupportTypeId
        {
            get { return supportTypeId; }
            set { supportTypeId = value; }
        }
        #endregion

    }
}