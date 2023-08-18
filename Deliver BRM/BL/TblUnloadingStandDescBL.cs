using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblUnloadingStandDescBL
    {
        #region Selection
        public static List<TblUnloadingStandDescTO> SelectAllTblUnloadingStandDescList()
        {
            return TblUnloadingStandDescDAO.SelectAllTblUnloadingStandDesc();
        }

        public static TblUnloadingStandDescTO SelectTblUnloadingStandDescTO(Int32 idUnloadingStandDesc)
        {
            return TblUnloadingStandDescDAO.SelectTblUnloadingStandDesc(idUnloadingStandDesc);
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] Added to select all Unloading Standard Descriptions for drop down
        /// </summary>
        /// <returns></returns>
        public static List<DropDownTO> SelectAllUnloadingStandDescForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = TblUnloadingStandDescDAO.SelectAllUnloadingStandDescForDropDown();

                if (list != null)               
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnloadingStandDescForDropDown");
                return null;
            }
        }



        #endregion

        #region Insertion
        public static int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO UnloadingStandDescTO)
        {
            return TblUnloadingStandDescDAO.InsertTblUnloadingStandDesc(UnloadingStandDescTO);
        }

        public static int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnloadingStandDescDAO.InsertTblUnloadingStandDesc(tblUnloadingStandDescTO, conn, tran);
        }



        #endregion

        #region Updation
        public static int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO)
        {
            return TblUnloadingStandDescDAO.UpdateTblUnloadingStandDesc(tblUnloadingStandDescTO);
        }

        public static int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnloadingStandDescDAO.UpdateTblUnloadingStandDesc(tblUnloadingStandDescTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc)
        {
            return TblUnloadingStandDescDAO.DeleteTblUnloadingStandDesc(idUnloadingStandDesc);
        }

        public static int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnloadingStandDescDAO.DeleteTblUnloadingStandDesc(idUnloadingStandDesc, conn, tran);
        }

        #endregion

    }
}
