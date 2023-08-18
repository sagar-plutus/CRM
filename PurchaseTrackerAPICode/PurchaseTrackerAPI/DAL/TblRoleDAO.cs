using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblRoleDAO : ITblRoleDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblRoleDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblRole]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  TblRoleTO SelectAllTblRole()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
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

        public  TblRoleTO SelectTblRole(Int32 idRole)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idRole = " + idRole + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRoleTO> list = ConvertDTToList(sqlReader);
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
        /// <summary>
        /// added by swati pisal to execute the query
        /// </summary>
        /// <param name="tblRoleTODT"></param>
        /// <returns></returns>
        public  List<TblRoleTO> ConvertDTToList(SqlDataReader tblRoleTODT)
        {
            List<TblRoleTO> tblRoleTOList = new List<TblRoleTO>();
            if (tblRoleTODT != null)
            {
                while (tblRoleTODT.Read())
                {
                    TblRoleTO tblRoleTONew = new TblRoleTO();
                    if (tblRoleTODT["idRole"] != DBNull.Value)
                        tblRoleTONew.IdRole = Convert.ToInt32(tblRoleTODT["idRole"].ToString());
                    if (tblRoleTODT["roleDesc"] != DBNull.Value)
                        tblRoleTONew.RoleDesc = tblRoleTODT["roleDesc"].ToString();
                    if (tblRoleTODT["isSystem"] != DBNull.Value)
                        tblRoleTONew.IsActive = Convert.ToInt32(tblRoleTODT["isSystem"].ToString());

                    if (tblRoleTODT["createdBy"] != DBNull.Value)
                        tblRoleTONew.CreatedBy = Convert.ToInt32(tblRoleTODT["createdBy"].ToString());
                    if (tblRoleTODT["isActive"] != DBNull.Value)
                        tblRoleTONew.IsActive = Convert.ToInt32(tblRoleTODT["isActive"].ToString());
                    if (tblRoleTODT["createdOn"] != DBNull.Value)
                        tblRoleTONew.CreatedOn = Convert.ToDateTime(tblRoleTODT["createdOn"].ToString());
                    tblRoleTOList.Add(tblRoleTONew);
                }
            }
            return tblRoleTOList;
        }

        public  TblRoleTO SelectAllTblRole(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public  int InsertTblRole(TblRoleTO tblRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblRoleTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblRoleTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblRole");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblRoleTO tblRoleTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRole]( " +
            "  [isActive]" +
            " ,[isSystem]" +
            " ,[createdBy]" +
            " ,[enableAreaAlloc]" +
            " ,[orgStructureId]" +
            " ,[createdOn]" +
            " ,[roleDesc]" +
            " )" +
            " VALUES (" +
            "  @IsActive " +
            " ,@IsSystem " +
            " ,@CreatedBy " +
            " ,@EnableAreaAlloc " +
            " ,@OrgStructureId " +
            " ,@CreatedOn " +
            " ,@RoleDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdRole", System.Data.SqlDbType.Int).Value = tblRoleTO.IdRole;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblRoleTO.IsActive;
            cmdInsert.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = tblRoleTO.IsSystem;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRoleTO.CreatedBy;
            cmdInsert.Parameters.Add("@EnableAreaAlloc", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblRoleTO.EnableAreaAlloc);
            cmdInsert.Parameters.Add("@OrgStructureId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblRoleTO.OrgStructureId);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRoleTO.CreatedOn;
            cmdInsert.Parameters.Add("@RoleDesc", System.Data.SqlDbType.NVarChar).Value = tblRoleTO.RoleDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateTblRole(TblRoleTO tblRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblRoleTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblRoleTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblRoleTO tblRoleTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblRole] SET " +
            "  [idRole] = @IdRole" +
            " ,[isActive]= @IsActive" +
            " ,[isSystem]= @IsSystem" +
            " ,[createdBy]= @CreatedBy" +
            " ,[enableAreaAlloc]= @EnableAreaAlloc" +
            " ,[orgStructureId]= @OrgStructureId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[roleDesc] = @RoleDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRole", System.Data.SqlDbType.Int).Value = tblRoleTO.IdRole;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblRoleTO.IsActive;
            cmdUpdate.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = tblRoleTO.IsSystem;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRoleTO.CreatedBy;
            cmdUpdate.Parameters.Add("@EnableAreaAlloc", System.Data.SqlDbType.Int).Value = tblRoleTO.EnableAreaAlloc;
            cmdUpdate.Parameters.Add("@OrgStructureId", System.Data.SqlDbType.Int).Value = tblRoleTO.OrgStructureId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRoleTO.CreatedOn;
            cmdUpdate.Parameters.Add("@RoleDesc", System.Data.SqlDbType.NVarChar).Value = tblRoleTO.RoleDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblRole(Int32 idRole)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idRole, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idRole, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idRole, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblRole] " +
            " WHERE idRole = " + idRole + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idRole", System.Data.SqlDbType.Int).Value = tblRoleTO.IdRole;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
