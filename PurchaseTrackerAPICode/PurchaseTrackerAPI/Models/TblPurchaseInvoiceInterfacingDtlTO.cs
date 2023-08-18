using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseInvoiceInterfacingDtlTO
    {
        #region Declarations
        Int32 idPurInvInterfacingDtl;
        Int32 voucherTypeId;
        Int32 cgstId;
        Int32 sgstId;
        Int32 igstId;
        Int32 ipTransportAdvAccId;
        Int32 costCategoryId;
        Int32 gradeId;
        Int32 purAccId;
        Int32 otherExpAccId;
        Int32 materialtemId;
        Int32 costCenterId;
        Int64 purchaseInvoiceId;
        String narration;
        String sGSTINPUT;
        String cGSTINPUT;
        String iGSTINPUT;
        String purchaseAcc;
        Int32 otherExpInsuAccId;
        Int32 tdsAccId;

        #endregion

        #region Constructor
        public TblPurchaseInvoiceInterfacingDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurInvInterfacingDtl
        {
            get { return idPurInvInterfacingDtl; }
            set { idPurInvInterfacingDtl = value; }
        }
        public Int32 VoucherTypeId
        {
            get { return voucherTypeId; }
            set { voucherTypeId = value; }
        }
        public Int32 CgstId
        {
            get { return cgstId; }
            set { cgstId = value; }
        }
        public Int32 SgstId
        {
            get { return sgstId; }
            set { sgstId = value; }
        }
        public Int32 IgstId
        {
            get { return igstId; }
            set { igstId = value; }
        }
        public Int32 IpTransportAdvAccId
        {
            get { return ipTransportAdvAccId; }
            set { ipTransportAdvAccId = value; }
        }
        public Int32 CostCategoryId
        {
            get { return costCategoryId; }
            set { costCategoryId = value; }
        }
        public Int32 GradeId
        {
            get { return gradeId; }
            set { gradeId = value; }
        }
        public Int32 PurAccId
        {
            get { return purAccId; }
            set { purAccId = value; }
        }
        public Int32 OtherExpAccId
        {
            get { return otherExpAccId; }
            set { otherExpAccId = value; }
        }
        public Int32 MaterialtemId
        {
            get { return materialtemId; }
            set { materialtemId = value; }
        }
        public Int32 CostCenterId
        {
            get { return costCenterId; }
            set { costCenterId = value; }
        }
        public Int64 PurchaseInvoiceId
        {
            get { return purchaseInvoiceId; }
            set { purchaseInvoiceId = value; }
        }
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        public String SGSTINPUT
        {
            get { return sGSTINPUT; }
            set { sGSTINPUT = value; }
        }
        public String CGSTINPUT
        {
            get { return cGSTINPUT; }
            set { cGSTINPUT = value; }
        }
        public String IGSTINPUT
        {
            get { return iGSTINPUT; }
            set { iGSTINPUT = value; }
        }
        public String PurchaseAcc
        {
            get { return purchaseAcc; }
            set { purchaseAcc = value; }
        }
        
        public Int32 OtherExpInsuAccId
        {
            get { return otherExpInsuAccId; }
            set { otherExpInsuAccId = value; }
        }
        public Int32 TdsAccId
        {
            get { return tdsAccId; }
            set { tdsAccId = value; }
        }
        #endregion
    }
}
