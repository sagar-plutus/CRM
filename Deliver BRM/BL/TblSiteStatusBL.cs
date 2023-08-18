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
    public class TblSiteStatusBL
    {
        #region Selection
        public static DataTable SelectAllTblSiteStatus()
        {
            return TblSiteStatusDAO.SelectAllTblSiteStatus();
        }

        public static List<TblSiteStatusTO> SelectAllTblSiteStatusList()
        {
            DataTable tblSiteStatusTODT = TblSiteStatusDAO.SelectAllTblSiteStatus();
            return ConvertDTToList(tblSiteStatusTODT);
        }

        public static TblSiteStatusTO SelectTblSiteStatusTO(Int32 idSiteStatus)
        {
            DataTable tblSiteStatusTODT = TblSiteStatusDAO.SelectTblSiteStatus(idSiteStatus);
            List<TblSiteStatusTO> tblSiteStatusTOList = ConvertDTToList(tblSiteStatusTODT);
            if (tblSiteStatusTOList != null && tblSiteStatusTOList.Count == 1)
                return tblSiteStatusTOList[0];
            else
                return null;
        }

        // Vaibhav [3-Oct-2017] added to select site status fro drop down
        public static List<DropDownTO> SelectAllSiteStatusForDropDown()
        {
            List<DropDownTO> siteStatusTOList = TblSiteStatusDAO.SelectSiteStatusForDropDown();
            if (siteStatusTOList != null)
                return siteStatusTOList;
            else
                return null;
        }

        public static List<TblSiteStatusTO> ConvertDTToList(DataTable tblSiteStatusTODT)
        {
            List<TblSiteStatusTO> tblSiteStatusTOList = new List<TblSiteStatusTO>();
            if (tblSiteStatusTODT != null)
            {
            }
            return tblSiteStatusTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblSiteStatus(TblSiteStatusTO tblSiteStatusTO)
        {
            return TblSiteStatusDAO.InsertTblSiteStatus(tblSiteStatusTO);
        }

        public static int InsertTblSiteStatus(ref TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                result = TblSiteStatusDAO.InsertTblSiteStatus(ref tblSiteStatusTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblSiteStatus");
                return -1;
            }
            finally
            {
                //conn.Close();
            }
        }

        // Vaibhav [3-Oct-2017] added to save new site status
        public static ResultMessage SaveNewSiteStatus(TblSiteStatusTO tblSiteStatusTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();

                result = InsertTblSiteStatus(ref tblSiteStatusTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewSiteStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO)
        {
            return TblSiteStatusDAO.UpdateTblSiteStatus(tblSiteStatusTO);
        }

        public static int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSiteStatusDAO.UpdateTblSiteStatus(tblSiteStatusTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblSiteStatus(Int32 idSiteStatus)
        {
            return TblSiteStatusDAO.DeleteTblSiteStatus(idSiteStatus);
        }

        public static int DeleteTblSiteStatus(Int32 idSiteStatus, SqlConnection conn, SqlTransaction tran)
        {
            return TblSiteStatusDAO.DeleteTblSiteStatus(idSiteStatus, conn, tran);
        }

        #endregion

    }
}
