using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblMaterialDAO : ITblMaterialDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblMaterialDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblMaterial.* , userDisplayName FROM tblMaterial "+
                                  " LEFT JOIN tblUser ON idUser = createdBy where tblMaterial.isActive=1";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblMaterialTO> SelectAllTblMaterial()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMaterialTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
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


        public  List<DropDownTO> SelectAllMaterialForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblMaterialTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblMaterialTODT != null)
                {
                    while (tblMaterialTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblMaterialTODT["idMaterial"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblMaterialTODT["idMaterial"].ToString());                      
                        if (tblMaterialTODT["materialSubType"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblMaterialTODT["materialSubType"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
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

        public  TblMaterialTO SelectTblMaterial(Int32 idMaterial)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idMaterial = " + idMaterial +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMaterialTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

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

        public  List<TblMaterialTO> ConvertDTToList(SqlDataReader tblMaterialTODT)
        {
            List<TblMaterialTO> tblMaterialTOList = new List<TblMaterialTO>();
            if (tblMaterialTODT != null)
            {
                while (tblMaterialTODT.Read())
                {
                    TblMaterialTO tblMaterialTONew = new TblMaterialTO();
                    if (tblMaterialTODT["idMaterial"] != DBNull.Value)
                        tblMaterialTONew.IdMaterial = Convert.ToInt32(tblMaterialTODT["idMaterial"].ToString());
                    if (tblMaterialTODT["mateCompOrgId"] != DBNull.Value)
                        tblMaterialTONew.MateCompOrgId = Convert.ToInt32(tblMaterialTODT["mateCompOrgId"].ToString());
                    if (tblMaterialTODT["mateSubCompOrgId"] != DBNull.Value)
                        tblMaterialTONew.MateSubCompOrgId = Convert.ToInt32(tblMaterialTODT["mateSubCompOrgId"].ToString());
                    if (tblMaterialTODT["materialTypeId"] != DBNull.Value)
                        tblMaterialTONew.MaterialTypeId = Convert.ToInt32(tblMaterialTODT["materialTypeId"].ToString());
                    if (tblMaterialTODT["createdBy"] != DBNull.Value)
                        tblMaterialTONew.CreatedBy = Convert.ToInt32(tblMaterialTODT["createdBy"].ToString());
                    if (tblMaterialTODT["createdOn"] != DBNull.Value)
                        tblMaterialTONew.CreatedOn = Convert.ToDateTime(tblMaterialTODT["createdOn"].ToString());
                    if (tblMaterialTODT["materialSubType"] != DBNull.Value)
                        tblMaterialTONew.MaterialSubType = Convert.ToString(tblMaterialTODT["materialSubType"].ToString());
                    if (tblMaterialTODT["userDisplayName"] != DBNull.Value)
                        tblMaterialTONew.UserDisplayName = Convert.ToString(tblMaterialTODT["userDisplayName"].ToString());
                    if (tblMaterialTODT["updatedBy"] != DBNull.Value)
                        tblMaterialTONew.UpdatedBy = Convert.ToInt32(tblMaterialTODT["updatedBy"].ToString());
                    if (tblMaterialTODT["updatedOn"] != DBNull.Value)
                        tblMaterialTONew.UpdatedOn = Convert.ToDateTime(tblMaterialTODT["updatedOn"].ToString());
                    if (tblMaterialTODT["deactivatedBy"] != DBNull.Value)
                        tblMaterialTONew.DeactivatedBy = Convert.ToInt32(tblMaterialTODT["deactivatedBy"].ToString());
                    if (tblMaterialTODT["deactivatedOn"] != DBNull.Value)
                        tblMaterialTONew.DeactivatedOn = Convert.ToDateTime(tblMaterialTODT["deactivatedOn"].ToString());
                    if (tblMaterialTODT["isActive"] != DBNull.Value)
                        tblMaterialTONew.IsActive = Convert.ToInt32(tblMaterialTODT["isActive"].ToString());
                    tblMaterialTOList.Add(tblMaterialTONew);
                }
            }
            return tblMaterialTOList;
        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To Get Material Type List
        /// </summary>
        /// <returns></returns>
        public  List<DropDownTO> SelectMaterialTypeDropDownList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                String SelectQry = " SELECT * from [dimMaterialType] where isActive=1";
                cmdSelect.CommandText = SelectQry;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblMaterialTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblMaterialTODT != null)
                {
                    while (tblMaterialTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblMaterialTODT["idMaterialType"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblMaterialTODT["idMaterialType"].ToString());
                        if (tblMaterialTODT["materialTypeDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblMaterialTODT["materialTypeDesc"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
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
        public  int InsertTblMaterial(TblMaterialTO tblMaterialTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblMaterialTO, cmdInsert);
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

        public  int InsertTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblMaterialTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblMaterialTO tblMaterialTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblMaterial]( " +
                            "  [mateCompOrgId]" +
                            " ,[mateSubCompOrgId]" +
                            " ,[materialTypeId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[materialSubType]" +
                            " ,[updatedBy]" +
                            ",[deactivatedBy]" +
                            " ,[updatedOn]" +
                            " ,[deactivatedOn]" +
                            " ,[isActive]" +
                            " )" +
                " VALUES (" +
                            "  @MateCompOrgId " +
                            " ,@MateSubCompOrgId " +
                            " ,@MaterialTypeId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@MaterialSubType " +
                            " ,@UpdatedBy " +
                            " ,@DeactivatedBy " +
                            " ,@UpdatedOn " +
                            " ,@DeactivatedOn " +
                            " ,@IsActive " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdMaterial", System.Data.SqlDbType.Int).Value = tblMaterialTO.IdMaterial;
            cmdInsert.Parameters.Add("@MateCompOrgId", System.Data.SqlDbType.Int).Value = tblMaterialTO.MateCompOrgId;
            cmdInsert.Parameters.Add("@MateSubCompOrgId", System.Data.SqlDbType.Int).Value = tblMaterialTO.MateSubCompOrgId;
            cmdInsert.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = tblMaterialTO.MaterialTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblMaterialTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblMaterialTO.CreatedOn;
            cmdInsert.Parameters.Add("@MaterialSubType", System.Data.SqlDbType.VarChar).Value = tblMaterialTO.MaterialSubType;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblMaterialTO.UpdatedBy);
            cmdInsert.Parameters.Add("@DeactivatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblMaterialTO.DeactivatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblMaterialTO.UpdatedOn);
            cmdInsert.Parameters.Add("@DeactivatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblMaterialTO.DeactivatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblMaterialTO.IsActive;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblMaterialTO.IdMaterial = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblMaterial(TblMaterialTO tblMaterialTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblMaterialTO, cmdUpdate);
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

        public  int UpdateTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblMaterialTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblMaterialTO tblMaterialTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblMaterial] SET " + 
                                "  [mateCompOrgId]= @MateCompOrgId" +
                                " ,[mateSubCompOrgId]= @MateSubCompOrgId" +
                                " ,[materialTypeId]= @MaterialTypeId" +
                                " ,[materialSubType] = @MaterialSubType" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[deactivatedBy]= @DeactivatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[deactivatedOn]= @DeactivatedOn" +
                                " ,[isActive]= @IsActive" +
                                " WHERE [idMaterial] = @IdMaterial"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdMaterial", System.Data.SqlDbType.Int).Value = tblMaterialTO.IdMaterial;
            cmdUpdate.Parameters.Add("@MateCompOrgId", System.Data.SqlDbType.Int).Value = tblMaterialTO.MateCompOrgId;
            cmdUpdate.Parameters.Add("@MateSubCompOrgId", System.Data.SqlDbType.Int).Value = tblMaterialTO.MateSubCompOrgId;
            cmdUpdate.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = tblMaterialTO.MaterialTypeId;
            cmdUpdate.Parameters.Add("@MaterialSubType", System.Data.SqlDbType.VarChar).Value = tblMaterialTO.MaterialSubType;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblMaterialTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@DeactivatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblMaterialTO.DeactivatedBy);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblMaterialTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@DeactivatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblMaterialTO.DeactivatedOn);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblMaterialTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblMaterial(Int32 idMaterial)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idMaterial, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblMaterial(Int32 idMaterial, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idMaterial, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public  int ExecuteDeletionCommand(Int32 idMaterial, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblMaterial] " +
            " WHERE idMaterial = " + idMaterial +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idMaterial", System.Data.SqlDbType.Int).Value = tblMaterialTO.IdMaterial;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
