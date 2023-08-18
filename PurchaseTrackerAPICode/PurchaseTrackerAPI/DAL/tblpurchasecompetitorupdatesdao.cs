using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Data;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.DAL
{

    public class tblpurchasecompetitorupdatesdao : Itblpurchasecompetitorupdatesdao
    {
        private readonly IConnectionString _iConnectionString;
        public tblpurchasecompetitorupdatesdao(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods

        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT org.firmName as competitorName,dealerOrg.firmName AS dealerName, userDtl.userDisplayName , compUpdate.* , ISNULL(LAG(price) OVER (PARTITION BY competitorOrgId ORDER BY updateDatetime),0) as lastPrice  " +
                                  " ,otherSource.otherDesc ,  ext.materialGrade as brandName,0 as prodCapacityMT" +
                                  " FROM tblPurchaseCompetitorUpdates compUpdate " +
                                  " LEFT JOIN tblPurchaseCompetitorExt ext ON ext.IdPurcompetitorExt=compUpdate.IdPurcompetitorExt" +
                                  " LEFT JOIN tblOrganization org " +
                                  " ON org.idOrganization = compUpdate.competitorOrgId" +
                                  " LEFT JOIN tblUser userDtl ON userDtl.idUser=compUpdate.createdBy" +
                                  " LEFT JOIN tblOrganization dealerOrg " +
                                  " ON dealerOrg.idOrganization = compUpdate.dealerId" +
                                  " LEFT JOIN tblOtherSource otherSource " +
                                  " ON otherSource.idOtherSource = compUpdate.otherSourceId";

            return sqlSelectQry;
        }

        #endregion

        #region Selection

        public  List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = " SELECT idPurCompetitorExt,materialType FROM tblPurchaseCompetitorExt " +
                                  " WHERE organizationId=" + competitorOrgId;

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idPurCompetitorExt"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idPurCompetitorExt"].ToString());
                    if (dateReader["materialType"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["materialType"].ToString());

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
        
        public  List<DropDownTO> SelectCompetitorMaterialGradeDropDownList(Int32 materialid,Int32 competitorOrgId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = " SELECT idPurCompetitorExt,materialGrade FROM tblPurchaseCompetitorExt " +
                                  " WHERE idPurCompetitorExt=" + materialid + "AND organizationId=" + competitorOrgId;

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idPurCompetitorExt"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idPurCompetitorExt"].ToString());
                    if (dateReader["materialGrade"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["materialGrade"].ToString());

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


        public  TblPurchaseCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();

                String aqlQuery = SqlSelectQuery() + " WHERE idCompeUpdate IN(SELECT TOP 1 idCompeUpdate FROM tblPurchaseCompetitorUpdates WHERE IdPurcompetitorExt=" + brandId + "ORDER BY updateDatetime DESC ) ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseCompetitorUpdatesTO> list = ConvertDTToList(rdr);
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


        public  List<TblPurchaseCompetitorUpdatesTO> ConvertDTToList(SqlDataReader tblCompetitorUpdatesTODT)
        {
            List<TblPurchaseCompetitorUpdatesTO> tblCompetitorUpdatesTOList = new List<TblPurchaseCompetitorUpdatesTO>();
            if (tblCompetitorUpdatesTODT != null)
            {
                while (tblCompetitorUpdatesTODT.Read())
                {
                    TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTONew = new TblPurchaseCompetitorUpdatesTO();
                    if (tblCompetitorUpdatesTODT["idCompeUpdate"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.IdCompeUpdate = Convert.ToInt32(tblCompetitorUpdatesTODT["idCompeUpdate"].ToString());
                    if (tblCompetitorUpdatesTODT["IdPurcompetitorExt"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CompetitorExtId = Convert.ToInt32(tblCompetitorUpdatesTODT["IdPurcompetitorExt"].ToString());
                    if (tblCompetitorUpdatesTODT["competitorOrgId"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CompetitorOrgId = Convert.ToInt32(tblCompetitorUpdatesTODT["competitorOrgId"].ToString());
                    if (tblCompetitorUpdatesTODT["createdBy"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CreatedBy = Convert.ToInt32(tblCompetitorUpdatesTODT["createdBy"].ToString());
                    if (tblCompetitorUpdatesTODT["updateDatetime"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.UpdateDatetime = Convert.ToDateTime(tblCompetitorUpdatesTODT["updateDatetime"].ToString());
                    if (tblCompetitorUpdatesTODT["createdOn"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CreatedOn = Convert.ToDateTime(tblCompetitorUpdatesTODT["createdOn"].ToString());
                    if (tblCompetitorUpdatesTODT["price"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.Price = Convert.ToDouble(tblCompetitorUpdatesTODT["price"].ToString());
                    if (tblCompetitorUpdatesTODT["informerName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.InformerName = Convert.ToString(tblCompetitorUpdatesTODT["informerName"].ToString());
                    if (tblCompetitorUpdatesTODT["alternateInformerName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.AlternateInformerName = Convert.ToString(tblCompetitorUpdatesTODT["alternateInformerName"].ToString());

                    if (tblCompetitorUpdatesTODT["competitorName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.FirmName = Convert.ToString(tblCompetitorUpdatesTODT["competitorName"].ToString());
                    if (tblCompetitorUpdatesTODT["lastPrice"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.LastPrice = Convert.ToDouble(tblCompetitorUpdatesTODT["lastPrice"].ToString());
                    if (tblCompetitorUpdatesTODT["userDisplayName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CreatedByName = Convert.ToString(tblCompetitorUpdatesTODT["userDisplayName"].ToString());

                    if (tblCompetitorUpdatesTODT["dealerId"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.DealerId = Convert.ToInt32(tblCompetitorUpdatesTODT["dealerId"].ToString());
                    if (tblCompetitorUpdatesTODT["dealerName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.DealerName = Convert.ToString(tblCompetitorUpdatesTODT["dealerName"].ToString());

                    if (tblCompetitorUpdatesTODT["otherSourceId"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.OtherSourceId = Convert.ToInt32(tblCompetitorUpdatesTODT["otherSourceId"].ToString());
                    if (tblCompetitorUpdatesTODT["otherDesc"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.OtherSourceDesc = Convert.ToString(tblCompetitorUpdatesTODT["otherDesc"].ToString());
                    if (tblCompetitorUpdatesTODT["otherSourceOtherDesc"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.OtherSourceOtherDesc = Convert.ToString(tblCompetitorUpdatesTODT["otherSourceOtherDesc"].ToString());

                    if (tblCompetitorUpdatesTODT["brandName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.BrandName = Convert.ToString(tblCompetitorUpdatesTODT["brandName"].ToString());
                    if (tblCompetitorUpdatesTODT["prodCapacityMT"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.ProdCapacityMT = Convert.ToDouble(tblCompetitorUpdatesTODT["prodCapacityMT"].ToString());

                    tblCompetitorUpdatesTOList.Add(tblCompetitorUpdatesTONew);
                }
            }
            return tblCompetitorUpdatesTOList;
        }


        /*
        public  List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdates()
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

                SqlDataReader tblCompetitorExtRdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCompetitorUpdatesTO> list = ConvertDTToList(tblCompetitorExtRdr);
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

        public  List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdates(Int32 competitorId, Int32 enteredBy, DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String whereCond = string.Empty;
            try
            {
                if (competitorId == 0 && enteredBy == 0)
                    whereCond = "";
                else if (competitorId > 0 && enteredBy == 0)
                    whereCond = " compUpdate.competitorOrgId=" + competitorId;
                else if (competitorId == 0 && enteredBy > 0)
                    whereCond = " compUpdate.createdBy=" + enteredBy;
                else if (competitorId > 0 && enteredBy > 0)
                    whereCond = " compUpdate.competitorOrgId=" + competitorId + " AND compUpdate.createdBy=" + enteredBy;

                conn.Open();
                if (whereCond == "")
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE  CONVERT (DATE,updateDatetime,102) BETWEEN @fromDate AND @toDate ORDER BY compUpdate.createdOn DESC";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE " + whereCond + " AND  CONVERT (DATE,updateDatetime,102) BETWEEN @fromDate AND @toDate ORDER BY compUpdate.createdOn DESC";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader tblCompetitorExtRdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCompetitorUpdatesTO> list = ConvertDTToList(tblCompetitorExtRdr);
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

        public  TblCompetitorUpdatesTO SelectTblCompetitorUpdates(Int32 idCompeUpdate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idCompeUpdate = " + idCompeUpdate + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCompetitorUpdatesTO> list = ConvertDTToList(rdr);
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

        public  List<TblCompetitorUpdatesTO> ConvertDTToList(SqlDataReader tblCompetitorUpdatesTODT)
        {
            List<TblCompetitorUpdatesTO> tblCompetitorUpdatesTOList = new List<TblCompetitorUpdatesTO>();
            if (tblCompetitorUpdatesTODT != null)
            {
                while (tblCompetitorUpdatesTODT.Read())
                {
                    TblCompetitorUpdatesTO tblCompetitorUpdatesTONew = new TblCompetitorUpdatesTO();
                    if (tblCompetitorUpdatesTODT["idCompeUpdate"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.IdCompeUpdate = Convert.ToInt32(tblCompetitorUpdatesTODT["idCompeUpdate"].ToString());
                    if (tblCompetitorUpdatesTODT["competitorExtId"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CompetitorExtId = Convert.ToInt32(tblCompetitorUpdatesTODT["competitorExtId"].ToString());
                    if (tblCompetitorUpdatesTODT["competitorOrgId"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CompetitorOrgId = Convert.ToInt32(tblCompetitorUpdatesTODT["competitorOrgId"].ToString());
                    if (tblCompetitorUpdatesTODT["createdBy"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CreatedBy = Convert.ToInt32(tblCompetitorUpdatesTODT["createdBy"].ToString());
                    if (tblCompetitorUpdatesTODT["updateDatetime"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.UpdateDatetime = Convert.ToDateTime(tblCompetitorUpdatesTODT["updateDatetime"].ToString());
                    if (tblCompetitorUpdatesTODT["createdOn"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CreatedOn = Convert.ToDateTime(tblCompetitorUpdatesTODT["createdOn"].ToString());
                    if (tblCompetitorUpdatesTODT["price"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.Price = Convert.ToDouble(tblCompetitorUpdatesTODT["price"].ToString());
                    if (tblCompetitorUpdatesTODT["informerName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.InformerName = Convert.ToString(tblCompetitorUpdatesTODT["informerName"].ToString());
                    if (tblCompetitorUpdatesTODT["alternateInformerName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.AlternateInformerName = Convert.ToString(tblCompetitorUpdatesTODT["alternateInformerName"].ToString());

                    if (tblCompetitorUpdatesTODT["competitorName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.FirmName = Convert.ToString(tblCompetitorUpdatesTODT["competitorName"].ToString());
                    if (tblCompetitorUpdatesTODT["lastPrice"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.LastPrice = Convert.ToDouble(tblCompetitorUpdatesTODT["lastPrice"].ToString());
                    if (tblCompetitorUpdatesTODT["userDisplayName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.CreatedByName = Convert.ToString(tblCompetitorUpdatesTODT["userDisplayName"].ToString());

                    if (tblCompetitorUpdatesTODT["dealerId"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.DealerId = Convert.ToInt32(tblCompetitorUpdatesTODT["dealerId"].ToString());
                    if (tblCompetitorUpdatesTODT["dealerName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.DealerName = Convert.ToString(tblCompetitorUpdatesTODT["dealerName"].ToString());

                    if (tblCompetitorUpdatesTODT["otherSourceId"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.OtherSourceId = Convert.ToInt32(tblCompetitorUpdatesTODT["otherSourceId"].ToString());
                    if (tblCompetitorUpdatesTODT["otherDesc"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.OtherSourceDesc = Convert.ToString(tblCompetitorUpdatesTODT["otherDesc"].ToString());
                    if (tblCompetitorUpdatesTODT["otherSourceOtherDesc"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.OtherSourceOtherDesc = Convert.ToString(tblCompetitorUpdatesTODT["otherSourceOtherDesc"].ToString());

                    if (tblCompetitorUpdatesTODT["brandName"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.BrandName = Convert.ToString(tblCompetitorUpdatesTODT["brandName"].ToString());
                    if (tblCompetitorUpdatesTODT["prodCapacityMT"] != DBNull.Value)
                        tblCompetitorUpdatesTONew.ProdCapacityMT = Convert.ToDouble(tblCompetitorUpdatesTODT["prodCapacityMT"].ToString());

                    tblCompetitorUpdatesTOList.Add(tblCompetitorUpdatesTONew);
                }
            }
            return tblCompetitorUpdatesTOList;
        }

        public  List<DropDownTO> SelectCompeUpdateUserDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = " SELECT distinct createdBy,userDisplayName FROM tblCompetitorUpdates " +
                                  " INNER JOIN tblUser ON idUser = createdBy ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["createdBy"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["createdBy"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

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

        public  TblCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();

                String aqlQuery = SqlSelectQuery() + " WHERE idCompeUpdate IN(SELECT TOP 1 idCompeUpdate FROM tblCompetitorUpdates WHERE competitorExtId=" + brandId + "ORDER BY updateDatetime DESC ) ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCompetitorUpdatesTO> list = ConvertDTToList(rdr);
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
        */
        #endregion

        #region Insertion

        public  int InsertTblCompetitorUpdates(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblCompetitorUpdatesTO, cmdInsert);
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

        public  int InsertTblCompetitorUpdates(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblCompetitorUpdatesTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlCommand cmdInsert)
        {
            //String sqlQuery =
            //                @" INSERT INTO [tblPurchaseCompetitorExt]( " +
            //                "  [organizationId]" +
            //                " ,[materialType]" +
            //                " ,[materialGrade]" +" )"
            //                +
            //                " VALUES (" +
            //                "  @competitorOrgId " +
            //                " ,@materialType" +
            //                " ,@materialGrade " +" )";


            String sqlQuery = @" INSERT INTO [tblPurchaseCompetitorUpdates]( " +
                         "  [competitorOrgId]" +
                         " ,[createdBy]" +
                         " ,[updateDatetime]" +
                         " ,[createdOn]" +
                         " ,[price]" +
                         " ,[informerName]" +
                         " ,[alternateInformerName]" +
                         " ,[dealerId]" +
                         " ,[otherSourceId]" +
                         " ,[otherSourceOtherDesc]" +
                         " ,[IdPurcompetitorExt]" +

                         " )" +
             " VALUES (" +
                         "  @competitorOrgId " +
                         " ,@CreatedBy " +
                         " ,@UpdateDatetime " +
                         " ,@CreatedOn " +
                         " ,@Price " +
                         " ,@InformerName " +
                         " ,@AlternateInformerName " +
                         " ,@dealerId " +
                         " ,@otherSourceId " +
                         " ,@otherSourceOtherDesc " +
                         " ,@competitorExtId " +
                         " )";
                          //  +
                          //@" INSERT INTO [tblPurchaseCompetitorExt]( " +
                          //  "  [organizationId]" +
                          //  " ,[materialType]" +
                          //  " ,[materialGrade]" + " )"
                          //  +
                          //  " VALUES (" +
                          //  //"  @competitorOrgId " +
                          //  "  1 " +
                          //  " ,@materialType" +
                          //  " ,@materialGrade " + " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            
            cmdInsert.Parameters.Add("@competitorOrgId", System.Data.SqlDbType.Int).Value = tblCompetitorUpdatesTO.CompetitorOrgId;
            //cmdInsert.Parameters.Add("@materialType", System.Data.SqlDbType.NVarChar).Value = tblCompetitorUpdatesTO.MaterialGrade;
            //cmdInsert.Parameters.Add("@materialGrade", System.Data.SqlDbType.NVarChar).Value = tblCompetitorUpdatesTO.MaterialType;

            cmdInsert.Parameters.Add("@IdCompeUpdate", System.Data.SqlDbType.Int).Value = tblCompetitorUpdatesTO.IdCompeUpdate;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblCompetitorUpdatesTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdateDatetime", System.Data.SqlDbType.DateTime).Value = tblCompetitorUpdatesTO.UpdateDatetime;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblCompetitorUpdatesTO.CreatedOn;
            cmdInsert.Parameters.Add("@Price", System.Data.SqlDbType.NVarChar).Value = tblCompetitorUpdatesTO.Price;
            cmdInsert.Parameters.Add("@InformerName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCompetitorUpdatesTO.InformerName);
            cmdInsert.Parameters.Add("@AlternateInformerName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCompetitorUpdatesTO.AlternateInformerName);
            cmdInsert.Parameters.Add("@dealerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCompetitorUpdatesTO.DealerId);
            cmdInsert.Parameters.Add("@otherSourceId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCompetitorUpdatesTO.OtherSourceId);
            cmdInsert.Parameters.Add("@otherSourceOtherDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCompetitorUpdatesTO.OtherSourceOtherDesc);
            cmdInsert.Parameters.Add("@competitorExtId", System.Data.SqlDbType.Int).Value = tblCompetitorUpdatesTO.CompetitorExtId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblCompetitorUpdatesTO.IdCompeUpdate = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        #endregion

        #region Updation
        /*
        public  int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblCompetitorUpdatesTO, cmdUpdate);
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

        public  int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblCompetitorUpdatesTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblCompetitorUpdates] SET " +
            "  [idCompeUpdate] = @IdCompeUpdate" +
            " ,[competitorExtId]= @CompetitorExtId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updateDatetime]= @UpdateDatetime" +
            " ,[createdOn]= @CreatedOn" +
            " ,[price]= @Price" +
            " ,[informerName]= @InformerName" +
            " ,[alternateInformerName] = @AlternateInformerName" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdCompeUpdate", System.Data.SqlDbType.Int).Value = tblCompetitorUpdatesTO.IdCompeUpdate;
            cmdUpdate.Parameters.Add("@CompetitorExtId", System.Data.SqlDbType.Int).Value = tblCompetitorUpdatesTO.CompetitorExtId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblCompetitorUpdatesTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdateDatetime", System.Data.SqlDbType.DateTime).Value = tblCompetitorUpdatesTO.UpdateDatetime;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblCompetitorUpdatesTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Price", System.Data.SqlDbType.NVarChar).Value = tblCompetitorUpdatesTO.Price;
            cmdUpdate.Parameters.Add("@InformerName", System.Data.SqlDbType.NVarChar).Value = tblCompetitorUpdatesTO.InformerName;
            cmdUpdate.Parameters.Add("@AlternateInformerName", System.Data.SqlDbType.NVarChar).Value = tblCompetitorUpdatesTO.AlternateInformerName;
            return cmdUpdate.ExecuteNonQuery();
        }
        */
        #endregion

        #region Deletion
        /*
        public  int DeleteTblCompetitorUpdates(Int32 idCompeUpdate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idCompeUpdate, cmdDelete);
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

        public  int DeleteTblCompetitorUpdates(Int32 idCompeUpdate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idCompeUpdate, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idCompeUpdate, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblCompetitorUpdates] " +
            " WHERE idCompeUpdate = " + idCompeUpdate + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idCompeUpdate", System.Data.SqlDbType.Int).Value = tblCompetitorUpdatesTO.IdCompeUpdate;
            return cmdDelete.ExecuteNonQuery();
        }
        */
        #endregion

    }
}
