using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblBookingDelAddrBL
    {
        #region Selection
       
        /// <summary>
        ///  Aditee [04-02-2020] : Added to get the existing booking address from existing bookings.
        /// </summary>
        /// <param name="dealerOrgId"></param>
        /// <returns></returns>
        public static List<TblBookingDelAddrTO> SelectExistingBookingAddrListByDealerId(Int32 dealerOrgId, String txnAddrTypeId)
        {
            Int32 cnt = 0;
            String txnAddrTypeIdtemp = String.Empty;

            if (!String.IsNullOrEmpty(txnAddrTypeId))
            {
                txnAddrTypeIdtemp = txnAddrTypeId;
            }

            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_EXISTING_ADDRESS_COUNT_FOR_BOOKING);
            if (tblConfigParamsTO != null)
            {
                cnt = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            }

            List<TblBookingDelAddrTO> tblBookingDelAddrTOList = new List<TblBookingDelAddrTO>();
            try
            {
                List<TblBookingDelAddrTO> tblBookingDelAddrTOListtemp = new List<TblBookingDelAddrTO>();
                tblBookingDelAddrTOList = TblBookingDelAddrDAO.SelectTblBookingsByDealerOrgId(dealerOrgId, txnAddrTypeIdtemp,cnt);
                //tblBookingDelAddrTOList = tblBookingDelAddrTOList.Where(ele => ele.BillingName != null || ele.Address != null).ToList();
                tblBookingDelAddrTOList = tblBookingDelAddrTOList.GroupBy(c => new { c.BillingName,c.Address,c.GstNo,c.PanNo,c.ContactNo }).Select(s => s.FirstOrDefault()).ToList();
                if (cnt > tblBookingDelAddrTOList.Count)
                {
                    tblBookingDelAddrTOListtemp = tblBookingDelAddrTOList;
                }
                else
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        TblBookingDelAddrTO tblBookingDelAddrTO = tblBookingDelAddrTOList[i];
                        tblBookingDelAddrTOListtemp.Add(tblBookingDelAddrTO);
                    }
                }
                return tblBookingDelAddrTOListtemp;
                //return tblBookingDelAddrTOList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrList()
        {
            return TblBookingDelAddrDAO.SelectAllTblBookingDelAddr();
        }
        public static List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId)
        {
            return TblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(scheduleId);
        }

        public static List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrList(int bookingId)
        {
            return TblBookingDelAddrDAO.SelectAllTblBookingDelAddr(bookingId);
        }

        public static List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrList(int bookingId, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.SelectAllTblBookingDelAddr(bookingId, conn, tran);
        }

        public static TblBookingDelAddrTO SelectTblBookingDelAddrTO(Int32 idBookingDelAddr)
        {
            return  TblBookingDelAddrDAO.SelectTblBookingDelAddr(idBookingDelAddr);
           
        }

        public static List<TblBookingDelAddrTO> SelectDeliveryAddrListFromDealer(Int32 addrSourceTypeId, Int32 entityId)
        {
            List<TblBookingDelAddrTO> list = null;
            if (addrSourceTypeId == (int)Constants.AddressSourceTypeE.FROM_BOOKINGS)
                list = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrList(entityId);
            else
            {
                list = new List<TblBookingDelAddrTO>();
                Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS;
                TblAddressTO tblAddressTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(entityId, addressTypeE);
                if (tblAddressTO == null)
                    return null;

                TblBookingDelAddrTO bookingDelAddrTO = new TblBookingDelAddrTO();
                String address = string.Empty;
                if (!string.IsNullOrEmpty(tblAddressTO.PlotNo))
                    address += tblAddressTO.PlotNo + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.StreetName))
                    address += tblAddressTO.StreetName + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.AreaName))
                    address += tblAddressTO.AreaName + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.VillageName))
                    address += tblAddressTO.VillageName + ",";

                bookingDelAddrTO.Address = address;
                bookingDelAddrTO.BillingName = DAL.TblOrganizationDAO.SelectFirmNameOfOrganiationById(entityId);
                bookingDelAddrTO.DistrictName = tblAddressTO.DistrictName;
                bookingDelAddrTO.TalukaName = tblAddressTO.TalukaName;
                bookingDelAddrTO.VillageName = tblAddressTO.VillageName;
                bookingDelAddrTO.StateId = tblAddressTO.StateId;
                bookingDelAddrTO.State = tblAddressTO.StateName;
                bookingDelAddrTO.Pincode = tblAddressTO.Pincode;
                List<TblOrgLicenseDtlTO> licList = BL.TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(entityId);
                if (licList != null)
                {
                    var gstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                    if (gstNoTO == null || string.IsNullOrEmpty(gstNoTO.LicenseValue))
                    {
                        var tempGstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.SGST_NO).FirstOrDefault();
                        if (tempGstNoTO != null && !string.IsNullOrEmpty(tempGstNoTO.LicenseValue))
                        {
                            if (tempGstNoTO.LicenseValue != "0")
                                bookingDelAddrTO.GstNo = tempGstNoTO.LicenseValue;
                        }
                    }
                    else if (gstNoTO.LicenseValue == "0")
                    {
                        var tempGstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.SGST_NO).FirstOrDefault();
                        if (tempGstNoTO != null && !string.IsNullOrEmpty(tempGstNoTO.LicenseValue))
                        {
                            if (tempGstNoTO.LicenseValue != "0")
                                bookingDelAddrTO.GstNo = tempGstNoTO.LicenseValue;
                        }
                    }
                    else
                        bookingDelAddrTO.GstNo = gstNoTO.LicenseValue;

                    var panTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                    if (panTO != null && !string.IsNullOrEmpty(panTO.LicenseValue))
                    {
                        if (panTO.LicenseValue != "0")
                            bookingDelAddrTO.PanNo = panTO.LicenseValue;
                    }
                }

                list.Add(bookingDelAddrTO);
            }

            return list;
        }

        public static List<TblBookingDelAddrTO> GetAllAddressesDetailsList(Int32 OrganisationId)
        {
            List<TblBookingDelAddrTO> list = null;
            List<TblAddressTO> tblAddressTOList = BL.TblAddressBL.SelectOrgAddressList(OrganisationId);
            if (tblAddressTOList == null || tblAddressTOList.Count == 0)
                return null;
            list = new List<TblBookingDelAddrTO>();
            for (int i = 0; i < tblAddressTOList.Count; i++)
            {
                TblAddressTO tblAddressTO = tblAddressTOList[i];
                TblBookingDelAddrTO bookingDelAddrTO = new TblBookingDelAddrTO();
                String address = string.Empty;
                if (!string.IsNullOrEmpty(tblAddressTO.PlotNo))
                    address += tblAddressTO.PlotNo;
                if (!string.IsNullOrEmpty(tblAddressTO.StreetName))
                {
                    if (!String.IsNullOrEmpty(address)) {
                        address += ", ";
                    }
                    address += tblAddressTO.StreetName;
                }
                   
                if (!string.IsNullOrEmpty(tblAddressTO.AreaName))
                {
                    if (!String.IsNullOrEmpty(address))
                    {
                        address += ", ";
                    }
                    address += tblAddressTO.AreaName;
                }
                if (!string.IsNullOrEmpty(tblAddressTO.VillageName))
                {
                    if (!String.IsNullOrEmpty(address))
                    {
                        address += ", ";
                    }
                    address += tblAddressTO.VillageName;
                }
                  
                bookingDelAddrTO.Address = address;
                bookingDelAddrTO.AddrId = tblAddressTO.IdAddr;
                bookingDelAddrTO.AddressType = tblAddressTO.AddressType;
                bookingDelAddrTO.BillingName = DAL.TblOrganizationDAO.SelectFirmNameOfOrganiationById(OrganisationId);
                Int32 DefaultAddress = DAL.TblOrganizationDAO.SelectDefaultAddrOfOrganiationById(OrganisationId);
                bookingDelAddrTO.IsDefault = 0;
                if (DefaultAddress == bookingDelAddrTO.AddrId)
                {
                    bookingDelAddrTO.IsDefault = 1;
                }
                bookingDelAddrTO.DistrictName = tblAddressTO.DistrictName;
                bookingDelAddrTO.TalukaName = tblAddressTO.TalukaName;
                bookingDelAddrTO.VillageName = tblAddressTO.VillageName;
                bookingDelAddrTO.StateId = tblAddressTO.StateId;
                bookingDelAddrTO.State = tblAddressTO.StateName;
                bookingDelAddrTO.Pincode = tblAddressTO.Pincode;

                List<TblOrgLicenseDtlTO> licList = BL.TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(OrganisationId);
                if (licList != null)
                {
                    var gstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                    if (gstNoTO == null || string.IsNullOrEmpty(gstNoTO.LicenseValue))
                    {
                        var tempGstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.SGST_NO).FirstOrDefault();
                        if (tempGstNoTO != null && !string.IsNullOrEmpty(tempGstNoTO.LicenseValue))
                        {
                            if (tempGstNoTO.LicenseValue != "0")
                                bookingDelAddrTO.GstNo = tempGstNoTO.LicenseValue;
                        }
                    }
                    else if (gstNoTO.LicenseValue == "0")
                    {
                        var tempGstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.SGST_NO).FirstOrDefault();
                        if (tempGstNoTO != null && !string.IsNullOrEmpty(tempGstNoTO.LicenseValue))
                        {
                            if (tempGstNoTO.LicenseValue != "0")
                                bookingDelAddrTO.GstNo = tempGstNoTO.LicenseValue;
                        }
                    }
                    else
                        bookingDelAddrTO.GstNo = gstNoTO.LicenseValue;

                    var panTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                    if (panTO != null && !string.IsNullOrEmpty(panTO.LicenseValue))
                    {
                        if (panTO.LicenseValue != "0")
                            bookingDelAddrTO.PanNo = panTO.LicenseValue;
                    }
                }
                list.Add(bookingDelAddrTO);
            }
            return list;
        }
        #endregion

        #region Insertion
        public static int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO)
        {
            return TblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingDelAddrTO);
        }

        public static int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO)
        {
            return TblBookingDelAddrDAO.UpdateTblBookingDelAddr(tblBookingDelAddrTO);
        }

        public static int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.UpdateTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblBookingDelAddr(Int32 idBookingDelAddr)
        {
            return TblBookingDelAddrDAO.DeleteTblBookingDelAddr(idBookingDelAddr);
        }

        public static int DeleteTblBookingDelAddr(Int32 idBookingDelAddr, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.DeleteTblBookingDelAddr(idBookingDelAddr, conn, tran);
        }

        #endregion
        
    }
}
