using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblItemBroadCategoriesTO
    {

        #region Declarations
        Int32 idItemBroadCategories;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String name;
        String description;
        String remark;
        Int32 weightMeasurUnitId;
        #endregion

        #region Constructor
        public TblItemBroadCategoriesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdItemBroadCategories
        {
            get { return idItemBroadCategories; }
            set { idItemBroadCategories = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Description
        {
            get { return description; }
            set { description = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public int WeightMeasurUnitId { get => weightMeasurUnitId; set => weightMeasurUnitId = value; }
        #endregion
    }
}
