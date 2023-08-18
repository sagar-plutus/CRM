using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblOverdueDtlBL
    {
        #region Selection

        public static List<TblOverdueDtlTO> SelectAllTblOverdueDtlList()
        {
            return  TblOverdueDtlDAO.SelectAllTblOverdueDtl();
        }

        /// <summary>
        /// [2017-12-01]Vijaymala:Added to get overdue detail List of organization
        /// </summary>
        /// <param name="dealerIds"></param>
        /// <returns></returns>
        public static List<TblOverdueDtlTO> SelectAllTblOverdueDtlList(String dealerIds)
        {
            return TblOverdueDtlDAO.SelectAllTblOverdueDtl(dealerIds);
        }

        public static TblOverdueDtlTO SelectTblOverdueDtlTO(Int32 idOverdueDtl)
        {
            return TblOverdueDtlDAO.SelectTblOverdueDtl(idOverdueDtl);
        }

        /// <summary>
        /// [2017-12-01]Vijaymala:Added to get overdue detail List of particular organization
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public static List<TblOverdueDtlTO> SelectTblOverdueDtlList(Int32 dealerId)
        {
            return TblOverdueDtlDAO.SelectTblOverdueDtlList(dealerId);
        }



        #endregion

        #region Insertion
        public static int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO)
        {
            return TblOverdueDtlDAO.InsertTblOverdueDtl(tblOverdueDtlTO);
        }

        public static int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOverdueDtlDAO.InsertTblOverdueDtl(tblOverdueDtlTO, conn, tran);
        }


        /// <summary>
        /// [11-12-2017]Vijaymala :Added to save  overDue detail of organization which exports from excel
        /// </summary>
        /// <param name="tblOverdueDtlTOList"></param>
        /// <returns></returns>

        public static ResultMessage SaveOrgOverDueDtl(List<TblOverdueDtlTO> tblOverdueDtlTOList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Delete previous records

                result = TblOverdueDtlBL.DeleteTblOverdueDtl(conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Exception Error While Delete TblOverdueDtl";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = -1;
                    return resultMessage;

                }

                #endregion


                #region Insert New Records

                DateTime createdOn = Constants.ServerDateTime;

                for (int i = 0; i < tblOverdueDtlTOList.Count; i++)
                {
                    TblOverdueDtlTO tblOverdueDtlTO = tblOverdueDtlTOList[i];
                    tblOverdueDtlTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblOverdueDtlTO.CreatedOn = createdOn;

                    result = InsertTblOverdueDtl(tblOverdueDtlTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Inserting Organization Enquiry Details";
                        return resultMessage;
                    }

                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "OverDue  Details Of Organization Updated Successfully.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.Text = "Exception Error While Record Save in BL : SaveOrgOverDueDtl";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO)
        {
            return TblOverdueDtlDAO.UpdateTblOverdueDtl(tblOverdueDtlTO);
        }

        public static int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOverdueDtlDAO.UpdateTblOverdueDtl(tblOverdueDtlTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblOverdueDtl(Int32 idOverdueDtl)
        {
            return TblOverdueDtlDAO.DeleteTblOverdueDtl(idOverdueDtl);
        }

        public static int DeleteTblOverdueDtl(Int32 idOverdueDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblOverdueDtlDAO.DeleteTblOverdueDtl(idOverdueDtl, conn, tran);
        }

        /// <summary>
        /// [11-12-2017]Vijaymala: Added to delete previous overdue details
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int DeleteTblOverdueDtl(SqlConnection conn, SqlTransaction tran)
        {
            return TblOverdueDtlDAO.DeleteTblOverdueDtl(conn, tran);
        }

        #endregion

    }
}
