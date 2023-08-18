using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class DimVehiclePhaseDAO :IDimVehiclePhaseDAO
    {


        private readonly IConnectionString _iConnectionString;

        private readonly Icommondao _iCommon;
        public DimVehiclePhaseDAO(IConnectionString iConnectionString, Icommondao iCommon)
        {
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimVehiclePhase] ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<DimVehiclePhaseTO> SelectAllDimVehiclePhase(Int32 isActive)
        {
        String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                if(isActive > 0)
                {
                    cmdSelect.CommandText += " where isActive = " + isActive;
                }
                cmdSelect.CommandText += " order by sequanceNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehiclePhase", System.Data.SqlDbType.Int).Value = dimVehiclePhaseTO.IdVehiclePhase;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehiclePhaseTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

        public List<DimVehiclePhaseTO> SelectAllDimVehiclePhase()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                
                cmdSelect.CommandText += " order by sequanceNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehiclePhase", System.Data.SqlDbType.Int).Value = dimVehiclePhaseTO.IdVehiclePhase;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehiclePhaseTO> list = ConvertDTToList(rdr);
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

        public  List<DimVehiclePhaseTO> SelectDimVehiclePhase(Int32 idVehiclePhase)
        {
         String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idVehiclePhase = " + idVehiclePhase +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehiclePhase", System.Data.SqlDbType.Int).Value = dimVehiclePhaseTO.IdVehiclePhase;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehiclePhaseTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

        public  List<DimVehiclePhaseTO> SelectAllDimVehiclePhase(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehiclePhase", System.Data.SqlDbType.Int).Value = dimVehiclePhaseTO.IdVehiclePhase;
               SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehiclePhaseTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

          public  List<DimVehiclePhaseTO> ConvertDTToList(SqlDataReader dimVehiclePhaseTODT )
        {
            List<DimVehiclePhaseTO> dimVehiclePhaseTOList = new List<DimVehiclePhaseTO>();
            if (dimVehiclePhaseTODT != null)
            {
                 while (dimVehiclePhaseTODT.Read())
                {
                    DimVehiclePhaseTO dimVehiclePhaseTONew = new DimVehiclePhaseTO();
                    if(dimVehiclePhaseTODT["idVehiclePhase"] != DBNull.Value)
                        dimVehiclePhaseTONew.IdVehiclePhase = Convert.ToInt32( dimVehiclePhaseTODT["idVehiclePhase"].ToString());
                    if(dimVehiclePhaseTODT["sequanceNo"] != DBNull.Value)
                        dimVehiclePhaseTONew.SequanceNo = Convert.ToInt32( dimVehiclePhaseTODT["sequanceNo"].ToString());
                    if(dimVehiclePhaseTODT["phaseName"] != DBNull.Value)
                        dimVehiclePhaseTONew.PhaseName = Convert.ToString( dimVehiclePhaseTODT["phaseName"].ToString());
                    dimVehiclePhaseTOList.Add(dimVehiclePhaseTONew);
                }
            }
            return dimVehiclePhaseTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO)
        {
        String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimVehiclePhaseTO, cmdInsert);
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

        public  int InsertDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimVehiclePhaseTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(DimVehiclePhaseTO dimVehiclePhaseTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimVehiclePhase]( " + 
            //"  [idVehiclePhase]" +
            "  [phaseName]" +
            " )" +
" VALUES (" +
            //"  @IdVehiclePhase " +
            "  @PhaseName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdVehiclePhase", System.Data.SqlDbType.Int).Value = dimVehiclePhaseTO.IdVehiclePhase;
            cmdInsert.Parameters.Add("@PhaseName", System.Data.SqlDbType.VarChar).Value = dimVehiclePhaseTO.PhaseName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO)
        {
        String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimVehiclePhaseTO, cmdUpdate);
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

        public  int UpdateDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimVehiclePhaseTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(DimVehiclePhaseTO dimVehiclePhaseTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimVehiclePhase] SET " + 
            //"  [idVehiclePhase] = @IdVehiclePhase" +
            " ,[phaseName] = @PhaseName" +
            " WHERE idVehiclePhase = @IdVehiclePhase "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehiclePhase", System.Data.SqlDbType.Int).Value = dimVehiclePhaseTO.IdVehiclePhase;
            cmdUpdate.Parameters.Add("@PhaseName", System.Data.SqlDbType.VarChar).Value = dimVehiclePhaseTO.PhaseName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteDimVehiclePhase(Int32 idVehiclePhase)
        {
       String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVehiclePhase, cmdDelete);
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

        public  int DeleteDimVehiclePhase(Int32 idVehiclePhase, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVehiclePhase, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idVehiclePhase, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimVehiclePhase] " +
            " WHERE idVehiclePhase = " + idVehiclePhase +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVehiclePhase", System.Data.SqlDbType.Int).Value = dimVehiclePhaseTO.IdVehiclePhase;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
