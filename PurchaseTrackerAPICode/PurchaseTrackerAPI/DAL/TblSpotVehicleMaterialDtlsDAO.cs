using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
//using System.Windows.Forms;
using System.Configuration;
using PurchaseTrackerAPI.Models;
using TO;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblSpotVehicleMaterialDtlsDAO : ITblSpotVehicleMaterialDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblSpotVehicleMaterialDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSpotVehMatDtls]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblSpotVehMatDtlsTO> SelectAllTblSpotVehicleMaterialDtls()
        {
            //  String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblSpotVehicleMaterialDtls", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.IdTblSpotVehicleMaterialDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSpotVehMatDtlsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                // String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblSpotVehMatDtlsTO> SelectTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls)
        {
            // String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE vehSpotEntryId = " + idTblSpotVehicleMaterialDtls + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblSpotVehicleMaterialDtls", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.IdTblSpotVehicleMaterialDtls;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSpotVehMatDtlsTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public  List<TblSpotVehMatDtlsTO> SelectTblSpotVehMatDtls(Int32 idVehicleSpotEntry)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE vehSpotEntryId = " + idVehicleSpotEntry + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSpotVehMatDtlsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblSpotVehMatDtlsTO> SelectAllTblSpotVehicleMaterialDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblSpotVehicleMaterialDtls", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.IdTblSpotVehicleMaterialDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSpotVehMatDtlsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public  List<TblSpotVehMatDtlsTO> ConvertDTToList(SqlDataReader tblSpotVehicleMaterialDtlsTODT)
        {
            List<TblSpotVehMatDtlsTO> tblSpotVehicleMaterialDtlsTOList = new List<TblSpotVehMatDtlsTO>();
            if (tblSpotVehicleMaterialDtlsTODT != null)
            {
                while (tblSpotVehicleMaterialDtlsTODT.Read())
                {
                    TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTONew = new TblSpotVehMatDtlsTO();
                    if (tblSpotVehicleMaterialDtlsTODT["idSpotVehMatDtls"] != DBNull.Value)
                        tblSpotVehicleMaterialDtlsTONew.IdSpotVehMatDtls = Convert.ToInt32(tblSpotVehicleMaterialDtlsTODT["idSpotVehMatDtls"].ToString());
                    if (tblSpotVehicleMaterialDtlsTODT["vehSpotEntryId"] != DBNull.Value)
                        tblSpotVehicleMaterialDtlsTONew.VehSpotEntryId = Convert.ToInt32(tblSpotVehicleMaterialDtlsTODT["vehSpotEntryId"].ToString());
                    if (tblSpotVehicleMaterialDtlsTODT["prodClassId"] != DBNull.Value)
                        tblSpotVehicleMaterialDtlsTONew.ProdClassId = Convert.ToInt32(tblSpotVehicleMaterialDtlsTODT["prodClassId"].ToString());
                    if (tblSpotVehicleMaterialDtlsTODT["prodItemId"] != DBNull.Value)
                        tblSpotVehicleMaterialDtlsTONew.ProdItemId = Convert.ToInt32(tblSpotVehicleMaterialDtlsTODT["prodItemId"].ToString());
                    if (tblSpotVehicleMaterialDtlsTODT["qty"] != DBNull.Value)
                        tblSpotVehicleMaterialDtlsTONew.Qty = Convert.ToDouble(tblSpotVehicleMaterialDtlsTODT["qty"].ToString());
                    tblSpotVehicleMaterialDtlsTOList.Add(tblSpotVehicleMaterialDtlsTONew);
                }
            }
            return tblSpotVehicleMaterialDtlsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO)
        {
            // String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSpotVehicleMaterialDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                // String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSpotVehicleMaterialDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                //  String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSpotVehMatDtls]( " +
            //"  [idSpotVehMatDtls]" +
            " [vehSpotEntryId]" +
            " ,[prodClassId]" +
            " ,[prodItemId]" +
            " ,[qty]" +
            " )" +
" VALUES (" +
            //"  @IdSpotVehMatDtls " +
            " @VehSpotEntryId " +
            " ,@ProdClassId " +
            " ,@ProdItemId " +
            " ,@Qty " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdSpotVehMatDtls", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.IdSpotVehMatDtls;
            cmdInsert.Parameters.Add("@VehSpotEntryId", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.VehSpotEntryId;
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.ProdClassId;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.ProdItemId;
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblSpotVehicleMaterialDtlsTO.Qty;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO)
        {
            // String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSpotVehicleMaterialDtlsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                // String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSpotVehicleMaterialDtlsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                //  String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSpotVehMatDtls] SET " +
            "  [idSpotVehMatDtls] = @IdSpotVehMatDtls" +
            " ,[vehSpotEntryId]= @VehSpotEntryId" +
            " ,[prodClassId]= @ProdClassId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[qty] = @Qty" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSpotVehMatDtls", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.IdSpotVehMatDtls;
            cmdUpdate.Parameters.Add("@VehSpotEntryId", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.VehSpotEntryId;
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.ProdClassId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.ProdItemId;
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblSpotVehicleMaterialDtlsTO.Qty;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls)
        {
            //String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTblSpotVehicleMaterialDtls, cmdDelete);
            }
            catch (Exception ex)
            {
                //  String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTblSpotVehicleMaterialDtls, cmdDelete);
            }
            catch (Exception ex)
            {
                // String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public  int ExecuteDeletionCommand(Int32 idTblSpotVehicleMaterialDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSpotVehMatDtls] " +
            " WHERE idSpotVehMatDtls = " + idTblSpotVehicleMaterialDtls + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTblSpotVehicleMaterialDtls", System.Data.SqlDbType.Int).Value = tblSpotVehicleMaterialDtlsTO.IdTblSpotVehicleMaterialDtls;
            return cmdDelete.ExecuteNonQuery();
        }

        public  int DeleteSpotVehMaterialDtls(Int32 spotVehicleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM  tblSpotVehMatDtls WHERE vehSpotEntryId=" + spotVehicleId + "";
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        #endregion

    }
}
