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
    public class TblUserLocationDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT *,NULL AS Result FROM [tblUserLocation]"; 
            return sqlSelectQry;
        }

        public static String SqlSelectQueryForNearByDealers()
        {
            String sqlSelectQry = " SELECT result.firmName,result.Result,result.areaname As visitePlace,result.lat,result.lng " +
                                  " FROM (SELECT distinct org.firmName,org.idOrganization, areaname,  addr.lat, addr.lng, " +
                                  " (((acos(sin((18.5204 * pi() / 180)) * sin((cast(addr.lat as float) * pi() / 180)) + cos((18.5204 * pi() / 180)) " +
                                  " * cos((cast(addr.lat as float) * pi() / 180)) * cos(((73.8567 - cast(addr.lng as float)) * pi() / 180)))) * 180 / pi()) " +
                                  " * 60 * 1.1515 * 1.609344) as Result FROM tblAddress addr INNER JOIN tblOrganization org on org.addrId = addr.idAddr ) " +
                                  " as result ";
            return sqlSelectQry;
        }

        #endregion

        #region Selection
        public static List<TblUserLocationTO> SelectAllTblUserLocation()
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
                List<TblUserLocationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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
      
        public static TblUserLocationTO SelectTblUserLocation()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ "  ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserLocationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblUserLocationTO> SelectAllTblUserLocation(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserLocationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public static List<TblUserLocationTO> UserLastLocationListOnUserId(string userIds)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "WITH userLocation AS ( SELECT *, ROW_NUMBER() OVER(PARTITION BY userId" +
                    " ORDER BY curTime DESC) AS Result FROM tblUserLocation tblUserLocation  " +
                    "where userid in("+ userIds + ")) SELECT* FROM userLocation WHERE Result = 1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserLocationTO> list = ConvertDTToList(sqlReader);
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

        /// <summary>
        /// Sudhir[14-AUG-2018] Added For Actual Plan 
        /// </summary>
        /// <param name="tblUserLocationTO"></param>
        /// <returns></returns>
        public static List<TblUserLocationTO> SelectActualPlanRoute(Int32 UserId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT *, " +
                                        " (SELECT(((ACOS(SIN((18.5204 * PI() / 180)) * SIN((CAST(UserLocation.latitude AS FLOAT) * PI() / 180)) " +
                                        " + COS((18.5204 * PI() / 180)) * COS((CAST(UserLocation.latitude AS FLOAT) * PI() / 180)) * COS(((73.8567 " +
                                        " - CAST(UserLocation.longitude AS FLOAT)) * PI() / 180)))) * 180 / PI()) * 60 * 1.1515 * 1.609344) " +
                                        " AS distance) AS Result FROM tblUserLocation UserLocation WHERE userId =" + UserId +
                                        " AND CONVERT(date, curTime)=(SELECT CONVERT(date, getdate())) ORDER BY curTime ASC ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserLocationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        /// <summary>
        /// Sudhir[14-AUG-2018] Added For Suggested Plan 
        /// </summary>
        /// <param name="tblUserLocationTO"></param>
        /// <returns></returns>
        public static List<TblUserLocationTO> SelectSuggestedPlanRoute(Int32 UserId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT *, " +
                                        " (SELECT(((ACOS(SIN((18.5204 * PI() / 180)) * SIN((CAST(UserLocation.latitude AS FLOAT) * PI() / 180)) " +
                                        " + COS((18.5204 * PI() / 180)) * COS((CAST(UserLocation.latitude AS FLOAT) * PI() / 180)) * COS(((73.8567 " +
                                        " - CAST(UserLocation.longitude AS FLOAT)) * PI() / 180)))) * 180 / PI()) * 60 * 1.1515 * 1.609344) " +
                                        " AS distance) AS Result FROM tblUserLocation UserLocation WHERE userId =" + UserId +
                                        " AND CONVERT(date, curTime)=(SELECT CONVERT(date, getdate())) ORDER BY Result ASC";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserLocationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<nearBymeTo> getNearBycustomer(int distance, int siteType, string currentLat, string currentLng)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT result.distance As Result,result.lat,result.lng,result.firmName,result.visitePlace " +
                    "FROM ( SELECT distinct addr.lat,addr.lng,firmName,visitePlace, " +
                    "(((acos(sin(("+ currentLat + " * pi() / 180)) * sin((cast(addr.lat as float) * pi() / 180)) + cos(("+ currentLat + " * pi() / 180)) * cos((cast(addr.lat as float) * pi() / 180)) * cos((( "+ currentLng + "- cast(addr.lng as float)) * pi() / 180)))) * 180 / pi()) * 60 * 1.1515 * 1.609344)" +
                    " as distance FROM tblAddress addr " +
                    "left join tblOrganization org on org.addrId = addr.idAddr " +
                    "left join tblCRMVisitDetails visitdetails on visitdetails.firmId = org.idOrganization" +
                    " where visitdetails.visitTypeId = "+ siteType  + " ) as result WHERE distance <= " + distance;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<nearBymeTo> list = ConvertDTToListNearByMe(sqlReader);
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

        /// <summary>
        /// Sudhir[14-AUG-2018] Added For Get Nearest Dealer Visit.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="cnfId"></param>
        /// <param name="tblUserRoleTO"></param>
        /// <returns></returns>
        public static List<nearBymeTo> SelectNearByDealer(int distance, Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String InQuery = String.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                conn.Open();

                if (cnfId > 0)
                {
                    if (isConfEn == 0)
                    {
                        InQuery = " SELECT DISTINCT idOrganization " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId;
                    }
                    else
                    {
                        InQuery = " SELECT DISTINCT idOrganization " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.cnfOrgId=" + cnfId + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }
                else
                {
                    if (isConfEn == 0)
                    {
                        InQuery = " SELECT idOrganization FROM tblOrganization LEFT JOIN(SELECT tblAddress.*, organizationId FROM tblOrgAddress  " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1) addrDtl " +
                                  " ON idOrganization = organizationId  WHERE isActive = 1 AND orgTypeId = 2";
                    }
                    else
                    {
                        InQuery = " SELECT DISTINCT idOrganization "+
                                  " FROM tblOrganization " +
                                  " INNER JOIN   (    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1) addrDtl ON idOrganization = organizationId " +
                                  " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId " +
                                  " WHERE tblOrganization.isActive = 1  AND orgTypeId =" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + " AND areaConf.isActive = 1 ";

                    }
                }

                cmdSelect.CommandText = SqlSelectQueryForNearByDealers()+ " WHERE Result <="+ distance+ " AND  result.idOrganization IN ("+ InQuery+")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<nearBymeTo> list = ConvertDTToListNearByMe(sqlReader);
                sqlReader.Dispose();
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


        #endregion

        #region Insertion
        public static int InsertTblUserLocation(TblUserLocationTO tblUserLocationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUserLocationTO, cmdInsert);
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

        public static int InsertTblUserLocation(TblUserLocationTO tblUserLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserLocationTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblUserLocationTO tblUserLocationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserLocation]( " +
            " [userId]" +
            " ,[curTime]" +
            " ,[latitude]" +
            " ,[longitude]" +
            " )" +
" VALUES (" +
           
            " @UserId " +
            " ,@CurTime " +
            " ,@Latitude " +
            " ,@Longitude " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@Idlocation", System.Data.SqlDbType.Int).Value = tblUserLocationTO.Idlocation;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserLocationTO.UserId;
            cmdInsert.Parameters.Add("@CurTime", System.Data.SqlDbType.DateTime).Value = tblUserLocationTO.CurTime;
            cmdInsert.Parameters.Add("@Latitude", System.Data.SqlDbType.VarChar).Value = tblUserLocationTO.Latitude;
            cmdInsert.Parameters.Add("@Longitude", System.Data.SqlDbType.NVarChar).Value = tblUserLocationTO.Longitude;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblUserLocation(TblUserLocationTO tblUserLocationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUserLocationTO, cmdUpdate);
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

        public static int UpdateTblUserLocation(TblUserLocationTO tblUserLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUserLocationTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblUserLocationTO tblUserLocationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUserLocation] SET " + 
            "  [idlocation] = @Idlocation" +
            " ,[userId]= @UserId" +
            " ,[curTime]= @CurTime" +
            " ,[latitude]= @Latitude" +
            " ,[longitude] = @Longitude" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@Idlocation", System.Data.SqlDbType.Int).Value = tblUserLocationTO.Idlocation;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserLocationTO.UserId;
            cmdUpdate.Parameters.Add("@CurTime", System.Data.SqlDbType.DateTime).Value = tblUserLocationTO.CurTime;
            cmdUpdate.Parameters.Add("@Latitude", System.Data.SqlDbType.VarChar).Value = tblUserLocationTO.Latitude;
            cmdUpdate.Parameters.Add("@Longitude", System.Data.SqlDbType.NVarChar).Value = tblUserLocationTO.Longitude;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
       
        public static int DeleteTblUserLocation(Int32 idlocation, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idlocation, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idlocation, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUserLocation] " +
            " ";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }

        public static int DeleteTblUserLocationPreviousDays(TblUserLocationTO tblUserLocationTO)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete= new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteInsertionCommandOnDays(tblUserLocationTO, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteInsertionCommandOnDays(TblUserLocationTO tblUserLocationTO, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM tblUserLocation WHERE curTime < @CurTime" + 
                                    " AND userId="+ tblUserLocationTO.UserId;

            cmdDelete.CommandType = System.Data.CommandType.Text;
            cmdDelete.Parameters.Add("@CurTime", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime.AddDays(Constants.PreviousRecordDeletionDays).Date;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        public static List<TblUserLocationTO> ConvertDTToList(SqlDataReader tblUserLocationTODT)
        {
            List<TblUserLocationTO> tblUserLocationTOList = new List<TblUserLocationTO>();
            if (tblUserLocationTODT != null)
            {
                while (tblUserLocationTODT.Read())
                {
                    TblUserLocationTO tblUserLocationTONew = new TblUserLocationTO();
                    if (tblUserLocationTODT["idlocation"] != DBNull.Value)
                        tblUserLocationTONew.Idlocation = Convert.ToInt32(tblUserLocationTODT["idlocation"].ToString());
                    if (tblUserLocationTODT["userId"] != DBNull.Value)
                        tblUserLocationTONew.UserId = Convert.ToInt32(tblUserLocationTODT["userId"].ToString());
                    if (tblUserLocationTODT["curTime"] != DBNull.Value)
                        tblUserLocationTONew.CurTime = Convert.ToDateTime(tblUserLocationTODT["curTime"].ToString());
                    if (tblUserLocationTODT["latitude"] != DBNull.Value)
                        tblUserLocationTONew.Latitude = Convert.ToString(tblUserLocationTODT["latitude"].ToString());
                    if (tblUserLocationTODT["longitude"] != DBNull.Value)
                        tblUserLocationTONew.Longitude = Convert.ToString(tblUserLocationTODT["longitude"].ToString());
                    if (tblUserLocationTODT["Result"] != DBNull.Value)
                        tblUserLocationTONew.Distance = Convert.ToString(tblUserLocationTODT["Result"].ToString());
                    tblUserLocationTOList.Add(tblUserLocationTONew);
                }
            }
            return tblUserLocationTOList;
        }

        public static List<nearBymeTo> ConvertDTToListNearByMe(SqlDataReader NearByMeTODT)
        {
            List<nearBymeTo> NearByMeTODTList = new List<nearBymeTo>();
            if (NearByMeTODT != null)
            {
                while (NearByMeTODT.Read())
                {
                    nearBymeTo nearByMeTONew = new nearBymeTo();
                    if (NearByMeTODT["lat"] != DBNull.Value)
                        nearByMeTONew.Lat = Convert.ToString(NearByMeTODT["lat"].ToString());
                    if (NearByMeTODT["lng"] != DBNull.Value)
                        nearByMeTONew.Lng = Convert.ToString(NearByMeTODT["lng"].ToString());
                    if (NearByMeTODT["firmName"] != DBNull.Value)
                        nearByMeTONew.FirmName = Convert.ToString(NearByMeTODT["firmName"].ToString());
                    if (NearByMeTODT["visitePlace"] != DBNull.Value)
                        nearByMeTONew.VisitePlace = Convert.ToString(NearByMeTODT["visitePlace"].ToString());
                    if (NearByMeTODT["Result"] != DBNull.Value)
                        nearByMeTONew.Distance = Convert.ToString(NearByMeTODT["Result"].ToString());
                    NearByMeTODTList.Add(nearByMeTONew);
                }
            }
            return NearByMeTODTList;
        }
        
    }
}
