using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblOtherDesignationsTO
    {
        #region Declarations
        Int32 idOtherDesignation;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String name;
        String designationDesc;
        String remark;
        #endregion

        #region Constructor
        public TblOtherDesignationsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOtherDesignation
        {
            get { return idOtherDesignation; }
            set { idOtherDesignation = value; }
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
        public String DesignationDesc
        {
            get { return designationDesc; }
            set { designationDesc = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        #endregion

    }
}
