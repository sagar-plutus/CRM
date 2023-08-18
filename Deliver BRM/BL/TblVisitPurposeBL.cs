using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.BL
{
    public class TblVisitPurposeBL
    {
        #region Selection
        public static List<TblVisitPurposeTO> SelectAllTblVisitPurpose()
        {
            return TblVisitPurposeDAO.SelectAllTblVisitPurpose();
        }

        // Vaibhav [2-Oct-2017] added to select visit purpose list
        public static List<DropDownTO> SelectVisitPurposeListForDropDown(int visitTypeId)
        {
            List<DropDownTO> reportingTypeList = TblVisitPurposeDAO.SelectVisitPurposeListForDropDown(visitTypeId);
            if (reportingTypeList != null)
                return reportingTypeList;
            else
                return null;
        }

        public static TblVisitPurposeTO SelectTblVisitPurposeTO(Int32 idVisitPurpose)
        {
            DataTable tblVisitPurposeTODT = TblVisitPurposeDAO.SelectTblVisitPurpose(idVisitPurpose);
            List<TblVisitPurposeTO> tblVisitPurposeTOList = ConvertDTToList(tblVisitPurposeTODT);
            if (tblVisitPurposeTOList != null && tblVisitPurposeTOList.Count == 1)
                return tblVisitPurposeTOList[0];
            else
                return null;
        }



        public static List<TblVisitPurposeTO> ConvertDTToList(DataTable tblVisitPurposeTODT)
        {
            List<TblVisitPurposeTO> tblVisitPurposeTOList = new List<TblVisitPurposeTO>();
            if (tblVisitPurposeTODT != null)
            {
                
            }
            return tblVisitPurposeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO)
        {
            return TblVisitPurposeDAO.InsertTblVisitPurpose(tblVisitPurposeTO);
        }

        public static int InsertTblVisitPurpose(ref TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                if(conn.State == ConnectionState.Closed)
                conn.Open();

                result = TblVisitPurposeDAO.InsertTblVisitPurpose(tblVisitPurposeTO, conn, tran);
                
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblVisitPurpose");
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitPurpose");
                return -1;
            }
            finally
            {
                //conn.Close();
            }
        }

        //// Vaibhav [3-Oct-2017] added to inser new visit purpose
        //public static ResultMessage SaveNewVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO)
        //{
        //    SqlConnection conn = new SqlConnection(Startup.ConnectionString);
        //    SqlTransaction tran = null;
        //    ResultMessage resultMessage = new ResultMessage();
        //    int result = 0;
        //    try
        //    {
        //        conn.Open();

        //        result = InsertTblVisitPurpose(tblVisitPurposeTO, conn, tran);

        //        if (result != 1)
        //        {
        //            resultMessage.DefaultBehaviour("Error While InsertTblVisitPurpose");
        //            return resultMessage;
        //        }
        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "SaveNewVisitPurpose");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        #endregion

        #region Updation
        public static int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO)
        {
            return TblVisitPurposeDAO.UpdateTblVisitPurpose(tblVisitPurposeTO);
        }

        public static int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitPurposeDAO.UpdateTblVisitPurpose(tblVisitPurposeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblVisitPurpose(Int32 idVisitPurpose)
        {
            return TblVisitPurposeDAO.DeleteTblVisitPurpose(idVisitPurpose);
        }

        public static int DeleteTblVisitPurpose(Int32 idVisitPurpose, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitPurposeDAO.DeleteTblVisitPurpose(idVisitPurpose, conn, tran);
        }

        #endregion

    }
}
