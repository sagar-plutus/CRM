using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SalesTrackerAPI.BL
{
    public class DimOrgTypeBL
    {
        #region Selection

        public static List<DimOrgTypeTO> SelectAllDimOrgTypeList()
        {
            return DimOrgTypeDAO.SelectAllDimOrgType();
        }

        public static DimOrgTypeTO SelectDimOrgTypeTO(Int32 idOrgType,SqlConnection conn,SqlTransaction tran)
        {
           return DimOrgTypeDAO.SelectDimOrgType(idOrgType,conn,tran);
        }

        public static DimOrgTypeTO SelectDimOrgTypeTO(Int32 idOrgType)
        {
            SqlConnection connection = new SqlConnection(Startup.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                return DimOrgTypeDAO.SelectDimOrgType(idOrgType, connection, transaction);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region Insertion
        public static int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            return DimOrgTypeDAO.InsertDimOrgType(dimOrgTypeTO);
        }

        public static int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimOrgTypeDAO.InsertDimOrgType(dimOrgTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            return DimOrgTypeDAO.UpdateDimOrgType(dimOrgTypeTO);
        }

        public static int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimOrgTypeDAO.UpdateDimOrgType(dimOrgTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimOrgType(Int32 idOrgType)
        {
            return DimOrgTypeDAO.DeleteDimOrgType(idOrgType);
        }

        public static int DeleteDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran)
        {
            return DimOrgTypeDAO.DeleteDimOrgType(idOrgType, conn, tran);
        }

        #endregion
        
    }
}
