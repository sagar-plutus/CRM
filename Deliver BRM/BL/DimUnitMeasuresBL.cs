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
    public class DimUnitMeasuresBL
    {
        #region Selection
        public static List<DimUnitMeasuresTO> SelectAllDimUnitMeasuresList()
        {
            return DimUnitMeasuresDAO.SelectAllDimUnitMeasures();
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] Added to select all measurement units for drop down
        /// </summary>
        /// <returns></returns>
        public static List<DropDownTO> SelectAllUnitMeasuresListForDropDown(int unitMeasureTypeId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = DimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDown(unitMeasureTypeId);

                if (list != null)
                {
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresListForDropDown");
                return null;
            }
        }
        /// <summary>
        /// Kiran [08-Sep-2018] Added to select all measurement units for drop down Using CatId
        /// </summary>
        /// <returns></returns>
        public static List<DropDownTO> SelectAllUnitMeasuresForDropDownByCatId(Int32 unitCatId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = DimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDownByCatId(unitCatId);

                if (list != null)
                {
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDownByCatId");
                return null;
            }
        }

        public static DimUnitMeasuresTO SelectDimUnitMeasuresTO(Int32 idWeightMeasurUnit)
        {
            return DimUnitMeasuresDAO.SelectDimUnitMeasures(idWeightMeasurUnit);
        }

        #endregion

        #region Insertion
        public static int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO)
        {
            return DimUnitMeasuresDAO.InsertDimUnitMeasures(dimUnitMeasuresTO);
        }

        public static int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimUnitMeasuresDAO.InsertDimUnitMeasures(dimUnitMeasuresTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO)
        {
            return DimUnitMeasuresDAO.UpdateDimUnitMeasures(dimUnitMeasuresTO);
        }

        public static int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimUnitMeasuresDAO.UpdateDimUnitMeasures(dimUnitMeasuresTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit)
        {
            return DimUnitMeasuresDAO.DeleteDimUnitMeasures(idWeightMeasurUnit);
        }

        public static int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit, SqlConnection conn, SqlTransaction tran)
        {
            return DimUnitMeasuresDAO.DeleteDimUnitMeasures(idWeightMeasurUnit, conn, tran);
        }

        #endregion

    }
}
