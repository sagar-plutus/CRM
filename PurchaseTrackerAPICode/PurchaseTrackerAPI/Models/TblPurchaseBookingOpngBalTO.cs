using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseBookingOpngBalTO
    {
        #region Declarations
        Int32 idPurchaseBookingOpngBal;
        Int32 enquiryId;
        DateTime balAsOnDate;
        Double openingBalQty;
        Int32 isActive;
        #endregion

        #region Constructor
        public TblPurchaseBookingOpngBalTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseBookingOpngBal
        {
            get { return idPurchaseBookingOpngBal; }
            set { idPurchaseBookingOpngBal = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 EnquiryId
        {
            get { return enquiryId; }
            set { enquiryId = value; }
        }
        public DateTime BalAsOnDate
        {
            get { return balAsOnDate; }
            set { balAsOnDate = value; }
        }
        public Double OpeningBalQty
        {
            get { return openingBalQty; }
            set { openingBalQty = value; }
        }
        #endregion
    }
}
