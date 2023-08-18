using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblAlertDefinitionTO
    {
        #region Declarations
        Int32 idAlertDef;
        Int32 isAutoReset;
        Int32 isSysGenerated;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String alertDefDesc;
        String defaultAlertTxt;
        List<TblAlertSubscribersTO> alertSubscribersTOList;
        String navigationUrl;
        #endregion

        #region Constructor
        public TblAlertDefinitionTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdAlertDef
        {
            get { return idAlertDef; }
            set { idAlertDef = value; }
        }
        public Int32 IsAutoReset
        {
            get { return isAutoReset; }
            set { isAutoReset = value; }
        }
        public Int32 IsSysGenerated
        {
            get { return isSysGenerated; }
            set { isSysGenerated = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String AlertDefDesc
        {
            get { return alertDefDesc; }
            set { alertDefDesc = value; }
        }
        public String DefaultAlertTxt
        {
            get { return defaultAlertTxt; }
            set { defaultAlertTxt = value; }
        }

        public List<TblAlertSubscribersTO> AlertSubscribersTOList
        {
            get
            {
                return alertSubscribersTOList;
            }

            set
            {
                alertSubscribersTOList = value;
            }
        }

        public string NavigationUrl { get => navigationUrl; set => navigationUrl = value; }
        #endregion
    }
}
