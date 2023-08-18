using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblFreightUpdateDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT freight.* , dist.districtName , tal.talukaName,org.firmName,vehType.vehicleTypeDesc,userDisplayName " +
                                  " FROM tblFreightUpdate freight " +
                                  " LEFT JOIN dimDistrict dist ON dist.idDistrict = freight.districtId " +
                                  " LEFT JOIN dimTaluka tal ON tal.idTaluka = freight.talukaId " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = freight.transporterId " +
                                  " LEFT JOIN dimVehicleType vehType ON vehType.idVehicleType = freight.vehicleTypeId " +
                                  " LEFT JOIN tblUser ON idUser = freight.createdBy";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblFreightUpdateTO> SelectAllTblFreightUpdate()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return  ConvertDTToList(sqlReader);
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {

                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblFreightUpdateTO SelectTblFreightUpdate(Int32 idFreightUpdate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idFreightUpdate = " + idFreightUpdate +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblFreightUpdateTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblFreightUpdateTO> SelectAllTblFreightUpdate(DateTime frmDt, DateTime toDt, int distrinctId, int talukaId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            String whereCond = string.Empty;
            try
            {
                conn.Open();
                if (distrinctId == 0 && talukaId == 0)
                    whereCond = "";
                else if (distrinctId > 0 && talukaId == 0)
                    whereCond = " freight.districtId=" + distrinctId + " AND";
                else if (distrinctId == 0 && talukaId > 0)
                    whereCond = " freight.talukaId=" + talukaId + " AND";
                else if (distrinctId > 0 && talukaId > 0)
                    whereCond = " freight.districtId=" + distrinctId + " AND freight.talukaId=" + talukaId + " AND";

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE " + whereCond + " CONVERT (DATE,freight.createdOn,103) BETWEEN @fromDate AND @toDate " +
                                        " ORDER BY freight.createdOn";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblFreightUpdateTO> ConvertDTToList(SqlDataReader tblFreightUpdateTODT)
        {
            List<TblFreightUpdateTO> tblFreightUpdateTOList = new List<TblFreightUpdateTO>();
            if (tblFreightUpdateTODT != null)
            {
                while (tblFreightUpdateTODT.Read())
                {
                    TblFreightUpdateTO tblFreightUpdateTONew = new TblFreightUpdateTO();
                    if (tblFreightUpdateTODT["idFreightUpdate"] != DBNull.Value)
                        tblFreightUpdateTONew.IdFreightUpdate = Convert.ToInt32(tblFreightUpdateTODT["idFreightUpdate"].ToString());
                    if (tblFreightUpdateTODT["districtId"] != DBNull.Value)
                        tblFreightUpdateTONew.DistrictId = Convert.ToInt32(tblFreightUpdateTODT["districtId"].ToString());
                    if (tblFreightUpdateTODT["talukaId"] != DBNull.Value)
                        tblFreightUpdateTONew.TalukaId = Convert.ToInt32(tblFreightUpdateTODT["talukaId"].ToString());
                    if (tblFreightUpdateTODT["transporterId"] != DBNull.Value)
                        tblFreightUpdateTONew.TransporterId = Convert.ToInt32(tblFreightUpdateTODT["transporterId"].ToString());
                    if (tblFreightUpdateTODT["vehicleTypeId"] != DBNull.Value)
                        tblFreightUpdateTONew.VehicleTypeId = Convert.ToInt32(tblFreightUpdateTODT["vehicleTypeId"].ToString());
                    if (tblFreightUpdateTODT["createdBy"] != DBNull.Value)
                        tblFreightUpdateTONew.CreatedBy = Convert.ToInt32(tblFreightUpdateTODT["createdBy"].ToString());
                    if (tblFreightUpdateTODT["createdOn"] != DBNull.Value)
                        tblFreightUpdateTONew.CreatedOn = Convert.ToDateTime(tblFreightUpdateTODT["createdOn"].ToString());
                    if (tblFreightUpdateTODT["freightAmt"] != DBNull.Value)
                        tblFreightUpdateTONew.FreightAmt = Convert.ToDouble(tblFreightUpdateTODT["freightAmt"].ToString());
                    if (tblFreightUpdateTODT["locationDesc"] != DBNull.Value)
                        tblFreightUpdateTONew.LocationDesc = Convert.ToString(tblFreightUpdateTODT["locationDesc"].ToString());

                    if (tblFreightUpdateTODT["districtName"] != DBNull.Value)
                        tblFreightUpdateTONew.DistrictDesc = Convert.ToString(tblFreightUpdateTODT["districtName"].ToString());
                    if (tblFreightUpdateTODT["talukaName"] != DBNull.Value)
                        tblFreightUpdateTONew.TalukaDesc = Convert.ToString(tblFreightUpdateTODT["talukaName"].ToString());
                    if (tblFreightUpdateTODT["firmName"] != DBNull.Value)
                        tblFreightUpdateTONew.TransporterName = Convert.ToString(tblFreightUpdateTODT["firmName"].ToString());
                    if (tblFreightUpdateTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblFreightUpdateTONew.VehicleType = Convert.ToString(tblFreightUpdateTODT["vehicleTypeDesc"].ToString());
                    if (tblFreightUpdateTODT["userDisplayName"] != DBNull.Value)
                        tblFreightUpdateTONew.CreatedUserName = Convert.ToString(tblFreightUpdateTODT["userDisplayName"].ToString());

                    tblFreightUpdateTOList.Add(tblFreightUpdateTONew);
                }
            }
            return tblFreightUpdateTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblFreightUpdateTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblFreightUpdateTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblFreightUpdateTO tblFreightUpdateTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblFreightUpdate]( " + 
                            "  [districtId]" +
                            " ,[talukaId]" +
                            " ,[transporterId]" +
                            " ,[vehicleTypeId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[freightAmt]" +
                            " ,[locationDesc]" +
                            " )" +
                " VALUES (" +
                            "  @DistrictId " +
                            " ,@TalukaId " +
                            " ,@TransporterId " +
                            " ,@VehicleTypeId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@FreightAmt " +
                            " ,@LocationDesc " + 
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdFreightUpdate", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.IdFreightUpdate;
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.DistrictId;
            cmdInsert.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.TalukaId;
            cmdInsert.Parameters.Add("@TransporterId", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.TransporterId;
            cmdInsert.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.VehicleTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblFreightUpdateTO.CreatedOn;
            cmdInsert.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = tblFreightUpdateTO.FreightAmt;
            cmdInsert.Parameters.Add("@LocationDesc", System.Data.SqlDbType.NVarChar).Value = tblFreightUpdateTO.LocationDesc;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = SalesTrackerAPI.StaticStuff.Constants.IdentityColumnQuery;
                tblFreightUpdateTO.IdFreightUpdate = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblFreightUpdateTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblFreightUpdateTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblFreightUpdateTO tblFreightUpdateTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblFreightUpdate] SET " + 
            "  [idFreightUpdate] = @IdFreightUpdate" +
            " ,[districtId]= @DistrictId" +
            " ,[talukaId]= @TalukaId" +
            " ,[transporterId]= @TransporterId" +
            " ,[vehicleTypeId]= @VehicleTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[freightAmt]= @FreightAmt" +
            " ,[locationDesc] = @LocationDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdFreightUpdate", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.IdFreightUpdate;
            cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.DistrictId;
            cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.TalukaId;
            cmdUpdate.Parameters.Add("@TransporterId", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.TransporterId;
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.VehicleTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblFreightUpdateTO.CreatedOn;
            cmdUpdate.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = tblFreightUpdateTO.FreightAmt;
            cmdUpdate.Parameters.Add("@LocationDesc", System.Data.SqlDbType.NVarChar).Value = tblFreightUpdateTO.LocationDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblFreightUpdate(Int32 idFreightUpdate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idFreightUpdate, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblFreightUpdate(Int32 idFreightUpdate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idFreightUpdate, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idFreightUpdate, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblFreightUpdate] " +
            " WHERE idFreightUpdate = " + idFreightUpdate +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idFreightUpdate", System.Data.SqlDbType.Int).Value = tblFreightUpdateTO.IdFreightUpdate;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
