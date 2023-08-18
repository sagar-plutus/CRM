using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;

namespace SalesTrackerAPI.BL
{
    public class DimRoleTypeBL
    {
        #region Selection

        public static List<DimRoleTypeTO> SelectAllDimRoleTypeList()
        {
            return  DimRoleTypeDAO.SelectAllDimRoleTypeList();
        }

        public static DimRoleTypeTO SelectDimRoleTypeTO(Int32 idRoleType)
        {
            return  DimRoleTypeDAO.SelectDimRoleType(idRoleType);           
        }

       

        #endregion
        
        #region Insertion
        public static int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO)
        {
            return DimRoleTypeDAO.InsertDimRoleType(dimRoleTypeTO);
        }

        public static int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimRoleTypeDAO.InsertDimRoleType(dimRoleTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO)
        {
            return DimRoleTypeDAO.UpdateDimRoleType(dimRoleTypeTO);
        }

        public static int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimRoleTypeDAO.UpdateDimRoleType(dimRoleTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimRoleType(Int32 idRoleType)
        {
            return DimRoleTypeDAO.DeleteDimRoleType(idRoleType);
        }

        public static int DeleteDimRoleType(Int32 idRoleType, SqlConnection conn, SqlTransaction tran)
        {
            return DimRoleTypeDAO.DeleteDimRoleType(idRoleType, conn, tran);
        }

        #endregion
        
    }
}
