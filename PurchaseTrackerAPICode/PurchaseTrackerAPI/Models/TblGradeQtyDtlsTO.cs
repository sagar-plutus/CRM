using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblGradeQtyDtlsTO
    {
        #region Declarations
        Int32 idGradeQtyDtls;
        Int32 prodClassId;
        Int32 prodItemId;
        Int32 supervisorId;
        Int32 qtyTypeId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime date;
        DateTime createdOn;
        DateTime updatedOn;
        Double qty;
        Double qty2;
        #endregion

        #region Constructor
        public TblGradeQtyDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGradeQtyDtls
        {
            get { return idGradeQtyDtls; }
            set { idGradeQtyDtls = value; }
        }
        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Int32 SupervisorId
        {
            get { return supervisorId; }
            set { supervisorId = value; }
        }
        public Int32 QtyTypeId
        {
            get { return qtyTypeId; }
            set { qtyTypeId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
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
        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        public Double Qty2
        {
            get { return qty2; }
            set { qty2 = value; }
        }
        #endregion
    }
}
