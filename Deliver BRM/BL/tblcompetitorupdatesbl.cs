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
    public class TblCompetitorUpdatesBL
    {
        #region Selection
       
        public static List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList()
        {
            return TblCompetitorUpdatesDAO.SelectAllTblCompetitorUpdates();
           
        }

        public static List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList(Int32 competitorId , Int32 enteredBy ,DateTime fromDate, DateTime toDate)
        {
            return TblCompetitorUpdatesDAO.SelectAllTblCompetitorUpdates(competitorId, enteredBy,fromDate, toDate);

        }
        //aniket
        public static List<TblCompetitorUpdatesTO> SelectAllTblOrganizationListDateWise(DateTime FromDate, DateTime ToDate)
        {
            return TblCompetitorUpdatesDAO.SelectAllOrganizationListDateWise(FromDate, ToDate);
        }
        public static TblCompetitorUpdatesTO SelectTblCompetitorUpdatesTO(Int32 idCompeUpdate)
        {
           return  TblCompetitorUpdatesDAO.SelectTblCompetitorUpdates(idCompeUpdate);
            
        }

        public static List<DropDownTO> SelectCompeUpdateUserDropDown()
        {
            return TblCompetitorUpdatesDAO.SelectCompeUpdateUserDropDown();

        }

        public static TblCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId)
        {
            return TblCompetitorUpdatesDAO.SelectLastPriceForCompetitorAndBrand(brandId);
        }


            #endregion

            #region Insertion
            public static int InsertTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO)
        {
            return TblCompetitorUpdatesDAO.InsertTblCompetitorUpdates(tblCompetitorUpdatesTO);
        }

        public static int InsertTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorUpdatesDAO.InsertTblCompetitorUpdates(tblCompetitorUpdatesTO, conn, tran);
        }

        internal static ResultMessage SaveMarketUpdate(List<TblCompetitorUpdatesTO> competitorUpdatesTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if(competitorUpdatesTOList==null || competitorUpdatesTOList.Count==0)
                {
                    tran.Rollback();
                    resultMessage.Text = "competitorUpdatesTOList Found Null : SaveMarketUpdate";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                for (int i = 0; i < competitorUpdatesTOList.Count; i++)
                {

                    if (competitorUpdatesTOList[i].OtherSourceId == 0 && competitorUpdatesTOList[i].DealerId==0)
                    {
                        TblOtherSourceTO otherSourceTO = new TblOtherSourceTO();
                        otherSourceTO.OtherDesc = competitorUpdatesTOList[i].OtherSourceOtherDesc;
                        otherSourceTO.CreatedBy = competitorUpdatesTOList[i].CreatedBy;
                        otherSourceTO.CreatedOn = competitorUpdatesTOList[i].CreatedOn;

                        result = BL.TblOtherSourceBL.InsertTblOtherSource(otherSourceTO, conn, tran);

                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error While InsertTblOtherSource : SaveMarketUpdate";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }

                        competitorUpdatesTOList[i].OtherSourceId = otherSourceTO.IdOtherSource;
                    }

                    result = InsertTblCompetitorUpdates(competitorUpdatesTOList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While InsertTblCompetitorUpdates : SaveMarketUpdate";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveMarketUpdate";
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
        #endregion

        #region Updation
        public static int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO)
        {
            return TblCompetitorUpdatesDAO.UpdateTblCompetitorUpdates(tblCompetitorUpdatesTO);
        }

        public static int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorUpdatesDAO.UpdateTblCompetitorUpdates(tblCompetitorUpdatesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblCompetitorUpdates(Int32 idCompeUpdate)
        {
            return TblCompetitorUpdatesDAO.DeleteTblCompetitorUpdates(idCompeUpdate);
        }

        public static int DeleteTblCompetitorUpdates(Int32 idCompeUpdate, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorUpdatesDAO.DeleteTblCompetitorUpdates(idCompeUpdate, conn, tran);
        }

       

        #endregion

    }
}
