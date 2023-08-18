using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System.Linq;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblParityDetailsBL
    {
        #region Selection

        public static List<TblParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, Int32 prodSpecId, Int32 stateId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblParityDetailsTO> list = null;
                if (parityId == 0)
                    list = TblParityDetailsDAO.SelectAllLatestParityDetails(stateId, prodSpecId, conn, tran);
                else
                {
                    list = TblParityDetailsDAO.SelectAllTblParityDetails(parityId, prodSpecId, conn, tran);
                }

                return list;
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

        public static List<TblParityDetailsTO> SelectAllEmptyParityDetailsList(Int32 prodSpecId, Int32 stateId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblParityDetailsTO> list = null;
                list = TblParityDetailsDAO.SelectAllLatestParityDetails(stateId, prodSpecId, conn, tran);
                return list;
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

        public static List<TblParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId,int prodSpecId, SqlConnection conn,SqlTransaction tran)
        {
            return TblParityDetailsDAO.SelectAllTblParityDetails(parityId,prodSpecId,conn,tran);
        }

        public static TblParityDetailsTO SelectTblParityDetailsTO(Int32 idParityDtl)
        {
            return  TblParityDetailsDAO.SelectTblParityDetails(idParityDtl);
        }

        public static List<TblParityDetailsTO> SelectAllParityDetailsListByIds(String parityDtlIds, SqlConnection conn, SqlTransaction tran)
        {
            return TblParityDetailsDAO.SelectAllParityDetailsListByIds(parityDtlIds, conn, tran);
        }

        #endregion



        //Sudhir[20-MARCH-2018] Added for get List of All Parity Details List
        public static List<TblParityDetailsTO> SelectAllParityDetailsList()
        {
            List<TblParityDetailsTO> ParityDetailsList = TblParityDetailsDAO.SelectAllParityDetails();
            if (ParityDetailsList != null && ParityDetailsList.Count > 0)
                return ParityDetailsList;
            else
                return null;

        }

        //Sudhir[20-MARCH-2018] Added for get List of All Parity Details List
        public static List<TblParityDetailsTO> SelectAllParityDetailsList(SqlConnection conn, SqlTransaction tran)
        {
            List<TblParityDetailsTO> ParityDetailsList = TblParityDetailsDAO.SelectAllParityDetails(conn,tran);
            if (ParityDetailsList != null && ParityDetailsList.Count > 0)
                return ParityDetailsList;
            else
                return null;

        }


        /// <summary>
        /// Sudhir[21-MARCH-2018] Added to Save Parity Details List For Other Item . 
        /// </summary>
        /// <param name="tblParitySummaryTO"></param>
        /// <returns></returns>
        public static ResultMessage SaveParityDetailsOtherItem(TblParitySummaryTO tblParitySummaryTO, Int32 isForUpdate)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (tblParitySummaryTO != null)
                {
                    List<TblParityDetailsTO> parityDetailsList = tblParitySummaryTO.ParityDetailList;
                    int result = -1;
                    if (parityDetailsList != null && parityDetailsList.Count > 0)
                    {

                        Int32 productItemId = parityDetailsList[0].ProdItemId;
                        //Int32 brandId = parityDetailsList[0].BrandId;

                        if (isForUpdate == 1)
                        {
                            //Select the TO Where parityDetailsTo isUpdate=1
                            TblParityDetailsTO parityDetailsTO = tblParitySummaryTO.ParityDetailList.Where(ele => ele.IsForUpdate == 1).FirstOrDefault();
                            if (parityDetailsTO != null)
                            {
                                result = DeactivateParityDetailsForUpdate(parityDetailsTO, conn, tran);
                                if (result == -1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Deactivating Updating Single Record");
                                    return resultMessage;
                                }

                                parityDetailsTO.CreatedBy = tblParitySummaryTO.CreatedBy;
                                parityDetailsTO.CreatedOn = tblParitySummaryTO.CreatedOn;
                                parityDetailsTO.IsActive = 1;
                                result = InsertTblParityDetails(parityDetailsTO, conn, tran);

                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Inserting New parityDetails on Single Record");
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            //1) Deactivate Record Against ProductItemId and BrandId Which are Aleready Active

                            for (int i = 0; i < parityDetailsList.Count; i++)
                            {
                                result = DeactivateParityDetails(parityDetailsList[i], conn, tran);
                                if (result == -1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Deactivating ParityDetails");
                                    return resultMessage;
                                }
                            }

                            //2)Insert one by one into parityDetails
                            foreach (TblParityDetailsTO parityDetailsTo in parityDetailsList)
                            {
                                parityDetailsTo.CreatedBy = tblParitySummaryTO.CreatedBy;
                                parityDetailsTo.CreatedOn = tblParitySummaryTO.CreatedOn;
                                parityDetailsTo.IsActive = 1;
                                result = InsertTblParityDetails(parityDetailsTo, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Adding Record into TblParityDetails");
                                    return resultMessage;
                                }
                            }
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While Adding Record into TblParityDetails");
                                return resultMessage;
                            }
                        }

                        tran.Commit();
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Parity Details List Found Null");
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour("Parity Summary Found Null");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveParityDetailsOtherItem");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// Sudhir[21-March-2018] Added for Deactivate the ParityDetails List Based On ProductItemId And Brand Id
        /// </summary>
        /// <param name="productItemId"></param>
        /// <returns></returns>
        public static Int32 DeactivateParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = -1;
            if (tblParityDetailsTO != null)
            {
                result = TblParityDetailsDAO.DeactivateAllParityDetails(tblParityDetailsTO, conn, tran);
            }
            return result;
        }


        /// <summary>
        /// Sudhir[21-March-2018] Added for Deactivate the ParityDetails List Based On ProductItemId And Brand Id
        /// </summary>
        /// <param name="productItemId"></param>
        /// <returns></returns>
        public static Int32 DeactivateParityDetailsForUpdate(TblParityDetailsTO parityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = -1;
            if (parityDetailsTO != null)
            {
                result = TblParityDetailsDAO.DeactivateAllParityDetailsForUpdate(parityDetailsTO, conn, tran);
            }
            return result;
        }

        ///// <summary>
        ///// Sudhir[20-March-2018] Added for Get Parity List Based on B
        ///// </summary>
        ///// <param name="productItemId"></param>
        ///// <param name="brandId"></param>
        ///// <param name="prodCatId"></param>
        ///// <param name="prodSpeecId"></param>
        ///// <param name="materialId"></param>
        ///// <returns></returns>
        ///// 
        //public static List<TblParityDetailsTO> SelectAllParityDetailsToList(Int32 productItemId, Int32 prodCatId, Int32 prodSpecId, Int32 materialId)
        //{
        //    List<TblParityDetailsTO> parityDetailslist = TblParityDetailsDAO.SelectAllParityDetailsOnProductItemId(productItemId, prodCatId, prodSpecId, materialId);
        //    if (parityDetailslist != null && parityDetailslist.Count > 0)
        //        return parityDetailslist;
        //    else
        //        return null;
        //}


        /// <summary>
        /// Priyanka [14-09-2018] Added for Get Parity List Based on B
        /// </summary>
        /// <param name="productItemId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="stateId"></param>
        /// <param name="currencyId"></param>
        /// <param name="productSpecInfoListTo"></param>
        /// <returns></returns>
        
        public static List<TblParityDetailsTO> SelectAllParityDetailsToList(Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo)
        {
            List<TblParityDetailsTO> parityDetailslist = TblParityDetailsDAO.SelectAllParityDetailsOnProductItemId(productItemId, prodCatId, stateId, currencyId, productSpecInfoListTo);
            if (parityDetailslist != null && parityDetailslist.Count > 0)
                return parityDetailslist;
            else
                return null;
        }


        ////Sudhir[20-MARCH-2018] Added for get List of All Parity Details List on ProductItem Id
        //public static List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 productItemId, Int32 prodCatId, Int32 prodSpecId, Int32 materialId)
        //{
        //    List<DimStateTO> allStateList = DimStateBL.SelectAllDimState().OrderBy(ele => ele.StateName).ToList(); 
        //    //List<TblParityDetailsTO> allParityDetailsList = TblParityDetailsDAO.SelectAllParityDetails();
        //    List<TblParityDetailsTO> productItemIdparityDetailsList = SelectAllParityDetailsToList(productItemId, prodCatId, prodSpecId, materialId);
        //    List<TblParityDetailsTO> newParityDetailsList = new List<TblParityDetailsTO>();
        //    if (allStateList != null && allStateList.Count > 0)
        //    {
        //        foreach (DimStateTO dimStateTO in allStateList)
        //        {
        //            TblParityDetailsTO tblParityDetailsTO = new TblParityDetailsTO();
        //            if (productItemIdparityDetailsList != null && productItemIdparityDetailsList.Count > 0)
        //            {
        //                tblParityDetailsTO = productItemIdparityDetailsList.Where(ele => ele.ProdItemId == productItemId && ele.StateId == dimStateTO.IdState
        //                                        && ele.IsActive == 1  && ele.ProdCatId == prodCatId
        //                                        && ele.ProdSpecId == prodSpecId && ele.MaterialId == materialId).FirstOrDefault();

        //                if (tblParityDetailsTO == null)
        //                {
        //                    tblParityDetailsTO = new TblParityDetailsTO();
        //                    tblParityDetailsTO.StateName = dimStateTO.StateName;
        //                    tblParityDetailsTO.StateId = dimStateTO.IdState;
        //                    tblParityDetailsTO.ProdItemId = productItemId;
        //                    tblParityDetailsTO.ProdCatId = prodCatId;
        //                    tblParityDetailsTO.ProdSpecId = prodSpecId;
        //                    tblParityDetailsTO.MaterialId = materialId;
        //                }
        //                else
        //                {
        //                    if (tblParityDetailsTO.StateId == dimStateTO.IdState)
        //                    {
        //                        tblParityDetailsTO.StateName = dimStateTO.StateName;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                tblParityDetailsTO = new TblParityDetailsTO();
        //                tblParityDetailsTO.StateName = dimStateTO.StateName;
        //                tblParityDetailsTO.StateId = dimStateTO.IdState;
        //                tblParityDetailsTO.ProdItemId = productItemId;
        //                tblParityDetailsTO.ProdCatId = prodCatId;
        //                tblParityDetailsTO.ProdSpecId = prodSpecId;
        //                tblParityDetailsTO.MaterialId = materialId;
        //            }
        //            newParityDetailsList.Add(tblParityDetailsTO);
        //        }
        //    }
        //    return newParityDetailsList;
        //}



        /// <summary>
        ///  Priyanka [14-09-2018] Added for get List of All Parity Details List on ProductItem Id
        /// </summary>
        /// <param name="productItemId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="stateId"></param>
        /// <param name="currencyId"></param>
        /// <param name="productSpecInfoListTo"></param>
        /// <returns></returns>

        public static List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo)
        {
            List<DimStateTO> allStateList = DimStateBL.SelectAllDimState().OrderBy(ele => ele.StateName).ToList();
            //List<TblParityDetailsTO> allParityDetailsList = TblParityDetailsDAO.SelectAllParityDetails();
            List<TblParityDetailsTO> productItemIdparityDetailsList = SelectAllParityDetailsToList(productItemId, prodCatId, stateId, currencyId, productSpecInfoListTo);
            //List<TblParityDetailsTO> newParityDetailsList = new List<TblParityDetailsTO>();
            //if (allStateList != null && allStateList.Count > 0)
            //{
            //    foreach (DimStateTO dimStateTO in allStateList)
            //    {
            //        TblParityDetailsTO tblParityDetailsTO = new TblParityDetailsTO();
            //        if (productItemIdparityDetailsList != null && productItemIdparityDetailsList.Count > 0)
            //        {
            //            tblParityDetailsTO = productItemIdparityDetailsList.Where(ele => ele.ProdItemId == productItemId && ele.StateId == dimStateTO.IdState
            //                                    && ele.IsActive == 1 && ele.ProdCatId == prodCatId
            //                                    && ele.ProdSpecId == prodSpecId && ele.MaterialId == materialId).FirstOrDefault();

            //            if (tblParityDetailsTO == null)
            //            {
            //                tblParityDetailsTO = new TblParityDetailsTO();
            //                tblParityDetailsTO.StateName = dimStateTO.StateName;
            //                tblParityDetailsTO.StateId = dimStateTO.IdState;
            //                tblParityDetailsTO.ProdItemId = productItemId;
            //                tblParityDetailsTO.ProdCatId = prodCatId;
            //                tblParityDetailsTO.ProdSpecId = prodSpecId;
            //                tblParityDetailsTO.MaterialId = materialId;
            //            }
            //            else
            //            {
            //                if (tblParityDetailsTO.StateId == dimStateTO.IdState)
            //                {
            //                    tblParityDetailsTO.StateName = dimStateTO.StateName;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            tblParityDetailsTO = new TblParityDetailsTO();
            //            tblParityDetailsTO.StateName = dimStateTO.StateName;
            //            tblParityDetailsTO.StateId = dimStateTO.IdState;
            //            tblParityDetailsTO.ProdItemId = productItemId;
            //            tblParityDetailsTO.ProdCatId = prodCatId;
            //            tblParityDetailsTO.ProdSpecId = prodSpecId;
            //            tblParityDetailsTO.MaterialId = materialId;
            //        }
            //        newParityDetailsList.Add(tblParityDetailsTO);
            //    }
            //}
            return productItemIdparityDetailsList;
        }






        /// <summary>
        /// Sudhir[23-MARCH-2018] Added for Get ParityDetail List based on Booking DateTime and Other Combination
        /// </summary>
        /// <returns></returns>
        public static TblParityDetailsTO SelectParityDetailToListOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 stateId, DateTime boookingDate)
        {
            List<TblParityDetailsTO> parityDetailslist = TblParityDetailsDAO.SelectParityDetailToListOnBooking(materialId, prodCatId, prodSpecId, productItemId, stateId, boookingDate);
            if (parityDetailslist != null && parityDetailslist.Count != 0)
            {
                TblParityDetailsTO tblParityDetailsTO = parityDetailslist.FirstOrDefault();
                return tblParityDetailsTO;
            }
            else
            {
                //Create Null To And Return That 
                TblParityDetailsTO tblParityDetailsTO = CreateEmptyParityDetailsTo(materialId, prodCatId, prodSpecId, productItemId,  stateId, boookingDate);
                return tblParityDetailsTO;
            }
        }


        /// <summary>
        /// Sudhir[23-MARCH-2018] Added for Get ParityDetail List based on Booking DateTime and Other Combination
        /// </summary>
        /// <returns></returns>
        public static TblParityDetailsTO CreateEmptyParityDetailsTo(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId,  Int32 stateId, DateTime boookingDate)
        {
            TblParityDetailsTO tblParityDetailsTO = new TblParityDetailsTO();
            tblParityDetailsTO.MaterialId = materialId;
            tblParityDetailsTO.ProdCatId = prodCatId;
            tblParityDetailsTO.ProdSpecId = prodSpecId;
            tblParityDetailsTO.ProdItemId = productItemId;
            tblParityDetailsTO.StateId = stateId;
            return tblParityDetailsTO;
        }


        /// <summary>
        /// Vijaymala[19-06-2018] Added for Get ParityDetail List based on Booking DateTime and state id
        /// </summary>
        /// <returns></returns>
        public static TblParityDetailsTO SelectParityDetailToListOnBooking(Int32 stateId, DateTime boookingDate)
        {
            List<TblParityDetailsTO> parityDetailslist = TblParityDetailsDAO.SelectParityDetailToListOnBooking( stateId, boookingDate);
            if (parityDetailslist != null && parityDetailslist.Count != 0)
            {
                //0th position record as in select query order by in desc
                TblParityDetailsTO tblParityDetailsTO = parityDetailslist[0];
                return tblParityDetailsTO;
            }
            else
            {
                //Create Null To And Return That 
                TblParityDetailsTO tblParityDetailsTO = new TblParityDetailsTO();
                return tblParityDetailsTO;
            }
        }

        #region Insertion
        public static int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        {
            return TblParityDetailsDAO.InsertTblParityDetails(tblParityDetailsTO);
        }

        public static int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblParityDetailsDAO.InsertTblParityDetails(tblParityDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        {
            return TblParityDetailsDAO.UpdateTblParityDetails(tblParityDetailsTO);
        }

        public static int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblParityDetailsDAO.UpdateTblParityDetails(tblParityDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblParityDetails(Int32 idParityDtl)
        {
            return TblParityDetailsDAO.DeleteTblParityDetails(idParityDtl);
        }

        public static int DeleteTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblParityDetailsDAO.DeleteTblParityDetails(idParityDtl, conn, tran);
        }

        #endregion

        #region Sudhir [30-APR-2018] Added for the Migrate Parity Data.
        public static ResultMessage MigrateParityRelatedData()
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

                //Get All ParitySummry List
                List<TblParitySummaryTO> allparitySummuryList = BL.TblParitySummaryBL.SelectAllTblParitySummaryList(conn, tran);

                //Get All Parity Details List.
                List<TblParityDetailsTO> allParityDetailsList = BL.TblParityDetailsBL.SelectAllParityDetailsList(conn, tran);

                //Set Parity Is is Null to tblBookings.
                result = BL.TblBookingsBL.UpdateParityIdNull(conn, tran);
                if (result == -1 || result == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Updaiton of parityId is NULL IN tblBookings");
                    resultMessage.DisplayMessage = "Error in Migration Data.";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                //Set ParityId Is is Null to tblBookingParities.
                //result = BL.TblBookingParitiesBL.UpdateParityIdIsNull(conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Updaiton of parityId is NULL tblBookingParities");
                    resultMessage.DisplayMessage = "Error in Migration Data.";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                //Update Parity Details Based on ParityId
                for (int i = 0; i < allparitySummuryList.Count; i++)
                {
                    TblParitySummaryTO tblParitySummaryTO = allparitySummuryList[i];
                    List<TblParityDetailsTO> tempParityDetails = allParityDetailsList.Where(ele => ele.ParityId == tblParitySummaryTO.IdParity).ToList();
                    if (tempParityDetails != null && tempParityDetails.Count > 0)
                    {
                        foreach (TblParityDetailsTO tblParityDetailsTO in tempParityDetails)
                        {
                            tblParityDetailsTO.BaseValCorAmt = tblParitySummaryTO.BaseValCorAmt;
                            tblParityDetailsTO.FreightAmt = tblParitySummaryTO.FreightAmt;
                            tblParityDetailsTO.ExpenseAmt = tblParitySummaryTO.ExpenseAmt;
                            tblParityDetailsTO.OtherAmt = tblParitySummaryTO.OtherAmt;
                            //tblParityDetailsTO.BrandId = tblParitySummaryTO.BrandId;
                            tblParityDetailsTO.StateId = tblParitySummaryTO.StateId;
                            tblParityDetailsTO.ParityId = 0;
                            tblParityDetailsTO.IsActive = tblParitySummaryTO.IsActive;
                            result = BL.TblParityDetailsBL.UpdateTblParityDetails(tblParityDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While Updaiton of ParityDetails From ParitySummry");
                                resultMessage.DisplayMessage = "Error in Migration Data.";
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }
                    }
                }

                //Delete Parity Details Based on ParityId
                result = BL.TblParitySummaryBL.DeleteAllTblParitySummary(conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Deleting of DeleteAllTblParity");
                    resultMessage.DisplayMessage = "Error in Migration Data.";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.DisplayMessage = "Migrate Data Succesfully";
                resultMessage.MessageType = ResultMessageE.Information;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "MigrateParityRelatedData");
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

    }
}
