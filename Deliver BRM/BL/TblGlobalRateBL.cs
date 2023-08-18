using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
namespace SalesTrackerAPI.BL
{
    public class TblGlobalRateBL
    {
        #region Selection
       

        public static TblGlobalRateTO SelectTblGlobalRateTO(Int32 idGlobalRate)
        {
            return  TblGlobalRateDAO.SelectTblGlobalRate(idGlobalRate);
           
        }

        public static TblGlobalRateTO SelectTblGlobalRateTO(Int32 idGlobalRate,SqlConnection conn,SqlTransaction tran)
        {
            return TblGlobalRateDAO.SelectTblGlobalRate(idGlobalRate,conn,tran);

        }

        public static TblGlobalRateTO SelectLatestTblGlobalRateTO()
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.BeginTransaction();
                tran = conn.BeginTransaction();
                return TblGlobalRateDAO.SelectLatestTblGlobalRateTO(conn, tran);
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

        public static TblGlobalRateTO SelectLatestTblGlobalRateTO(SqlConnection conn,SqlTransaction tran)
        {
            return TblGlobalRateDAO.SelectLatestTblGlobalRateTO(conn,tran);
        }

        public static List<TblGlobalRateTO> SelectTblGlobalRateTOList(DateTime fromDate,DateTime toDate)
        {
            return TblGlobalRateDAO.SelectLatestTblGlobalRateTOList(fromDate,toDate);

        }
        public static List<GlobalRateTOFroGraph> SelectTblGlobalRateListForGraph(DateTime fromDate, DateTime toDate)
        {
            List<GlobalRateTOFroGraph> globalRateTOFroGraphList= TblGlobalRateDAO.SelectRateForGraph(fromDate, toDate);
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
            if(tblConfigParamsTO!=null)
            {
                Int32 orgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                TblOrganizationTO tblOrganizationTO = BL.TblOrganizationBL.SelectTblOrganizationTO(orgId);

                if (tblOrganizationTO != null)
                {
                    if (globalRateTOFroGraphList != null && globalRateTOFroGraphList.Count > 0)
                    {
                        for(int k=0;k<globalRateTOFroGraphList.Count;k++)
                        {
                            globalRateTOFroGraphList[k].OrgId = tblOrganizationTO.IdOrganization;
                            globalRateTOFroGraphList[k].FirmName = tblOrganizationTO.FirmName;
                        }
                    }
                }
            }
            List<TblCompetitorUpdatesTO> tblCompetirorUpdateTolist = BL.TblCompetitorUpdatesBL.SelectAllTblCompetitorUpdatesList(0,0,fromDate, toDate);
            if(tblCompetirorUpdateTolist!=null && tblCompetirorUpdateTolist.Count >0)
            {
                for(int i =0;i< tblCompetirorUpdateTolist.Count;i++)
                {
                    GlobalRateTOFroGraph globalRateTOFroGraph = new GlobalRateTOFroGraph();
                    TblCompetitorUpdatesTO tblCompetitorUpdatesTO = tblCompetirorUpdateTolist[i];
                    globalRateTOFroGraph.CreatedOn = tblCompetitorUpdatesTO.UpdateDatetime;
                    globalRateTOFroGraph.Rate = tblCompetitorUpdatesTO.LastPrice;
                    globalRateTOFroGraph.FirmName = tblCompetitorUpdatesTO.FirmName;
                    globalRateTOFroGraph.OrgId = tblCompetitorUpdatesTO.CompetitorOrgId;
                    globalRateTOFroGraphList.Add(globalRateTOFroGraph);
                }
            }
            return globalRateTOFroGraphList;
        }
        public static Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            return TblGlobalRateDAO.IsRateAlreadyDeclaredForTheDate(date, conn,tran);

        }
        #endregion

        #region Insertion
        public static int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO)
        {
            return TblGlobalRateDAO.InsertTblGlobalRate(tblGlobalRateTO);
        }

        public static int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGlobalRateDAO.InsertTblGlobalRate(tblGlobalRateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO)
        {
            return TblGlobalRateDAO.UpdateTblGlobalRate(tblGlobalRateTO);
        }

        public static int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGlobalRateDAO.UpdateTblGlobalRate(tblGlobalRateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblGlobalRate(Int32 idGlobalRate)
        {
            return TblGlobalRateDAO.DeleteTblGlobalRate(idGlobalRate);
        }

        public static int DeleteTblGlobalRate(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran)
        {
            return TblGlobalRateDAO.DeleteTblGlobalRate(idGlobalRate, conn, tran);
        }

        #endregion
        
    }
}
