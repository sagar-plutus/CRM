using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblPaymentTermBL
    {
        #region Selection
        public static DataTable SelectAllTblPaymentTerm()
        {
            return TblPaymentTermDAO.SelectAllTblPaymentTerm();
        }

        public static List<TblPaymentTermTO> SelectAllTblPaymentTermList()
        {
            DataTable tblPaymentTermTODT = TblPaymentTermDAO.SelectAllTblPaymentTerm();
            return ConvertDTToList(tblPaymentTermTODT);
        }

        public static TblPaymentTermTO SelectTblPaymentTermTO(Int32 idPaymentTerm)
        {
            DataTable tblPaymentTermTODT = TblPaymentTermDAO.SelectTblPaymentTerm(idPaymentTerm);
            List<TblPaymentTermTO> tblPaymentTermTOList = ConvertDTToList(tblPaymentTermTODT);
            if (tblPaymentTermTOList != null && tblPaymentTermTOList.Count == 1)
                return tblPaymentTermTOList[0];
            else
                return null;
        }

        public static List<DropDownTO> SelectPaymentTermListForDopDown()
        {
            List<DropDownTO> paymentTermTOList = TblPaymentTermDAO.SelecPaymentTermForDropDown();
            if (paymentTermTOList != null)
                return paymentTermTOList;
            else
                return null;
        }

        public static List<TblPaymentTermTO> ConvertDTToList(DataTable tblPaymentTermTODT)
        {
            List<TblPaymentTermTO> tblPaymentTermTOList = new List<TblPaymentTermTO>();
            if (tblPaymentTermTODT != null)
            {
            }
            return tblPaymentTermTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO)
        {
            return TblPaymentTermDAO.InsertTblPaymentTerm(tblPaymentTermTO);
        }

        public static int InsertTblPaymentTerm(ref TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                if(conn.State == ConnectionState.Closed)
                conn.Open();

                result = TblPaymentTermDAO.InsertTblPaymentTerm(ref tblPaymentTermTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblPaymentTerm");
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblPaymentTerm");
                return -1;
            }
            finally
            {
                //conn.Close();
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO)
        {
            return TblPaymentTermDAO.UpdateTblPaymentTerm(tblPaymentTermTO);
        }

        public static int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPaymentTermDAO.UpdateTblPaymentTerm(tblPaymentTermTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblPaymentTerm(Int32 idPaymentTerm)
        {
            return TblPaymentTermDAO.DeleteTblPaymentTerm(idPaymentTerm);
        }

        public static int DeleteTblPaymentTerm(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran)
        {
            return TblPaymentTermDAO.DeleteTblPaymentTerm(idPaymentTerm, conn, tran);
        }

        #endregion

    }
}
