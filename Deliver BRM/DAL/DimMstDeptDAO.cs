using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class DimMstDeptDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimMstDept]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DimMstDeptTO> SelectAllDimMstDept()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                sqlQuery = SqlSelectQuery() + " WHERE  isVisible = 1";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dimMstDeptTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimMstDeptTO> list = ConvertDTToList(dimMstDeptTODT);

                return list;
            }

            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimMstDept");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


 public static List<DimMstDeptTO> SelectAllDimMstDept(SqlConnection conn,SqlTransaction tran)
        {
        
             SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            cmdSelect.Connection = conn;
            cmdSelect.Transaction = tran;
            ResultMessage resultMessage = new ResultMessage();

            try
            {
               
                cmdSelect.CommandText =  SqlSelectQuery() + " WHERE  isVisible = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

               reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimMstDeptTO> list = ConvertDTToList(reader);

                return list;
            }

            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimMstDept");
                return null;
            }
            finally
            {
               
                reader.Close();
                cmdSelect.Dispose();
            }
        }

        public static DimMstDeptTO SelectDimMstDept(Int32 idDept)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idDept = " + idDept + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimMstDeptTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectDimMstDept");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DimMstDeptTO SelectDimMstDept(Int32 idDept,SqlConnection conn,SqlTransaction tran)
        {
           
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            cmdSelect.Connection = conn;
            cmdSelect.Transaction = tran;
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idDept = " + idDept + " ";              
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimMstDeptTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectDimMstDept");
                return null;
            }
            finally
            {
                reader.Close();
                cmdSelect.Dispose();
            }
        }


        // Vaibhav [15-Sep-2017] added to fill department masters drop down.
        public static List<DropDownTO> SelectAllDepartmentMasterForDropDown(Int32 DeptTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                if (DeptTypeId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isVisible = 1 ";
                else if (DeptTypeId == (int)Constants.DepartmentTypeE.DEPARTMENT)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isVisible = 1 And deptTypeId= " + (int)Constants.DepartmentTypeE.DIVISION;
                else if (DeptTypeId == (int)Constants.DepartmentTypeE.SUB_DEPARTMENT)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isVisible = 1 And deptTypeId= " + (int)Constants.DepartmentTypeE.DEPARTMENT;
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isVisible = 1 And deptTypeId= " + DeptTypeId;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (departmentTODT["idDept"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idDept"].ToString());
                        if (departmentTODT["deptDisplayName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["deptDisplayName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDepartmentForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Vaibhav [19-Sep-2017] added to fill department drop down.
        public static List<DropDownTO> SelectDepartmentDropDownListByDivision(Int32 DivisionId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                if (DivisionId != 0)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isVisible = 1 AND parentDeptId = " + DivisionId +
                                            " AND deptTypeId = "+ (int)Constants.DepartmentTypeE.DEPARTMENT;
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (departmentTODT["idDept"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idDept"].ToString());
                        if (departmentTODT["deptDisplayName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["deptDisplayName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectDepartmentDropDownListByDivision");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Vaibhav [19-Sep-2017] added to get BOM department TO
        public static DropDownTO SelectBOMDepartmentTO()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isVisible = 1 AND deptTypeId = " + (int)Constants.DepartmentTypeE.BOM;

                conn.Open();
                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTO = new DropDownTO();
                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        if (departmentTODT["idDept"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idDept"].ToString());
                        if (departmentTODT["deptDisplayName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["deptDisplayName"].ToString());
                    }
                }
                return dropDownTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectBOMDepartmentTO");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Vaibhav [25-Sep-2017] added to fill sub department drop down.
        public static List<DropDownTO> SelectSubDepartmentDropDownListByDepartment(Int32 DepartmentId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                if (DepartmentId != 0)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isVisible = 1 AND parentDeptId = " + DepartmentId +
                                           " AND deptTypeId = " + (int)Constants.DepartmentTypeE.SUB_DEPARTMENT;
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader subDepartmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (subDepartmentTODT != null)
                {
                    while (subDepartmentTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (subDepartmentTODT["idDept"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(subDepartmentTODT["idDept"].ToString());
                        if (subDepartmentTODT["deptDisplayName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(subDepartmentTODT["deptDisplayName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectSubDepartmentDropDownListByDepartment");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //public static DataTable SelectAllDimMstDept(SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        cmdSelect.CommandText = SqlSelectQuery();
        //        cmdSelect.Connection = conn;
        //        cmdSelect.Transaction = tran;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@idDept", System.Data.SqlDbType.Int).Value = dimMstDeptTO.IdDept;
        //        SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch(Exception ex)
        //    {
        //        String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //        String userName = System.Windows.Forms.SystemInformation.UserName;
        //        return null;
        //    }
        //    finally
        //    {
        //        cmdSelect.Dispose();
        //    }
        //}


        public static List<DimMstDeptTO> ConvertDTToList(SqlDataReader dimMstDeptTODT)
        {
            List<DimMstDeptTO> dimMstDeptTOList = new List<DimMstDeptTO>();
            if (dimMstDeptTODT != null)
            {
                while (dimMstDeptTODT.Read())
                {
                    DimMstDeptTO dimMstDeptTONew = new DimMstDeptTO();
                    if (dimMstDeptTODT["idDept"] != DBNull.Value)
                        dimMstDeptTONew.IdDept = Convert.ToInt32(dimMstDeptTODT["idDept"].ToString());
                    if (dimMstDeptTODT["parentDeptId"] != DBNull.Value)
                        dimMstDeptTONew.ParentDeptId = Convert.ToInt32(dimMstDeptTODT["parentDeptId"]);
                    if (dimMstDeptTODT["deptTypeId"] != DBNull.Value)
                        dimMstDeptTONew.DeptTypeId = Convert.ToInt32(dimMstDeptTODT["deptTypeId"].ToString());
                    if (dimMstDeptTODT["orgUnitId"] != DBNull.Value)
                        dimMstDeptTONew.OrgUnitId = Convert.ToInt32(dimMstDeptTODT["orgUnitId"].ToString());
                    if (dimMstDeptTODT["isVisible"] != DBNull.Value)
                        dimMstDeptTONew.IsVisible = Convert.ToInt32(dimMstDeptTODT["isVisible"].ToString());
                    if (dimMstDeptTODT["deptCode"] != DBNull.Value)
                        dimMstDeptTONew.DeptCode = Convert.ToString(dimMstDeptTODT["deptCode"].ToString());
                    if (dimMstDeptTODT["deptDisplayName"] != DBNull.Value)
                        dimMstDeptTONew.DeptDisplayName = Convert.ToString(dimMstDeptTODT["deptDisplayName"].ToString());
                    if (dimMstDeptTODT["deptDesc"] != DBNull.Value)
                        dimMstDeptTONew.DeptDesc = Convert.ToString(dimMstDeptTODT["deptDesc"].ToString());
                    dimMstDeptTOList.Add(dimMstDeptTONew);

                }
            }
            return dimMstDeptTOList;
        }

        public static Int32 SelectLastDeptId(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Connection = conn;
            cmdSelect.Transaction = tran;
            ResultMessage resultMessage = new ResultMessage();
            int result;

            try
            {
                cmdSelect.CommandText = "SELECT Max(idDept) + 1 FROM dimMstDept ";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                result = Convert.ToInt32(cmdSelect.ExecuteScalar());

                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectLastDeptId");
                return -1;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public static int InsertDimMstDept(DimMstDeptTO dimMstDeptTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimMstDeptTO, cmdInsert);
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

        public static int InsertDimMstDept(DimMstDeptTO dimMstDeptTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                return ExecuteInsertionCommand(dimMstDeptTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertDimMstDept");
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(DimMstDeptTO dimMstDeptTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimMstDept]( " +
            " [parentDeptId]" +
            " ,[deptTypeId]" +
            " ,[orgUnitId]" +
            " ,[isVisible]" +
            " ,[deptCode]" +
            " ,[deptDisplayName]" +
            " ,[deptDesc]" +
            " )" +
            " VALUES (" +
            " @ParentDeptId " +
            " ,@DeptTypeId " +
            " ,@OrgUnitId " +
            " ,@IsVisible " +
            " ,@DeptCode " +
            " ,@DeptDisplayName " +
            " ,@DeptDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@ParentDeptId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(dimMstDeptTO.ParentDeptId);
            cmdInsert.Parameters.Add("@DeptTypeId", System.Data.SqlDbType.Int).Value = dimMstDeptTO.DeptTypeId;
            cmdInsert.Parameters.Add("@OrgUnitId", System.Data.SqlDbType.Int).Value = dimMstDeptTO.OrgUnitId;
            cmdInsert.Parameters.Add("@IsVisible", System.Data.SqlDbType.Int).Value = dimMstDeptTO.IsVisible;
            cmdInsert.Parameters.Add("@DeptCode", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(dimMstDeptTO.DeptCode);
            cmdInsert.Parameters.Add("@DeptDisplayName", System.Data.SqlDbType.NVarChar).Value = dimMstDeptTO.DeptDisplayName;
            cmdInsert.Parameters.Add("@DeptDesc", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(dimMstDeptTO.DeptDesc);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateDimMstDept(DimMstDeptTO dimMstDeptTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimMstDeptTO, cmdUpdate);
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

        public static int UpdateDimMstDept(DimMstDeptTO dimMstDeptTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimMstDeptTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(DimMstDeptTO dimMstDeptTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimMstDept] SET " +
            " [parentDeptId]= @ParentDeptId" +
            " ,[deptTypeId]= @DeptTypeId" +
            " ,[orgUnitId]= @OrgUnitId" +
            " ,[isVisible]= @IsVisible" +
            " ,[deptCode]= @DeptCode" +
            " ,[deptDisplayName]= @DeptDisplayName" +
            " ,[deptDesc] = @DeptDesc" +
            " WHERE [idDept] = @IdDept ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdDept", System.Data.SqlDbType.Int).Value = dimMstDeptTO.IdDept;
            cmdUpdate.Parameters.Add("@ParentDeptId", System.Data.SqlDbType.Int).Value = dimMstDeptTO.ParentDeptId;
            cmdUpdate.Parameters.Add("@DeptTypeId", System.Data.SqlDbType.Int).Value = dimMstDeptTO.DeptTypeId;
            cmdUpdate.Parameters.Add("@OrgUnitId", System.Data.SqlDbType.Int).Value = dimMstDeptTO.OrgUnitId;
            cmdUpdate.Parameters.Add("@IsVisible", System.Data.SqlDbType.Int).Value = dimMstDeptTO.IsVisible;
            cmdUpdate.Parameters.Add("@DeptCode", System.Data.SqlDbType.NVarChar).Value = dimMstDeptTO.DeptCode;
            cmdUpdate.Parameters.Add("@DeptDisplayName", System.Data.SqlDbType.NVarChar).Value = dimMstDeptTO.DeptDisplayName;
            cmdUpdate.Parameters.Add("@DeptDesc", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(dimMstDeptTO.DeptDesc);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteDimMstDept(Int32 idDept)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idDept, cmdDelete);
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

        public static int DeleteDimMstDept(Int32 idDept, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idDept, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idDept, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimMstDept] " +
            " WHERE idDept = " + idDept + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idDept", System.Data.SqlDbType.Int).Value = dimMstDeptTO.IdDept;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
