using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblForgotPswTO
    {
          #region Declarations
          String primaryEmail;
          String mobileNo;
          #endregion

          #region  Constructor
          public TblForgotPswTO()
        {
        }
          #endregion

          #region GetSet
        public String MobileNo
        {
            get { return mobileNo; }
            set { mobileNo = value; }
        }
        public String PrimaryEmail
        {
            get { return primaryEmail; }
            set { primaryEmail = value; }
        }

        #endregion
    }
}