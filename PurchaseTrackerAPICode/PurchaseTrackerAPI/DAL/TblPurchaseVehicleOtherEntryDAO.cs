using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseVehicleOtherEntryDAO : ITblPurchaseVehicleOtherEntryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseVehicleOtherEntryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " select tblPurchaseVehicleOtherEntry.*, dimVehicleType.vehicleTypeDesc from tblPurchaseVehicleOtherEntry  " +
                                 
                                 " Inner Join dimVehicleType dimVehicleType on dimVehicleType.idVehicleType=tblPurchaseVehicleSpotEntry.vehicleTypeId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection        
        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseVehicleOtherEntryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblPurchaseVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseVehicleSpotEntryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseVehicleOtherEntry]( " + 
            
            "  [categoryId]" +
            " ,[vehicleTypeId]" +       
            " ,[createdBy]" +            
            " ,[createdOn]" +           
            " ,[vehicleNo]" +
            " ,[driverName]" +
            " ,[remark]" +            
            " ,[driverContactNo]" +
             " ,[isSelfVehicle]" +
            " )" +
" VALUES (" +
            
            "  @CategoryId " +
            " ,@VehicleTypeId " +           
            " ,@CreatedBy " +           
            " ,@CreatedOn " +           
            " ,@VehicleNo " +
            " ,@DriverName " +
            " ,@Remark " +            
            " ,@driverContactNo " +
            " ,@isSelfVehicle " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@CategoryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleOtherEntryTO.CategoryId);
            cmdInsert.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleOtherEntryTO.VehicleTypeId);
             cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleOtherEntryTO.CreatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleOtherEntryTO.CreatedOn);
              cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseVehicleOtherEntryTO.VehicleNo;
            cmdInsert.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleOtherEntryTO.DriverName);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleOtherEntryTO.Remark);
             cmdInsert.Parameters.Add("@isSelfVehicle", System.Data.SqlDbType.Int).Value =(tblPurchaseVehicleOtherEntryTO.IsSelfVehicle);
            cmdInsert.Parameters.Add("@driverContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleOtherEntryTO.DriverContactNo);
              if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblPurchaseVehicleOtherEntryTO.IdVehicleOtherEntry = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        
        #endregion
        
        #region Deletion
       
        #endregion

    
        
    }
}
