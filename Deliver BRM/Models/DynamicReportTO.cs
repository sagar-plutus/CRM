using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class DynamicReportTO
    {
        List<DropDownTO> dropDownList;

        public List<DropDownTO> DropDownList { get => dropDownList; set => dropDownList = value; }
    }
}
