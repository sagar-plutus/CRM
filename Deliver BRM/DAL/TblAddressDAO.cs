using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblAddressDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT addr.* ,tal.talukaName,dist.districtName,stat.stateName " +
                                  " FROM tblAddress addr " +
                                  " LEFT JOIN dimTaluka tal ON tal.idTaluka = addr.talukaId " +
                                  " LEFT JOIN dimDistrict dist ON dist.idDistrict = addr.districtId " +
                                  " LEFT JOIN dimState stat ON stat.idState = addr.stateId ";

            return sqlSelectQry;
        }
        public static String SqlSelectQuery1()
        {
            String sqlSelectQry = " SELECT addr.*,dimAddressType.*, orgAddr.addrTypeId,orgAddr.idOrgAddr,tal.talukaName,dist.districtName,stat.stateName,stat.stateCode,stat.stateOrUTCode,country.countryCode,country.countryName " +
                                  " FROM tblAddress addr " +
                                  " LEFT JOIN dimTaluka tal ON tal.idTaluka = addr.talukaId " +
                                  " LEFT JOIN dimDistrict dist ON dist.idDistrict = addr.districtId " +
                                  " LEFT JOIN dimState stat ON stat.idState = addr.stateId " +
                                  " LEFT JOIN dimCountry country on country.idCountry=addr.countryId " +
                                  " LEFT JOIN tblOrgAddress orgAddr ON addr.idAddr = orgAddr.addressId " +
                                  " LEFT JOIN dimAddressType dimAddressType ON orgAddr.addrTypeId = dimAddressType.idAddressType " +
                                  " Where addr.isAddrVisible = 1";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblAddressTO> SelectAllTblAddress()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddressTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblAddressTO SelectTblAddress(Int32 idAddr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idAddr = " + idAddr +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddressTO> list = ConvertDTToList(reader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId,StaticStuff.Constants.AddressTypeE addressTypeE,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            int addressTypeId = (int)addressTypeE;
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText =SqlSelectQuery() +  " LEFT JOIN tblOrgAddress orgAddr " +
                                        " ON addr.idAddr = orgAddr.addressId " +
                                        " WHERE organizationId=" + orgId + " AND addrTypeId=" + addressTypeId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddressTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblAddressTO> SelectOrgAddressList(Int32 orgId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery1() + " and organizationId=" + orgId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList1(reader);
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

        public static List<TblAddressTO> ConvertDTToList(SqlDataReader tblAddressTODT)
        {
            List<TblAddressTO> tblAddressTOList = new List<TblAddressTO>();
            if (tblAddressTODT != null)
            {
                while (tblAddressTODT.Read())
                {
                    TblAddressTO tblAddressTONew = new TblAddressTO();
                    if (tblAddressTODT["idAddr"] != DBNull.Value)
                        tblAddressTONew.IdAddr = Convert.ToInt32(tblAddressTODT["idAddr"].ToString());
                    if (tblAddressTODT["talukaId"] != DBNull.Value)
                        tblAddressTONew.TalukaId = Convert.ToInt32(tblAddressTODT["talukaId"].ToString());
                    if (tblAddressTODT["districtId"] != DBNull.Value)
                        tblAddressTONew.DistrictId = Convert.ToInt32(tblAddressTODT["districtId"].ToString());
                    if (tblAddressTODT["stateId"] != DBNull.Value)
                        tblAddressTONew.StateId = Convert.ToInt32(tblAddressTODT["stateId"].ToString());
                    if (tblAddressTODT["countryId"] != DBNull.Value)
                        tblAddressTONew.CountryId = Convert.ToInt32(tblAddressTODT["countryId"].ToString());
                    if (tblAddressTODT["pincode"] != DBNull.Value)
                        tblAddressTONew.Pincode = Convert.ToInt32(tblAddressTODT["pincode"].ToString());
                    if (tblAddressTODT["createdBy"] != DBNull.Value)
                        tblAddressTONew.CreatedBy = Convert.ToInt32(tblAddressTODT["createdBy"].ToString());
                    if (tblAddressTODT["createdOn"] != DBNull.Value)
                        tblAddressTONew.CreatedOn = Convert.ToDateTime(tblAddressTODT["createdOn"].ToString());
                    if (tblAddressTODT["plotNo"] != DBNull.Value)
                        tblAddressTONew.PlotNo = Convert.ToString(tblAddressTODT["plotNo"].ToString());
                    if (tblAddressTODT["streetName"] != DBNull.Value)
                        tblAddressTONew.StreetName = Convert.ToString(tblAddressTODT["streetName"].ToString());
                    if (tblAddressTODT["areaName"] != DBNull.Value)
                        tblAddressTONew.AreaName = Convert.ToString(tblAddressTODT["areaName"].ToString());
                    if (tblAddressTODT["villageName"] != DBNull.Value)
                        tblAddressTONew.VillageName = Convert.ToString(tblAddressTODT["villageName"].ToString());
                    if (tblAddressTODT["comments"] != DBNull.Value)
                        tblAddressTONew.Comments = Convert.ToString(tblAddressTODT["comments"].ToString());
                    if (tblAddressTODT["talukaName"] != DBNull.Value)
                        tblAddressTONew.TalukaName = Convert.ToString(tblAddressTODT["talukaName"].ToString());
                    if (tblAddressTODT["districtName"] != DBNull.Value)
                        tblAddressTONew.DistrictName = Convert.ToString(tblAddressTODT["districtName"].ToString());
                    if (tblAddressTODT["stateName"] != DBNull.Value)
                        tblAddressTONew.StateName = Convert.ToString(tblAddressTODT["stateName"].ToString());
                    tblAddressTOList.Add(tblAddressTONew);
                }
            }
            return tblAddressTOList;
        }
        public static List<TblAddressTO> ConvertDTToList1(SqlDataReader tblAddressTODT)
        {
            List<TblAddressTO> tblAddressTOList = new List<TblAddressTO>();
            if (tblAddressTODT != null)
            {
                while (tblAddressTODT.Read())
                {
                    TblAddressTO tblAddressTONew = new TblAddressTO();
                    if (tblAddressTODT["idAddr"] != DBNull.Value)
                        tblAddressTONew.IdAddr = Convert.ToInt32(tblAddressTODT["idAddr"].ToString());
                    if (tblAddressTODT["talukaId"] != DBNull.Value)
                        tblAddressTONew.TalukaId = Convert.ToInt32(tblAddressTODT["talukaId"].ToString());
                    if (tblAddressTODT["districtId"] != DBNull.Value)
                        tblAddressTONew.DistrictId = Convert.ToInt32(tblAddressTODT["districtId"].ToString());
                    if (tblAddressTODT["stateId"] != DBNull.Value)
                        tblAddressTONew.StateId = Convert.ToInt32(tblAddressTODT["stateId"].ToString());
                    if (tblAddressTODT["countryId"] != DBNull.Value)
                        tblAddressTONew.CountryId = Convert.ToInt32(tblAddressTODT["countryId"].ToString());
                    if (tblAddressTODT["pincode"] != DBNull.Value)
                        tblAddressTONew.Pincode = Convert.ToInt32(tblAddressTODT["pincode"].ToString());
                    if (tblAddressTODT["createdBy"] != DBNull.Value)
                        tblAddressTONew.CreatedBy = Convert.ToInt32(tblAddressTODT["createdBy"].ToString());
                    if (tblAddressTODT["createdOn"] != DBNull.Value)
                        tblAddressTONew.CreatedOn = Convert.ToDateTime(tblAddressTODT["createdOn"].ToString());
                    if (tblAddressTODT["plotNo"] != DBNull.Value)
                        tblAddressTONew.PlotNo = Convert.ToString(tblAddressTODT["plotNo"].ToString());
                    if (tblAddressTODT["streetName"] != DBNull.Value)
                        tblAddressTONew.StreetName = Convert.ToString(tblAddressTODT["streetName"].ToString());
                    if (tblAddressTODT["areaName"] != DBNull.Value)
                        tblAddressTONew.AreaName = Convert.ToString(tblAddressTODT["areaName"].ToString());
                    if (tblAddressTODT["villageName"] != DBNull.Value)
                        tblAddressTONew.VillageName = Convert.ToString(tblAddressTODT["villageName"].ToString());
                    if (tblAddressTODT["comments"] != DBNull.Value)
                        tblAddressTONew.Comments = Convert.ToString(tblAddressTODT["comments"].ToString());
                    if (tblAddressTODT["talukaName"] != DBNull.Value)
                        tblAddressTONew.TalukaName = Convert.ToString(tblAddressTODT["talukaName"].ToString());
                    if (tblAddressTODT["districtName"] != DBNull.Value)
                        tblAddressTONew.DistrictName = Convert.ToString(tblAddressTODT["districtName"].ToString());
                    if (tblAddressTODT["stateName"] != DBNull.Value)
                        tblAddressTONew.StateName = Convert.ToString(tblAddressTODT["stateName"].ToString());
                    if (tblAddressTODT["stateOrUTCode"] != DBNull.Value)
                        tblAddressTONew.StateOrUTCode = Convert.ToString(tblAddressTODT["stateOrUTCode"].ToString());
                    if (tblAddressTODT["stateCode"] != DBNull.Value)
                        tblAddressTONew.StateCode = Convert.ToString(tblAddressTODT["stateCode"].ToString());
                    if (tblAddressTODT["countryCode"] != DBNull.Value)
                        tblAddressTONew.CountryCode = Convert.ToString(tblAddressTODT["countryCode"].ToString());
                    if (tblAddressTODT["countryName"] != DBNull.Value)
                        tblAddressTONew.CountryName = Convert.ToString(tblAddressTODT["countryName"].ToString());
                    if (tblAddressTODT["addrTypeId"] != DBNull.Value)
                        tblAddressTONew.AddrTypeId = Convert.ToInt32(tblAddressTODT["addrTypeId"].ToString());
                    if (tblAddressTODT["idOrgAddr"] != DBNull.Value)
                        tblAddressTONew.IdOrgAddr = Convert.ToInt32(tblAddressTODT["idOrgAddr"].ToString());
                    if (tblAddressTODT["addressType"] != DBNull.Value)
                        tblAddressTONew.AddressType = Convert.ToString(tblAddressTODT["addressType"].ToString());
                    tblAddressTOList.Add(tblAddressTONew);
                }
            }
            return tblAddressTOList;
        }
        #endregion

        #region Insertion
        public static int InsertTblAddress(TblAddressTO tblAddressTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAddressTO, cmdInsert);
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

        public static int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAddressTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblAddressTO tblAddressTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAddress]( " + 
                                "  [talukaId]" +
                                " ,[districtId]" +
                                " ,[stateId]" +
                                " ,[countryId]" +
                                " ,[pincode]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[plotNo]" +
                                " ,[streetName]" +
                                " ,[areaName]" +
                                " ,[villageName]" +
                                " ,[comments]" +
                                " )" +
                    " VALUES (" +
                                "  @TalukaId " +
                                " ,@DistrictId " +
                                " ,@StateId " +
                                " ,@CountryId " +
                                " ,@Pincode " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@PlotNo " +
                                " ,@StreetName " +
                                " ,@AreaName " +
                                " ,@VillageName " +
                                " ,@Comments " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdAddr", System.Data.SqlDbType.Int).Value = tblAddressTO.IdAddr;
            cmdInsert.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.TalukaId);
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblAddressTO.DistrictId;
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblAddressTO.StateId;
            cmdInsert.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = tblAddressTO.CountryId;
            cmdInsert.Parameters.Add("@Pincode", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.Pincode);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAddressTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAddressTO.CreatedOn;
            cmdInsert.Parameters.Add("@PlotNo", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.PlotNo);
            cmdInsert.Parameters.Add("@StreetName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.StreetName);
            cmdInsert.Parameters.Add("@AreaName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.AreaName);
            cmdInsert.Parameters.Add("@VillageName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.VillageName);
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue( tblAddressTO.Comments);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAddressTO.IdAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblAddress(TblAddressTO tblAddressTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAddressTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAddressTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblAddressTO tblAddressTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAddress] SET " + 
                            "  [talukaId]= @TalukaId" +
                            " ,[districtId]= @DistrictId" +
                            " ,[stateId]= @StateId" +
                            " ,[countryId]= @CountryId" +
                            " ,[pincode]= @Pincode" +
                            " ,[plotNo]= @PlotNo" +
                            " ,[streetName]= @StreetName" +
                            " ,[areaName]= @AreaName" +
                            " ,[villageName]= @VillageName" +
                            " ,[comments] = @Comments" +
                            " WHERE  [idAddr] = @IdAddr"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAddr", System.Data.SqlDbType.Int).Value = tblAddressTO.IdAddr;
            cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.TalukaId);
            cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblAddressTO.DistrictId;
            cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblAddressTO.StateId;
            cmdUpdate.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = tblAddressTO.CountryId;
            cmdUpdate.Parameters.Add("@Pincode", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.Pincode);
            cmdUpdate.Parameters.Add("@PlotNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.PlotNo);
            cmdUpdate.Parameters.Add("@StreetName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.StreetName);
            cmdUpdate.Parameters.Add("@AreaName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.AreaName);
            cmdUpdate.Parameters.Add("@VillageName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.VillageName);
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblAddressTO.Comments);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblAddress(Int32 idAddr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAddr, cmdDelete);
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

        public static int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAddr, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idAddr, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAddress] " +
            " WHERE idAddr = " + idAddr +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAddr", System.Data.SqlDbType.Int).Value = tblAddressTO.IdAddr;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
