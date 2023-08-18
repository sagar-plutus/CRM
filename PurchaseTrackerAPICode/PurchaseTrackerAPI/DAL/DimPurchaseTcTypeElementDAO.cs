using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL
{
    public class DimPurchaseTcTypeElementDAO : IDimPurchaseTcTypeElementDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimPurchaseTcTypeElementDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimPurchaseTcTypeElement]"; 
            return sqlSelectQry;
        }

        public String SelectQuery()
        {
            String sqlSelectQry = " SELECT dimPurchaseTcType.TcTypeName, dimPurchaseTcElement.TcElementName, dimPurchaseTcTypeElement.* FROM dimPurchaseTcTypeElement dimPurchaseTcTypeElement " +
                                   " LEFT JOIN dimPurchaseTcType dimPurchaseTcType ON dimPurchaseTcType.idTcType = dimPurchaseTcTypeElement.tcTypeId  " +
                                   " LEFT JOIN dimPurchaseTcElement dimPurchaseTcElement ON dimPurchaseTcElement.idTcElement = dimPurchaseTcTypeElement.tcElementId " +
                                   " WHERE dimPurchaseTcTypeElement.isActive = 1 AND dimPurchaseTcType.isActive = 1 " +
                                   " AND dimPurchaseTcElement.isActive = 1 ";
            return sqlSelectQry;
        }

        #endregion

        #region Selection
        public List<DimPurchaseTcTypeElementTO> SelectAllDimPurchaseTcTypeElement()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimPurchaseTcTypeElementTO> list = ConvertDTToList(rdr);
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

        public List<DimPurchaseTcTypeElementTO> SelectDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchasseTcTypeElement = " + idPurchasseTcTypeElement +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimPurchaseTcTypeElementTO> list = ConvertDTToList(rdr);
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

        public List<DimPurchaseTcTypeElementTO> SelectAllDimPurchaseTcTypeElement(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimPurchaseTcTypeElementTO> list = ConvertDTToList(rdr);
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

        public List<DimPurchaseTcTypeElementTO> ConvertDTToList(SqlDataReader dimPurchaseTcTypeElementTODT)
        {
            List<DimPurchaseTcTypeElementTO> dimPurchaseTcTypeElementTOList = new List<DimPurchaseTcTypeElementTO>();
            if (dimPurchaseTcTypeElementTODT != null)
            {
                while(dimPurchaseTcTypeElementTODT.Read())
                {
                    DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTONew = new DimPurchaseTcTypeElementTO();
                    if (dimPurchaseTcTypeElementTODT ["idPurchasseTcTypeElement"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.IdPurchasseTcTypeElement = Convert.ToInt32(dimPurchaseTcTypeElementTODT ["idPurchasseTcTypeElement"].ToString());
                    if (dimPurchaseTcTypeElementTODT ["tcTypeId"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.TcTypeId = Convert.ToInt32(dimPurchaseTcTypeElementTODT ["tcTypeId"].ToString());
                    if (dimPurchaseTcTypeElementTODT ["tcElementId"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.TcElementId = Convert.ToInt32(dimPurchaseTcTypeElementTODT ["tcElementId"].ToString());
                    if (dimPurchaseTcTypeElementTODT ["isActive"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.IsActive = Convert.ToInt32(dimPurchaseTcTypeElementTODT ["isActive"].ToString());
                    if (dimPurchaseTcTypeElementTODT ["createdBy"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.CreatedBy = Convert.ToInt32(dimPurchaseTcTypeElementTODT ["createdBy"].ToString());
                    if (dimPurchaseTcTypeElementTODT ["updatedBy"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.UpdatedBy = Convert.ToInt32(dimPurchaseTcTypeElementTODT ["updatedBy"].ToString());
                    if (dimPurchaseTcTypeElementTODT ["createdOn"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.CreatedOn = Convert.ToDateTime(dimPurchaseTcTypeElementTODT ["createdOn"].ToString());
                    if (dimPurchaseTcTypeElementTODT ["updatedOn"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.UpdatedOn = Convert.ToDateTime(dimPurchaseTcTypeElementTODT ["updatedOn"].ToString());

                    if (dimPurchaseTcTypeElementTODT["TcTypeName"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.TcTypeName = Convert.ToString(dimPurchaseTcTypeElementTODT["TcTypeName"].ToString());

                    if (dimPurchaseTcTypeElementTODT["TcElementName"] != DBNull.Value)
                        dimPurchaseTcTypeElementTONew.TcElementName = Convert.ToString(dimPurchaseTcTypeElementTODT["TcElementName"].ToString());

                    dimPurchaseTcTypeElementTOList.Add(dimPurchaseTcTypeElementTONew);
                }
            }
            return dimPurchaseTcTypeElementTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimPurchaseTcTypeElementTO, cmdInsert);
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

        public int InsertDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimPurchaseTcTypeElementTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimPurchaseTcTypeElement]( " + 
            //"  [idPurchasseTcTypeElement]" +
            "  [tcTypeId]" +
            " ,[tcElementId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " )" +
" VALUES (" +
            //"  @IdPurchasseTcTypeElement " +
            "  @TcTypeId " +
            " ,@TcElementId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurchasseTcTypeElement", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.IdPurchasseTcTypeElement;
            cmdInsert.Parameters.Add("@TcTypeId", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.TcTypeId;
            cmdInsert.Parameters.Add("@TcElementId", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.TcElementId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimPurchaseTcTypeElementTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = dimPurchaseTcTypeElementTO.UpdatedOn;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimPurchaseTcTypeElementTO, cmdUpdate);
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

        public int UpdateDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimPurchaseTcTypeElementTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimPurchaseTcTypeElement] SET " + 
            //"  [idPurchasseTcTypeElement] = @IdPurchasseTcTypeElement" +
            "  [tcTypeId]= @TcTypeId" +
            " ,[tcElementId]= @TcElementId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE idPurchasseTcTypeElement = @IdPurchasseTcTypeElement "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchasseTcTypeElement", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.IdPurchasseTcTypeElement;
            cmdUpdate.Parameters.Add("@TcTypeId", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.TcTypeId;
            cmdUpdate.Parameters.Add("@TcElementId", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.TcElementId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimPurchaseTcTypeElementTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = dimPurchaseTcTypeElementTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchasseTcTypeElement, cmdDelete);
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

        public int DeleteDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchasseTcTypeElement, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchasseTcTypeElement, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimPurchaseTcTypeElement] " +
            " WHERE idPurchasseTcTypeElement = " + idPurchasseTcTypeElement +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchasseTcTypeElement", System.Data.SqlDbType.Int).Value = dimPurchaseTcTypeElementTO.IdPurchasseTcTypeElement;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
