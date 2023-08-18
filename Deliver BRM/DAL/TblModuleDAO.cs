using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
namespace SalesTrackerAPI.DAL
{
    public class TblModuleDAO
    {
        #region Methods
        public static String SqlSelectQuery() 
        {
            String sqlSelectQry = "SELECT module.*,idSysElement FROM [tblModule] module LEFT JOIN tblSysElements sysElements ON sysElements.moduleId = module.idModule AND sysElements.type = 'M' "; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DropDownTO> SelectAllTblModule()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                // List<TblModuleTO> list = ConvertDTToList(rdr);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idModule"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idModule"].ToString());
                    if (dateReader["moduleName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["moduleName"].ToString());
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblModuleTO> SelectTblModuleList()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleTO> list = ConvertDTToList(dateReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblModuleTO SelectTblModule(Int32 idModule)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idModule = " + idModule +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleTO> list = ConvertDTToList(rdr);
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
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static TblModuleTO SelectTblModule(Int32 idModule, SqlConnection conn, SqlTransaction transaction)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idModule = " + idModule + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = transaction;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleTO> list = ConvertDTToList(rdr);
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
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }
        public static List<TblModuleTO> ConvertDTToList(SqlDataReader tblModuleTODT)
        {
            List<TblModuleTO> tblModuleTOList = new List<TblModuleTO>();
            if (tblModuleTODT != null)
            {
                while (tblModuleTODT.Read())
                {
                    TblModuleTO tblModuleTONew = new TblModuleTO();
                    if (tblModuleTODT["idModule"] != DBNull.Value)
                        tblModuleTONew.IdModule = Convert.ToInt32(tblModuleTODT["idModule"].ToString());
                    if (tblModuleTODT["createdOn"] != DBNull.Value)
                        tblModuleTONew.CreatedOn = Convert.ToDateTime(tblModuleTODT["createdOn"].ToString());
                    if (tblModuleTODT["moduleName"] != DBNull.Value)
                        tblModuleTONew.ModuleName = Convert.ToString(tblModuleTODT["moduleName"].ToString());
                    if (tblModuleTODT["moduleDesc"] != DBNull.Value)
                        tblModuleTONew.ModuleDesc = Convert.ToString(tblModuleTODT["moduleDesc"].ToString());
                    if (tblModuleTODT["navigateUrl"] != DBNull.Value)
                       tblModuleTONew.NavigateUrl = Convert.ToString(tblModuleTODT["navigateUrl"].ToString());
                    if (tblModuleTODT["isActive"] != DBNull.Value)
                        tblModuleTONew.IsActive = Convert.ToInt16(tblModuleTODT["isActive"].ToString());
                    if (tblModuleTODT["logoUrl"] != DBNull.Value)
                       tblModuleTONew.LogoUrl = Convert.ToString(tblModuleTODT["logoUrl"].ToString());
                    if (tblModuleTODT["idSysElement"] != DBNull.Value)
                      tblModuleTONew.SysElementId = Convert.ToInt32(tblModuleTODT["idSysElement"].ToString());
                    if (tblModuleTODT["androidUrl"] != DBNull.Value)
                        tblModuleTONew.AndroidUrl = Convert.ToString(tblModuleTODT["androidUrl"].ToString());
                    if (tblModuleTODT["isSubscribe"] != DBNull.Value)
                        tblModuleTONew.IsSubscribe = Convert.ToInt16(tblModuleTODT["isSubscribe"].ToString());
                    if (tblModuleTODT["containerName"] != DBNull.Value)
                        tblModuleTONew.ContainerName = Convert.ToString(tblModuleTODT["containerName"].ToString());
                    if (tblModuleTODT["isExternal"] != DBNull.Value)
                        tblModuleTONew.IsExternal = Convert.ToInt16(tblModuleTODT["isExternal"].ToString());
                    
                          // Sanjay [25-Feb-2019] User Subscription Management
                    if (tblModuleTODT["noOfAllowedLicenseCnt"] != DBNull.Value)
                        tblModuleTONew.NoOfAllowedLicenseCnt = Convert.ToInt32(tblModuleTODT["noOfAllowedLicenseCnt"].ToString());
                    if (tblModuleTODT["noOfConfiguredLicenseCnt"] != DBNull.Value)
                        tblModuleTONew.NoOfConfiguredLicenseCnt = Convert.ToInt32(tblModuleTODT["noOfConfiguredLicenseCnt"].ToString());
                    if (tblModuleTODT["noOfActiveLicenseCnt"] != DBNull.Value)
                        tblModuleTONew.NoOfActiveLicenseCnt = Convert.ToInt32(tblModuleTODT["noOfActiveLicenseCnt"].ToString());
                    if (tblModuleTODT["iotconfigSetting"] != DBNull.Value)
                        tblModuleTONew.IotconfigSetting = Convert.ToInt32(tblModuleTODT["iotconfigSetting"].ToString());

                    if (tblModuleTODT["modeId"] != DBNull.Value)
                        tblModuleTONew.ModeId = Convert.ToInt32(tblModuleTODT["modeId"].ToString());

                    if (tblModuleTONew.IsSubscribe == 1)
                        tblModuleTONew.IsSubscribeBit = true;
                    else
                        tblModuleTONew.IsSubscribeBit = false;

                    tblModuleTOList.Add(tblModuleTONew);
                }
            }
            return tblModuleTOList;
        }

        #endregion


#region usersubscription
//vipul[15/03/2019]
public static int GetUserSubscriptionSetting()
           {
                 String sqlConnStr =  Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
               try{

               
            
            SqlCommand cmdSelect = new SqlCommand(" select configParamVal from tblConfigparams where configParamName='UserSubscriptionSettingByTabOrNot'",conn);
             conn.Open();
             int result=(Convert.ToInt32(cmdSelect.ExecuteScalar()));
               conn.Close();
               return result;
               }
               catch(Exception ex)
               {
                   conn.Close();
                    return 0;
               }
            return 0;

           }
 public static List<TblModuleTO> SelectAllActiveUserCount()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                   int userSubscriptionActiveCntSetting=GetUserSubscriptionSetting();
             
             if(userSubscriptionActiveCntSetting==(Int32)Constants.UsersSubscriptionActiveCntCalSetting.ByTab)
             {
cmdSelect.CommandText ="select count(currentModuleId) as noOfActiveLicenseCnt,currentModuleId as idModule from tblModuleCommHis where outTime is null group by currentModuleId";
             }
             else
             {
  cmdSelect.CommandText =    " select count(currentModuleId) as noOfActiveLicenseCnt,currentModuleId as idModule from( "+
 " select  (loginId),currentModuleId from tblModuleCommhis where   outTime is null "+
  " group by loginId,currentModuleId)sql  group by currentModuleId";
             }
                conn.Open();
                
              
        
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleTO> list = ActiveCntConvertDTToList(dateReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<TblModuleTO> ActiveCntConvertDTToList(SqlDataReader tblModuleTODT)
        {
            List<TblModuleTO> tblModuleTOList = new List<TblModuleTO>();
            if (tblModuleTODT != null)
            {
                while (tblModuleTODT.Read())
                {
                    TblModuleTO tblModuleTONew = new TblModuleTO();
                    if (tblModuleTODT["idModule"] != DBNull.Value)
                        tblModuleTONew.IdModule = Convert.ToInt32(tblModuleTODT["idModule"].ToString());
                      
                    if (tblModuleTODT["noOfActiveLicenseCnt"] != DBNull.Value)
                        tblModuleTONew.NoOfActiveLicenseCnt = Convert.ToInt32(tblModuleTODT["noOfActiveLicenseCnt"].ToString());

                 


                    tblModuleTOList.Add(tblModuleTONew);
                }
            }
            return tblModuleTOList;
        }
#endregion


           #region userTrackingEntry  
         public static int InserttblModuleCommunicationHistory(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran)
        {
           String sqlQuery = @" INSERT INTO [tblModuleCommHis]( " + 
            "  [currentModuleId]" +
            " ,[recentModuleId]" +
            " ,[inTime]" +
            " ,[outTime]" +
            " ,[userId]" +
            " ,[loginId]" +
            " )" +
" VALUES (" +
            "  @currentModuleId " +
            
            " ,@recentModuleId " +
            " ,@inTime " +
            " ,@outTime " +
            " ,@userId "+
              " ,@loginId "+
          
            " )";
            SqlCommand cmdInsert=new SqlCommand();
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Connection=conn;
             if(tran!=null)
            {
              cmdInsert.Transaction=tran;
            }

            cmdInsert.Parameters.Add("@currentModuleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModuleCommHisTO.CurrentModuleId);
            cmdInsert.Parameters.Add("@recentModuleId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblModuleCommHisTO.RecentModuleId);
            cmdInsert.Parameters.Add("@inTime", System.Data.SqlDbType.DateTime).Value = tblModuleCommHisTO.InTime;
            cmdInsert.Parameters.Add("@outTime", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblModuleCommHisTO.OutTime);
            cmdInsert.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value = tblModuleCommHisTO.UserId;
              cmdInsert.Parameters.Add("@loginId", System.Data.SqlDbType.Int).Value = tblModuleCommHisTO.LoginId;
        if(conn.State==ConnectionState.Closed)
        {
   conn.Open();
        }
     
           int result= cmdInsert.ExecuteNonQuery();
           if(result==1)
           {
           cmdInsert.CommandText = "Select @@Identity";
           tblModuleCommHisTO.IdModuleCommHis = Int32.Parse(cmdInsert.ExecuteScalar().ToString());
           return  result;
           }
          return result;
        }
         public static int CheckIsImpPersonFromLoginId(TblModuleCommHisTO tblModuleCommHis,SqlConnection conn,SqlTransaction transaction)
        {
             if(conn==null)
            {
                conn=new SqlConnection( Startup.ConnectionString);
            }
               
               conn.Open();
            int isImpPerson=0;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = "select isImpPerson from tblSysEleUserEntitlements inner join tblsysElements "+
	" on tblsysElements.idSysElement=tblSysEleUserEntitlements.sysEleId where moduleId="+tblModuleCommHis.CurrentModuleId+" And type='M' AND "+
" userId=(select userId from tblLogin where idLogin=( "+
"select max(idLogin) from tblLogin where logoutDate IS NULL AND idLogin NOT IN ("+tblModuleCommHis.UserLogin+")))";
                cmdSelect.Connection = conn;
               // cmdSelect.Transaction = transaction;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                isImpPerson =Convert.ToInt32(cmdSelect.ExecuteScalar());
                return isImpPerson;
              
            }
            catch (Exception ex)
            {
                return isImpPerson;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }
         public static int GetPreviousLoginId(TblModuleCommHisTO tblModuleCommHis,SqlConnection conn,SqlTransaction transaction)
        {
               if(conn==null)
            {
                conn=new SqlConnection( Startup.ConnectionString);
            }
               
               conn.Open();
            int loginId=0;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            
            try
            {
                cmdSelect.CommandText = "select max(idLogin) from tblLogin where logoutDate IS  NULL AND  idLogin NOT IN("+tblModuleCommHis.UserLogin+")";
                cmdSelect.Connection = conn;
               // cmdSelect.Transaction = transaction;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                loginId =Convert.ToInt32(cmdSelect.ExecuteScalar());
                return loginId;
              
            }
            catch (Exception ex)
            {
                return loginId;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }
           public static int UpdateAlltblModuleCommunicationHistory(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran)
        {
           String sqlQuery = @" Update [tblModuleCommHis] SET " + 
                       
            " [outTime]=@outTime " +
           
          
            " where userId =@userId AND outTime IS NULL" ;

            SqlCommand cmdUpdate=new SqlCommand();
            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Connection=conn;
            if(tran!=null)
            {
              cmdUpdate.Transaction=tran;
            }

            cmdUpdate.Parameters.Add("@outTime", System.Data.SqlDbType.DateTime).Value = tblModuleCommHisTO.OutTime;
            cmdUpdate.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value =(tblModuleCommHisTO.UserId);
           if(conn.State==ConnectionState.Closed)
           {
                  conn.Open();
           }
       
           int result= cmdUpdate.ExecuteNonQuery();
          
          return result;
        }
           public static int UpdatetblModuleCommunicationHistory(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran)
        {
             String sqlQuery="";
      
            sqlQuery = @" Update [tblModuleCommHis] SET " + 
                       
            " [outTime]=@outTime " +
           
          
            " where LoginId =@LoginId AND outTime IS NULL" ;
            if(tblModuleCommHisTO.IdModuleCommHis!=0)
            {
                sqlQuery+=" and idModuleCommHis="+tblModuleCommHisTO.IdModuleCommHis;
            }

            SqlCommand cmdUpdate=new SqlCommand();
            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Connection=conn;
            if(tran!=null)
            {
              cmdUpdate.Transaction=tran;
            }

            cmdUpdate.Parameters.Add("@outTime", System.Data.SqlDbType.DateTime).Value = tblModuleCommHisTO.OutTime;
            cmdUpdate.Parameters.Add("@LoginId", System.Data.SqlDbType.Int).Value =(tblModuleCommHisTO.LoginId);
           if(conn.State==ConnectionState.Closed)
           {
                  conn.Open();
           }
       
           int result= cmdUpdate.ExecuteNonQuery();
          
          return result;
        }
         public static List<TblModuleCommHisTO> GetAllTblModuleCommHis(int userId)
        {
            String sqlConnStr =  Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                conn.Open();
               // cmdSelect.CommandText ="select * from tblModuleCommHis left join tblModule on tblModule.idModule=tblModuleCommHis.currentModuleId where outTime IS NULL and userId="+userId;
               if(userId!=0)
               {
                   //  cmdSelect.CommandText ="select * from tblModuleCommHis left join tblLogin on tblModuleCommHis.loginId=tblLogin.idLogin where tblLogin.logoutDate IS NULL and tblLogin.userId="+userId;
                     cmdSelect.CommandText ="select * from  tblLogin  where logoutDate IS NULL and userId="+userId +
                     " and idLogin!=  (select max(idLogin) from tblLogin where userId="+userId+")";
               }
                
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleCommHisTO> list = TblModuleCommHisConvertDTToList(dateReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

          public static TblModuleTO GetAllActiveAllowedCnt(int moduleId,int userId,int loginId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                 int userSubscriptionActiveCntSetting=GetUserSubscriptionSetting();
             
             if(userSubscriptionActiveCntSetting==(Int32)Constants.UsersSubscriptionActiveCntCalSetting.ByTab)
             {
                    cmdSelect.CommandText ="select (select ISNULL(noOfAllowedLicenseCnt,0)  from tblModule where idModule="+moduleId+") AS AllowedLicenseCount,(select count(*)  from tblModuleCommHis where currentModuleId="+moduleId+" AND  outTime IS  NULL) AS ActiveLicenseCount, "+
                 " ISNULL((select isImpPerson from tblSysEleUserEntitlements inner join tblsysElements on tblsysElements.idSysElement=tblSysEleUserEntitlements.sysEleId "+
                " where moduleId="+moduleId+" and userId="+userId+" and type='M'),0) as IsImpPerson, "+
                   "(select COUNT( *) as impPersonCount from tblModuleCommHis t "+
                  "inner join tblSysEleUserEntitlements u on u.userId=t.userId "+
                    "where t.currentModuleId="+moduleId+" and t.outtime is null and u.isImpPerson=1 and  sysELeId=(select idSysElement from tblsysElements where moduleId="+moduleId+" and type='M' ) as impPersonCount, "+
                      "(select case when logoutDate is null then 1 else 0 End as IsLogin from tbllogin where idlogin ="+loginId+" )  as IsLogin ";
             }
             else{
                    cmdSelect.CommandText ="select (select ISNULL(noOfAllowedLicenseCnt,0)  from tblModule where idModule="+moduleId+") AS AllowedLicenseCount,(select count(*) from( select  (loginId),currentModuleId from tblModuleCommhis where currentModuleId="+moduleId+" and outTime is null group by loginId,currentModuleId)sql) AS ActiveLicenseCount, "+
                   " ISNULL((select isImpPerson from tblSysEleUserEntitlements inner join tblsysElements on tblsysElements.idSysElement=tblSysEleUserEntitlements.sysEleId "+
                  " where moduleId="+moduleId+" and userId="+userId+" and type='M'),0) as IsImpPerson, "+
                  " (select count(*) "+
			" from( select  (t.loginId),t.currentModuleId "+
		"from tblModuleCommhis t inner join tblSysEleUserEntitlements u  "+
		" on u.userId=t.userId where currentModuleId="+moduleId+" and u.isImpPerson=1 and  sysELeId=(select idSysElement from tblsysElements where moduleId="+moduleId+" and type='M' ) "+
			" and outTime is null group by loginId,currentModuleId)sql) as impPersonCount, "+
            "(select case when logoutDate is null then 1 else 0 End as IsLogin from tbllogin where idlogin ="+loginId+" )  as IsLogin ";
             }
               
            
              
                  
                 

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
    conn.Open();
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                 TblModuleTO tblModule=new TblModuleTO();
               while(dateReader.Read())
               {
                   if(dateReader["ActiveLicenseCount"]!=null)
                        tblModule.NoOfActiveLicenseCnt=Convert.ToInt32(dateReader["ActiveLicenseCount"].ToString());
                   if(dateReader["AllowedLicenseCount"] !=null)
                        tblModule.NoOfAllowedLicenseCnt=Convert.ToInt32(dateReader["AllowedLicenseCount"].ToString());
                    if (dateReader["IsImpPerson"] != null)
                        tblModule.IsImpPerson=Convert.ToInt32(dateReader["IsImpPerson"].ToString());
                    if (dateReader["impPersonCount"] != null)
                        tblModule.ImpPersonCount=Convert.ToInt32(dateReader["impPersonCount"].ToString());
                    if (dateReader["IsLogin"] != null)
                        tblModule.IsLogin=Convert.ToInt32(dateReader["IsLogin"].ToString());
               }  
                return tblModule;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
 public static List<TblModuleCommHisTO> GetActiveUserDetail(int moduleId)
        {
            String sqlConnStr =  Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                conn.Open();
               // cmdSelect.CommandText ="select * from tblModuleCommHis left join tblModule on tblModule.idModule=tblModuleCommHis.currentModuleId where outTime IS NULL and userId="+userId;
               if(moduleId!=0)
               {
                    cmdSelect.CommandText ="select loginIP,loginId, tblModuleCommHis.userId,count(tblModuleCommHis.userId) as LoginCount,tblUser.userLogin from tblModuleCommHis "+
" left join tblLogin  " +
" on tblLogin.idLogin=tblModuleCommHis.loginId " +
" inner join tblUser on tblUser.idUser=tblLogin.userId "+
 " where currentModuleId="+moduleId +" and outTime is NUll "+
" group by tblModuleCommHis.userId,loginIP,tblUser.userLogin,loginId";
               }
                
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleCommHisTO> list = new  List<TblModuleCommHisTO>();
                while(dateReader.Read())
                {
                   TblModuleCommHisTO tblModuleComm=new TblModuleCommHisTO();
                   tblModuleComm.LoginIP=dateReader["loginIP"].ToString();
                   tblModuleComm.UserId=Convert.ToInt32(dateReader["userId"].ToString());
                   tblModuleComm.LoginCount=Convert.ToInt32(dateReader["LoginCount"].ToString());
                   tblModuleComm.UserLogin=dateReader["userLogin"].ToString();
                   tblModuleComm.LoginId=Convert.ToInt32( dateReader["loginId"].ToString());
                   list.Add(tblModuleComm);
                }
            
                    return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

  public static List<TblModuleCommHisTO> TblModuleCommHisConvertDTToList(SqlDataReader tblModuleTODT)
        {
            List<TblModuleCommHisTO> tblModuleCommHisTOList = new List<TblModuleCommHisTO>();
            if (tblModuleTODT != null)
            {
                while (tblModuleTODT.Read())
                {
                    TblModuleCommHisTO tblModuleTONew = new TblModuleCommHisTO();
                   // if (tblModuleTODT["idModuleCommHis"] != DBNull.Value)
                   //     tblModuleTONew.IdModuleCommHis = Convert.ToInt32(tblModuleTODT["idModuleCommHis"].ToString());
                      
                   // if (tblModuleTODT["currentModuleId"] != DBNull.Value)
                   //     tblModuleTONew.CurrentModuleId = Convert.ToInt32(tblModuleTODT["currentModuleId"].ToString());

                    if (tblModuleTODT["loginDate"] != DBNull.Value)
                        tblModuleTONew.InTime = Convert.ToDateTime(tblModuleTODT["loginDate"].ToString());
                    if (tblModuleTODT["loginIP"] != DBNull.Value)
                        tblModuleTONew.LoginIP =tblModuleTODT["loginIP"].ToString();
                        if (tblModuleTODT["userId"] != DBNull.Value)
                        tblModuleTONew.UserId =Convert.ToInt32(tblModuleTODT["userId"].ToString());
                       //   if (tblModuleTODT["loginId"] != DBNull.Value)
                      //  tblModuleTONew.LoginId =Convert.ToInt32(tblModuleTODT["loginId"].ToString());
                         if (tblModuleTODT["idLogin"] != DBNull.Value)
                        tblModuleTONew.LoginId =Convert.ToInt32(tblModuleTODT["idLogin"].ToString());
                         if (tblModuleTODT["deviceId"] != DBNull.Value)
                        tblModuleTONew.DeviceId =tblModuleTODT["deviceId"].ToString();
                    tblModuleCommHisTOList.Add(tblModuleTONew);
                }
            }
            return tblModuleCommHisTOList;
        }
        public static int UpdatetblLogin(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran)
        {
             String sqlQuery="";
      
            sqlQuery = @" update tblLogin set logoutDate= @outTime where idLogin= @idLogin" ;
            SqlCommand cmdUpdate=new SqlCommand();
            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Connection=conn;
            if(tran!=null)
            {
              cmdUpdate.Transaction=tran;
            }

            cmdUpdate.Parameters.Add("@outTime", System.Data.SqlDbType.DateTime).Value = tblModuleCommHisTO.OutTime;
            cmdUpdate.Parameters.Add("@idLogin", System.Data.SqlDbType.Int).Value =(tblModuleCommHisTO.LoginId);
           if(conn.State==ConnectionState.Closed)
           {
                  conn.Open();
           }
       
           int result= cmdUpdate.ExecuteNonQuery();
          
          return result;
        }
           public static int UpdateAlltblLogin(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran)
        {
             String sqlQuery="";
      
           sqlQuery = @" update tblLogin set logoutDate= @outTime where userId= @userId AND idLogin!=(select max(idLogin) from tblLogin where userId=@userId)" ;
            SqlCommand cmdUpdate=new SqlCommand();
            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Connection=conn;
            if(tran!=null)
            {
              cmdUpdate.Transaction=tran;
            }

            cmdUpdate.Parameters.Add("@outTime", System.Data.SqlDbType.DateTime).Value = tblModuleCommHisTO.OutTime;
            cmdUpdate.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value =(tblModuleCommHisTO.UserId);
             // cmdUpdate.Parameters.Add("@loginId", System.Data.SqlDbType.Int).Value =(tblModuleCommHisTO.LoginId);
           if(conn.State==ConnectionState.Closed)
           {
                  conn.Open();
           }
       
           int result= cmdUpdate.ExecuteNonQuery();
          
          return result;
        }
        #endregion

        #region Insertion
        public static int InsertTblModule(TblModuleTO tblModuleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblModuleTO, cmdInsert);
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

        public static int InsertTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblModuleTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblModuleTO tblModuleTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblModule]( " + 
            "  [idModule]" +
            " ,[createdOn]" +
            " ,[moduleName]" +
            " ,[moduleDesc]" +
            " ,[androidUrl ]"+
            " ,[isSubscribe] "+
            " ,[containerName]" +
            " )" +
" VALUES (" +
            "  @IdModule " +
            " ,@CreatedOn " +
            " ,@ModuleName " +
            " ,@ModuleDesc " +
            " ,@AndroidUrl " +
            " ,@IsSubscribe " +
            " ,@containerName" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdModule", System.Data.SqlDbType.Int).Value = tblModuleTO.IdModule;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblModuleTO.CreatedOn;
            cmdInsert.Parameters.Add("@ModuleName", System.Data.SqlDbType.VarChar).Value = tblModuleTO.ModuleName;
            cmdInsert.Parameters.Add("@ModuleDesc", System.Data.SqlDbType.VarChar).Value = tblModuleTO.ModuleDesc;
            cmdInsert.Parameters.Add("@AndroidUrl", System.Data.SqlDbType.VarChar).Value = tblModuleTO.AndroidUrl;
            cmdInsert.Parameters.Add("@IsSubscribe", System.Data.SqlDbType.Int).Value = tblModuleTO.IsSubscribe;
            cmdInsert.Parameters.Add("@containerName", System.Data.SqlDbType.NVarChar).Value = tblModuleTO.ContainerName; //Sudhir[11-OCT-2018] Added for Azure Container Name.
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblModule(TblModuleTO tblModuleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblModuleTO, cmdUpdate);
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

        public static int UpdateTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblModuleTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblModuleTO tblModuleTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblModule] SET " + 
            "  [idModule] = @IdModule" +
            " ,[createdOn]= @CreatedOn" +
            " ,[moduleName]= @ModuleName" +
            " ,[moduleDesc] = @ModuleDesc" +
            " ,[isSubscribe]=@IsSubscribe "+
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdModule", System.Data.SqlDbType.Int).Value = tblModuleTO.IdModule;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblModuleTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ModuleName", System.Data.SqlDbType.VarChar).Value = tblModuleTO.ModuleName;
            cmdUpdate.Parameters.Add("@ModuleDesc", System.Data.SqlDbType.VarChar).Value = tblModuleTO.ModuleDesc;
            cmdUpdate.Parameters.Add("@IsSubscribe", System.Data.SqlDbType.Int).Value = tblModuleTO.IsSubscribe;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblModule(Int32 idModule)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idModule, cmdDelete);
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

        public static int DeleteTblModule(Int32 idModule, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idModule, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idModule, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblModule] " +
            " WHERE idModule = " + idModule +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idModule", System.Data.SqlDbType.Int).Value = tblModuleTO.IdModule;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
