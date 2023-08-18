using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblpurchaseEnqShipmemtDtlsExtBL : ITblpurchaseEnqShipmemtDtlsExtBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblpurchaseEnqShipmemtDtlsExtDAO _iTblpurchaseEnqShipmemtDtlsExtDAO;
        public TblpurchaseEnqShipmemtDtlsExtBL(IConnectionString iConnectionString, ITblpurchaseEnqShipmemtDtlsExtDAO iTblpurchaseEnqShipmemtDtlsExtDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblpurchaseEnqShipmemtDtlsExtDAO = iTblpurchaseEnqShipmemtDtlsExtDAO;
        }
        #region Selection
        public  List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExt()
        {
            return _iTblpurchaseEnqShipmemtDtlsExtDAO.SelectAllTblpurchaseEnqShipmemtDtlsExt();
        }

        public List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(Int32 shipmentDtlsId)
        {
            return _iTblpurchaseEnqShipmemtDtlsExtDAO.SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(shipmentDtlsId);
        }

        public List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExtList()
        {
          return _iTblpurchaseEnqShipmemtDtlsExtDAO.SelectAllTblpurchaseEnqShipmemtDtlsExt();
        }

        public  TblpurchaseEnqShipmemtDtlsExtTO SelectTblpurchaseEnqShipmemtDtlsExtTO(Int32 idShipmentDtlsExt)
        {
           return _iTblpurchaseEnqShipmemtDtlsExtDAO.SelectTblpurchaseEnqShipmemtDtlsExt(idShipmentDtlsExt);
        }

        #endregion
        
        #region Insertion
        public  int InsertTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO)
        {
            return _iTblpurchaseEnqShipmemtDtlsExtDAO.InsertTblpurchaseEnqShipmemtDtlsExt(tblpurchaseEnqShipmemtDtlsExtTO);
        }

        public  int InsertTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblpurchaseEnqShipmemtDtlsExtDAO.InsertTblpurchaseEnqShipmemtDtlsExt(tblpurchaseEnqShipmemtDtlsExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO)
        {
            return _iTblpurchaseEnqShipmemtDtlsExtDAO.UpdateTblpurchaseEnqShipmemtDtlsExt(tblpurchaseEnqShipmemtDtlsExtTO);
        }

        public  int UpdateTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblpurchaseEnqShipmemtDtlsExtDAO.UpdateTblpurchaseEnqShipmemtDtlsExt(tblpurchaseEnqShipmemtDtlsExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt)
        {
            return _iTblpurchaseEnqShipmemtDtlsExtDAO.DeleteTblpurchaseEnqShipmemtDtlsExt(idShipmentDtlsExt);
        }

        public  int DeleteTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblpurchaseEnqShipmemtDtlsExtDAO.DeleteTblpurchaseEnqShipmemtDtlsExt(idShipmentDtlsExt, conn, tran);
        }

        #endregion
        
    }
}
