using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class DimStateBL : IDimStateBL
    {
        private readonly IDimStateDAO _iDimStateDAO;
        public DimStateBL(IDimStateDAO iDimStateDAO) {
            _iDimStateDAO = iDimStateDAO;
        }
        #region Selection
        public  List<DimStateTO> SelectAllDimState()
        {
            return _iDimStateDAO.SelectAllDimState();
        }

        //public static List<DimStateTO> SelectAllDimStateList()
        //{
        //    List<DimStateTO> dimStateTODT = _iDimStateDAO.SelectAllDimState();
        //    return dimStateTODT;
        //}

        public  DimStateTO SelectDimStateTO(Int32 idState)
        {
            DimStateTO dimStateTO = _iDimStateDAO.SelectDimState(idState);
            if (dimStateTO != null)
                return dimStateTO;
            else
                return null;
        }

        public  List<DimStateTO> ConvertDTToList(DataTable dimStateTODT)
        {
            List<DimStateTO> dimStateTOList = new List<DimStateTO>();
            if (dimStateTODT != null)
            {
             
            }
            return dimStateTOList;
        }

        #endregion

        #region Insertion
        public  int InsertDimState(DimStateTO dimStateTO)
        {
            return _iDimStateDAO.InsertDimState(dimStateTO);
        }

        public  int InsertDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStateDAO.InsertDimState(dimStateTO, conn, tran);
        }

        //Sudhir[09-12-2017] Added for SaveNewState.
        public  ResultMessage SaveNewState(DimStateTO dimStateTO)
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

        public  ResultMessage UpdateState(DimStateTO dimStateTO)
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
        public  int UpdateDimState(DimStateTO dimStateTO)
        {
            return _iDimStateDAO.UpdateDimState(dimStateTO);
        }

        public  int UpdateDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStateDAO.UpdateDimState(dimStateTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteDimState(Int32 idState)
        {
            return _iDimStateDAO.DeleteDimState(idState);
        }

        public  int DeleteDimState(Int32 idState, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStateDAO.DeleteDimState(idState, conn, tran);
        }

        #endregion

    }
}

