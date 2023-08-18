using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseVehicleMaterialSampleDAO : ITblPurchaseVehicleMaterialSampleDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseVehicleMaterialSampleDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  List<TblPurchaseMaterialSampleTypeTo> ConvertSampleTyoeList(SqlDataReader dimVehicleTypeTODT)
        {
            List<TblPurchaseMaterialSampleTypeTo> dimSampleTypeTOList = new List<TblPurchaseMaterialSampleTypeTo>();
            if (dimVehicleTypeTODT != null)
            {
                while (dimVehicleTypeTODT.Read())
                {
                    TblPurchaseMaterialSampleTypeTo dimVehicleTypeTONew = new TblPurchaseMaterialSampleTypeTo();
                    if (dimVehicleTypeTODT["idPurchaseMaterialSampleType"] != DBNull.Value)
                        dimVehicleTypeTONew.IdPurchaseMaterialSampleType = Convert.ToInt32(dimVehicleTypeTODT["idPurchaseMaterialSampleType"].ToString());
                    if (dimVehicleTypeTODT["typeName"] != DBNull.Value)
                        dimVehicleTypeTONew.TypeName = Convert.ToString(dimVehicleTypeTODT["typeName"].ToString());
                    dimSampleTypeTOList.Add(dimVehicleTypeTONew);
                }
            }
            return dimSampleTypeTOList;
        }
        public  List<TblPurchaseMaterialSampleTO> ConvertSampleMaterialList(SqlDataReader dimVehicleTypeTODT)
        {
            List<TblPurchaseMaterialSampleTO> dimSampleTypeTOList = new List<TblPurchaseMaterialSampleTO>();
            if (dimVehicleTypeTODT != null)
            {
                while (dimVehicleTypeTODT.Read())
                {
                    TblPurchaseMaterialSampleTO dimVehicleTypeTONew = new TblPurchaseMaterialSampleTO();

                    if (dimVehicleTypeTODT["idPurchaseMaterialSample"] != DBNull.Value)
                        dimVehicleTypeTONew.IdPurchaseMaterialSample = Convert.ToInt32(dimVehicleTypeTODT["idPurchaseMaterialSample"].ToString());

                    if (dimVehicleTypeTODT["userId"] != DBNull.Value)
                        dimVehicleTypeTONew.UserId = Convert.ToInt32(dimVehicleTypeTODT["userId"].ToString());

                    if (dimVehicleTypeTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        dimVehicleTypeTONew.PurchaseScheduleSummaryId = Convert.ToInt32(dimVehicleTypeTODT["purchaseScheduleSummaryId"].ToString());

                    if (dimVehicleTypeTODT["phaseId"] != DBNull.Value)
                        dimVehicleTypeTONew.PhaseId= Convert.ToInt32(dimVehicleTypeTODT["phaseId"].ToString());

                    if (dimVehicleTypeTODT["PurchaseMaterialSampleTypeId"] != DBNull.Value)
                        dimVehicleTypeTONew.PurchaseMaterialSampleTypeId = Convert.ToInt32(dimVehicleTypeTODT["PurchaseMaterialSampleTypeId"].ToString());

                    if (dimVehicleTypeTODT["PurchaseMaterialSampleCategoryId"] != DBNull.Value)
                        dimVehicleTypeTONew.PurchaseMaterialSampleCategoryId = Convert.ToInt32(dimVehicleTypeTODT["PurchaseMaterialSampleCategoryId"].ToString());
                    if (dimVehicleTypeTODT["vehicleNo"] != DBNull.Value)
                        dimVehicleTypeTONew.VehicleNo = (dimVehicleTypeTODT["vehicleNo"].ToString());

                    if (dimVehicleTypeTODT["comments"] != DBNull.Value)
                        dimVehicleTypeTONew.Comments = (dimVehicleTypeTODT["comments"].ToString());

                    if (dimVehicleTypeTODT["isDone"] != DBNull.Value)
                        dimVehicleTypeTONew.IsDone = Convert.ToBoolean(dimVehicleTypeTODT["isDone"].ToString());
                    if (dimVehicleTypeTODT["TestPhysical"] != DBNull.Value)
                        dimVehicleTypeTONew.TestPhysical = Convert.ToBoolean(dimVehicleTypeTODT["TestPhysical"].ToString());

                    if (dimVehicleTypeTODT["Test5KG"] != DBNull.Value)
                        dimVehicleTypeTONew.Test5KG = Convert.ToBoolean(dimVehicleTypeTODT["Test5KG"].ToString());

                    if (dimVehicleTypeTODT["TestSpectro"] != DBNull.Value)
                        dimVehicleTypeTONew.TestSpectro = Convert.ToBoolean(dimVehicleTypeTODT["TestSpectro"].ToString());
                    

                    dimSampleTypeTOList.Add(dimVehicleTypeTONew);
                }
            }
            return dimSampleTypeTOList;
        }
        public  List<TblPurchaseMaterialSampleCategoryTo> ConvertSampleCategoryList(SqlDataReader dimVehicleTypeTODT)
        {
            List<TblPurchaseMaterialSampleCategoryTo> dimSampleTypeTOList = new List<TblPurchaseMaterialSampleCategoryTo>();
            if (dimVehicleTypeTODT != null)
            {
                while (dimVehicleTypeTODT.Read())
                {
                    TblPurchaseMaterialSampleCategoryTo dimVehicleTypeTONew = new TblPurchaseMaterialSampleCategoryTo();
                    if (dimVehicleTypeTODT["idPurchaseMaterialSampleCategory"] != DBNull.Value)
                        dimVehicleTypeTONew.IdPurchaseMaterialSampleCategory = Convert.ToInt32(dimVehicleTypeTODT["idPurchaseMaterialSampleCategory"].ToString());
                    if (dimVehicleTypeTODT["categoryName"] != DBNull.Value)
                        dimVehicleTypeTONew.CategoryName = Convert.ToString(dimVehicleTypeTODT["categoryName"].ToString());
                    dimSampleTypeTOList.Add(dimVehicleTypeTONew);
                }
            }
            return dimSampleTypeTOList;
        }
        public  List<TblPurchaseMaterialSampleTypeTo> SqlSelectSampleTypeQuery()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " select * from tblPurchaseMaterialSampleType ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseMaterialSampleTypeTo> list = ConvertSampleTyoeList(rdr);
                rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public  TblPurchaseMaterialSampleTO getTblPurchaseMaterialSample(int purchaseScheduleSummaryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " select * from tblPurchaseMaterialSample where purchaseScheduleSummaryId =" + purchaseScheduleSummaryId.ToString();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseMaterialSampleTO> list = ConvertSampleMaterialList(rdr);
                rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public  List<TblPurchaseMaterialSampleCategoryTo> SqlSelectSampleCategoryQuery()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " select * from tblPurchaseMaterialSampleCategory ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseMaterialSampleCategoryTo> list = ConvertSampleCategoryList(rdr);
                rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  String SqlSelectSampleQuery(int purchaseScheduleSummaryId)
        {
            String sqlSelectQry = " select s.*,c.categoryName,t.typeName from tblPurchaseMaterialSample s " +
            " inner join   tblPurchaseMaterialSampleCategory c on s.PurchaseMaterialSampleTypeId = c.idPurchaseMaterialSampleCategory " +
           
            " where s.purchaseScheduleSummaryId = " + Convert.ToString(purchaseScheduleSummaryId);
            return sqlSelectQry;
        }
        #endregion

        #region Selection



        #endregion

        #region Insertion
        public  int InserttblPurchaseMaterialSample(TblPurchaseMaterialSampleTO tblPurchaseMaterialSampleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseMaterialSampleTO, cmdInsert);
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

        //public static int InsertTblPurchaseVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdInsert = new SqlCommand();
        //    try
        //    {
        //        cmdInsert.Connection = conn;
        //        cmdInsert.Transaction = tran;
        //        return ExecuteInsertionCommand(TblPurchaseMaterialSampleTO, cmdInsert);
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        cmdInsert.Dispose();
        //    }
        //}

        public  int ExecuteInsertionCommand(TblPurchaseMaterialSampleTO tblPurchaseMaterialSampleTO, SqlCommand cmdInsert)
        {
            String sqlQuery = string.Empty;
            if (tblPurchaseMaterialSampleTO.IdPurchaseMaterialSample == 0)
            {
                sqlQuery = @" INSERT INTO [tblPurchaseMaterialSample]( " +

              "  [userId]" +
              " ,[purchaseScheduleSummaryId]" +
              " ,[phaseId]" +
              " ,[PurchaseMaterialSampleTypeId]" +
              " ,[PurchaseMaterialSampleCategoryId]" +
              " ,[vehicleNo]" +
          //    " ,[comments]" +
              " ,[createdBy]" +
              " ,[createdOn]" +
              ",[isDone]" +
              ",[TestTypeGalvanize]" +
              ",[TestTypeRustAndRust]" +
              ",[TestPhysical]" +
              ",[Test5KG]" +
              ",[TestSpectro]" +
              " )" +
              " VALUES (" +
              "  @userId " +
              " ,@purchaseScheduleSummaryId " +
              " ,@phaseId " +
              " ,@PurchaseMaterialSampleTypeId " +
              " ,@PurchaseMaterialSampleCategoryId " +
              " ,@vehicleNo " +
          //    " ,@comments " +
              " ,@createdBy " +
              " ,@createdOn " +
              " ,@isDone " +
              ",@TestTypeGalvanize" +
              ",@TestTypeRustAndRust" +
              ",@TestPhysical" +
              ",@Test5KG" +
              ",@TestSpectro" +
              " )";
            }
            else if (tblPurchaseMaterialSampleTO.IdPurchaseMaterialSample > 0)
            {
                sqlQuery = @" UPDATE [tblPurchaseMaterialSample] SET " +
                     "  [userId] = @userId" +
              " ,[purchaseScheduleSummaryId] = @purchaseScheduleSummaryId" +
              " ,[phaseId] = @phaseId" +
              " ,[PurchaseMaterialSampleTypeId] = @PurchaseMaterialSampleTypeId" +
              " ,[PurchaseMaterialSampleCategoryId] = @PurchaseMaterialSampleCategoryId" +
              " ,[vehicleNo] = @vehicleNo " +
            //  " ,[comments] = @comments" +
              " ,[updatedBy] = @createdBy" +
              " ,[updatedOn] = @createdOn" +
              ",[isDone] =@isDone" +
              ",[TestTypeGalvanize] =@TestTypeGalvanize" +
              ",[TestTypeRustAndRust] =@TestTypeRustAndRust" +
              ",[TestPhysical] =@TestPhysical" +
              ",[Test5KG] =@Test5KG" +
              ",[TestSpectro] = @TestSpectro" +
               " WHERE idPurchaseMaterialSample = @IdPurchaseScheduleSummary ";
                cmdInsert.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = (tblPurchaseMaterialSampleTO.IdPurchaseMaterialSample);

            }
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.UserId);
            cmdInsert.Parameters.Add("@purchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.PurchaseScheduleSummaryId);
            cmdInsert.Parameters.Add("@phaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.PhaseId);
            cmdInsert.Parameters.Add("@PurchaseMaterialSampleTypeId", System.Data.SqlDbType.Int).Value = 0;
            cmdInsert.Parameters.Add("@PurchaseMaterialSampleCategoryId", System.Data.SqlDbType.Int).Value = 0;
            cmdInsert.Parameters.Add("@vehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.VehicleNo);
           // cmdInsert.Parameters.Add("@comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.Comments== null ? "": tblPurchaseMaterialSampleTO.Comments);

            cmdInsert.Parameters.Add("@createdBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.CreatedBy);
            cmdInsert.Parameters.Add("@createdOn", System.Data.SqlDbType.DateTime).Value = (tblPurchaseMaterialSampleTO.CreatedOn);
            cmdInsert.Parameters.Add("@isDone", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.IsDone);

            cmdInsert.Parameters.Add("@TestTypeGalvanize", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.TestTypeGalvanize);
            cmdInsert.Parameters.Add("@TestTypeRustAndRust", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.TestTypeRustAndRust);
            cmdInsert.Parameters.Add("@TestPhysical", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.TestPhysical);
            cmdInsert.Parameters.Add("@Test5KG", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.Test5KG);
            cmdInsert.Parameters.Add("@TestSpectro", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseMaterialSampleTO.TestSpectro);
           





            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                if (tblPurchaseMaterialSampleTO.IdPurchaseMaterialSample == 0)
                {
                    cmdInsert.CommandText = Constants.IdentityColumnQuery;
                    tblPurchaseMaterialSampleTO.IdPurchaseMaterialSample = Convert.ToInt32(cmdInsert.ExecuteScalar());
                }
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation

        #endregion

        #region Deletion

        #endregion



    }
}
