using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblProdClassificationDAO : ITblProdClassificationDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblProdClassificationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
             String sqlSelectQry = " SELECT prodClass.*,itemProdCatg.itemProdCategory FROM [tblProdClassification] prodClass " +
                                   " LEFT JOIN dimItemProdCateg itemProdCatg ON itemProdCatg.idItemProdCat=prodClass.itemProdCatId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblProdClassificationTO> SelectAllTblProdClassification(string prodClassType = "")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                if (string.IsNullOrEmpty(prodClassType))
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1 AND prodClassType='" + prodClassType + "'";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Prajakta[2018-08-13] Added to get sub Category or specification List
         public  List<TblProdClassificationTO> SelectAllTblProdClassification(string parentProdClassId,string prodClassType = "")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

               cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND prodClassType='" + prodClassType + "' AND parentProdClassId in (" + parentProdClassId + ")";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //sudhir[12-jan-2018] added connection, transaction.
        public  List<TblProdClassificationTO> SelectAllTblProdClassification(SqlConnection conn, SqlTransaction tran, string prodClassType = "")
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {

                if (string.IsNullOrEmpty(prodClassType))
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1 AND prodClassType='" + prodClassType + "'";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                     cmdSelect.Dispose();
            }
        }
        public  List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                if (parentClassId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1 AND prodClassType='C'";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1 AND parentProdClassId=" + parentClassId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (reader["idProdClass"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(reader["idProdClass"].ToString());
                        if (reader["prodClassDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(reader["prodClassDesc"].ToString());
                        if (reader["prodClassType"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(reader["prodClassType"].ToString());

                        list.Add(dropDownTO);
                    }
                }
                        return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblProdClassificationTO SelectTblProdClassification(Int32 idProdClass)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idProdClass = " + idProdClass +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
 public  List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND prodClass.itemProdCatId=" + (int)itemProdCategoryE;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // public  List<TblProdClassificationTO> ConvertDTToList(SqlDataReader tblProdClassificationTODT)
        // {
        //     List<TblProdClassificationTO> tblProdClassificationTOList = new List<TblProdClassificationTO>();
        //     if (tblProdClassificationTODT != null)
        //     {
        //         while (tblProdClassificationTODT.Read())
        //         {
        //             TblProdClassificationTO tblProdClassificationTONew = new TblProdClassificationTO();
        //             if (tblProdClassificationTODT["idProdClass"] != DBNull.Value)
        //                 tblProdClassificationTONew.IdProdClass = Convert.ToInt32(tblProdClassificationTODT["idProdClass"].ToString());
        //             if (tblProdClassificationTODT["parentProdClassId"] != DBNull.Value)
        //                 tblProdClassificationTONew.ParentProdClassId = Convert.ToInt32(tblProdClassificationTODT["parentProdClassId"].ToString());
        //             if (tblProdClassificationTODT["createdBy"] != DBNull.Value)
        //                 tblProdClassificationTONew.CreatedBy = Convert.ToInt32(tblProdClassificationTODT["createdBy"].ToString());
        //             if (tblProdClassificationTODT["updatedBy"] != DBNull.Value)
        //                 tblProdClassificationTONew.UpdatedBy = Convert.ToInt32(tblProdClassificationTODT["updatedBy"].ToString());
        //             if (tblProdClassificationTODT["createdOn"] != DBNull.Value)
        //                 tblProdClassificationTONew.CreatedOn = Convert.ToDateTime(tblProdClassificationTODT["createdOn"].ToString());
        //             if (tblProdClassificationTODT["updatedOn"] != DBNull.Value)
        //                 tblProdClassificationTONew.UpdatedOn = Convert.ToDateTime(tblProdClassificationTODT["updatedOn"].ToString());
        //             if (tblProdClassificationTODT["prodClassType"] != DBNull.Value)
        //                 tblProdClassificationTONew.ProdClassType = Convert.ToString(tblProdClassificationTODT["prodClassType"].ToString());
        //             if (tblProdClassificationTODT["prodClassDesc"] != DBNull.Value)
        //                 tblProdClassificationTONew.ProdClassDesc = Convert.ToString(tblProdClassificationTODT["prodClassDesc"].ToString());
        //             if (tblProdClassificationTODT["remark"] != DBNull.Value)
        //                 tblProdClassificationTONew.Remark = Convert.ToString(tblProdClassificationTODT["remark"].ToString());
        //             if (tblProdClassificationTODT["isActive"] != DBNull.Value)
        //                 tblProdClassificationTONew.IsActive = Convert.ToInt32(tblProdClassificationTODT["isActive"].ToString());
        //             if (tblProdClassificationTODT["displayName"] != DBNull.Value)
        //                 tblProdClassificationTONew.DisplayName = Convert.ToString(tblProdClassificationTODT["displayName"].ToString());
                    
        //             tblProdClassificationTOList.Add(tblProdClassificationTONew);
        //         }
        //     }
        //     return tblProdClassificationTOList;
        // }
 public  List<TblProdClassificationTO> ConvertDTToList(SqlDataReader tblProdClassificationTODT)
        {
            List<TblProdClassificationTO> tblProdClassificationTOList = new List<TblProdClassificationTO>();
            if (tblProdClassificationTODT != null)
            {
                while (tblProdClassificationTODT.Read())
                {
                    TblProdClassificationTO tblProdClassificationTONew = new TblProdClassificationTO();
                    if (tblProdClassificationTODT["idProdClass"] != DBNull.Value)
                        tblProdClassificationTONew.IdProdClass = Convert.ToInt32(tblProdClassificationTODT["idProdClass"].ToString());
                    if (tblProdClassificationTODT["parentProdClassId"] != DBNull.Value)
                        tblProdClassificationTONew.ParentProdClassId = Convert.ToInt32(tblProdClassificationTODT["parentProdClassId"].ToString());
                    if (tblProdClassificationTODT["createdBy"] != DBNull.Value)
                        tblProdClassificationTONew.CreatedBy = Convert.ToInt32(tblProdClassificationTODT["createdBy"].ToString());
                    if (tblProdClassificationTODT["updatedBy"] != DBNull.Value)
                        tblProdClassificationTONew.UpdatedBy = Convert.ToInt32(tblProdClassificationTODT["updatedBy"].ToString());
                    if (tblProdClassificationTODT["createdOn"] != DBNull.Value)
                        tblProdClassificationTONew.CreatedOn = Convert.ToDateTime(tblProdClassificationTODT["createdOn"].ToString());
                    if (tblProdClassificationTODT["updatedOn"] != DBNull.Value)
                        tblProdClassificationTONew.UpdatedOn = Convert.ToDateTime(tblProdClassificationTODT["updatedOn"].ToString());
                    if (tblProdClassificationTODT["prodClassType"] != DBNull.Value)
                        tblProdClassificationTONew.ProdClassType = Convert.ToString(tblProdClassificationTODT["prodClassType"].ToString());
                    if (tblProdClassificationTODT["prodClassDesc"] != DBNull.Value)
                        tblProdClassificationTONew.ProdClassDesc = Convert.ToString(tblProdClassificationTODT["prodClassDesc"].ToString());
                    if (tblProdClassificationTODT["remark"] != DBNull.Value)
                        tblProdClassificationTONew.Remark = Convert.ToString(tblProdClassificationTODT["remark"].ToString());
                    if (tblProdClassificationTODT["isActive"] != DBNull.Value)
                        tblProdClassificationTONew.IsActive = Convert.ToInt32(tblProdClassificationTODT["isActive"].ToString());
                    if (tblProdClassificationTODT["displayName"] != DBNull.Value)
                        tblProdClassificationTONew.DisplayName = Convert.ToString(tblProdClassificationTODT["displayName"].ToString());
                    
                    //Sanjay [2018-02-19] Added To Distinguish between regular Product and Scrap
                    if (tblProdClassificationTODT["itemProdCatId"] != DBNull.Value)
                        tblProdClassificationTONew.ItemProdCatId = Convert.ToInt32(tblProdClassificationTODT["itemProdCatId"].ToString());
                    if (tblProdClassificationTODT["itemProdCategory"] != DBNull.Value)
                        tblProdClassificationTONew.ItemProdCategory = Convert.ToString(tblProdClassificationTODT["itemProdCategory"].ToString());


                    //if (tblProdClassificationTODT["isSetDefault"] != DBNull.Value)
                    //    tblProdClassificationTONew.IsSetDefault = Convert.ToInt32(tblProdClassificationTODT["isSetDefault"].ToString());
                    
                    //Priyanka [16-05-2018] Added for tax type id
                    if (tblProdClassificationTODT["codeTypeId"] != DBNull.Value)
                        tblProdClassificationTONew.CodeTypeId = Convert.ToInt32(tblProdClassificationTODT["codeTypeId"].ToString());
                    tblProdClassificationTOList.Add(tblProdClassificationTONew);
                }
            }
            return tblProdClassificationTOList;
        }
        #endregion

        #region Insertion
        public  int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProdClassificationTO, cmdInsert);
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

        public  int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProdClassificationTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProdClassification]( " + 
                            "  [parentProdClassId]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[prodClassType]" +
                            " ,[prodClassDesc]" +
                            " ,[remark]" +
                            " ,[isActive]" +
                            " ,[displayName]" +
                            " )" +
                " VALUES (" +
                            "  @ParentProdClassId " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@ProdClassType " +
                            " ,@ProdClassDesc " +
                            " ,@Remark " +
                            " ,@isActive " +
                            " ,@displayName " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProdClass", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IdProdClass;
            cmdInsert.Parameters.Add("@ParentProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.ParentProdClassId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdClassificationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ProdClassType", System.Data.SqlDbType.Char).Value = tblProdClassificationTO.ProdClassType;
            cmdInsert.Parameters.Add("@ProdClassDesc", System.Data.SqlDbType.NVarChar).Value = tblProdClassificationTO.ProdClassDesc;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.Remark);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IsActive;
            cmdInsert.Parameters.Add("@displayName", System.Data.SqlDbType.NVarChar).Value = tblProdClassificationTO.DisplayName;
            if (cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblProdClassificationTO.IdProdClass = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProdClassificationTO, cmdUpdate);
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

        public  int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProdClassificationTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProdClassification] SET " + 
                                "  [parentProdClassId]= @ParentProdClassId" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[prodClassType]= @ProdClassType" +
                                " ,[prodClassDesc]= @ProdClassDesc" +
                                " ,[remark] = @Remark" +
                                " ,[isActive] = @isActive" +
                                " ,[displayName]=@displayName" +
                                " WHERE [idProdClass] = @IdProdClass"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdClass", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IdProdClass;
            cmdUpdate.Parameters.Add("@ParentProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.ParentProdClassId);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProdClassificationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ProdClassType", System.Data.SqlDbType.Char).Value = tblProdClassificationTO.ProdClassType;
            cmdUpdate.Parameters.Add("@ProdClassDesc", System.Data.SqlDbType.NVarChar).Value = tblProdClassificationTO.ProdClassDesc;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.Remark);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IsActive;
            cmdUpdate.Parameters.Add("@displayName", System.Data.SqlDbType.NVarChar).Value = tblProdClassificationTO.DisplayName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblProdClassification(Int32 idProdClass)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdClass, cmdDelete);
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

        public  int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdClass, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idProdClass, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProdClassification] " +
            " WHERE idProdClass = " + idProdClass +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdClass", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IdProdClass;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
