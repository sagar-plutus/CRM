using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;


namespace SalesTrackerAPI.BL
{
    public class TblLoadingQuotaConfigBL
    {
        #region Selection

        public static List<TblLoadingQuotaConfigTO> SelectAllTblLoadingQuotaConfigList()
        {
           return  TblLoadingQuotaConfigDAO.SelectAllTblLoadingQuotaConfig();
        }

        public static TblLoadingQuotaConfigTO SelectTblLoadingQuotaConfigTO(Int32 idLoadQuotaConfig)
        {
            return TblLoadingQuotaConfigDAO.SelectTblLoadingQuotaConfig(idLoadQuotaConfig);
        }

        public static List<TblLoadingQuotaConfigTO> SelectLatestLoadingQuotaConfigList(Int32 prodCatId, Int32 prodSpecId)
        {
            return TblLoadingQuotaConfigDAO.SelectLatestLoadingQuotaConfig(prodCatId,prodSpecId);

        }

        //Sudhir[06-APR-2018] Added for Get List of Other Item With CNF Wise.
        public static List<TblLoadingQuotaConfigTO> SelectLatestLoadingQuotaConfigForOther()
        {
            return TblLoadingQuotaConfigDAO.SelectLatestLoadingQuotaConfigForOther();

        }
        

        public static List<TblLoadingQuotaConfigTO> SelectEmptyLoadingQuotaConfig(SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingQuotaConfigDAO.SelectEmptyLoadingQuotaConfig(conn, tran);
        }

       
        #endregion

        #region Insertion
        public static int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO)
        {
            return TblLoadingQuotaConfigDAO.InsertTblLoadingQuotaConfig(tblLoadingQuotaConfigTO);
        }

        public static int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaConfigDAO.InsertTblLoadingQuotaConfig(tblLoadingQuotaConfigTO, conn, tran);
        }

        public static ResultMessage SaveNewLoadingQuotaConfiguration(List<TblLoadingQuotaConfigTO> loadingQuotaConfigTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = SaveNewLoadingQuotaConfiguration(loadingQuotaConfigTOList, conn, tran);
                if (resultMessage.MessageType == ResultMessageE.Error)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Record Could Not Be Saved";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveNewLoadingQuotaConfiguration";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static ResultMessage SaveNewLoadingQuotaConfiguration(List<TblLoadingQuotaConfigTO> loadingQuotaConfigTOList, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {

                if (loadingQuotaConfigTOList != null && loadingQuotaConfigTOList.Count > 0)
                {

                    DateTime deactivatedDate = Constants.ServerDateTime;
                    for (int i = 0; i < loadingQuotaConfigTOList.Count; i++)
                    {
                        //If Already Exist Then Deactivate it

                        TblLoadingQuotaConfigTO tblLoadingQuotaConfigTOOld = new TblLoadingQuotaConfigTO();
                        tblLoadingQuotaConfigTOOld.IdLoadQuotaConfig = loadingQuotaConfigTOList[i].IdLoadQuotaConfig;
                        tblLoadingQuotaConfigTOOld.IsActive = 0;
                        tblLoadingQuotaConfigTOOld.DeactivatedBy = loadingQuotaConfigTOList[i].CreatedBy;
                        tblLoadingQuotaConfigTOOld.DeactivatedOn = deactivatedDate;
                        tblLoadingQuotaConfigTOOld.CnfOrgId = loadingQuotaConfigTOList[i].CnfOrgId;
                        tblLoadingQuotaConfigTOOld.MaterialId = loadingQuotaConfigTOList[i].MaterialId;
                        tblLoadingQuotaConfigTOOld.ProdCatId = loadingQuotaConfigTOList[i].ProdCatId;
                        tblLoadingQuotaConfigTOOld.ProdSpecId = loadingQuotaConfigTOList[i].ProdSpecId;

                        //Sudhir[06-APR-2018] Added for the Product Item Id.
                        tblLoadingQuotaConfigTOOld.ProdItemId = loadingQuotaConfigTOList[i].ProdItemId;

                        result = DAL.TblLoadingQuotaConfigDAO.DeactivateLoadingQuotaConfig(tblLoadingQuotaConfigTOOld, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While DeactivateLoadingQuotaConfig";
                            resultMessage.Result = 0;
                            return resultMessage;
                        }

                        //Insert New Configuration
                        loadingQuotaConfigTOList[i].IsActive = 1;
                        result = InsertTblLoadingQuotaConfig(loadingQuotaConfigTOList[i], conn, tran);
                        if (result != 1)
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblLoadingQuotaConfig";
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loadingQuotaConfigTOList Found Null";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveNewLoadingQuotaConfiguration";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {

            }
        }

        #endregion

        #region Updation
        public static int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO)
        {
            return TblLoadingQuotaConfigDAO.UpdateTblLoadingQuotaConfig(tblLoadingQuotaConfigTO);
        }

        public static int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaConfigDAO.UpdateTblLoadingQuotaConfig(tblLoadingQuotaConfigTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig)
        {
            return TblLoadingQuotaConfigDAO.DeleteTblLoadingQuotaConfig(idLoadQuotaConfig);
        }

        public static int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaConfigDAO.DeleteTblLoadingQuotaConfig(idLoadQuotaConfig, conn, tran);
        }

        #endregion
        
    }
}
