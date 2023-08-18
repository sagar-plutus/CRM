using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TbltaskWithoutSubscBL
    {
        #region Selection
        public static List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubsc()
        {
            return TbltaskWithoutSubscDAO.SelectAllTbltaskWithoutSubsc();
        }

        public static List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubscList()
        {
            return TbltaskWithoutSubscDAO.SelectAllTbltaskWithoutSubsc();
        }
        /// <summary>
        /// Sudhir[30-AUG-2018]
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static List<TbltaskWithoutSubscTO> SelectTbltaskWithoutSubscList(Int32 moduleId,Int32 entityId)
        {
            return TbltaskWithoutSubscDAO.SelectTbltaskWithoutSubscList(moduleId, entityId);
        }

        public static TbltaskWithoutSubscTO SelectTbltaskWithoutSubscTO(Int32 idTaskWithoutSubsc)
        {
            TbltaskWithoutSubscTO tbltaskWithoutSubscTODT = TbltaskWithoutSubscDAO.SelectTbltaskWithoutSubsc(idTaskWithoutSubsc);
            if(tbltaskWithoutSubscTODT != null)
                return tbltaskWithoutSubscTODT;
            else
                return null;
        }
        #endregion

        #region Insertion
        public static int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO)
        {
            return TbltaskWithoutSubscDAO.InsertTbltaskWithoutSubsc(tbltaskWithoutSubscTO);
        }

        public static int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran)
        {
            return TbltaskWithoutSubscDAO.InsertTbltaskWithoutSubsc(tbltaskWithoutSubscTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO)
        {
            return TbltaskWithoutSubscDAO.UpdateTbltaskWithoutSubsc(tbltaskWithoutSubscTO);
        }

        public static int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran)
        {
            return TbltaskWithoutSubscDAO.UpdateTbltaskWithoutSubsc(tbltaskWithoutSubscTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc)
        {
            return TbltaskWithoutSubscDAO.DeleteTbltaskWithoutSubsc(idTaskWithoutSubsc);
        }

        public static int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc, SqlConnection conn, SqlTransaction tran)
        {
            return TbltaskWithoutSubscDAO.DeleteTbltaskWithoutSubsc(idTaskWithoutSubsc, conn, tran);
        }

        #endregion
    }
}
