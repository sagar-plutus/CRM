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
    public class TblEInvoiceSessionApiResponseBL
    {
        
        public TblEInvoiceSessionApiResponseBL()
        {
            
        }

        #region Selection

        public List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponseList()
        {
            return TblEInvoiceSessionApiResponseDAO.SelectAllTblEInvoiceSessionApiResponse();
        }

        public List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponseList(Int32 apiId)
        {
            return TblEInvoiceSessionApiResponseDAO.SelectAllTblEInvoiceSessionApiResponse(apiId);
        }

        public List<TblEInvoiceSessionApiResponseTO> SelectTblEInvoiceSessionApiResponseList(int idResponse)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Insertion

        public int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO)
        {
            return TblEInvoiceSessionApiResponseDAO.InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO);
        }
        public int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEInvoiceSessionApiResponseDAO.InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO, conn, tran);
        }

        #endregion

        #region Deletion

        public int DeleteTblEInvoiceSessionApiResponse(Int32 idApi)
        {
            return TblEInvoiceSessionApiResponseDAO.DeleteTblEInvoiceSessionApiResponse(idApi);
        }
        public int DeleteTblEInvoiceSessionApiResponse(Int32 idApi, SqlConnection conn, SqlTransaction tran)
        {
            return TblEInvoiceSessionApiResponseDAO.DeleteTblEInvoiceSessionApiResponse(idApi, conn, tran);
        }
        #endregion
    }
}
