using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class DimQualitySampleTypeDAO : IDimQualitySampleTypeDAO
    {
        private readonly IConnectionString _iConnectionString;

        public DimQualitySampleTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimQualitySampleType]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<DimQualitySampleTypeTO> SelectAllDimQualitySampleType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idQualitySampleType", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IdQualitySampleType;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimQualitySampleTypeTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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


        public  List<DimQualitySampleTypeTO> SelectDimQualitySampleType(Int32 idQualitySampleType)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idQualitySampleType = " + idQualitySampleType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idQualitySampleType", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IdQualitySampleType;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimQualitySampleTypeTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

        public  DataTable SelectAllDimQualitySampleType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idQualitySampleType", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IdQualitySampleType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        public  List<DimQualitySampleTypeTO> ConvertDTToList(SqlDataReader dimQualitySampleTypeTODT)
        {
            List<DimQualitySampleTypeTO> dimQualitySampleTypeTOList = new List<DimQualitySampleTypeTO>();
            if (dimQualitySampleTypeTODT != null)
            {
                while (dimQualitySampleTypeTODT.Read())
                {
                    DimQualitySampleTypeTO dimQualitySampleTypeTONew = new DimQualitySampleTypeTO();
                    if (dimQualitySampleTypeTODT["idQualitySampleType"] != DBNull.Value)
                        dimQualitySampleTypeTONew.IdQualitySampleType = Convert.ToInt32(dimQualitySampleTypeTODT["idQualitySampleType"].ToString());
                    if (dimQualitySampleTypeTODT["parentSampleTypeId"] != DBNull.Value)
                        dimQualitySampleTypeTONew.ParentSampleTypeId = Convert.ToInt32(dimQualitySampleTypeTODT["parentSampleTypeId"].ToString());
                    if (dimQualitySampleTypeTODT["isActive"] != DBNull.Value)
                        dimQualitySampleTypeTONew.IsActive = Convert.ToInt32(dimQualitySampleTypeTODT["isActive"].ToString());
                    if (dimQualitySampleTypeTODT["seqNo"] != DBNull.Value)
                        dimQualitySampleTypeTONew.SeqNo = Convert.ToInt32(dimQualitySampleTypeTODT["seqNo"].ToString());
                    if (dimQualitySampleTypeTODT["sampleTypeName"] != DBNull.Value)
                        dimQualitySampleTypeTONew.SampleTypeName = Convert.ToString(dimQualitySampleTypeTODT["sampleTypeName"].ToString());
                    if (dimQualitySampleTypeTODT["sampleTypeDesc"] != DBNull.Value)
                        dimQualitySampleTypeTONew.SampleTypeDesc = Convert.ToString(dimQualitySampleTypeTODT["sampleTypeDesc"].ToString());
                    if (dimQualitySampleTypeTODT["phaseId"] != DBNull.Value)
                        dimQualitySampleTypeTONew.PhaseId = Convert.ToInt32(dimQualitySampleTypeTODT["phaseId"].ToString());

                    if (dimQualitySampleTypeTODT["flagTypeId"] != DBNull.Value)
                        dimQualitySampleTypeTONew.FlagTypeId = Convert.ToInt32(dimQualitySampleTypeTODT["flagTypeId"].ToString());

                    dimQualitySampleTypeTOList.Add(dimQualitySampleTypeTONew);
                }
            }
            return dimQualitySampleTypeTOList;
        }

        #endregion

        #region Insertion
        public  int InsertDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimQualitySampleTypeTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimQualitySampleTypeTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimQualitySampleType]( " +
            //"  [idQualitySampleType]" +
            "  [parentSampleTypeId]" +
            " ,[isActive]" +
            " ,[seqNo]" +
            " ,[sampleTypeName]" +
            " ,[sampleTypeDesc]" +
            " ,[phaseId]" +
            " )" +
" VALUES (" +
            //"  @IdQualitySampleType " +
            "  @ParentSampleTypeId " +
            " ,@IsActive " +
            " ,@SeqNo " +
            " ,@SampleTypeName " +
            " ,@SampleTypeDesc " +
            " ,@PhaseId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdQualitySampleType", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IdQualitySampleType;
            cmdInsert.Parameters.Add("@ParentSampleTypeId", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.ParentSampleTypeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IsActive;
            cmdInsert.Parameters.Add("@SeqNo", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.SeqNo;
            cmdInsert.Parameters.Add("@SampleTypeName", System.Data.SqlDbType.VarChar).Value = dimQualitySampleTypeTO.SampleTypeName;
            cmdInsert.Parameters.Add("@SampleTypeDesc", System.Data.SqlDbType.VarChar).Value = dimQualitySampleTypeTO.SampleTypeDesc;
            cmdInsert.Parameters.Add("@PhaseId", System.Data.SqlDbType.VarChar).Value = dimQualitySampleTypeTO.PhaseId;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimQualitySampleTypeTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimQualitySampleTypeTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimQualitySampleType] SET " +
            // "  [idQualitySampleType] = @IdQualitySampleType" +
            "  [parentSampleTypeId]= @ParentSampleTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[seqNo]= @SeqNo" +
            " ,[sampleTypeName]= @SampleTypeName" +
            " ,[sampleTypeDesc] = @SampleTypeDesc" +
            " ,[phaseId] = @PhaseId" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdQualitySampleType", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IdQualitySampleType;
            cmdUpdate.Parameters.Add("@ParentSampleTypeId", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.ParentSampleTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@SeqNo", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.SeqNo;
            cmdUpdate.Parameters.Add("@SampleTypeName", System.Data.SqlDbType.VarChar).Value = dimQualitySampleTypeTO.SampleTypeName;
            cmdUpdate.Parameters.Add("@SampleTypeDesc", System.Data.SqlDbType.VarChar).Value = dimQualitySampleTypeTO.SampleTypeDesc;
            cmdUpdate.Parameters.Add("@PhaseId", System.Data.SqlDbType.VarChar).Value = dimQualitySampleTypeTO.PhaseId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteDimQualitySampleType(Int32 idQualitySampleType)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idQualitySampleType, cmdDelete);
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

        public  int DeleteDimQualitySampleType(Int32 idQualitySampleType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idQualitySampleType, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idQualitySampleType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimQualitySampleType] " +
            " WHERE idQualitySampleType = " + idQualitySampleType + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idQualitySampleType", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IdQualitySampleType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
