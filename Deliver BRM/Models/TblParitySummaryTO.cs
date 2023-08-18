using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblParitySummaryTO
    {
        #region Declarations
        Int32 idParity;
        Int32 createdBy;
        Int32 isActive;
        DateTime createdOn;
        String remark;

        List<TblParityDetailsTO> parityDetailList;
        Int32 stateId;
        String stateName;
        Double baseValCorAmt;
        Double freightAmt;
        Double expenseAmt;
        Double otherAmt;
        #endregion

        #region Constructor
        public TblParitySummaryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdParity
        {
            get { return idParity; }
            set { idParity = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public List<TblParityDetailsTO> ParityDetailList
        {
            get
            {
                return parityDetailList;
            }

            set
            {
                parityDetailList = value;
            }
        }

        public int StateId { get => stateId; set => stateId = value; }
        public string StateName { get => stateName; set => stateName = value; }
        public double BaseValCorAmt { get => baseValCorAmt; set => baseValCorAmt = value; }
        public double FreightAmt { get => freightAmt; set => freightAmt = value; }
        public double ExpenseAmt { get => expenseAmt; set => expenseAmt = value; }
        public double OtherAmt { get => otherAmt; set => otherAmt = value; }
        #endregion
    }
}
