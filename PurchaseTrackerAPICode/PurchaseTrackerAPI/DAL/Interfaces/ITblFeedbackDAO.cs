using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblFeedbackDAO
    {
        String SqlSelectQuery();
        List<TblFeedbackTO> SelectAllTblFeedback();
        TblFeedbackTO SelectTblFeedback(Int32 idFeedback);
        List<TblFeedbackTO> SelectAllTblFeedback(int userId, DateTime frmDt, DateTime toDt);
        List<TblFeedbackTO> ConvertDTToList(SqlDataReader tblFeedbackTODT);
        int InsertTblFeedback(TblFeedbackTO tblFeedbackTO);
        int InsertTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblFeedbackTO tblFeedbackTO, SqlCommand cmdInsert);
        int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO);
        int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblFeedbackTO tblFeedbackTO, SqlCommand cmdUpdate);
        int DeleteTblFeedback(Int32 idFeedback);
        int DeleteTblFeedback(Int32 idFeedback, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idFeedback, SqlCommand cmdDelete);

    }
}