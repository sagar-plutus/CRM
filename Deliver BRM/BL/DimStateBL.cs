using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class DimStateBL
    {
        #region Selection
        public static List<DimStateTO> SelectAllDimState()
        {
            return DimStateDAO.SelectAllDimState();
        }

        //public static List<DimStateTO> SelectAllDimStateList()
        //{
        //    List<DimStateTO> dimStateTODT = DimStateDAO.SelectAllDimState();
        //    return dimStateTODT;
        //}

        public static DimStateTO SelectDimStateTO(Int32 idState)
        {
            DimStateTO dimStateTO = DimStateDAO.SelectDimState(idState);
            if (dimStateTO != null)
                return dimStateTO;
            else
                return null;
        }

        public static List<DimStateTO> ConvertDTToList(DataTable dimStateTODT)
        {
            List<DimStateTO> dimStateTOList = new List<DimStateTO>();
            if (dimStateTODT != null)
            {
             
            }
            return dimStateTOList;
        }

        #endregion

        #region Insertion
        public static int InsertDimState(DimStateTO dimStateTO)
        {
            return DimStateDAO.InsertDimState(dimStateTO);
        }

        public static int InsertDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimStateDAO.InsertDimState(dimStateTO, conn, tran);
        }

        //Sudhir[09-12-2017] Added for SaveNewState.
        public static ResultMessage SaveNewState(DimStateTO dimStateTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                result = InsertDimState(dimStateTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("SaveNewState");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewState");
                return resultMessage;
            }
            finally
            {

            }
        }

        public static ResultMessage UpdateState(DimStateTO dimStateTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                result = UpdateDimState(dimStateTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("UpdateState");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateState");
                return resultMessage;
            }
            finally
            {

            }
        }
        #endregion

        #region Updation
        public static int UpdateDimState(DimStateTO dimStateTO)
        {
            return DimStateDAO.UpdateDimState(dimStateTO);
        }

        public static int UpdateDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimStateDAO.UpdateDimState(dimStateTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteDimState(Int32 idState)
        {
            return DimStateDAO.DeleteDimState(idState);
        }

        public static int DeleteDimState(Int32 idState, SqlConnection conn, SqlTransaction tran)
        {
            return DimStateDAO.DeleteDimState(idState, conn, tran);
        }

        #endregion

    }
}

