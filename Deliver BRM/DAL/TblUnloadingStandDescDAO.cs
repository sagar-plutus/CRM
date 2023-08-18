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
    public class TblUnloadingStandDescDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblUnloadingStandDesc]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection

        // Vaibhav [13-Sep-2017] Added to select all Unloading Standard Descriptions for drop down
        public static List<DropDownTO> SelectAllUnloadingStandDescForDropDown()
        {            
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblUnloadingStandDescTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblUnloadingStandDescTODT != null)
                {
                    while (tblUnloadingStandDescTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblUnloadingStandDescTODT["idUnloadingStandDesc"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblUnloadingStandDescTODT["idUnloadingStandDesc"].ToString());
                        if (tblUnloadingStandDescTODT["standardDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblUnloadingStandDescTODT["standardDesc"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<TblUnloadingStandDescTO> SelectAllTblUnloadingStandDesc()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnloadingStandDescTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
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

        public static TblUnloadingStandDescTO SelectTblUnloadingStandDesc(Int32 idUnloadingStandDesc)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idUnloadingStandDesc = " + idUnloadingStandDesc + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnloadingStandDescTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblUnloadingStandDescTO> ConvertDTToList(SqlDataReader tblUnloadingStandDescTO)
        {
            List<TblUnloadingStandDescTO> tblUnloadingStandDescTOList = new List<TblUnloadingStandDescTO>();
            if (tblUnloadingStandDescTO != null)
            {
                while (tblUnloadingStandDescTO.Read())
                {
                    TblUnloadingStandDescTO tblUnloadingStandDescTONew = new TblUnloadingStandDescTO();
                    if (tblUnloadingStandDescTO["idUnloadingStandDesc"] != DBNull.Value)
                        tblUnloadingStandDescTONew.IdUnloadingStandDesc = Convert.ToInt32(tblUnloadingStandDescTO["idUnloadingStandDesc"].ToString());
                    if (tblUnloadingStandDescTO["standardDesc"] != DBNull.Value)
                        tblUnloadingStandDescTONew.StandardDesc = tblUnloadingStandDescTO["standardDesc"].ToString();
                    if (tblUnloadingStandDescTO["remark"] != DBNull.Value)
                        tblUnloadingStandDescTONew.Remark = tblUnloadingStandDescTO["remark"].ToString();
                    if (tblUnloadingStandDescTO["isActive"] != DBNull.Value)
                        tblUnloadingStandDescTONew.IsActive = Convert.ToInt32(tblUnloadingStandDescTO["isActive"].ToString());
                    tblUnloadingStandDescTOList.Add(tblUnloadingStandDescTONew);
                }
            }
            return tblUnloadingStandDescTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO UnloadingStandDescTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(UnloadingStandDescTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblUnloadingStandDesc");
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUnloadingStandDescTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUnloadingStandDesc]( " +
            " [isActive]" +
            " ,[standardDesc]" +
            " ,[remark] " +
            " )" +
            " VALUES (" +
            " @IsActive " +
            " ,@StandardDesc " +
            " ,@Remark " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUnloadingStandDescTO.IsActive;
            cmdInsert.Parameters.Add("@StandardDesc", System.Data.SqlDbType.NVarChar).Value = tblUnloadingStandDescTO.StandardDesc;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblUnloadingStandDescTO.Remark;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUnloadingStandDescTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblUnloadingStandDesc");
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUnloadingStandDescTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUnloadingStandDesc] SET " +
            " [isActive]= @IsActive" +
            " ,[standardDesc] = @StandardDesc" +
             " ,[remark] =@Remark " +
            " WHERE idUnloadingStandDesc = @IdUnloadingStandDesc ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUnloadingStandDesc", System.Data.SqlDbType.Int).Value = tblUnloadingStandDescTO.IdUnloadingStandDesc;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUnloadingStandDescTO.IsActive;
            cmdUpdate.Parameters.Add("@StandardDesc", System.Data.SqlDbType.NVarChar).Value = tblUnloadingStandDescTO.StandardDesc;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblUnloadingStandDescTO.Remark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUnloadingStandDesc, cmdDelete);
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

        public static int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUnloadingStandDesc, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idUnloadingStandDesc, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUnloadingStandDesc] " +
            " WHERE idUnloadingStandDesc = " + idUnloadingStandDesc + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUnloadingStandDesc", System.Data.SqlDbType.Int).Value = tblUnloadingStandDescTO.IdUnloadingStandDesc;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
