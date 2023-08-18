using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblFeedbackBL
    {

        List<TblFeedbackTO> SelectAllTblFeedbackList();
        List<TblFeedbackTO> SelectAllTblFeedbackList(int userId, DateTime frmDt, DateTime toDt);
        TblFeedbackTO SelectTblFeedbackTO(Int32 idFeedback);
        int InsertTblFeedback(TblFeedbackTO tblFeedbackTO);
        int InsertTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO);
        int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblFeedback(Int32 idFeedback);
        int DeleteTblFeedback(Int32 idFeedback, SqlConnection conn, SqlTransaction tran);

    }
}