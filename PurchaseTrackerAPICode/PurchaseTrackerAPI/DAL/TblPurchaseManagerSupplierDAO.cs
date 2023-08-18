using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseManagerSupplierDAO : ITblPurchaseManagerSupplierDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseManagerSupplierDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQueryToGetSupplier()
        {
            //to get list of all supplier with assigned purchase manager
            String sqlSelectQry = "select * from tblOrganization " +
                                  " WHERE orgTypeId = " + (int)Constants.OrgTypeE.SUPPLIER + " and tblOrganization.isActive =1 ";
            //"(Select idOrgType From dimOrgType Where OrgType = 'Supplier')";

            //" SELECT PurchaseManagerSupplier.*,firmName, orgTypeId, Organization.idOrganization as SupplierId " + // Organization.* " +
            //              "FROM [tblOrganization] Organization " +
            //              "LEFT JOIN [tblPurchaseManagerSupplier] PurchaseManagerSupplier " +
            //              "ON PurchaseManagerSupplier.organizationId=Organization.idOrganization " +
            //              "WHERE Organization.orgTypeId=(Select idOrgType From dimOrgType Where OrgType='Supplier')";
            return sqlSelectQry;
        }

        public  String SqlSelectQueryTogetPMSupplier()
        {
            String sqlSelectQry = "select * from tblPurchaseManagerSupplier PurchaseManagerSupplier";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        /// <summary>
        /// to get PurchaseManager List
        /// </summary>
        /// <returns></returns>
        public  List<DropDownTO> SelectPurchaseFromRoleForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                return SelectPurchaseFromRoleForDropDown(conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        public  List<DropDownTO> SelectPurchaseFromRoleForDropDown(SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                //String aqlQuery = " SELECT idUser,userDisplayName FROM tblUser " +
                //                  " INNER JOIN tblUserRole ON userId = idUser AND tblUserRole.isActive=1" +
                //                  " INNER JOIN tblRole ON tblRole.idRole = tblUserRole.roleId" +
                //                  " WHERE tblUser.isActive=1  AND tblUserRole.roleId IN( " +
                //                  " select configParamVal from tblConfigParams where configParamName = '" + Constants.CP_PURCHASE_MANAGER_ROLE_ID + "')  ";


                String aqlQuery = " SELECT idUser,userDisplayName FROM tblUser " +
                                  " INNER JOIN tblUserRole ON userId = idUser AND tblUserRole.isActive=1" +
                                  " INNER JOIN tblRole ON tblRole.idRole = tblUserRole.roleId" +
                                  " WHERE tblUser.isActive=1  AND tblRole.roleTypeId = " + Convert.ToInt32(Constants.SystemRoleTypeE.PURCHASE_MANAGER);

                cmdSelect = new SqlCommand(aqlQuery, conn, tran);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idUser"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                cmdSelect.Dispose();
            }

        }


        /// <summary>
        /// to get PurchaseManager List
        /// </summary>
        /// <returns></returns>
        public  List<DropDownTO> GetSupplierByPMDropDownList(string userId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery;
                if(userId == "0")
                {
                    userId = string.Empty;
                }
                if (!String.IsNullOrEmpty(userId))
                {
                    aqlQuery = SqlSelectQueryTogetPMSupplier() + " left join tblOrganization Organization on Organization.idOrganization = PurchaseManagerSupplier.organizationId" +
                                                                " where PurchaseManagerSupplier.isActive=1 AND PurchaseManagerSupplier.userId IN (" + userId + ")" +
                                                                " and Organization.isActive=1 AND Organization.orgTypeId=" + Convert.ToInt32(Constants.OrgTypeE.SUPPLIER);
                }
                else
                {
                    aqlQuery = "select idOrganization as organizationId ,* from tblOrganization where isActive=1 AND orgTypeId=" + Convert.ToInt32(Constants.OrgTypeE.SUPPLIER);
                    ////aqlQuery = "select idOrganization as organizationId,* from tblOrganization " +
                    ////    //"left join tblPurchaseManagerSupplier PurchaseManagerSupplier on PurchaseManagerSupplier.organizationId =  tblOrganization.idOrganization "+
                    ////    "where tblOrganization.isActive=1 AND orgTypeId=" + Convert.ToInt32(Constants.OrgTypeE.SUPPLIER);

                    //aqlQuery = "select idOrganization as organizationId ,PurchaseManagerSupplier.userId as userId,* from tblOrganization " +
                    //    "left join tblPurchaseManagerSupplier PurchaseManagerSupplier on PurchaseManagerSupplier.organizationId =  tblOrganization.idOrganization "+
                    //    "where tblOrganization.isActive=1 AND PurchaseManagerSupplier.isActive=1 AND orgTypeId=" + Convert.ToInt32(Constants.OrgTypeE.SUPPLIER);
                }
                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["organizationId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["organizationId"].ToString());
                    if (dateReader["firmName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["firmName"].ToString());
                    //if (dateReader["userId"] != DBNull.Value)
                    //    dropDownTONew.Tag = Convert.ToInt32(dateReader["userId"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }
        public  List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery;

                aqlQuery = " select tblPurchaseManagerSupplier .*,tblUser.userDisplayName from tblPurchaseManagerSupplier tblPurchaseManagerSupplier " +
                           " INNER JOIN tblUser tblUser on tblUser.idUser=tblPurchaseManagerSupplier.userId " +
                           " where tblPurchaseManagerSupplier.isActive = 1 AND tblPurchaseManagerSupplier.organizationId=" + supplierId;

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["userId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["userId"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {

                String aqlQuery;

                aqlQuery = " select tblPurchaseManagerSupplier .*,tblUser.userDisplayName from tblPurchaseManagerSupplier tblPurchaseManagerSupplier " +
                           " INNER JOIN tblUser tblUser on tblUser.idUser=tblPurchaseManagerSupplier.userId " +
                           "  where tblPurchaseManagerSupplier.isActive = 1 AND tblPurchaseManagerSupplier.organizationId=" + supplierId;


                cmdSelect = new SqlCommand(aqlQuery, conn, tran);


                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["userId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["userId"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                cmdSelect.Dispose();
            }

        }
        /// <summary>
        /// swati pisal [2018-02-07] Get list of supplier
        /// </summary>
        /// <returns></returns>
        public  List<TblPurchaseManagerSupplierTO> SelectAllPurchaseManagerSupplier() //(Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryToGetSupplier();// + " WHERE PurchaseManagerSupplier.isActive=1";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseManagerSupplierTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Get list of assigned supplier
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  Dictionary<int, int> SelectPurchaseManagerWithSupplierDCT(Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblPurchaseManagerSupplierTODT = null;
            Dictionary<int, int> pmSupplierDCT = new Dictionary<int, int>();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryTogetPMSupplier() + " where PurchaseManagerSupplier.isActive=1 AND PurchaseManagerSupplier.userId =" + userId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblPurchaseManagerSupplierTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblPurchaseManagerSupplierTODT.Read())
                {
                    int organizationId = 0;
                    if (tblPurchaseManagerSupplierTODT["organizationId"] != DBNull.Value)
                        organizationId = Convert.ToInt32(tblPurchaseManagerSupplierTODT["organizationId"].ToString());
                    if (tblPurchaseManagerSupplierTODT["userId"] != DBNull.Value)
                        userId = Convert.ToInt32(tblPurchaseManagerSupplierTODT["userId"].ToString());

                    if (userId > 0)// && !string.IsNullOrEmpty(permission))
                        pmSupplierDCT.Add(organizationId, userId);
                }

                return pmSupplierDCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblPurchaseManagerSupplierTODT != null)
                    tblPurchaseManagerSupplierTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Get supplier state id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  Int32 GetSupplierStateId(Int32 supplierID)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblPurchaseManagerSupplierTODT = null;
            int stateId = 0;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select stateId	from[tblOrgAddress] left join tblAddress ON tblAddress.idAddr = tblOrgAddress.addressId" +
                                        " where[tblOrgAddress].organizationId = " + supplierID;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblPurchaseManagerSupplierTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblPurchaseManagerSupplierTODT.Read())
                {

                    if (tblPurchaseManagerSupplierTODT["stateId"] != DBNull.Value)
                        stateId = Convert.ToInt32(tblPurchaseManagerSupplierTODT["stateId"].ToString());
                }

                return stateId;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (tblPurchaseManagerSupplierTODT != null)
                    tblPurchaseManagerSupplierTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblPurchaseManagerSupplierTO> ConvertDTToList(SqlDataReader tblPurchaseManagerSupplierTODT)
        {
            List<TblPurchaseManagerSupplierTO> tblPurchaseManagerSupplierTOList = new List<TblPurchaseManagerSupplierTO>();
            if (tblPurchaseManagerSupplierTODT != null)
            {
                while (tblPurchaseManagerSupplierTODT.Read())
                {
                    TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTONew = new TblPurchaseManagerSupplierTO();
                    if (tblPurchaseManagerSupplierTODT["idOrganization"] != DBNull.Value)
                        tblPurchaseManagerSupplierTONew.OrganizationId = Convert.ToInt32(tblPurchaseManagerSupplierTODT["idOrganization"].ToString());
                    if (tblPurchaseManagerSupplierTODT["firmName"] != DBNull.Value)
                        tblPurchaseManagerSupplierTONew.SupplierName = tblPurchaseManagerSupplierTODT["firmName"].ToString();
                    tblPurchaseManagerSupplierTOList.Add(tblPurchaseManagerSupplierTONew);
                }
            }
            return tblPurchaseManagerSupplierTOList;
        }

        #endregion

        #region Insertion
        public  int InsertUpdateTblPurchaseManagerSupplier(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader dateReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.Connection = conn;
                cmdSelect.CommandText = SqlSelectQueryTogetPMSupplier() + " where PurchaseManagerSupplier.organizationId=" + tblPurchaseManagerSupplierTO.OrganizationId + "" +
                                        " AND PurchaseManagerSupplier.userId =" + tblPurchaseManagerSupplierTO.UserId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (dateReader.HasRows)
                {
                    dateReader.Dispose();
                    return ExecuteUpdationCommand(tblPurchaseManagerSupplierTO, cmdSelect);
                }
                else
                {
                    dateReader.Dispose();
                    return ExecuteInsertionCommand(tblPurchaseManagerSupplierTO, cmdSelect);
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseManagerSupplier]( " +
                            "  [userId]" +
                            " ,[organizationId]" +
                            " ,[isActive]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " )" +
                " VALUES (" +
                            "  @UserId " +
                            " ,@OrganizationId " +
                            " ,@IsActive " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurchaseManagerSupplier", System.Data.SqlDbType.Int).Value = tblPurchaseManagerSupplierTO.IdPurchaseManagerSupplier;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblPurchaseManagerSupplierTO.UserId;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblPurchaseManagerSupplierTO.OrganizationId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseManagerSupplierTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseManagerSupplierTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseManagerSupplierTO.CreatedOn;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblPurchaseManagerSupplierTO.IdPurchaseManagerSupplier = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation

        public  int ExecuteUpdationCommand(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO, SqlCommand cmdUpdate) //(String organizationIds, Int32 userId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseManagerSupplier] SET " +
                            "  [isActive]= @IsActive" +
                            " WHERE [UserId] = @UserId AND organizationId = @OrganizationId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblPurchaseManagerSupplierTO.UserId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseManagerSupplierTO.IsActive;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.NVarChar, 50).Value = tblPurchaseManagerSupplierTO.OrganizationId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

    }
}
