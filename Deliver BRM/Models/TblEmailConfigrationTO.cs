using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblEmailConfigrationTO
    {
        #region Declarations
        Int32 idEmailConfig;
        Int32 portNumber;
        Int32 isActive;
        String emailId;
        String userName;
        String password;
        #endregion

        #region Constructor
        public TblEmailConfigrationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdEmailConfig
        {
            get { return idEmailConfig; }
            set { idEmailConfig = value; }
        }
        public Int32 PortNumber
        {
            get { return portNumber; }
            set { portNumber = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String EmailId
        {
            get { return emailId; }
            set { emailId = value; }
        }
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public String Password
        {
            get { return password; }
            set { password = value; }
        }
        #endregion
    }
}
