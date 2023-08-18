using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblpurchaseEnqShipmemtDtlsBL : ITblpurchaseEnqShipmemtDtlsBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblpurchaseEnqShipmemtDtlsDAO _iTblpurchaseEnqShipmemtDtlsDAO;
        public TblpurchaseEnqShipmemtDtlsBL(IConnectionString iConnectionString, ITblpurchaseEnqShipmemtDtlsDAO iTblpurchaseEnqShipmemtDtlsDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblpurchaseEnqShipmemtDtlsDAO = iTblpurchaseEnqShipmemtDtlsDAO;
        }
        #region Selection
        public List<TblpurchaseEnqShipmemtDtlsTO> SelectAllTblpurchaseEnqShipmemtDtls()
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.SelectAllTblpurchaseEnqShipmemtDtls();
        }

        public  List<TblpurchaseEnqShipmemtDtlsTO> SelectAllTblpurchaseEnqShipmemtDtlsList()
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.SelectAllTblpurchaseEnqShipmemtDtls();
        }

        public  TblpurchaseEnqShipmemtDtlsTO SelectTblpurchaseEnqShipmemtDtlsTO(Int32 idShipmentDtls)
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.SelectTblpurchaseEnqShipmemtDtls(idShipmentDtls);
        }

        public List<TblpurchaseEnqShipmemtDtlsTO> SelectTblpurchaseEnqShipmemtDtlsTOByEnqId(Int32 purchaseEnqId)
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.SelectTblpurchaseEnqShipmemtDtlsTOByEnqId(purchaseEnqId);
        }

        #endregion

        #region Insertion
        public int InsertTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO)
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.InsertTblpurchaseEnqShipmemtDtls(tblpurchaseEnqShipmemtDtlsTO);
        }

        public  int InsertTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.InsertTblpurchaseEnqShipmemtDtls(tblpurchaseEnqShipmemtDtlsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO)
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.UpdateTblpurchaseEnqShipmemtDtls(tblpurchaseEnqShipmemtDtlsTO);
        }

        public  int UpdateTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.UpdateTblpurchaseEnqShipmemtDtls(tblpurchaseEnqShipmemtDtlsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblpurchaseEnqShipmemtDtls(Int32 idShipmentDtls)
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.DeleteTblpurchaseEnqShipmemtDtls(idShipmentDtls);
        }

        public  int DeleteTblpurchaseEnqShipmemtDtls(Int32 idShipmentDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblpurchaseEnqShipmemtDtlsDAO.DeleteTblpurchaseEnqShipmemtDtls(idShipmentDtls, conn, tran);
        }

        #endregion
        
    }
}
