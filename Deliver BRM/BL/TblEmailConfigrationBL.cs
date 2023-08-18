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
    public class TblEmailConfigrationBL
    {
        #region Selection
        public static List<TblEmailConfigrationTO> SelectAllDimEmailConfigration()
        {
            return TblEmailConfigrationDAO.SelectAllDimEmailConfigration();
        }

        public static List<TblEmailConfigrationTO> SelectAllDimEmailConfigrationList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblEmailConfigrationTO> list = TblEmailConfigrationDAO.SelectAllDimEmailConfigration();
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimEmailConfigrationList");
                return null;
            }
        }

        public static TblEmailConfigrationTO SelectDimEmailConfigrationTO()
        {
            TblEmailConfigrationTO dimEmailConfigrationTODT = TblEmailConfigrationDAO.SelectDimEmailConfigrationIsActive();
            if (dimEmailConfigrationTODT != null)
            {
                return dimEmailConfigrationTODT;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Insertion
        public static int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            return TblEmailConfigrationDAO.InsertDimEmailConfigration(dimEmailConfigrationTO);
        }

        public static int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEmailConfigrationDAO.InsertDimEmailConfigration(dimEmailConfigrationTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            return TblEmailConfigrationDAO.UpdateDimEmailConfigration(dimEmailConfigrationTO);
        }

        public static int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEmailConfigrationDAO.UpdateDimEmailConfigration(dimEmailConfigrationTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteDimEmailConfigration(Int32 idEmailConfig)
        {
            return TblEmailConfigrationDAO.DeleteDimEmailConfigration(idEmailConfig);
        }

        public static int DeleteDimEmailConfigration(Int32 idEmailConfig, SqlConnection conn, SqlTransaction tran)
        {
            return TblEmailConfigrationDAO.DeleteDimEmailConfigration(idEmailConfig, conn, tran);
        }

        #endregion
    }
}
