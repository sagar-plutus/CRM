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
    public class DimPageElementTypesDAO : IDimPageElementTypesDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimPageElementTypesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimPageElementTypes]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<DimPageElementTypesTO> SelectAllDimPageElementTypes()
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimPageElementTypesTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  DimPageElementTypesTO SelectDimPageElementTypes(Int32 idPageEleType)
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPageEleType = " + idPageEleType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimPageElementTypesTO> list = ConvertDTToList(rdr);
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
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<DimPageElementTypesTO> ConvertDTToList(SqlDataReader dimPageElementTypesTODT)
        {
            List<DimPageElementTypesTO> dimPageElementTypesTOList = new List<DimPageElementTypesTO>();
            if (dimPageElementTypesTODT != null)
            {
                while (dimPageElementTypesTODT.Read())
                {
                    DimPageElementTypesTO dimPageElementTypesTONew = new DimPageElementTypesTO();
                    if (dimPageElementTypesTODT["idPageEleType"] != DBNull.Value)
                        dimPageElementTypesTONew.IdPageEleType = Convert.ToInt32(dimPageElementTypesTODT["idPageEleType"].ToString());
                    if (dimPageElementTypesTODT["pageEleTypeName"] != DBNull.Value)
                        dimPageElementTypesTONew.PageEleTypeName = Convert.ToString(dimPageElementTypesTODT["pageEleTypeName"].ToString());
                    if (dimPageElementTypesTODT["pageEleTypeDesc"] != DBNull.Value)
                        dimPageElementTypesTONew.PageEleTypeDesc = Convert.ToString(dimPageElementTypesTODT["pageEleTypeDesc"].ToString());
                    dimPageElementTypesTOList.Add(dimPageElementTypesTONew);
                }
            }
            return dimPageElementTypesTOList;
        }

        #endregion

        #region Insertion
        public  int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimPageElementTypesTO, cmdInsert);
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

        public  int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimPageElementTypesTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(DimPageElementTypesTO dimPageElementTypesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimPageElementTypes]( " + 
            "  [idPageEleType]" +
            " ,[pageEleTypeName]" +
            " ,[pageEleTypeDesc]" +
            " )" +
" VALUES (" +
            "  @IdPageEleType " +
            " ,@PageEleTypeName " +
            " ,@PageEleTypeDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPageEleType", System.Data.SqlDbType.Int).Value = dimPageElementTypesTO.IdPageEleType;
            cmdInsert.Parameters.Add("@PageEleTypeName", System.Data.SqlDbType.NVarChar).Value = dimPageElementTypesTO.PageEleTypeName;
            cmdInsert.Parameters.Add("@PageEleTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimPageElementTypesTO.PageEleTypeDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimPageElementTypesTO, cmdUpdate);
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

        public  int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimPageElementTypesTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(DimPageElementTypesTO dimPageElementTypesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimPageElementTypes] SET " + 
            "  [idPageEleType] = @IdPageEleType" +
            " ,[pageEleTypeName]= @PageEleTypeName" +
            " ,[pageEleTypeDesc] = @PageEleTypeDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPageEleType", System.Data.SqlDbType.Int).Value = dimPageElementTypesTO.IdPageEleType;
            cmdUpdate.Parameters.Add("@PageEleTypeName", System.Data.SqlDbType.NVarChar).Value = dimPageElementTypesTO.PageEleTypeName;
            cmdUpdate.Parameters.Add("@PageEleTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimPageElementTypesTO.PageEleTypeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteDimPageElementTypes(Int32 idPageEleType)
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPageEleType, cmdDelete);
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

        public  int DeleteDimPageElementTypes(Int32 idPageEleType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPageEleType, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPageEleType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimPageElementTypes] " +
            " WHERE idPageEleType = " + idPageEleType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPageEleType", System.Data.SqlDbType.Int).Value = dimPageElementTypesTO.IdPageEleType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
