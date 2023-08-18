using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class DimMstDesignationBL
    {
        #region Selection
       

        public static List<DimMstDesignationTO> SelectAllDimMstDesignationList()
        {
            return DimMstDesignationDAO.SelectAllDimMstDesignation();
        }

        public static DimMstDesignationTO SelectDimMstDesignationTO(Int32 idDesignation)
        {
            return DimMstDesignationDAO.SelectDimMstDesignation(idDesignation);
        
        }

        public static List<DropDownTO> SelectAllDesignationForDropDownList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> dropDownTO = DAL.DimMstDesignationDAO.SelectAllDesignationForDropDownList();
                if (dropDownTO != null)
                    return dropDownTO;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDesignationForDropDown");
                return null;
            }
        }



        #endregion

        #region Insertion
        public static int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {
            return DimMstDesignationDAO.InsertDimMstDesignation(dimMstDesignationTO);
        }

        public static int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDesignationDAO.InsertDimMstDesignation(dimMstDesignationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {
            return DimMstDesignationDAO.UpdateDimMstDesignation(dimMstDesignationTO);
        }

        public static int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDesignationDAO.UpdateDimMstDesignation(dimMstDesignationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimMstDesignation(Int32 idDesignation)
        {
            return DimMstDesignationDAO.DeleteDimMstDesignation(idDesignation);
        }

        public static int DeleteDimMstDesignation(Int32 idDesignation, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDesignationDAO.DeleteDimMstDesignation(idDesignation, conn, tran);
        }

        #endregion
        
    }
}
