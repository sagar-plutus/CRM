using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class DayWiseRateChartTO
    {

        #region Declaration
        String date;
        double declaredRate;
        double variableRate;
        double unldRateOfScrap;
        double unldRateOfSponge;
        double unldRateOfBillets;
        double unldRateOfSillicoMn;
        double declaredRateOfTMTBars;
        double saleRateOfTMTBars;
        String billType;

        #endregion

        #region Get Set

        public string Date { get => date; set => date = value; }

        public double DeclaredRate { get => declaredRate; set => declaredRate = value; }
        public double VariableRate { get => variableRate; set => variableRate = value; }
        public double UnldRateOfScrap { get => unldRateOfScrap; set => unldRateOfScrap = value; }
        public double UnldRateOfSponge { get => unldRateOfSponge; set => unldRateOfSponge = value; }
        public double UnldRateOfBillets { get => unldRateOfBillets; set => unldRateOfBillets = value; }
        public double UnldRateOfSillicoMn { get => unldRateOfSillicoMn; set => unldRateOfSillicoMn = value; }
        public double DeclaredRateOfTMTBars { get => declaredRateOfTMTBars; set => declaredRateOfTMTBars = value; }
        public double SaleRateOfTMTBars { get => saleRateOfTMTBars; set => saleRateOfTMTBars = value; }

        public String BillType { get => billType; set => billType = value; }

        #endregion





    }
}
