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
    public class TblCompetitorExtDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblCompetitorExt]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblCompetitorExtTO> SelectAllTblCompetitorExt()
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

                SqlDataReader tblCompetitorExtRdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCompetitorExtTO> list = ConvertDTToList(tblCompetitorExtRdr);
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

        public static List<TblCompetitorExtTO> SelectAllTblCompetitorExt(Int32 orgId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblCompetitorExtRdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgId="+orgId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblCompetitorExtRdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCompetitorExtTO> list = ConvertDTToList(tblCompetitorExtRdr);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblCompetitorExtRdr != null)
                    tblCompetitorExtRdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String WhereCond = String.Empty;
            try
            {
                if (competitorOrgId > 0)
                    WhereCond = " WHERE orgId = " + competitorOrgId;
                conn.Open();

                if (competitorOrgId > 0)
                    WhereCond = " WHERE orgId = " + competitorOrgId;

                String aqlQuery = " SELECT idCompetitorExt,brandName FROM tblCompetitorExt "+ WhereCond;
                                  

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCompetitorExt"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCompetitorExt"].ToString());
                    if (dateReader["brandName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["brandName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public static TblCompetitorExtTO SelectTblCompetitorExt(Int32 idCompetitorExt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idCompetitorExt = " + idCompetitorExt +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblCompetitorExtRdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCompetitorExtTO> list = ConvertDTToList(tblCompetitorExtRdr);
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
        public static List<TblCompetitorExtTO> ConvertDTToList(SqlDataReader tblCompetitorExtTODT)
        {
            List<TblCompetitorExtTO> tblCompetitorExtTOList = new List<TblCompetitorExtTO>();
            if (tblCompetitorExtTODT != null)
            {
                while (tblCompetitorExtTODT.Read())
                {
                    TblCompetitorExtTO tblCompetitorExtTONew = new TblCompetitorExtTO();
                    if (tblCompetitorExtTODT["idCompetitorExt"] != DBNull.Value)
                        tblCompetitorExtTONew.IdCompetitorExt = Convert.ToInt32(tblCompetitorExtTODT["idCompetitorExt"].ToString());
                    if (tblCompetitorExtTODT["orgId"] != DBNull.Value)
                        tblCompetitorExtTONew.OrgId = Convert.ToInt32(tblCompetitorExtTODT["orgId"].ToString());
                    if (tblCompetitorExtTODT["prodCapacityMT"] != DBNull.Value)
                        tblCompetitorExtTONew.ProdCapacityMT = Convert.ToDouble(tblCompetitorExtTODT["prodCapacityMT"].ToString());
                    if (tblCompetitorExtTODT["brandName"] != DBNull.Value)
                        tblCompetitorExtTONew.BrandName = Convert.ToString(tblCompetitorExtTODT["brandName"].ToString());
                    if (tblCompetitorExtTODT["mfgCompanyName"] != DBNull.Value)
                        tblCompetitorExtTONew.MfgCompanyName = Convert.ToString(tblCompetitorExtTODT["mfgCompanyName"].ToString());
                    tblCompetitorExtTOList.Add(tblCompetitorExtTONew);
                }
            }
            return tblCompetitorExtTOList;
        }

        /// <summary>
        /// Sudhir[17-APR-2018] Added for CompetitorList Based On BrandId.
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static List<DropDownTO> SelectCompetitorListOnBrandId(Int32 brandId)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();


                String aqlQuery = " SELECT idOrganization,firmName FROM tblOrganization Organization " +
                                  " LEFT JOIN tblCompetitorExt competitorExt ON Organization.idOrganization = competitorExt.orgId " +
                                  " WHERE competitorExt.idCompetitorExt = " + brandId + " AND Organization.orgTypeId = " + (int)Constants.OrgTypeE.COMPETITOR;


                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idOrganization"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idOrganization"].ToString());
                    if (dateReader["firmName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["firmName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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
        public static int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblCompetitorExtTO, cmdInsert);
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

        public static int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblCompetitorExtTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblCompetitorExtTO tblCompetitorExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblCompetitorExt]( " +
                            "  [orgId]" +
                            " ,[prodCapacityMT]" +
                            " ,[brandName]" +
                            " ,[mfgCompanyName]" +
                            " )" +
                " VALUES (" +
                            "  @OrgId " +
                            " ,@ProdCapacityMT " +
                            " ,@BrandName " +
                            " ,@MfgCompanyName " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            String sqlSelectIdentityQry = "Select @@Identity";
            //cmdInsert.Parameters.Add("@IdCompetitorExt", System.Data.SqlDbType.Int).Value = tblCompetitorExtTO.IdCompetitorExt;
            cmdInsert.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblCompetitorExtTO.OrgId;
            cmdInsert.Parameters.Add("@ProdCapacityMT", System.Data.SqlDbType.NVarChar).Value = tblCompetitorExtTO.ProdCapacityMT;
            cmdInsert.Parameters.Add("@BrandName", System.Data.SqlDbType.NVarChar).Value = tblCompetitorExtTO.BrandName;
            cmdInsert.Parameters.Add("@MfgCompanyName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCompetitorExtTO.MfgCompanyName);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblCompetitorExtTO.IdCompetitorExt = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblCompetitorExtTO, cmdUpdate);
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

        public static int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblCompetitorExtTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblCompetitorExtTO tblCompetitorExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblCompetitorExt] SET " + 
                            "  [orgId]= @OrgId" +
                            " ,[prodCapacityMT]= @ProdCapacityMT" +
                            " ,[brandName]= @BrandName" +
                            " ,[mfgCompanyName] = @MfgCompanyName" +
                            " WHERE [idCompetitorExt] = @IdCompetitorExt "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdCompetitorExt", System.Data.SqlDbType.Int).Value = tblCompetitorExtTO.IdCompetitorExt;
            cmdUpdate.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblCompetitorExtTO.OrgId;
            cmdUpdate.Parameters.Add("@ProdCapacityMT", System.Data.SqlDbType.NVarChar).Value = tblCompetitorExtTO.ProdCapacityMT;
            cmdUpdate.Parameters.Add("@BrandName", System.Data.SqlDbType.NVarChar).Value = tblCompetitorExtTO.BrandName;
            cmdUpdate.Parameters.Add("@MfgCompanyName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCompetitorExtTO.MfgCompanyName);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblCompetitorExt(Int32 idCompetitorExt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idCompetitorExt, cmdDelete);
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

        public static int DeleteTblCompetitorExt(Int32 idCompetitorExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idCompetitorExt, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idCompetitorExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblCompetitorExt] " +
            " WHERE idCompetitorExt = " + idCompetitorExt +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idCompetitorExt", System.Data.SqlDbType.Int).Value = tblCompetitorExtTO.IdCompetitorExt;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
