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
    public class TblEInvoiceApiResponseBL
    {

        public TblEInvoiceApiResponseBL()
        {

        }

        #region Selection

        public List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponseList()
        {
            return TblEInvoiceApiResponseDAO.SelectAllTblEInvoiceApiResponse();
        }

        public List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponseList(Int32 apiId)
        {
            return TblEInvoiceApiResponseDAO.SelectAllTblEInvoiceApiResponse(apiId);
        }

        public List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseList(int idResponse)
        {
            return TblEInvoiceApiResponseDAO.SelectTblEInvoiceApiResponseList(idResponse);
        }
        public List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(int invoiceId)
        {
            return TblEInvoiceApiResponseDAO.SelectTblEInvoiceApiResponseListForInvoiceId(invoiceId);
        }
        public static List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(int invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblEInvoiceApiResponseDAO.SelectTblEInvoiceApiResponseListForInvoiceId(invoiceId, conn, tran);
        }

        #endregion

        #region Insertion

        public int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO)
        {
            return TblEInvoiceApiResponseDAO.InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO);
        }
        public int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEInvoiceApiResponseDAO.InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO, conn, tran);
        }

        #endregion

        #region Deletion

        public int DeleteTblEInvoiceApiResponse(Int32 idApi)
        {
            return TblEInvoiceApiResponseDAO.DeleteTblEInvoiceApiResponse(idApi);
        }
        public int DeleteTblEInvoiceApiResponse(Int32 idApi, SqlConnection conn, SqlTransaction tran)
        {
            return TblEInvoiceApiResponseDAO.DeleteTblEInvoiceApiResponse(idApi, conn, tran);
        }

        #endregion
    }
}
