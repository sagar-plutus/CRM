using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.BL;
using SalesTrackerAPI.DAL;

namespace SalesTrackerAPI.BL
{
    public class TblEInvoiceApiBL
    {
        public TblEInvoiceApiBL()
        {
            
        }

        #region Selection

        public List<TblEInvoiceApiTO> SelectAllTblEInvoiceApiList()
        {
            return TblEInvoiceApiDAO.SelectAllTblEInvoiceApi();
        }

        public List<TblEInvoiceApiTO> SelectAllTblEInvoiceApiList(Int32 idApi)
        {
            return TblEInvoiceApiDAO.SelectAllTblEInvoiceApi(idApi);
        }

        public List<TblEInvoiceApiTO> SelectTblEInvoiceApiList(string apiName)
        {
            return TblEInvoiceApiDAO.SelectTblEInvoiceApi(apiName);
        }

        #endregion

        #region Insertion

        public int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO)
        {
            return TblEInvoiceApiDAO.InsertTblEInvoiceApi(tblEInvoiceApiTO);
        }
        public int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEInvoiceApiDAO.InsertTblEInvoiceApi(tblEInvoiceApiTO, conn, tran);
        }

        #endregion

        #region Updation

        public int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO)
        {
            return TblEInvoiceApiDAO.UpdateTblEInvoiceApi(tblEInvoiceApiTO);
        }
        public int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEInvoiceApiDAO.UpdateTblEInvoiceApi(tblEInvoiceApiTO, conn, tran);
        }

        #endregion

        #region Deletion

        public int DeleteTblEInvoiceApi(Int32 idApi)
        {
            return TblEInvoiceApiDAO.DeleteTblEInvoiceApi(idApi);
        }
        public int DeleteTblEInvoiceApi(Int32 idApi, SqlConnection conn, SqlTransaction tran)
        {
            return TblEInvoiceApiDAO.DeleteTblEInvoiceApi(idApi, conn, tran);
        }
        #endregion
    }
}
