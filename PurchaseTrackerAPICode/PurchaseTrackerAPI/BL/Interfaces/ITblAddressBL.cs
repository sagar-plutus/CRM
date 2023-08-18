using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAddressBL
    {
        List<TblAddressTO> SelectAllTblAddressList();
          TblAddressTO SelectTblAddressTO(Int32 idAddr);
        TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS);
        TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE, SqlConnection conn, SqlTransaction tran);
        List<TblAddressTO> SelectOrgAddressList(Int32 orgId);
        List<TblAddressTO> SelectOrgAddressDetailOfRegion(string orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS);
        int InsertTblAddress(TblAddressTO tblAddressTO);
        int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAddress(TblAddressTO tblAddressTO);
        int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran);
          int DeleteTblAddress(Int32 idAddr);
        int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran);

    }
}