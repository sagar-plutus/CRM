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
    public class DimVehicleTypeBL
    {
        #region Selection

        public static List<DimVehicleTypeTO> SelectAllDimVehicleTypeList()
        {
            return DimVehicleTypeDAO.SelectAllDimVehicleType();
        }

        public static DimVehicleTypeTO SelectDimVehicleTypeTO(Int32 idVehicleType)
        {
            return DimVehicleTypeDAO.SelectDimVehicleType(idVehicleType);
        }

        

        #endregion
        
        #region Insertion
        public static int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
            return DimVehicleTypeDAO.InsertDimVehicleType(dimVehicleTypeTO);
        }

        public static int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehicleTypeDAO.InsertDimVehicleType(dimVehicleTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
            return DimVehicleTypeDAO.UpdateDimVehicleType(dimVehicleTypeTO);
        }

        public static int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehicleTypeDAO.UpdateDimVehicleType(dimVehicleTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimVehicleType(Int32 idVehicleType)
        {
            return DimVehicleTypeDAO.DeleteDimVehicleType(idVehicleType);
        }

        public static int DeleteDimVehicleType(Int32 idVehicleType, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehicleTypeDAO.DeleteDimVehicleType(idVehicleType, conn, tran);
        }

        #endregion
        
    }
}
