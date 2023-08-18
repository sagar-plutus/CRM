using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblPersonAddrDtlBL
    {
        #region Selection
        public static List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtl()
        {
            return TblPersonAddrDtlDAO.SelectAllTblPersonAddrDtl();
        }

        public static List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtlList()
        {
            List<TblPersonAddrDtlTO> tblPersonAddrDtlTODT = TblPersonAddrDtlDAO.SelectAllTblPersonAddrDtl();
            return tblPersonAddrDtlTODT;
        }

        public static TblPersonAddrDtlTO SelectTblPersonAddrDtlTO(Int32 idPersonAddrDtl)
        {
            TblPersonAddrDtlTO tblPersonAddrDtlTO = TblPersonAddrDtlDAO.SelectTblPersonAddrDtl(idPersonAddrDtl);
            if (tblPersonAddrDtlTO != null)
                return tblPersonAddrDtlTO;
            else
                return null;
        }

        public static TblPersonAddrDtlTO SelectTblPersonAddrDtlTO(Int32 personId, Int32 addressTypeId)
        {
            TblPersonAddrDtlTO tblPersonAddrDtlTO = TblPersonAddrDtlDAO.SelectTblPersonAddrDtl(personId, addressTypeId);
            if (tblPersonAddrDtlTO != null)
                return tblPersonAddrDtlTO;
            else
                return null;
        }


        public static TblAddressTO SelectAddressTOonPersonAddrDtlId(Int32 personId, Int32 addressTypeId)
        {
            TblPersonAddrDtlTO tblPersonAddrDtlTO = SelectTblPersonAddrDtlTO(personId, addressTypeId);
            if (tblPersonAddrDtlTO != null)
            {
                TblAddressTO tblAddressTO = TblAddressBL.SelectTblAddressTO(tblPersonAddrDtlTO.AddressId);
                if (tblAddressTO != null)
                {
                    return tblAddressTO;
                }
                else
                    return null;

            }
            else
                return null;

        }


        #endregion

        #region Insertion
        public static int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO)
        {
            return TblPersonAddrDtlDAO.InsertTblPersonAddrDtl(tblPersonAddrDtlTO);
        }

        public static int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPersonAddrDtlDAO.InsertTblPersonAddrDtl(tblPersonAddrDtlTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO)
        {
            return TblPersonAddrDtlDAO.UpdateTblPersonAddrDtl(tblPersonAddrDtlTO);
        }

        public static int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPersonAddrDtlDAO.UpdateTblPersonAddrDtl(tblPersonAddrDtlTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl)
        {
            return TblPersonAddrDtlDAO.DeleteTblPersonAddrDtl(idPersonAddrDtl);
        }

        public static int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblPersonAddrDtlDAO.DeleteTblPersonAddrDtl(idPersonAddrDtl, conn, tran);
        }

        #endregion
    }
}
