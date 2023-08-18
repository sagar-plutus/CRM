using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblAddonsFunDtlsBL
    {
        #region Selection
        public static List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtlsList()
        {
            return TblAddonsFunDtlsDAO.SelectAllTblAddonsFunDtls();
        }
        
        public static TblAddonsFunDtlsTO SelectTblAddonsFunDtlsTO(int idAddonsfunDtls)
        {
           return  TblAddonsFunDtlsDAO.SelectTblAddonsFunDtls(idAddonsfunDtls);
               
        }

        public static List<TblAddonsFunDtlsTO> SelectAddonDetails(int transId, int ModuleId, String TransactionType, String PageElementId, String transIds)
        {
            return TblAddonsFunDtlsDAO.SelectAddonDetailsList(transId, ModuleId, TransactionType, PageElementId, transIds);
        }

        #endregion

        #region Insertion
        public static ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            tblAddonsFunDtlsTO.CreatedOn = Constants.ServerDateTime;
            tblAddonsFunDtlsTO.IsActive = 1;
            result = TblAddonsFunDtlsDAO.InsertTblAddonsFunDtls(tblAddonsFunDtlsTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Inserting Data into TblAddonsFunDtls";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public static ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            result= TblAddonsFunDtlsDAO.InsertTblAddonsFunDtls(tblAddonsFunDtlsTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region Updation
        public static ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            tblAddonsFunDtlsTO.UpdatedOn = Constants.ServerDateTime;
            tblAddonsFunDtlsTO.UpdatedBy = tblAddonsFunDtlsTO.CreatedBy;
            result = TblAddonsFunDtlsDAO.UpdateTblAddonsFunDtls(tblAddonsFunDtlsTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Updating Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Updating Data";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }


        public static ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
           result=  TblAddonsFunDtlsDAO.UpdateTblAddonsFunDtls(tblAddonsFunDtlsTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region Deletion
        public static int DeleteTblAddonsFunDtls(int idAddonsfunDtls)
        {
            return TblAddonsFunDtlsDAO.DeleteTblAddonsFunDtls(idAddonsfunDtls);
        }

        public static int DeleteTblAddonsFunDtls(int idAddonsfunDtls, SqlConnection conn, SqlTransaction tran)
        {
            return TblAddonsFunDtlsDAO.DeleteTblAddonsFunDtls(idAddonsfunDtls, conn, tran);
        }

        #endregion
        
    }
}
