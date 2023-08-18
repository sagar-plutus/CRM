using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Globalization;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class ReportDAO : IReportDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly Icommondao _iCommonDAO;
        public ReportDAO(IConnectionString iConnectionString, ITblConfigParamsBL iTblConfigParamsBL, Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iCommonDAO = icommondao;
        }

        //public List<TallyReportTO> SelectTallyReportDetailsV2(DateTime fromDate, DateTime toDate)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
        //    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = "SELECT summary.idPurchaseScheduleSummary,  " +
        //            //"summary.createdOn AS date,summary.vehicleNo AS truckNo," +
        //            "summary.corretionCompletedOn AS date,summary.vehicleNo AS truckNo," +
        //            "userdetails.userDisplayName AS pm," +
        //            "proditem.itemName AS grade,details.qty AS gradeQty," +
        //            " case when  proditem.isNonCommercialItem =1 then details.qty end as dustQty ," +
        //              " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  details.rate end as gradeRate," +
        //             " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  cast((details.qty * details.rate) as decimal(10, 2)) end as total" +
        //            " , CASE WHEN summary.cOrNCId = 1 THEN  ISNULL(invoiceAddr.billingPartyName,org.firmName)  WHEN summary.cOrNCId = 0 THEN org.firmName   END AS supplierName," +
        //            " CASE WHEN summary.cOrNCId = 1 THEN 'ORDER' WHEN summary.cOrNCId = 0 THEN 'ENQUIRY' END AS billType,classifictaion.prodClassDesc AS materialType,summary.containerNo AS containerNo,tblPurchaseVehicleSpotEntry.location as location  FROM dbo.tblPurchaseScheduleSummary summary " +
        //            "LEFT JOIN tblPurchaseEnquiry PurchaseEnquiry ON summary.purchaseEnquiryId = PurchaseEnquiry.idPurchaseEnquiry " +
        //            "LEFT JOIN tblProdClassification classifictaion ON PurchaseEnquiry.prodClassId = classifictaion.idProdClass " +
        //            "LEFT JOIN dbo.tblUser userdetails ON PurchaseEnquiry.userId = userdetails.idUser " +
        //            "LEFT JOIN tblPurchaseScheduleDetails details ON summary.idPurchaseScheduleSummary = details.purchaseScheduleSummaryId " +
        //            "LEFT JOIN tblOrganization org ON summary.supplierId = org.idOrganization " +
        //            " left join tblPurchaseInvoice invoice on invoice.purSchSummaryId = summary.rootScheduleId " +
        //            " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase " +
        //            " LEFT JOIN tblProductItem proditem ON details.prodItemId = proditem.idProdItem " +
        //            " LEFT JOIN tblPurchaseVehicleSpotEntry tblPurchaseVehicleSpotEntry on tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId = isnull(summary.rootScheduleId,summary.idPurchaseScheduleSummary) " +
        //            " WHERE summary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS) 
        //            //Prajakta[2019-03-06] Commented and added
        //            //+ " AND summary.createdOn BETWEEN @fromDate AND @toDate " 
        //            + " AND  summary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
        //            + " AND isnull(summary.isCorrectionCompleted,0) =1 AND summary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
        //        SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TallyReportTO> list = ConvertDTToList(reader);
        //        reader.Close();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        //public List<TallyReportTO> SelectTallyReportDetails(DateTime fromDate, DateTime toDate, int ConfirmTypeId, int supplierId, int purchaseManagerId, int materialTypeId)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
        //    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = "SELECT summary.idPurchaseScheduleSummary,  " +
        //            //"summary.createdOn AS date,summary.vehicleNo AS truckNo," +
        //            " summary.corretionCompletedOn AS date,summary.vehicleNo AS truckNo," +
        //            " userdetails.userDisplayName AS pm," +
        //            " proditem.itemName AS grade,details.qty AS gradeQty," +
        //            " case when  proditem.isNonCommercialItem =1 then details.qty end as dustQty ," +
        //            " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  details.rate end as gradeRate," +
        //            " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  cast((details.qty * details.rate) as decimal(10, 2)) end as total" +
        //            " , CASE WHEN summary.cOrNCId = 1 THEN  ISNULL(invoiceAddr.billingPartyName,org.firmName)  WHEN summary.cOrNCId = 0 THEN org.firmName   END AS supplierName," +
        //            " CASE WHEN summary.cOrNCId = 1 THEN 'ORDER' WHEN summary.cOrNCId = 0 THEN 'ENQUIRY' END AS billType,classifictaion.prodClassDesc AS materialType,summary.containerNo AS containerNo,tblPurchaseVehicleSpotEntry.location AS location FROM dbo.tblPurchaseScheduleSummary summary " +
        //            "LEFT JOIN tblPurchaseEnquiry PurchaseEnquiry ON summary.purchaseEnquiryId = PurchaseEnquiry.idPurchaseEnquiry " +
        //            "LEFT JOIN tblProdClassification classifictaion ON PurchaseEnquiry.prodClassId = classifictaion.idProdClass " +
        //            "LEFT JOIN dbo.tblUser userdetails ON PurchaseEnquiry.userId = userdetails.idUser " +
        //            "LEFT JOIN tblPurchaseScheduleDetails details ON summary.idPurchaseScheduleSummary = details.purchaseScheduleSummaryId " +
        //            "LEFT JOIN tblOrganization org ON summary.supplierId = org.idOrganization " +
        //            " left join tblPurchaseInvoice invoice on invoice.purSchSummaryId = summary.rootScheduleId " +
        //            " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase " +
        //            " LEFT JOIN tblProductItem proditem ON details.prodItemId = proditem.idProdItem " +
        //            " LEFT JOIN tblPurchaseVehicleSpotEntry tblPurchaseVehicleSpotEntry on tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId = isnull(summary.rootScheduleId,summary.idPurchaseScheduleSummary)" +
        //            " WHERE summary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS)
        //            //Prajakta[2019-03-06] Commented and added
        //            //+ " AND summary.createdOn BETWEEN @fromDate AND @toDate " 
        //            + " AND  summary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
        //            + " AND isnull(summary.isCorrectionCompleted,0) =1 AND summary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
        //        if (ConfirmTypeId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
        //            cmdSelect.CommandText += " AND summary.cOrNCId=" + ConfirmTypeId;
        //        if (supplierId > 0)
        //            cmdSelect.CommandText += " AND summary.supplierId=" + supplierId;
        //        if (purchaseManagerId > 0)
        //            cmdSelect.CommandText += " AND PurchaseEnquiry.userId=" + purchaseManagerId;
        //        if (materialTypeId > 0)//
        //            cmdSelect.CommandText += " AND PurchaseEnquiry.prodClassId=" + materialTypeId;
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
        //        SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TallyReportTO> list = ConvertDTToList(reader);
        //        reader.Close();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        //Added by minal (Tally Report gradeRate and Total divide by 1000) 
        public List<TallyReportTO> SelectTallyReportDetails(DateTime fromDate, DateTime toDate, int ConfirmTypeId, int supplierId, String purchaseManagerIds, int materialTypeId,string vehicleIds,String dateOfBackYears, Int32 isConsiderTm = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            if (String.IsNullOrEmpty(vehicleIds))
            {
                if (isConsiderTm == 0)
                {
                    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
                    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
                }
                else
                {
                    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = (fromDate);
                    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = (toDate);
                }
            }
            String stringOfDate = String.Empty;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "WITH cte_TallyReport AS (";
                cmdSelect.CommandText += "SELECT summary.idPurchaseScheduleSummary,summary.rootScheduleId,  " +
                    //"summary.createdOn AS date,summary.vehicleNo AS truckNo," +
                    " summary.corretionCompletedOn AS date,summary.corretionCompletedOn AS correctionCompletedOn,summary.vehicleNo AS truckNo," +
                    " userdetails.userDisplayName AS pm," +
                    " proditem.itemName AS grade,proditem.displaySequanceNo,details.qty AS gradeQty," +
                    " case when  proditem.isNonCommercialItem =1 then details.qty end as dustQty ," +
                    " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  details.rate end as gradeRate," +
                    " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  (details.qty * details.rate) end as total" +
                    " , CASE WHEN summary.cOrNCId = 1 THEN  ISNULL(invoiceAddr.billingPartyName,org.firmName)  WHEN summary.cOrNCId = 0 THEN org.firmName   END AS supplierName," +
                    " CASE WHEN summary.cOrNCId = 1 THEN 'ORDER' WHEN summary.cOrNCId = 0 THEN 'ENQUIRY' END AS billType,classifictaion.prodClassDesc AS materialType,summary.containerNo AS containerNo,tblPurchaseVehicleSpotEntry.location AS location,LEFT(purchaseWeighing.godown,3) AS godown ," +
                    //" CAST((ISNULL(summary.processChargePerVeh,0)/1000) as decimal(10, 3)) AS processChargePerVeh, " +
                    " summary.processChargePerVeh AS processChargePerVeh, " +
                    " summary.cOrNCId,summary.isBoth,PurchaseEnquiry.remark,summary.correNarration,detailsGrandTotal.grandTtl, " +
                    " containerNoTbl.containerNo AS spotVehContainerNo,PurchaseEnquiry.prodClassId " +
                    " , purchaseWeighingGrossWeight.grossWeightMT As GrossWeight, purchaseWeighingTareWeight.actualWeightMT As TareWeight, (purchaseWeighingGrossWeight.grossWeightMT - purchaseWeighingTareWeight.actualWeightMT) As NetWeight " +
                    " , partyWeighingMeasures.netWt as partyNetWeight, partyWeighingMeasures.tareWt as partyTareWeight, partyWeighingMeasures.grossWt as partyGrossWeight " +
                    " FROM dbo.tblPurchaseScheduleSummary summary " +
                    " LEFT JOIN tblPurchaseEnquiry PurchaseEnquiry ON summary.purchaseEnquiryId = PurchaseEnquiry.idPurchaseEnquiry " +
                    " LEFT JOIN tblProdClassification classifictaion ON PurchaseEnquiry.prodClassId = classifictaion.idProdClass " +
                    " LEFT JOIN dbo.tblUser userdetails ON PurchaseEnquiry.userId = userdetails.idUser " +
                    " LEFT JOIN tblPurchaseScheduleDetails details ON summary.idPurchaseScheduleSummary = details.purchaseScheduleSummaryId " +
                    " LEFT JOIN ( " +
                    "             SELECT tblPurchaseScheduleDetails.purchaseScheduleSummaryId, " +
                    "                    ISNULL((SUM(tblPurchaseScheduleDetails.productAomunt)),0) AS grandTtl " +
                    "             FROM tblPurchaseScheduleDetails tblPurchaseScheduleDetails " +
                    "             GROUP BY tblPurchaseScheduleDetails.purchaseScheduleSummaryId " +
                    "            ) AS detailsGrandTotal " +
                    " ON detailsGrandTotal.purchaseScheduleSummaryId = summary.idPurchaseScheduleSummary " +
                    " LEFT JOIN tblOrganization org ON summary.supplierId = org.idOrganization " +
                    " left join tblPurchaseInvoice invoice on invoice.purSchSummaryId = summary.rootScheduleId " +
                    " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    " LEFT JOIN tblProductItem proditem ON details.prodItemId = proditem.idProdItem " +
                    " LEFT JOIN tblPurchaseVehicleSpotEntry tblPurchaseVehicleSpotEntry on tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId = isnull(summary.rootScheduleId,summary.idPurchaseScheduleSummary)" +
                    " LEFT JOIN " +
                    " ( " +
                    "  SELECT purchaseWeighingStageSummary.purchaseScheduleSummaryId,tblWeighingMachine.machineName AS godown" +
                    "  FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary" +
                    "  LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = purchaseWeighingStageSummary.weighingMachineId" +
                    "  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                    "  ) AS purchaseWeighing" +
                    " ON purchaseWeighing.purchaseScheduleSummaryId = ISNULL(summary.rootScheduleId,summary.idPurchaseScheduleSummary)" +

                    " LEFT JOIN( " +
                    " SELECT tblPurchaseVehicleSpotEntry.idVehicleSpotEntry  , " +
                    " STUFF((SELECT ', ' + tblSpotEntryContainerDtls.containerNo FROM tblSpotEntryContainerDtls tblSpotEntryContainerDtls  " +
                    " WHERE tblPurchaseVehicleSpotEntry.idVehicleSpotEntry = tblSpotEntryContainerDtls.vehicleSpotEntryId "+
                    " FOR XML PATH('')), 1, 1, '')[containerNo] " +
                    " FROM tblPurchaseVehicleSpotEntry "+
                    " GROUP BY tblPurchaseVehicleSpotEntry.idVehicleSpotEntry) AS  containerNoTbl ON containerNoTbl.idVehicleSpotEntry = tblPurchaseVehicleSpotEntry.idVehicleSpotEntry " +

                    " LEFT JOIN(SELECT MAX(grossWeightMT) as grossWeightMT, purchaseScheduleSummaryId " +
                    " FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 3 GROUP BY purchaseScheduleSummaryId) AS purchaseWeighingGrossWeight ON purchaseWeighingGrossWeight.purchaseScheduleSummaryId = ISNULL(summary.rootScheduleId, summary.idPurchaseScheduleSummary) " +

                    " LEFT JOIN(SELECT Max(actualWeightMT) AS actualWeightMT, purchaseScheduleSummaryId " +
                    " FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 1 GROUP BY purchaseScheduleSummaryId) AS purchaseWeighingTareWeight ON purchaseWeighingTareWeight.purchaseScheduleSummaryId = ISNULL(summary.rootScheduleId, summary.idPurchaseScheduleSummary) " +

                    " LEFT JOIN(SELECT netWt, tareWt, grossWt, purchaseScheduleSummaryId " +
                    " FROM tblPartyWeighingMeasures partyWeighingMeasures) AS partyWeighingMeasures ON partyWeighingMeasures.purchaseScheduleSummaryId = ISNULL(summary.rootScheduleId, summary.idPurchaseScheduleSummary) " +

                    " WHERE summary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS)
                    //Prajakta[2019-03-06] Commented and added
                    //+ " AND summary.createdOn BETWEEN @fromDate AND @toDate " 
                    //+ " AND  summary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
                    + " AND isnull(summary.isCorrectionCompleted,0) =1 AND summary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
                if (!String.IsNullOrEmpty(vehicleIds))
                {
                    cmdSelect.CommandText += " AND summary.rootScheduleId IN(" + vehicleIds + ")";
                }
                else
                {
                    cmdSelect.CommandText += " AND  summary.corretionCompletedOn BETWEEN @fromDate AND @toDate ";
                }
                if (ConfirmTypeId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                    cmdSelect.CommandText += " AND summary.cOrNCId=" + ConfirmTypeId;
                if (supplierId > 0)
                    cmdSelect.CommandText += " AND summary.supplierId=" + supplierId;
                if (!String.IsNullOrEmpty(purchaseManagerIds))
                    cmdSelect.CommandText += " AND PurchaseEnquiry.userId IN (" + purchaseManagerIds + " ) ";
                if (materialTypeId > 0)//
                    cmdSelect.CommandText += " AND PurchaseEnquiry.prodClassId=" + materialTypeId;
               
                cmdSelect.CommandText += ")";

                if (Startup.IsForBRM)
                {
                    cmdSelect.CommandText += " SELECT idPurchaseScheduleSummary,rootScheduleId,FORMAT(date,'dd-MM-yyyy') AS date,truckNo,pm,grade,gradeQty,dustQty,gradeRate,total,";
                    cmdSelect.CommandText += " supplierName,billType,materialType,containerNo,spotVehContainerNo,location,godown,processChargePerVeh,cOrNCId,isBoth," +
                                             " remark,correNarration, " +
                                                 //" CASE WHEN cOrNCId = 0 AND isBoth = 1 THEN ((ISNULL(grandTtl,0))-(ISNULL(processChargePerVeh,0))) " +
                                                 //    "  WHEN cOrNCId = 1 AND isBoth = 1 THEN grandTtl " +
                                                 //    "  ELSE ((ISNULL(grandTtl,0))-(ISNULL(processChargePerVeh,0))) " +
                                                 //    "  END AS grandTotal," +
                                                 " NULL AS voucherNo,NULL AS purchaseLedger,0 AS displayRecordInFirstRow  " +
                                                 " , GrossWeight, TareWeight, NetWeight " +
                                                 " , partyGrossWeight, partyTareWeight, partyNetWeight " +
                                                 "  FROM cte_TallyReport " +
                                                 " ORDER BY correctionCompletedOn ASC ";
                }
                if (!Startup.IsForBRM)
                {
                    cmdSelect.CommandText += " SELECT idPurchaseScheduleSummary,rootScheduleId,truckNo,pm,grade,gradeQty,dustQty,gradeRate,total,";

                    stringOfDate = " FORMAT(date,'dd-MM-yyyy') AS date,";
                    if (!String.IsNullOrEmpty(dateOfBackYears))
                    {
                        stringOfDate = " FORMAT(DATEADD(yyyy, -" + dateOfBackYears + ", date),'MM-dd-yyyy') AS date,";
                    }

                    cmdSelect.CommandText += stringOfDate;
                    cmdSelect.CommandText += " supplierName,billType,materialType,containerNo,spotVehContainerNo,location,godown,processChargePerVeh,cOrNCId,isBoth," +
                                             " remark,correNarration, " +
                                             " CAST(FORMAT((date),'MMM') AS NVARCHAR) +'/'+ CAST(DAY(date) AS NVARCHAR) + '/'+ CAST(DENSE_RANK() Over(ORDER BY correctionCompletedOn ASC) AS NVARCHAR) AS voucherNo," +
                                             //" CASE WHEN materialType LIKE '%Scrap%' THEN materialType + ' Purchase A/C'" +
                                             " CASE " +
                                             " WHEN prodClassId = " + (int)StaticStuff.Constants.MaterialTypeE.LOCAL_AND_INDUSTRIAL //for kalika tally report
                                             + " THEN 'Scrap Purchase A/C' " +
                                             " WHEN prodClassId = " + (int)StaticStuff.Constants.MaterialTypeE.IMPORT_SCRAP //for kalika tally report
                                             + " THEN 'Imported Scrap Purchase A/C' " +
                                             " ELSE LEFT(materialType,CHARINDEX(' ',materialType + ' ')-1) + ' Purchase A/C' END AS purchaseLedger," +
                                             " ROW_NUMBER() OVER(PARTITION BY rootScheduleId,cOrNCId ORDER BY idPurchaseScheduleSummary) AS displayRecordInFirstRow "+
                                             " , GrossWeight, TareWeight, NetWeight " +
                                             " , partyGrossWeight, partyTareWeight, partyNetWeight " +
                                             "  FROM cte_TallyReport " +
                                             " ORDER BY correctionCompletedOn,displaySequanceNo ASC ";
                }

                

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TallyReportTO> list = ConvertDTToList(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Added by minal

        public List<PartWiseReportTO> SelectPartyWiseReportDetails(TblReportsTO tblReportsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            DataTable dt;
            try
            {
                conn.Open();
                sqlQuery = " SELECT CASE WHEN ROW_NUMBER() OVER(PARTITION BY organization.firmName ORDER BY proditem.itemName DESC) = 1" +
                           " THEN organization.firmName ELSE NULL END AS supplier," +
                           " purchaseScheduleSummary.supplierId,0 AS isTotalRow, " +
                           " proditem.itemName AS grade,SUM(purchaseScheduleDetails.qty) AS totalQty,SUM(purchaseScheduleDetails.productAomunt) AS totalAmount," +
                           " CAST(ROUND((SUM(purchaseScheduleDetails.productAomunt)/SUM(purchaseScheduleDetails.qty)),2) AS NUMERIC(36,2)) AS averageRate," +
                           " organization.firmName AS displaySupplier" +
                           " FROM   tblPurchaseScheduleSummary purchaseScheduleSummary" +
                           " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleSummary.idPurchaseScheduleSummary = purchaseScheduleDetails.purchaseScheduleSummaryId" +
                           " INNER JOIN tblProductItem proditem ON purchaseScheduleDetails.prodItemId = proditem.idProdItem" +
                           " LEFT JOIN tblOrganization organization ON organization.idOrganization = purchaseScheduleSummary.supplierId" +
                           " WHERE  purchaseScheduleSummary.vehiclePhaseId = 4 AND   " +
                           " CAST(purchaseScheduleSummary.corretionCompletedOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)  AND    " +
                           " ISNULL(purchaseScheduleSummary.isCorrectionCompleted,0) = 1  AND " +
                           " purchaseScheduleSummary.statusId =509 ";
                           
                           

                if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
                {
                    for (int i = 0; i < tblReportsTO.TblFilterReportTOList1.Count; i++)
                    {
                        TblFilterReportTO filterTO = tblReportsTO.TblFilterReportTOList1[i];

                        if (filterTO.OutputValue != null && filterTO.OutputValue != String.Empty && filterTO.SqlDbTypeValue == 8 && filterTO.IsOptional == 0)
                        {
                            String sqlQuery1 = " AND purchaseScheduleSummary.supplierId =";
                            sqlQuery = sqlQuery + sqlQuery1;
                            sqlQuery += filterTO.WhereClause;
                            cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, (SqlDbType)filterTO.SqlDbTypeValue).Value = filterTO.OutputValue;

                        }
                        else if (filterTO.OutputValue != null && filterTO.OutputValue != String.Empty && filterTO.SqlDbTypeValue == 33)
                        {
                            cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, (SqlDbType)filterTO.SqlDbTypeValue).Value = filterTO.OutputValue;
                        }


                    }
                }

                if (!String.IsNullOrEmpty(tblReportsTO.RoleTypeCond))
                {
                    sqlQuery += tblReportsTO.RoleTypeCond;
                }

                cmdSelect.CommandText = sqlQuery + " GROUP BY organization.firmName,proditem.itemName,purchaseScheduleSummary.supplierId"; 
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<PartWiseReportTO> list = ConvertDTToListForPartyWise(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        public Double GetPartyWiseProcessChargeForReportDetails(DateTime fromDate, DateTime toDate,Int64 supplierId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            DataTable dt;
            double processChargePerVeh = 0;
            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;
            cmdSelect.Parameters.Add("@supplierId", System.Data.SqlDbType.Int).Value = supplierId;
            try
            {
                conn.Open();
                sqlQuery = " SELECT sum(processChargePerVeh) AS processChargePerVeh FROM tblPurchaseScheduleSummary WHERE isActive = 1 AND " +
                           " rootScheduleId IN (SELECT DISTINCT(purchaseScheduleSummary.rootScheduleId) FROM tblPurchaseScheduleSummary purchaseScheduleSummary" +
                           " WHERE  purchaseScheduleSummary.vehiclePhaseId = 4 AND" +
                           " CAST(purchaseScheduleSummary.corretionCompletedOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)  AND    " +
                           " ISNULL(purchaseScheduleSummary.isCorrectionCompleted,0) = 1  AND " +
                           " purchaseScheduleSummary.statusId =509 AND" +
                           " purchaseScheduleSummary.supplierId = @supplierId)";


               
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        processChargePerVeh = Convert.ToDouble(reader["processChargePerVeh"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //return null;
            }
            finally
            {
                cmdSelect.Dispose();
                conn.Close();
            }
            return processChargePerVeh;
        }


        public DataTable SelectGradeWiseReportDetails(TblReportsTO tblReportsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            DataTable dt;
            try
            {
                conn.Open();
                sqlQuery = " WITH cte_gradeWiseReport AS " +
                           " ( " +
                           " SELECT proditem.itemName AS Grade," +
                           " CAST(ROUND((SUM(purchaseScheduleDetails.qty)),3) AS NUMERIC(36,3)) AS Quantity," +
                           " CAST(ROUND((SUM(purchaseScheduleDetails.productAomunt)/SUM(purchaseScheduleDetails.qty)),2) AS NUMERIC(36,2)) AS AverageRate, " +
                           " CAST(ROUND(SUM(purchaseScheduleDetails.productAomunt),3) AS NUMERIC(36,2)) AS Amount" +
                           " FROM tblPurchaseScheduleSummary purchaseScheduleSummary" +
                           " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleSummary.idPurchaseScheduleSummary = purchaseScheduleDetails.purchaseScheduleSummaryId  " +
                           " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = purchaseScheduleSummary.purchaseEnquiryId      " +
                           " INNER JOIN tblProductItem proditem ON purchaseScheduleDetails.prodItemId = proditem.idProdItem" +
                           " WHERE  purchaseScheduleSummary.vehiclePhaseId = 4 AND " +
                           " CAST(purchaseScheduleSummary.corretionCompletedOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)  AND    " +
                           " ISNULL(purchaseScheduleSummary.isCorrectionCompleted,0) = 1  AND " +
                           " purchaseScheduleSummary.statusId =509  ";                            

                if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
                {
                    for (int i = 0; i < tblReportsTO.TblFilterReportTOList1.Count; i++)
                    {
                        TblFilterReportTO filterTO = tblReportsTO.TblFilterReportTOList1[i];

                        if (filterTO.OutputValue != null && filterTO.OutputValue != String.Empty && filterTO.SqlDbTypeValue == 8 && filterTO.IsOptional == 0)
                        {
                            if (filterTO.IdHtml == "loadingstatus" && !String.IsNullOrEmpty(filterTO.OutputValue))
                            {
                                if (Convert.ToInt32(filterTO.OutputValue) < 2)
                                {
                                    sqlQuery += " AND ISNULL(purchaseScheduleSummary.cOrNCId,0) = @IsConfirmed";
                                    cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, (SqlDbType)filterTO.SqlDbTypeValue).Value = filterTO.OutputValue;
                                }
                            }
                            else
                            {
                                sqlQuery += " AND  1 = CASE WHEN isnull(@prodClassId,'') = '' THEN 1 " +
                               " ELSE" +
                               " CASE WHEN tblPurchaseEnquiry.prodClassId = @prodClassId THEN 1 ELSE 0 END" +
                               " END  ";
                                cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, (SqlDbType)filterTO.SqlDbTypeValue).Value = filterTO.OutputValue;
                            }
                        }
                        else if (filterTO.OutputValue != null && filterTO.OutputValue != String.Empty && filterTO.SqlDbTypeValue == 33)
                        {
                            cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(filterTO.OutputValue);
                        }


                    }
                }

                if (!String.IsNullOrEmpty(tblReportsTO.RoleTypeCond))
                {
                    sqlQuery += tblReportsTO.RoleTypeCond;
                }

                sqlQuery += " GROUP BY proditem.itemName " +
                           " ) " +
                           " " +
                           " " +
                           " SELECT gradeWiseReport.Grade,gradeWiseReport.Quantity,gradeWiseReport.AverageRate,gradeWiseReport.Amount,isTotalRow" +
                           " FROM" +
                           " (" +
                           "   SELECT Grade,Quantity,AverageRate,Amount,0 AS isTotalRow" +
                           "   FROM cte_gradeWiseReport" +
                           "   UNION ALL" +
                           "   SELECT 'Grand Total',SUM(Quantity),NULL,SUM(Amount),1 AS isTotalRow" +
                           "   FROM cte_gradeWiseReport" +
                           " ) AS gradeWiseReport";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                conn.Close();
            }
        }
        //Added by minal


        public List<VehicleWiseReportTO> SelectVehicleWiseReportDetails(TblReportsTO tblReportsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            
            try
            {
                conn.Open();
                sqlQuery = " SELECT CASE WHEN ROW_NUMBER() OVER(PARTITION BY purchaseScheduleSummary.vehicleNo,purchaseScheduleSummary.rootscheduleid ORDER BY proditem.displaySequanceNo) = 1" +
                           " THEN purchaseScheduleSummary.vehicleNo ELSE NULL END AS vehicleNumber," +
                           " CASE WHEN ROW_NUMBER() OVER(PARTITION BY FORMAT(purchaseScheduleSummary.createdOn,'dd/MM/yyyy'),purchaseScheduleSummary.rootscheduleid ORDER BY proditem.displaySequanceNo) = 1" +
                           " THEN FORMAT(purchaseScheduleSummary.createdOn,'dd/MM/yyyy') ELSE NULL END AS date," +
                           " CASE WHEN ROW_NUMBER() OVER(PARTITION BY tblOrganization.firmName,purchaseScheduleSummary.rootscheduleid ORDER BY proditem.displaySequanceNo) = 1" +
                           " THEN tblOrganization.firmName ELSE NULL END AS supplier,	" +
                           " CASE WHEN ROW_NUMBER() OVER(PARTITION BY tblPurchaseEnquiry.remark,purchaseScheduleSummary.rootscheduleid ORDER BY proditem.displaySequanceNo) = 1" +
                           " THEN tblPurchaseEnquiry.remark ELSE NULL END AS remark, " +
                           " proditem.itemName AS grade," +
                           " purchaseScheduleSummary.processChargePerVeh," +
                           " purchaseScheduleSummary.idPurchaseScheduleSummary," +
                           " SUM(purchaseScheduleDetails.qty) AS totalQty, " +
                           " SUM(purchaseScheduleDetails.productAomunt)/  SUM(purchaseScheduleDetails.qty) as rate," +
                           " SUM(purchaseScheduleDetails.productAomunt) AS totalAmount,0 AS isTotalRow,purchaseScheduleSummary.cOrNCId,purchaseScheduleSummary.isBoth,  " +
                           " purchaseScheduleSummary.vehicleNo AS displayVehicleNo,FORMAT(purchaseScheduleSummary.createdOn,'dd/MM/yyyy') AS displayDate,tblOrganization.firmName AS displaySupplier,tblPurchaseEnquiry.remark AS displayRemark  " +
                           " FROM   tblPurchaseScheduleSummary purchaseScheduleSummary" +
                           " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleSummary.idPurchaseScheduleSummary = purchaseScheduleDetails.purchaseScheduleSummaryId " +
                           " INNER JOIN tblProductItem proditem ON purchaseScheduleDetails.prodItemId = proditem.idProdItem" +
                           " LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization = purchaseScheduleSummary.supplierId" +
                           " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry on tblPurchaseEnquiry.idPurchaseEnquiry = purchaseScheduleSummary.purchaseEnquiryId" +
                           " WHERE  purchaseScheduleSummary.vehiclePhaseId = 4 AND" +
                           " CAST(purchaseScheduleSummary.corretionCompletedOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)  AND   " +
                           " ISNULL(purchaseScheduleSummary.isCorrectionCompleted,0) = 1  AND" +
                           " purchaseScheduleSummary.statusId =509 ";
                           

                if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
                {
                    for (int i = 0; i < tblReportsTO.TblFilterReportTOList1.Count; i++)
                    {
                        TblFilterReportTO filterTO = tblReportsTO.TblFilterReportTOList1[i];

                        if (filterTO.OutputValue != null && filterTO.OutputValue != String.Empty && filterTO.SqlDbTypeValue == 8 && filterTO.IsOptional == 0)
                        {
                            String sqlQuery1 = " AND purchaseScheduleSummary.supplierId = ";
                            sqlQuery = sqlQuery + sqlQuery1;
                            sqlQuery  += (filterTO.WhereClause);
                            cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, (SqlDbType)filterTO.SqlDbTypeValue).Value = filterTO.OutputValue;

                        }
                        else if (filterTO.OutputValue != null && filterTO.OutputValue != String.Empty && filterTO.SqlDbTypeValue == 33)
                        {
                            cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, (SqlDbType)filterTO.SqlDbTypeValue).Value = filterTO.OutputValue;
                        }


                    }
                }

                if (!String.IsNullOrEmpty(tblReportsTO.RoleTypeCond))
                {
                    sqlQuery += tblReportsTO.RoleTypeCond;
                }                

                String sqlQuery2 = " GROUP BY purchaseScheduleSummary.rootScheduleId,purchaseScheduleSummary.vehicleNo,proditem.itemName,proditem.displaySequanceNo,purchaseScheduleSummary.processChargePerVeh,purchaseScheduleSummary.idPurchaseScheduleSummary," +
                           " tblOrganization.firmName,tblPurchaseEnquiry.remark,FORMAT(purchaseScheduleSummary.createdOn,'dd/MM/yyyy'),purchaseScheduleSummary.cOrNCId,purchaseScheduleSummary.isBoth" +
                           " ORDER BY FORMAT(purchaseScheduleSummary.createdOn,'dd/MM/yyyy') DESC";
                cmdSelect.CommandText = sqlQuery + sqlQuery2;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<VehicleWiseReportTO> list = ConvertDTToListForVehicleWise(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                conn.Close();
            }
        }
        public List<TallyReportTO> SelectTallyReportForExcel(String vehicleIds, int ConfirmTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            //cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT summary.rootScheduleId,summary.idPurchaseScheduleSummary,  " +
                    //"summary.createdOn AS date,summary.vehicleNo AS truckNo," +
                    "summary.corretionCompletedOn AS date,summary.vehicleNo AS truckNo," +
                    "userdetails.userDisplayName AS pm,summary.location AS location," +
                    "proditem.itemName AS grade,details.qty AS gradeQty," +
                    " case when  proditem.isNonCommercialItem =1 then details.qty end as dustQty ," +
                      " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  details.rate end as gradeRate," +
                     " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  cast((details.qty * details.rate) as decimal(10, 2)) end as total" +
                    " , CASE WHEN summary.cOrNCId = 1 THEN  ISNULL(invoiceAddr.billingPartyName,org.firmName)  WHEN summary.cOrNCId = 0 THEN org.firmName   END AS supplierName," +
                    " CASE WHEN summary.cOrNCId = 1 THEN 'ORDER' WHEN summary.cOrNCId = 0 THEN 'ENQUIRY' END AS billType,classifictaion.prodClassDesc AS materialType,summary.containerNo AS containerNo,purchaseWeighing.godown, " +
                     " CAST((ISNULL(summary.processChargePerVeh,0)/1000) as decimal(10, 3)) AS processChargePerVeh, " +
                    " summary.cOrNCId,summary.isBoth " +
                    "FROM dbo.tblPurchaseScheduleSummary summary " +
                    "LEFT JOIN tblPurchaseEnquiry PurchaseEnquiry ON summary.purchaseEnquiryId = PurchaseEnquiry.idPurchaseEnquiry " +
                    "LEFT JOIN tblProdClassification classifictaion ON PurchaseEnquiry.prodClassId = classifictaion.idProdClass " +
                    "LEFT JOIN dbo.tblUser userdetails ON PurchaseEnquiry.userId = userdetails.idUser " +
                    "LEFT JOIN tblPurchaseScheduleDetails details ON summary.idPurchaseScheduleSummary = details.purchaseScheduleSummaryId " +
                    "LEFT JOIN tblOrganization org ON summary.supplierId = org.idOrganization " +
                    " left join tblPurchaseInvoice invoice on invoice.purSchSummaryId = summary.rootScheduleId " +
                    " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    "  LEFT JOIN tblProductItem proditem ON details.prodItemId = proditem.idProdItem " +
                     " LEFT JOIN " +
                    " ( " +
                    "  SELECT purchaseWeighingStageSummary.purchaseScheduleSummaryId,tblWeighingMachine.machineName AS godown" +
                    "  FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary" +
                    "  LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = purchaseWeighingStageSummary.weighingMachineId" +
                    "  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                    "  ) AS purchaseWeighing" +
                    " ON purchaseWeighing.purchaseScheduleSummaryId = ISNULL(summary.rootScheduleId,summary.idPurchaseScheduleSummary)" +
                    " WHERE summary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS)
                    //Prajakta[2019-03-06] Commented and added
                    //+ " AND summary.createdOn BETWEEN @fromDate AND @toDate " 
                    //+ " AND  summary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
                    + " AND isnull(summary.isCorrectionCompleted,0) =1 AND summary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
                if (ConfirmTypeId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                    cmdSelect.CommandText += " AND summary.cOrNCId=" + ConfirmTypeId;

                if(!String.IsNullOrEmpty(vehicleIds))
                {
                    cmdSelect.CommandText += " AND summary.rootScheduleId IN (" + vehicleIds + ")";
                }
                
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TallyReportTO> list = ConvertDTToList(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<PadtaReportTO> SelectPadtaReportDetails(DateTime fromDate, DateTime toDate,String purchaseManagerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT summary.vehicleNo, summary.idPurchaseScheduleSummary,details.productAomunt,details.productAomunt,case when isnull(details.qty,0)=0 then 0 when isnull(details.qty,0) > 0 then " +
                    //" ((details.productRecovery/details.qty) * 100) end AS  productRecovery,userdetails.userDisplayName AS pm,vehicltype.vehicleTypeDesc AS truckType, " +
                    " details.recovery end AS  productRecovery,userdetails.userDisplayName AS pm,vehicltype.vehicleTypeDesc AS truckType, " +
                    " proditem.itemName AS grade,details.qty AS qty,PurchaseEnquiry.bookingRate AS gradeRate, CASE WHEN summary.cOrNCId = 1 THEN 'ORDER' WHEN summary.cOrNCId = 0 THEN 'ENQUIRY' " +
                    " END AS billType, CASE WHEN summary.cOrNCId = 1 THEN  invoiceAddr.billingPartyName  WHEN summary.cOrNCId = 0 THEN org.firmName  END AS supplierName," +
                    " CASE WHEN summary.cOrNCId = 1 THEN summary.padta WHEN summary.cOrNCId = 0 THEN summary.padtaForEnquiry " +
                    " END AS padta_MT, CASE WHEN summary.cOrNCId = 1 THEN summary.baseMetalCost WHEN summary.cOrNCId = 0 THEN summary.baseMetalCostForEnquiry END AS sRate, tblBaseItemMetalCost.baseRecovery,  " +
                    " case when  proditem.isNonCommercialItem =1 then details.qty end as dustQty " +
                    " FROM dbo.tblPurchaseScheduleSummary summary " +
                    "LEFT JOIN tblPurchaseEnquiry PurchaseEnquiry ON summary.purchaseEnquiryId = PurchaseEnquiry.idPurchaseEnquiry " +
                    "LEFT JOIN tblBaseItemMetalCost tblBaseItemMetalCost ON PurchaseEnquiry.globalRatePurchaseId = tblBaseItemMetalCost.globalRatePurchaseId and tblBaseItemMetalCost.cOrNcId =summary.cOrNCId " +
                    "LEFT JOIN tblProdClassification classifictaion ON PurchaseEnquiry.prodClassId = classifictaion.idProdClass " +
                    "LEFT JOIN dbo.tblUser userdetails ON PurchaseEnquiry.userId = userdetails.idUser " +
                    "LEFT JOIN tblPurchaseScheduleDetails details ON summary.idPurchaseScheduleSummary = details.purchaseScheduleSummaryId " +
                    "LEFT JOIN tblOrganization org ON summary.supplierId = org.idOrganization " +
                    "left join tblPurchaseInvoice invoice on invoice.purSchSummaryId =summary.rootScheduleId " +
                    "LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    "LEFT JOIN dimVehicleType vehicltype ON summary.vehicleTypeId = vehicltype.idVehicleType " +
                    "LEFT JOIN tblProductItem proditem ON details.prodItemId = proditem.idProdItem WHERE summary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS)
                    //Prajakta[2019-03-06] Commented and added
                    //+ "  AND summary.createdOn BETWEEN @fromDate AND @toDate " 
                    + " AND  summary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
                    + " and summary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED + " AND isnull(summary.isCorrectionCompleted,0) =1";

                if (!String.IsNullOrEmpty(purchaseManagerIds))
                {
                    cmdSelect.CommandText += "  AND PurchaseEnquiry.userId IN (" + purchaseManagerIds +")";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<PadtaReportTO> list = ConvertDTToListForPadta(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<purchaseSummuryReportTo> SelectPurchaseSummuryReportForOld(DateTime fromDate, DateTime toDate, string otherTaxIds, string otherTaxIdsTransporter,String purchaseMangerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            DateTime f = Constants.GetStartDateTime(fromDate);
            DateTime t = Constants.GetEndDateTime(toDate);

            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = //"SELECT isnull(tblPurchaseScheduleSummary.updatedOn,tblPurchaseScheduleSummary.createdOn) as createdOn," + 
                    "SELECT tblPurchaseScheduleSummary.corretionCompletedOn as createdOn," +
                    "invoice.invoiceNo,invoice.invoiceDate,invoice.transportorName,(tblPurchaseScheduleSummary.vehicleNo + '/' + invoice.lrNumber) AS vehicleNo,invoice.electronicRefNo,invoice.ewayBillDate,invoice.ewayBillExpiryDate,masterValue.masterValueName AS voucherType,masterValue4.masterValueName AS purAcc,  " +
                    "masterValue1.masterValueName cgst,masterCostCat.masterValueName as costCategory,masterCostCentrer.masterValueName as costCenter, masterValue2.masterValueName AS sgst,masterValue3.masterValueName AS igst,masterValue5.masterValueName AS otherExpAcc ," +
                    "masterValue6.masterValueName AS ipTransportAdvAcc,invoiceAddr.billingPartyName as supplierName,interItemDesc.masterValueName as  productItemDesc,ItemDetails.invoiceQty,ItemDetails.rate,ItemDetails.basicTotal,invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,(select sum(taxableAmt) from tblPurchaseInvoiceItemDetails where purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId in (" + otherTaxIds + ")) as otherExpAmt," +
                    "(select SUM(taxableAmt) as taxableAmt from tblPurchaseInvoiceItemDetails where purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId in  (" + otherTaxIdsTransporter + ")) as transportorAdvAmt,invoice.grandTotal," +
                    "interfacing.narration,masterValue7.mastervaluename,purchaseWeighing.godown  FROM dbo.tblPurchaseInvoice invoice " +
                    "LEFT JOIN  tblPurchaseInvoiceInterfacingDtl interfacing ON interfacing.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    "LEFT JOIN tblPurchaseInvoiceItemDetails ItemDetails ON ItemDetails.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    //"LEFT JOIN tblPurchaseInvoiceItemTaxDetails ItemTaxDetails ON ItemTaxDetails.purchaseInvoiceItemId = ItemDetails.idPurchaseInvoiceItem " +
                    "LEFT JOIN dimMasterValue masterValue ON masterValue.idMasterValue = interfacing.voucherTypeId " +
                    "LEFT JOIN tblPurchaseScheduleSummary on ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) = invoice.purSchSummaryId AND tblPurchaseScheduleSummary.statusId=" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED +
                    " AND tblPurchaseScheduleSummary.vehiclePhaseId=" + (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " AND ISNULL(tblPurchaseScheduleSummary.isCorrectionCompleted,0)=1 AND ISNULL(tblPurchaseScheduleSummary.cOrNCId,0)=1 " +
                    "LEFT JOIN dimMasterValue masterValue1 ON masterValue1.idMasterValue = interfacing.cgstId " +
                    "LEFT JOIN dimMasterValue masterValue2 ON masterValue2.idMasterValue = interfacing.sgstId " +
                    "LEFT JOIN dimMasterValue masterValue3 ON masterValue3.idMasterValue = interfacing.igstId " +
                    "LEFT JOIN dimMasterValue masterValue4 ON masterValue4.idMasterValue = interfacing.purAccId " +
                    "LEFT JOIN dimMasterValue masterValue5 ON masterValue5.idMasterValue = interfacing.otherExpAccId " +
                    " LEFT JOIN dimMasterValue masterValue7 ON masterValue7.idMasterValue = interfacing.gradeid " +
                    " LEFT JOIN dimMasterValue masterCostCat ON masterCostCat.idMasterValue = interfacing.costCategoryId " +
                    " LEFT JOIN dimMasterValue interItemDesc ON interItemDesc.idMasterValue = interfacing.materialtemId " +
                    " LEFT JOIN tblPurchaseInvoiceItemDetails ItemDetailtax ON ItemDetailtax.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase and invoiceAddr.txnAddrTypeId = " + (Int32)Constants.TxnTypeE.FOR_SUPPLIER +
                    " LEFT JOIN dimMasterValue masterCostCentrer ON masterCostCentrer.idMasterValue = interfacing.costCenterId " +
                    " LEFT JOIN dimMasterValue masterValue6 ON masterValue6.idMasterValue = interfacing.ipTransportAdvAccId " +
                    " LEFT JOIN " +
                                " ( " +
                                "  SELECT purchaseWeighingStageSummary.purchaseScheduleSummaryId,tblWeighingMachine.machineName AS godown" +
                                "  FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary" +
                                "  LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = purchaseWeighingStageSummary.weighingMachineId" +
                                "  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                                "  ) AS purchaseWeighing" +
                    " ON purchaseWeighing.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                    " WHERE isnull(ItemDetails.otherTaxId,0) = 0 " +
                    //Prajakta[2019-03-06] Commented and added
                    //" AND isnull(tblPurchaseScheduleSummary.updatedOn,tblPurchaseScheduleSummary.createdOn) BETWEEN @fromDate AND @toDate " 
                    " AND tblPurchaseScheduleSummary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
                      + " and isnull(tblPurchaseScheduleSummary.isCorrectionCompleted,0)=1";

                if (!String.IsNullOrEmpty(purchaseMangerIds))
                {
                    cmdSelect.CommandText += " AND tblPurchaseScheduleSummary.purchaseEnquiryId IN (SELECT tblPurchaseEnquiry.idPurchaseEnquiry " +
                                             "                                                      FROM tblPurchaseEnquiry tblPurchaseEnquiry " +
                                             "                                                      WHERE tblPurchaseEnquiry.userId IN ( " + purchaseMangerIds + "))";
                }

                cmdSelect.CommandText += " group by ItemDetailtax.purchaseInvoiceId,tblPurchaseScheduleSummary.corretionCompletedOn,tblPurchaseScheduleSummary.createdOn,tblPurchaseScheduleSummary.updatedOn,invoice.invoiceNo,invoice.invoiceDate,invoice.transportorName," +
                    " tblPurchaseScheduleSummary.vehicleNo,invoice.lrNumber,invoice.electronicRefNo,invoice.ewayBillDate,invoice.ewayBillExpiryDate,masterValue.masterValueName,masterValue4.masterValueName ,  masterValue1.masterValueName,masterCostCat.masterValueName ,masterCostCentrer.masterValueName, masterValue2.masterValueName ," +
                    " masterValue3.masterValueName,masterValue5.masterValueName ,masterValue6.masterValueName,invoiceAddr.billingPartyName,interItemDesc.masterValueName,invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,invoice.transportorAdvAmt,invoice.grandTotal,interfacing.narration,masterValue7.mastervaluename," +
                    " ItemDetails.invoiceQty,ItemDetails.rate,ItemDetails.basicTotal,invoice.idInvoicePurchase,purchaseWeighing.godown";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<purchaseSummuryReportTo> list = ConvertDTToListForPurschaseForOld(reader);

                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<purchaseSummuryReportTo> SelectPurchaseSummuryReportForNew(DateTime fromDate, DateTime toDate, string otherTaxIds, string otherTaxIdsTransporter,string otherExpensesInsuranceId,String purchaseMangerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            DateTime f = Constants.GetStartDateTime(fromDate);
            DateTime t = Constants.GetEndDateTime(toDate);

            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = //"SELECT isnull(tblPurchaseScheduleSummary.updatedOn,tblPurchaseScheduleSummary.createdOn) as createdOn," + 
                    " SELECT tblPurchaseScheduleSummary.corretionCompletedOn as createdOn, tblPurchaseScheduleSummary.qty," +
                    " invoice.invoiceNo,invoice.invoiceDate,invoice.transportorName,(tblPurchaseScheduleSummary.vehicleNo + '/' + invoice.lrNumber) AS vehicleNo,invoice.electronicRefNo,invoice.ewayBillDate,invoice.ewayBillExpiryDate,masterValue.masterValueName AS voucherType,masterValue4.masterValueName AS purAcc,  " +
                    " masterValue1.masterValueName cgst,masterCostCat.masterValueName as costCategory,masterCostCentrer.masterValueName as costCenter, masterValue2.masterValueName AS sgst,masterValue3.masterValueName AS igst,masterValue5.masterValueName AS otherExpAcc ," +
                    " masterValue6.masterValueName AS ipTransportAdvAcc,invoiceAddr.billingPartyName as supplierName,interItemDesc.masterValueName as  productItemDesc,ItemDetails.invoiceQty,ItemDetails.rate,ItemDetails.basicTotal,invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,(select sum(taxableAmt) from tblPurchaseInvoiceItemDetails where purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId in (" + otherTaxIds + ")) as otherExpAmt," +
                    " (select SUM(taxableAmt) as taxableAmt from tblPurchaseInvoiceItemDetails where purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId in  (" + otherTaxIdsTransporter + ")) as transportorAdvAmt,invoice.grandTotal," +
                    " interfacing.narration,masterValue7.mastervaluename,purchaseWeighing.godown, " +
                    " masterValue8.masterValueName AS otherExpensesInsuranceInput," +
                    " (SELECT SUM(taxableAmt) FROM tblPurchaseInvoiceItemDetails WHERE purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId IN ( " + otherExpensesInsuranceId + " )) as otherExpensesInsuranceamt, " +
                    " masterValue9.masterValueName AS tdsInput,ROUND(((ISNULL(invoice.grandTotal,0)) * 0.001),0) AS tdsAmt " +
                    " FROM dbo.tblPurchaseInvoice invoice " +
                    " LEFT JOIN  tblPurchaseInvoiceInterfacingDtl interfacing ON interfacing.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    " LEFT JOIN tblPurchaseInvoiceItemDetails ItemDetails ON ItemDetails.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    //"LEFT JOIN tblPurchaseInvoiceItemTaxDetails ItemTaxDetails ON ItemTaxDetails.purchaseInvoiceItemId = ItemDetails.idPurchaseInvoiceItem " +
                    " LEFT JOIN dimMasterValue masterValue ON masterValue.idMasterValue = interfacing.voucherTypeId " +
                    " LEFT JOIN tblPurchaseScheduleSummary on ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) = invoice.purSchSummaryId AND tblPurchaseScheduleSummary.statusId=" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED +
                    " AND tblPurchaseScheduleSummary.vehiclePhaseId=" + (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " AND ISNULL(tblPurchaseScheduleSummary.isCorrectionCompleted,0)=1 AND ISNULL(tblPurchaseScheduleSummary.cOrNCId,0)=1 " +
                    " LEFT JOIN dimMasterValue masterValue1 ON masterValue1.idMasterValue = interfacing.cgstId " +
                    " LEFT JOIN dimMasterValue masterValue2 ON masterValue2.idMasterValue = interfacing.sgstId " +
                    " LEFT JOIN dimMasterValue masterValue3 ON masterValue3.idMasterValue = interfacing.igstId " +
                    " LEFT JOIN dimMasterValue masterValue4 ON masterValue4.idMasterValue = interfacing.purAccId " +
                    " LEFT JOIN dimMasterValue masterValue5 ON masterValue5.idMasterValue = interfacing.otherExpAccId " +
                    " LEFT JOIN dimMasterValue masterValue7 ON masterValue7.idMasterValue = interfacing.gradeid " +
                    " LEFT JOIN dimMasterValue masterCostCat ON masterCostCat.idMasterValue = interfacing.costCategoryId " +
                    " LEFT JOIN dimMasterValue interItemDesc ON interItemDesc.idMasterValue = interfacing.materialtemId " +
                    " LEFT JOIN tblPurchaseInvoiceItemDetails ItemDetailtax ON ItemDetailtax.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase and invoiceAddr.txnAddrTypeId = " + (Int32)Constants.TxnTypeE.FOR_SUPPLIER +
                    " LEFT JOIN dimMasterValue masterCostCentrer ON masterCostCentrer.idMasterValue = interfacing.costCenterId " +
                    " LEFT JOIN dimMasterValue masterValue6 ON masterValue6.idMasterValue = interfacing.ipTransportAdvAccId " +
                    " LEFT JOIN " +
                                " ( " +
                                "  SELECT purchaseWeighingStageSummary.purchaseScheduleSummaryId,tblWeighingMachine.machineName AS godown" +
                                "  FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary" +
                                "  LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = purchaseWeighingStageSummary.weighingMachineId" +
                                "  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                                "  ) AS purchaseWeighing" +
                    " ON purchaseWeighing.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                    " LEFT JOIN dimMasterValue masterValue8 ON masterValue8.idMasterValue = interfacing.otherExpInsuAccId " +
                    " LEFT JOIN dimMasterValue masterValue9 ON masterValue9.idMasterValue = interfacing.tdsAccId " +

                    " WHERE isnull(ItemDetails.otherTaxId,0) = 0 " +
                    //Prajakta[2019-03-06] Commented and added
                    //" AND isnull(tblPurchaseScheduleSummary.updatedOn,tblPurchaseScheduleSummary.createdOn) BETWEEN @fromDate AND @toDate " 
                    " AND tblPurchaseScheduleSummary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
                      + " and isnull(tblPurchaseScheduleSummary.isCorrectionCompleted,0)=1";

                if (!String.IsNullOrEmpty(purchaseMangerIds))
                {
                    cmdSelect.CommandText += " AND tblPurchaseScheduleSummary.purchaseEnquiryId IN (SELECT tblPurchaseEnquiry.idPurchaseEnquiry" +
                                             "                                                      FROM tblPurchaseEnquiry tblPurchaseEnquiry " +
                                             "                                                      WHERE tblPurchaseEnquiry.userId IN ( " + purchaseMangerIds + "))";
                }

             cmdSelect.CommandText += " group by ItemDetailtax.purchaseInvoiceId,tblPurchaseScheduleSummary.corretionCompletedOn,tblPurchaseScheduleSummary.createdOn,tblPurchaseScheduleSummary.updatedOn,invoice.invoiceNo,invoice.invoiceDate,invoice.transportorName," +
                    " tblPurchaseScheduleSummary.vehicleNo,invoice.lrNumber,invoice.electronicRefNo,invoice.ewayBillDate,invoice.ewayBillExpiryDate,masterValue.masterValueName,masterValue4.masterValueName ,  masterValue1.masterValueName,masterCostCat.masterValueName ,masterCostCentrer.masterValueName, masterValue2.masterValueName ," +
                    " masterValue3.masterValueName,masterValue5.masterValueName ,masterValue6.masterValueName,invoiceAddr.billingPartyName,interItemDesc.masterValueName,invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,invoice.transportorAdvAmt,invoice.grandTotal,interfacing.narration,masterValue7.mastervaluename," +
                    " ItemDetails.invoiceQty,ItemDetails.rate,ItemDetails.basicTotal,invoice.idInvoicePurchase,purchaseWeighing.godown,masterValue8.masterValueName,masterValue9.masterValueName,tblPurchaseScheduleSummary.qty";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<purchaseSummuryReportTo> list = ConvertDTToListForPurschase(reader);

                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<purchaseSummuryReportTo> SelectPurchaseSummuryReport(DateTime fromDate, DateTime toDate, string otherTaxIds, string otherTaxIdsTransporter)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            DateTime f = Constants.GetStartDateTime(fromDate);
            DateTime t = Constants.GetEndDateTime(toDate);

            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = //"SELECT isnull(tblPurchaseScheduleSummary.updatedOn,tblPurchaseScheduleSummary.createdOn) as createdOn," + 
                    "SELECT tblPurchaseScheduleSummary.corretionCompletedOn as createdOn,tblPurchaseScheduleSummary.qty," +
                    "invoice.invoiceNo,invoice.invoiceDate,invoice.transportorName,(tblPurchaseScheduleSummary.vehicleNo + '/' + invoice.lrNumber) AS vehicleNo,invoice.electronicRefNo,invoice.ewayBillDate,invoice.ewayBillExpiryDate,masterValue.masterValueName AS voucherType,masterValue4.masterValueName AS purAcc,  " +
                    "masterValue1.masterValueName cgst,masterCostCat.masterValueName as costCategory,masterCostCentrer.masterValueName as costCenter, masterValue2.masterValueName AS sgst,masterValue3.masterValueName AS igst,masterValue5.masterValueName AS otherExpAcc ," +
                    "masterValue6.masterValueName AS ipTransportAdvAcc,invoiceAddr.billingPartyName as supplierName,interItemDesc.masterValueName as  productItemDesc,ItemDetails.invoiceQty,ItemDetails.rate,ItemDetails.basicTotal,invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,(select sum(taxableAmt) from tblPurchaseInvoiceItemDetails where purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId in (" + otherTaxIds + ")) as otherExpAmt," +
                    "(select SUM(taxableAmt) as taxableAmt from tblPurchaseInvoiceItemDetails where purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId in  (" + otherTaxIdsTransporter + ")) as transportorAdvAmt,invoice.grandTotal," +
                    "interfacing.narration,masterValue7.mastervaluename  FROM dbo.tblPurchaseInvoice invoice " +
                    "LEFT JOIN  tblPurchaseInvoiceInterfacingDtl interfacing ON interfacing.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    "LEFT JOIN tblPurchaseInvoiceItemDetails ItemDetails ON ItemDetails.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    //"LEFT JOIN tblPurchaseInvoiceItemTaxDetails ItemTaxDetails ON ItemTaxDetails.purchaseInvoiceItemId = ItemDetails.idPurchaseInvoiceItem " +
                    "LEFT JOIN dimMasterValue masterValue ON masterValue.idMasterValue = interfacing.voucherTypeId " +
                    "LEFT JOIN tblPurchaseScheduleSummary on ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) = invoice.purSchSummaryId AND tblPurchaseScheduleSummary.statusId=" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED +
                    " AND tblPurchaseScheduleSummary.vehiclePhaseId=" + (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " AND ISNULL(tblPurchaseScheduleSummary.isCorrectionCompleted,0)=1 AND ISNULL(tblPurchaseScheduleSummary.cOrNCId,0)=1 " +
                    "LEFT JOIN dimMasterValue masterValue1 ON masterValue1.idMasterValue = interfacing.cgstId " +
                    "LEFT JOIN dimMasterValue masterValue2 ON masterValue2.idMasterValue = interfacing.sgstId " +
                    "LEFT JOIN dimMasterValue masterValue3 ON masterValue3.idMasterValue = interfacing.igstId " +
                    "LEFT JOIN dimMasterValue masterValue4 ON masterValue4.idMasterValue = interfacing.purAccId " +
                    "LEFT JOIN dimMasterValue masterValue5 ON masterValue5.idMasterValue = interfacing.otherExpAccId " +
                    " LEFT JOIN dimMasterValue masterValue7 ON masterValue7.idMasterValue = interfacing.gradeid " +
                    " LEFT JOIN dimMasterValue masterCostCat ON masterCostCat.idMasterValue = interfacing.costCategoryId " +
                    " LEFT JOIN dimMasterValue interItemDesc ON interItemDesc.idMasterValue = interfacing.materialtemId " +
                    " LEFT JOIN tblPurchaseInvoiceItemDetails ItemDetailtax ON ItemDetailtax.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase and invoiceAddr.txnAddrTypeId = " + (Int32)Constants.TxnTypeE.FOR_SUPPLIER +
                    " LEFT JOIN dimMasterValue masterCostCentrer ON masterCostCentrer.idMasterValue = interfacing.costCenterId " +
                    " LEFT JOIN dimMasterValue masterValue6 ON masterValue6.idMasterValue = interfacing.ipTransportAdvAccId WHERE isnull(ItemDetails.otherTaxId,0) = 0 " +
                    //Prajakta[2019-03-06] Commented and added
                    //" AND isnull(tblPurchaseScheduleSummary.updatedOn,tblPurchaseScheduleSummary.createdOn) BETWEEN @fromDate AND @toDate " 
                    " AND tblPurchaseScheduleSummary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
                      + " and isnull(tblPurchaseScheduleSummary.isCorrectionCompleted,0)=1" +
                    " group by ItemDetailtax.purchaseInvoiceId,tblPurchaseScheduleSummary.corretionCompletedOn,tblPurchaseScheduleSummary.createdOn,tblPurchaseScheduleSummary.updatedOn,invoice.invoiceNo,invoice.invoiceDate,invoice.transportorName," +
                    " tblPurchaseScheduleSummary.vehicleNo,invoice.lrNumber,invoice.electronicRefNo,invoice.ewayBillDate,invoice.ewayBillExpiryDate,masterValue.masterValueName,masterValue4.masterValueName ,  masterValue1.masterValueName,masterCostCat.masterValueName ,masterCostCentrer.masterValueName, masterValue2.masterValueName ," +
                    " masterValue3.masterValueName,masterValue5.masterValueName ,masterValue6.masterValueName,invoiceAddr.billingPartyName,interItemDesc.masterValueName,invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,invoice.transportorAdvAmt,invoice.grandTotal,interfacing.narration,masterValue7.mastervaluename," +
                    " ItemDetails.invoiceQty,ItemDetails.rate,ItemDetails.basicTotal,invoice.idInvoicePurchase,tblPurchaseScheduleSummary.qty ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<purchaseSummuryReportTo> list = ConvertDTToListForPurschase(reader);

                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //public List<purchaseSummuryReportTo> SelectPurchaseSummuryReport(DateTime fromDate, DateTime toDate, string otherTaxIds, string otherTaxIdsTransporter)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();

        //    DateTime f = Constants.GetStartDateTime(fromDate);
        //    DateTime t = Constants.GetEndDateTime(toDate);

        //    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
        //    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = //"SELECT isnull(tblPurchaseScheduleSummary.updatedOn,tblPurchaseScheduleSummary.createdOn) as createdOn," + 
        //            "SELECT tblPurchaseScheduleSummary.corretionCompletedOn as createdOn, " +
        //            "invoice.invoiceNo,invoice.invoiceDate,invoice.transportorName,(tblPurchaseScheduleSummary.vehicleNo + '/' + invoice.lrNumber) AS vehicleNo,invoice.electronicRefNo,invoice.ewayBillDate,invoice.ewayBillExpiryDate,masterValue.masterValueName AS voucherType,masterValue4.masterValueName AS purAcc,  " +
        //            "masterValue1.masterValueName cgst,masterCostCat.masterValueName as costCategory,masterCostCentrer.masterValueName as costCenter, masterValue2.masterValueName AS sgst,masterValue3.masterValueName AS igst,masterValue5.masterValueName AS otherExpAcc ," +
        //            "masterValue6.masterValueName AS ipTransportAdvAcc,invoiceAddr.billingPartyName as supplierName,interItemDesc.masterValueName as  productItemDesc,ItemDetails.invoiceQty,ItemDetails.rate,ItemDetails.basicTotal,invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,(select sum(taxableAmt) from tblPurchaseInvoiceItemDetails where purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId in (" + otherTaxIds + ")) as otherExpAmt," +
        //            "(select SUM(taxableAmt) as taxableAmt from tblPurchaseInvoiceItemDetails where purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId in  (" + otherTaxIdsTransporter + ")) as transportorAdvAmt,invoice.grandTotal," +
        //            "interfacing.narration,masterValue7.mastervaluename,  " +
        //            " masterValue8.masterValueName AS otherExpensesInsuranceInput,(SELECT grandTotal FROM tblPurchaseInvoiceItemDetails WHERE purchaseInvoiceId = invoice.idInvoicePurchase and otherTaxId = 6) as otherExpensesInsuranceamt, " +
        //            " masterValue9.masterValueName AS tdsInput,ISNULL(((ISNULL(invoice.grandTotal,0)) * 0.01),0) AS tdsAmt,purchaseWeighing.godown " +
        //            " FROM dbo.tblPurchaseInvoice invoice " +
        //            "LEFT JOIN  tblPurchaseInvoiceInterfacingDtl interfacing ON interfacing.purchaseInvoiceId = invoice.idInvoicePurchase " +
        //            "LEFT JOIN tblPurchaseInvoiceItemDetails ItemDetails ON ItemDetails.purchaseInvoiceId = invoice.idInvoicePurchase " +
        //            //"LEFT JOIN tblPurchaseInvoiceItemTaxDetails ItemTaxDetails ON ItemTaxDetails.purchaseInvoiceItemId = ItemDetails.idPurchaseInvoiceItem " +
        //            "LEFT JOIN dimMasterValue masterValue ON masterValue.idMasterValue = interfacing.voucherTypeId " +
        //            "LEFT JOIN tblPurchaseScheduleSummary on ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) = invoice.purSchSummaryId AND tblPurchaseScheduleSummary.statusId=" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED +
        //            " AND tblPurchaseScheduleSummary.vehiclePhaseId=" + (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " AND ISNULL(tblPurchaseScheduleSummary.isCorrectionCompleted,0)=1 AND ISNULL(tblPurchaseScheduleSummary.cOrNCId,0)=1 " +
        //            "LEFT JOIN dimMasterValue masterValue1 ON masterValue1.idMasterValue = interfacing.cgstId " +
        //            "LEFT JOIN dimMasterValue masterValue2 ON masterValue2.idMasterValue = interfacing.sgstId " +
        //            "LEFT JOIN dimMasterValue masterValue3 ON masterValue3.idMasterValue = interfacing.igstId " +
        //            "LEFT JOIN dimMasterValue masterValue4 ON masterValue4.idMasterValue = interfacing.purAccId " +
        //            "LEFT JOIN dimMasterValue masterValue5 ON masterValue5.idMasterValue = interfacing.otherExpAccId " +
        //            " LEFT JOIN dimMasterValue masterValue7 ON masterValue7.idMasterValue = interfacing.gradeid " +
        //            " LEFT JOIN dimMasterValue masterCostCat ON masterCostCat.idMasterValue = interfacing.costCategoryId " +
        //            " LEFT JOIN dimMasterValue interItemDesc ON interItemDesc.idMasterValue = interfacing.materialtemId " +
        //            " LEFT JOIN tblPurchaseInvoiceItemDetails ItemDetailtax ON ItemDetailtax.purchaseInvoiceId = invoice.idInvoicePurchase " +
        //            " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase and invoiceAddr.txnAddrTypeId = " + (Int32)Constants.TxnTypeE.FOR_SUPPLIER +
        //            " LEFT JOIN dimMasterValue masterCostCentrer ON masterCostCentrer.idMasterValue = interfacing.costCenterId " +
        //            " LEFT JOIN dimMasterValue masterValue6 ON masterValue6.idMasterValue = interfacing.ipTransportAdvAccId " +
        //            " LEFT JOIN dimMasterValue masterValue8 ON masterValue8.idMasterValue = interfacing.otherExpInsuAccId " +
        //            " LEFT JOIN dimMasterValue masterValue9 ON masterValue9.idMasterValue = interfacing.tdsAccId " +
        //            " LEFT JOIN " +
        //            " ( " +
        //            "  SELECT purchaseWeighingStageSummary.purchaseScheduleSummaryId,tblWeighingMachine.machineName AS godown" +
        //            "  FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary" +
        //            "  LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = purchaseWeighingStageSummary.weighingMachineId" +
        //            "  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 1" +
        //            "  ) AS purchaseWeighing" +
        //            " ON purchaseWeighing.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
        //            "WHERE isnull(ItemDetails.otherTaxId,0) = 0 " +
        //            //Prajakta[2019-03-06] Commented and added
        //            //" AND isnull(tblPurchaseScheduleSummary.updatedOn,tblPurchaseScheduleSummary.createdOn) BETWEEN @fromDate AND @toDate " 
        //            " AND tblPurchaseScheduleSummary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
        //              + " and isnull(tblPurchaseScheduleSummary.isCorrectionCompleted,0)=1" +
        //            " group by ItemDetailtax.purchaseInvoiceId,tblPurchaseScheduleSummary.corretionCompletedOn,tblPurchaseScheduleSummary.createdOn,tblPurchaseScheduleSummary.updatedOn,invoice.invoiceNo,invoice.invoiceDate,invoice.transportorName," +
        //            " tblPurchaseScheduleSummary.vehicleNo,invoice.lrNumber,invoice.electronicRefNo,invoice.ewayBillDate,invoice.ewayBillExpiryDate,masterValue.masterValueName,masterValue4.masterValueName ,  masterValue1.masterValueName,masterCostCat.masterValueName ,masterCostCentrer.masterValueName, masterValue2.masterValueName ," +
        //            " masterValue3.masterValueName,masterValue5.masterValueName ,masterValue6.masterValueName,invoiceAddr.billingPartyName,interItemDesc.masterValueName,invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,invoice.transportorAdvAmt,invoice.grandTotal,interfacing.narration,masterValue7.mastervaluename," +
        //            " ItemDetails.invoiceQty,ItemDetails.rate,ItemDetails.basicTotal,invoice.idInvoicePurchase,masterValue8.masterValueName,masterValue9.masterValueName,purchaseWeighing.godown";
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
        //        SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

        //        List<purchaseSummuryReportTo> list = ConvertDTToListForPurschase(reader);

        //        reader.Close();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        public List<purchaseSummuryReportTo> PurchaseSummaryReportH(DateTime fromDate, DateTime toDate,String purchaseMangerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            DateTime f = Constants.GetStartDateTime(fromDate);
            DateTime t = Constants.GetEndDateTime(toDate);
            int otherTaxFreightId = 2;
            int otherTaxTCSId = 5;

            TblConfigParamsTO otherTaxFreightIdTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_OTHER_TAXES_FOR_FREIGHT);
            if (otherTaxFreightIdTO != null)
                otherTaxFreightId = Convert.ToInt32(otherTaxFreightIdTO.ConfigParamVal);
            TblConfigParamsTO otherTaxTCSIdTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS);
            if (otherTaxTCSIdTO != null)
                otherTaxTCSId = Convert.ToInt32(otherTaxTCSIdTO.ConfigParamVal);

            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = " Select distinct " +
                                        " tblPurchaseScheduleSummary.createdOn,invoice.idInvoicePurchase,invoice.invoiceNo,invoice.invoiceDate,invAddrBill.billingPartyName as partyName,invAddrBill.supplierDist , invAddrBill.stateName as salerState , " +
                                        " invAddrBill.gstinNo as salerGstNo,invAddrBill.supplierAddress,purchaseManager.userDisplayName purchaseManager,tblPurchaseEnquiry.bookingRate, " +
                                        " tblProdClassification.prodClassDesc as materialType,itemDetails.productItemDesc, itemDetails.prodClassId, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.taxableAmt  " +
                                        " as basinAmt  ,freightItem.freightAmt,itemDetails.productItemId as invoiceItemId,invoice.cgstAmt, " +
                                        " invoice.igstAmt,invoice.sgstAmt,itemDetails.rate,itemDetails.idPurchaseInvoiceItem,itemDetails.cdAmt,itemDetails.otherTaxId,  " +
                                        " transportOrg.firmName as transporterName,invoice.vehicleNo,transportOrg.registeredMobileNos as transporterMobNo " +
                                        " , invoice.grandTotal as partyPayable, invoice.statusId  , supplier.registeredMobileNos as supplierMobNo ,broker.registeredMobileNos as brokerMobNo,  invoice.lrDate , invoice.lrNumber,supplier.overdue_ref_id as tallyRefId,supplier.firmName" +
                                        " ,TCSItem.TCSAmt FROM tblPurchaseInvoice invoice  " +
                                        " LEFT JOIN(select invAddrB.purchaseInvoiceId, invAddrB.billingPartyName, " +
                                        " invAddrB.txnAddrTypeId,  invAddrB.gstinNo,invAddrB.district as supplierDist, invAddrB.state as stateName, invAddrB.address as supplierAddress from tblPurchaseInvoiceAddr invAddrB  " +
                                        " where txnAddrTypeId =  1)invAddrBill  on invAddrBill.purchaseInvoiceId = invoice.idInvoicePurchase  " +
                                        " LEFT JOIN tblOrganization supplier  ON supplier.idOrganization = invoice.supplierId  " +
                                        " LEFT JOIN tblOrganization broker ON broker.idOrganization= invoice.brokerId " +
                                        " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                                        " INNER JOIN tblPurchaseInvoiceItemDetails itemDetails  ON itemDetails.purchaseInvoiceId = invoice.idInvoicePurchase AND itemDetails.otherTaxId is  NULL " +
                                        " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId  " +
                                        " LEFT JOIN tblProdClassification mat on mat.idProdClass = prodGstCodeDtl.prodClassId  " +
                                        " LEFT JOIN(select purchaseInvoiceId, taxableAmt as freightAmt  from tblPurchaseInvoiceItemDetails where otherTaxId = " + otherTaxFreightId + "  ) " +
                                        " freightItem On freightItem.purchaseInvoiceId = invoice.idInvoicePurchase " +
                                        " LEFT JOIN(select purchaseInvoiceId, taxableAmt as TCSAmt  from tblPurchaseInvoiceItemDetails where otherTaxId = " + otherTaxTCSId + "  ) " +
                                        " TCSItem On TCSItem.purchaseInvoiceId = invoice.idInvoicePurchase " +
                                        " LEFT JOIN tblPurchaseScheduleSummary tblPurchaseScheduleSummary on isnull(rootScheduleId,idPurchaseScheduleSummary)=invoice.purSchSummaryId " +
                                        " left join tblPurchaseEnquiry tblPurchaseEnquiry on tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
                                        " left join tblUser purchaseManager on purchaseManager.idUser = tblPurchaseEnquiry.userId " +
                                        " left join tblProdClassification tblProdClassification on tblProdClassification.idProdClass = itemDetails.prodClassId " +
                                        " WHERE CAST(tblPurchaseScheduleSummary.corretionCompletedOn AS DATE) " +
                                        " BETWEEN @fromDate AND @toDate AND invoice.statusId = 9 and " +
                                        " tblPurchaseScheduleSummary.vehiclePhaseId = " +
                                        (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " and tblPurchaseScheduleSummary.isCorrectionCompleted =1 " +
                                        " and tblPurchaseScheduleSummary.statusId = " + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
                if (!String.IsNullOrEmpty(purchaseMangerIds))
                {
                    cmdSelect.CommandText += " AND tblPurchaseScheduleSummary.purchaseEnquiryId IN (SELECT tblPurchaseEnquiry.idPurchaseEnquiry" +
                                             "                                                      FROM tblPurchaseEnquiry tblPurchaseEnquiry" +
                                             "                                                      WHERE tblPurchaseEnquiry.userId IN (" + purchaseMangerIds + "))";
                }
                cmdSelect.CommandText += " order by invoice.invoiceNo asc ";
                                       //tblPurchaseScheduleSummary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED + " and
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<purchaseSummuryReportTo> list = ConvertDTToListForPurschaseSummaryReportH(reader);

                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //chetan[2019-11-01] added for print GradeWiswWnloadingReport
        public List<GradeWiseWnloadingReportTO> SelectGradeWiseWnloadingReport(DateTime fromDate, DateTime toDate, int ConfirmTypeId,String purchaseManagerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                //Added by minal QtyMT = display 3 decimal point for report
                cmdSelect.CommandText = " select  scheduleDetails.prodItemId, CAST(ROUND((sum(scheduleDetails.qty)),3) AS NUMERIC(36,3)) as qty,tblProductItem.itemName from tblPurchaseScheduleSummary summary " +
                                        " left join tblPurchaseScheduleDetails scheduleDetails on scheduleDetails.purchaseScheduleSummaryId = summary.idPurchaseScheduleSummary " +
                                        " left join tblProductItem tblProductItem on tblProductItem.idProdItem = scheduleDetails.prodItemId " +
                                        " where summary.corretionCompletedOn BETWEEN @fromDate AND @toDate " +
                                        " AND ISNULL(summary.isCorrectionCompleted, 0) = 1 AND summary.statusId = " + (int)StaticStuff.Constants.TranStatusE.UNLOADING_COMPLETED +
                                        " and summary.vehiclePhaseId =" + (int)StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS;

                if (ConfirmTypeId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                    cmdSelect.CommandText += " and summary.cOrNCId =" + ConfirmTypeId;
                if (!String.IsNullOrEmpty(purchaseManagerIds))
                {
                    cmdSelect.CommandText += " AND summary.purchaseEnquiryId IN (SELECT tblPurchaseEnquiry.idPurchaseEnquiry " +
                                             "                                    FROM tblPurchaseEnquiry tblPurchaseEnquiry " +
                                             "                                    WHERE tblPurchaseEnquiry.userId IN ( " + purchaseManagerIds + "))";
                }
                cmdSelect.CommandText += " group by scheduleDetails.prodItemId,tblProductItem.itemName ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                // cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<GradeWiseWnloadingReportTO> list = ConvertDTToListGradeWiseWnloadingReport(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<PartWiseReportTO> ConvertDTToListForPartyWise(SqlDataReader partyWiseReportTODT)
        {
            List<PartWiseReportTO> partWiseReportTOList = new List<PartWiseReportTO>();
            if (partyWiseReportTODT != null)
            {
                while (partyWiseReportTODT.Read())
                {
                    PartWiseReportTO partWiseReportTONew = new PartWiseReportTO();
                    if (partyWiseReportTODT["supplier"] != DBNull.Value)
                        partWiseReportTONew.Supplier = Convert.ToString(partyWiseReportTODT["supplier"].ToString());
                    if (partyWiseReportTODT["supplierId"] != DBNull.Value)
                        partWiseReportTONew.SupplierId = Convert.ToInt64(partyWiseReportTODT["supplierId"].ToString());
                    if (partyWiseReportTODT["isTotalRow"] != DBNull.Value)
                        partWiseReportTONew.IsTotalRow = Convert.ToInt16(partyWiseReportTODT["isTotalRow"].ToString());
                    if (partyWiseReportTODT["grade"] != DBNull.Value)
                        partWiseReportTONew.Grade = Convert.ToString(partyWiseReportTODT["grade"].ToString());
                    if (partyWiseReportTODT["totalQty"] != DBNull.Value)
                        partWiseReportTONew.TotalQty = Convert.ToDouble(partyWiseReportTODT["totalQty"].ToString());
                    if (partyWiseReportTODT["totalAmount"] != DBNull.Value)
                        partWiseReportTONew.TotalAmount = Convert.ToDouble(partyWiseReportTODT["totalAmount"].ToString());
                    if (partyWiseReportTODT["averageRate"] != DBNull.Value)
                        partWiseReportTONew.AverageRate = Convert.ToDouble(partyWiseReportTODT["averageRate"].ToString());
                    if (partyWiseReportTODT["displaySupplier"] != DBNull.Value)
                        partWiseReportTONew.DisplaySupplier = Convert.ToString(partyWiseReportTODT["displaySupplier"].ToString());
                    partWiseReportTOList.Add(partWiseReportTONew);
                }
            }
            return partWiseReportTOList;
        }

        public List<VehicleWiseReportTO> ConvertDTToListForVehicleWise(SqlDataReader vehicleWiseReportTODT)
        {
            List<VehicleWiseReportTO> vehicleWiseReportTOTOList = new List<VehicleWiseReportTO>();
            if (vehicleWiseReportTODT != null)
            {
                while (vehicleWiseReportTODT.Read())
                {
                    VehicleWiseReportTO vehicleWiseReportTONew = new VehicleWiseReportTO();
                    if (vehicleWiseReportTODT["vehicleNumber"] != DBNull.Value)
                        vehicleWiseReportTONew.VehicleNumber = Convert.ToString(vehicleWiseReportTODT["vehicleNumber"].ToString());
                    if (vehicleWiseReportTODT["date"] != DBNull.Value)
                       vehicleWiseReportTONew.Date = Convert.ToString(vehicleWiseReportTODT["date"].ToString());
                    if (vehicleWiseReportTODT["supplier"] != DBNull.Value)
                        vehicleWiseReportTONew.Supplier = Convert.ToString(vehicleWiseReportTODT["supplier"].ToString());
                    if (vehicleWiseReportTODT["remark"] != DBNull.Value)
                        vehicleWiseReportTONew.Remark = Convert.ToString(vehicleWiseReportTODT["remark"].ToString());
                    if (vehicleWiseReportTODT["grade"] != DBNull.Value)
                        vehicleWiseReportTONew.Grade = Convert.ToString(vehicleWiseReportTODT["grade"].ToString());
                    if (vehicleWiseReportTODT["processChargePerVeh"] != DBNull.Value)
                        vehicleWiseReportTONew.ProcessChargePerVeh = Convert.ToDouble(vehicleWiseReportTODT["processChargePerVeh"].ToString());
                    if (vehicleWiseReportTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        vehicleWiseReportTONew.IdPurchaseScheduleSummary = Convert.ToInt32(vehicleWiseReportTODT["idPurchaseScheduleSummary"].ToString());
                    if (vehicleWiseReportTODT["totalQty"] != DBNull.Value)
                        vehicleWiseReportTONew.TotalQty = Convert.ToDouble(vehicleWiseReportTODT["totalQty"].ToString());
                    if (vehicleWiseReportTODT["rate"] != DBNull.Value)
                        vehicleWiseReportTONew.Rate = Convert.ToDouble(vehicleWiseReportTODT["rate"].ToString());
                    if (vehicleWiseReportTODT["totalAmount"] != DBNull.Value)
                        vehicleWiseReportTONew.TotalAmount = Convert.ToDouble(vehicleWiseReportTODT["totalAmount"].ToString());
                    if(vehicleWiseReportTODT["isTotalRow"] != DBNull.Value)
                        vehicleWiseReportTONew.IsTotalRow = Convert.ToInt32(vehicleWiseReportTODT["isTotalRow"].ToString());
                    if (vehicleWiseReportTODT["cOrNCId"] != DBNull.Value)
                        vehicleWiseReportTONew.COrNCId = Convert.ToInt32(vehicleWiseReportTODT["cOrNCId"].ToString());
                    if (vehicleWiseReportTODT["isBoth"] != DBNull.Value)
                        vehicleWiseReportTONew.IsBoth = Convert.ToInt32(vehicleWiseReportTODT["isBoth"].ToString());
                    if (vehicleWiseReportTODT["displayVehicleNo"] != DBNull.Value)
                        vehicleWiseReportTONew.DisplayVehicleNo = Convert.ToString(vehicleWiseReportTODT["displayVehicleNo"].ToString());
                    if (vehicleWiseReportTODT["displayDate"] != DBNull.Value)
                        vehicleWiseReportTONew.DisplayDate = Convert.ToString(vehicleWiseReportTODT["displayDate"].ToString());
                    if (vehicleWiseReportTODT["displaySupplier"] != DBNull.Value)
                        vehicleWiseReportTONew.DisplaySupplier = Convert.ToString(vehicleWiseReportTODT["displaySupplier"].ToString());
                    if (vehicleWiseReportTODT["displayRemark"] != DBNull.Value)
                        vehicleWiseReportTONew.DisplayRemark = Convert.ToString(vehicleWiseReportTODT["displayRemark"].ToString());
                    vehicleWiseReportTOTOList.Add(vehicleWiseReportTONew);
                }
            }
            return vehicleWiseReportTOTOList;
        }

        public List<TallyReportTO> ConvertDTToList(SqlDataReader tallyReportTODT)
        {
            List<TallyReportTO> tallyReportTOTOList = new List<TallyReportTO>();
            if (tallyReportTODT != null)
            {
                while (tallyReportTODT.Read())
                {
                    TallyReportTO tallyReportTONew = new TallyReportTO();
                    if (tallyReportTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tallyReportTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tallyReportTODT["idPurchaseScheduleSummary"].ToString());
                    if (tallyReportTODT["date"] != DBNull.Value)
                    {
                        // DateTime date = new DateTime();
                        // date = Convert.ToDateTime(tallyReportTODT["date"]);
                        // tallyReportTONew.Date = date.ToShortDateString();

                        //DateTime dt = Convert.ToDateTime(tallyReportTODT["date"].ToString());
                        //tallyReportTONew.Date = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        //if (!Startup.IsForBRM)
                        //{
                        //    DateTime dt1 = Convert.ToDateTime(tallyReportTODT["date"].ToString());
                        //    tallyReportTONew.Date = dt1.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                        //}
                        tallyReportTONew.Date = Convert.ToString(tallyReportTODT["date"].ToString());
                    }
                    if (tallyReportTODT["truckNo"] != DBNull.Value)
                        tallyReportTONew.TruckNo = Convert.ToString(tallyReportTODT["truckNo"].ToString());
                    if (tallyReportTODT["supplierName"] != DBNull.Value)
                        tallyReportTONew.SupplierName = Convert.ToString(tallyReportTODT["supplierName"].ToString());
                    if (tallyReportTODT["pm"] != DBNull.Value)
                        tallyReportTONew.PM = Convert.ToString(tallyReportTODT["pm"].ToString());
                    if (tallyReportTODT["location"] != DBNull.Value)
                        tallyReportTONew.Location = Convert.ToString(tallyReportTODT["location"].ToString());
                    if (tallyReportTODT["grade"] != DBNull.Value)
                        tallyReportTONew.Grade = Convert.ToString(tallyReportTODT["grade"].ToString());
                    if (tallyReportTODT["gradeQty"] != DBNull.Value)
                    {
                        tallyReportTONew.GradeQty = Convert.ToDouble(tallyReportTODT["gradeQty"].ToString());
                        double a = Convert.ToDouble(tallyReportTODT["gradeQty"].ToString());
                        tallyReportTONew.DisplayGradeQty = String.Format("{0:0.000}", tallyReportTONew.GradeQty);
                    }
                    if (tallyReportTODT["gradeRate"] != DBNull.Value)
                    {
                        tallyReportTONew.GradeRate = Convert.ToDouble(tallyReportTODT["gradeRate"].ToString());
                        //Added by minal for display gradeRate after two decimal point for report                        
                        
                        if (Startup.IsForBRM)
                        {
                            tallyReportTONew.GradeRate = tallyReportTONew.GradeRate / 1000;
                        }
                        else if (!Startup.IsForBRM)
                        {
                            tallyReportTONew.GradeRate = tallyReportTONew.GradeRate / 100;
                        }
                        tallyReportTONew.DisplayGradeRate = String.Format("{0:0.00}", tallyReportTONew.GradeRate);
                        if (Startup.IsForBRM)
                        {
                            tallyReportTONew.DisplayGradeRate = String.Format("{0:0.000}", tallyReportTONew.GradeRate);
                        }                        
                    }
                    if (tallyReportTODT["total"] != DBNull.Value)
                    {
                        tallyReportTONew.Total = Convert.ToDouble(tallyReportTODT["total"].ToString());
                        
                        if (Startup.IsForBRM)
                        {
                            tallyReportTONew.Total = tallyReportTONew.Total / 1000;
                        }
                        else if (!Startup.IsForBRM)
                        {
                            tallyReportTONew.Total = tallyReportTONew.Total / 100;
                        }
                        //tallyReportTONew.Total = Math.Round(tallyReportTONew.Total, 2);
                        //Added by minal for display Total after two decimal point for report                       
                        tallyReportTONew.DisplayTotal = String.Format("{0:0.00}", tallyReportTONew.Total);
                        if (Startup.IsForBRM)
                        {
                            tallyReportTONew.DisplayTotal = String.Format("{0:0.000}", tallyReportTONew.Total);
                        }                        
                    }
                    if (tallyReportTODT["billType"] != DBNull.Value)
                        tallyReportTONew.BillType = Convert.ToString(tallyReportTODT["billType"].ToString());
                    if (tallyReportTODT["materialType"] != DBNull.Value)
                        tallyReportTONew.MaterialType = Convert.ToString(tallyReportTODT["materialType"].ToString());
                    if (tallyReportTODT["containerNo"] != DBNull.Value)
                        tallyReportTONew.ContainerNo = Convert.ToString(tallyReportTODT["containerNo"].ToString());

                    if(!Startup.IsForBRM)
                    {
                        if (tallyReportTODT["spotVehContainerNo"] != DBNull.Value)
                            tallyReportTONew.ContainerNo = Convert.ToString(tallyReportTODT["spotVehContainerNo"].ToString());
                    }

                    if (tallyReportTODT["dustQty"] != DBNull.Value)
                        tallyReportTONew.DustQty = Convert.ToDouble(tallyReportTODT["dustQty"].ToString());
                    if (tallyReportTODT["godown"] != DBNull.Value)
                        tallyReportTONew.Godown = Convert.ToString(tallyReportTODT["godown"].ToString());
                    if (tallyReportTODT["processChargePerVeh"] != DBNull.Value)
                        tallyReportTONew.ProcessChargePerVeh = Convert.ToDouble(tallyReportTODT["processChargePerVeh"].ToString());

                    if (Startup.IsForBRM)
                    {
                        tallyReportTONew.ProcessChargePerVeh = tallyReportTONew.ProcessChargePerVeh / 1000;
                    }
                    else
                    {
                        tallyReportTONew.ProcessChargePerVeh = tallyReportTONew.ProcessChargePerVeh / 100;
                    }

                    tallyReportTONew.DisplayProcessChargePerVeh = String.Format("{0:0.00}", tallyReportTONew.ProcessChargePerVeh);
                    if (Startup.IsForBRM)
                    {
                        tallyReportTONew.DisplayProcessChargePerVeh = String.Format("{0:0.000}", tallyReportTONew.ProcessChargePerVeh);
                    }

                    if (tallyReportTODT["cOrNCId"] != DBNull.Value)
                        tallyReportTONew.COrNCId = Convert.ToInt32(tallyReportTODT["cOrNCId"].ToString());
                    if (tallyReportTODT["isBoth"] != DBNull.Value)
                        tallyReportTONew.IsBoth = Convert.ToInt32(tallyReportTODT["isBoth"].ToString());
                    if (tallyReportTODT["rootScheduleId"] != DBNull.Value)
                        tallyReportTONew.RootScheduleId = Convert.ToInt32(tallyReportTODT["rootScheduleId"].ToString());
                    if (tallyReportTODT["correNarration"] != DBNull.Value) //Added by minal for Kalika
                        tallyReportTONew.Narration = Convert.ToString(tallyReportTODT["correNarration"].ToString());
                    if (Startup.IsForBRM)
                    {
                        if (tallyReportTODT["remark"] != DBNull.Value) 
                            tallyReportTONew.Narration = Convert.ToString(tallyReportTODT["remark"].ToString());
                    }
                    else
                    {
                        try
                        {
                            //Added by Dhananjay for Kalika
                            string PartyWeight = "";

                            if (tallyReportTODT["partyTareWeight"] != DBNull.Value)
                                PartyWeight += Convert.ToString(tallyReportTODT["partyTareWeight"].ToString());

                            if(string.IsNullOrEmpty(PartyWeight))
                            {
                                if (tallyReportTODT["partyNetWeight"] != DBNull.Value)
                                    PartyWeight += Convert.ToString(tallyReportTODT["partyNetWeight"].ToString());
                            }
                            else
                            {
                                if (tallyReportTODT["partyNetWeight"] != DBNull.Value)
                                    PartyWeight += ", " + Convert.ToString(tallyReportTODT["partyNetWeight"].ToString());
                            }

                            if (string.IsNullOrEmpty(PartyWeight))
                            {
                                if (tallyReportTODT["partyGrossWeight"] != DBNull.Value)
                                    PartyWeight += Convert.ToString(tallyReportTODT["partyGrossWeight"].ToString());
                            }
                            else
                            {
                                if (tallyReportTODT["partyGrossWeight"] != DBNull.Value)
                                    PartyWeight += ", " + Convert.ToString(tallyReportTODT["partyGrossWeight"].ToString());
                            }

                            if (!string.IsNullOrEmpty(PartyWeight))
                            {
                                tallyReportTONew.Narration += " Party weight: " + PartyWeight;
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                    //if (tallyReportTODT["grandTotal"] != DBNull.Value)
                    //{
                    //    tallyReportTONew.GrandTotal = Convert.ToDouble(tallyReportTODT["grandTotal"].ToString());

                    //    if (Startup.IsForBRM)
                    //    {
                    //        tallyReportTONew.GrandTotal = tallyReportTONew.GrandTotal / 1000;
                    //    }
                    //    else if (!Startup.IsForBRM)
                    //    {
                    //        tallyReportTONew.GrandTotal = tallyReportTONew.GrandTotal / 100;
                    //    }
                    //    //tallyReportTONew.Total = Math.Round(tallyReportTONew.Total, 2);
                    //    //Added by minal for display Total after two decimal point for report                       
                    //    tallyReportTONew.DisplayGrandTotal = String.Format("{0:0.00}", tallyReportTONew.GrandTotal);
                    //    if (Startup.IsForBRM)
                    //    {
                    //        tallyReportTONew.DisplayGrandTotal = String.Format("{0:0.000}", tallyReportTONew.GrandTotal);
                    //    }
                    //}
                    if (tallyReportTODT["voucherNo"] != DBNull.Value)
                        tallyReportTONew.VoucherNo = Convert.ToString(tallyReportTODT["voucherNo"].ToString());
                    if (tallyReportTODT["purchaseLedger"] != DBNull.Value)
                        tallyReportTONew.PurchaseLedger = Convert.ToString(tallyReportTODT["purchaseLedger"].ToString());
                    if (tallyReportTODT["displayRecordInFirstRow"] != DBNull.Value)
                        tallyReportTONew.DisplayRecordInFirstRow = Convert.ToInt32(tallyReportTODT["displayRecordInFirstRow"].ToString());

                    if (tallyReportTODT["GrossWeight"] != DBNull.Value)
                        tallyReportTONew.GrossWeight = Convert.ToDouble(tallyReportTODT["GrossWeight"].ToString());
                    if (tallyReportTODT["TareWeight"] != DBNull.Value)
                        tallyReportTONew.TareWeight = Convert.ToDouble(tallyReportTODT["TareWeight"].ToString());
                    if (tallyReportTODT["NetWeight"] != DBNull.Value)
                        tallyReportTONew.NetWeight = Convert.ToDouble(tallyReportTODT["NetWeight"].ToString());

                    tallyReportTOTOList.Add(tallyReportTONew);
                }
            }
            return tallyReportTOTOList;
        }

        public List<TallyReportTO> ConvertDTToListForTallyPREnquiryDropbox(SqlDataReader tallyReportTODT)
        {
            List<TallyReportTO> tallyReportTOTOList = new List<TallyReportTO>();
            if (tallyReportTODT != null)
            {
                while (tallyReportTODT.Read())
                {
                    TallyReportTO tallyReportTONew = new TallyReportTO();
                    if (tallyReportTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tallyReportTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tallyReportTODT["idPurchaseScheduleSummary"].ToString());
                    if (tallyReportTODT["date"] != DBNull.Value)
                    {
                        // DateTime date = new DateTime();
                        // date = Convert.ToDateTime(tallyReportTODT["date"]);
                        // tallyReportTONew.Date = date.ToShortDateString();

                        DateTime dt = Convert.ToDateTime(tallyReportTODT["date"].ToString());
                        tallyReportTONew.Date = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                    }
                    if (tallyReportTODT["truckNo"] != DBNull.Value)
                        tallyReportTONew.TruckNo = Convert.ToString(tallyReportTODT["truckNo"].ToString());
                    if (tallyReportTODT["supplierName"] != DBNull.Value)
                        tallyReportTONew.SupplierName = Convert.ToString(tallyReportTODT["supplierName"].ToString());
                    if (tallyReportTODT["pm"] != DBNull.Value)
                        tallyReportTONew.PM = Convert.ToString(tallyReportTODT["pm"].ToString());
                    if (tallyReportTODT["location"] != DBNull.Value)
                        tallyReportTONew.Location = Convert.ToString(tallyReportTODT["location"].ToString());
                    if (tallyReportTODT["grade"] != DBNull.Value)
                        tallyReportTONew.Grade = Convert.ToString(tallyReportTODT["grade"].ToString());
                    if (tallyReportTODT["gradeQty"] != DBNull.Value)
                    {
                        tallyReportTONew.GradeQty = Convert.ToDouble(tallyReportTODT["gradeQty"].ToString());
                        double a = Convert.ToDouble(tallyReportTODT["gradeQty"].ToString());
                        tallyReportTONew.DisplayGradeQty = String.Format("{0:0.000}", tallyReportTONew.GradeQty);
                    }
                    if (tallyReportTODT["gradeRate"] != DBNull.Value)
                    {
                        tallyReportTONew.GradeRate = Convert.ToDouble(tallyReportTODT["gradeRate"].ToString());
                        //Added by minal for display gradeRate after two decimal point for report                        

                        if (Startup.IsForBRM)
                        {
                            tallyReportTONew.GradeRate = tallyReportTONew.GradeRate / 1000;
                        }
                        else if (!Startup.IsForBRM)
                        {
                            tallyReportTONew.GradeRate = tallyReportTONew.GradeRate / 100;
                        }
                        tallyReportTONew.DisplayGradeRate = String.Format("{0:0.00}", tallyReportTONew.GradeRate);
                        if (Startup.IsForBRM)
                        {
                            tallyReportTONew.DisplayGradeRate = String.Format("{0:0.000}", tallyReportTONew.GradeRate);
                        }
                    }
                    if (tallyReportTODT["total"] != DBNull.Value)
                    {
                        tallyReportTONew.Total = Convert.ToDouble(tallyReportTODT["total"].ToString());

                        if (Startup.IsForBRM)
                        {
                            tallyReportTONew.Total = tallyReportTONew.Total / 1000;
                        }
                        else if (!Startup.IsForBRM)
                        {
                            tallyReportTONew.Total = tallyReportTONew.Total / 100;
                        }
                        //tallyReportTONew.Total = Math.Round(tallyReportTONew.Total, 2);
                        //Added by minal for display Total after two decimal point for report                       
                        tallyReportTONew.DisplayTotal = String.Format("{0:0.00}", tallyReportTONew.Total);
                        if (Startup.IsForBRM)
                        {
                            tallyReportTONew.DisplayTotal = String.Format("{0:0.000}", tallyReportTONew.Total);
                        }
                    }
                    if (tallyReportTODT["billType"] != DBNull.Value)
                        tallyReportTONew.BillType = Convert.ToString(tallyReportTODT["billType"].ToString());
                    if (tallyReportTODT["materialType"] != DBNull.Value)
                        tallyReportTONew.MaterialType = Convert.ToString(tallyReportTODT["materialType"].ToString());
                    if (tallyReportTODT["containerNo"] != DBNull.Value)
                        tallyReportTONew.ContainerNo = Convert.ToString(tallyReportTODT["containerNo"].ToString());
                    if (tallyReportTODT["dustQty"] != DBNull.Value)
                        tallyReportTONew.DustQty = Convert.ToDouble(tallyReportTODT["dustQty"].ToString());
                    if (tallyReportTODT["godown"] != DBNull.Value)
                        tallyReportTONew.Godown = Convert.ToString(tallyReportTODT["godown"].ToString());
                    if (tallyReportTODT["processChargePerVeh"] != DBNull.Value)
                        tallyReportTONew.ProcessChargePerVeh = Convert.ToDouble(tallyReportTODT["processChargePerVeh"].ToString());

                    if (Startup.IsForBRM)
                    {
                        tallyReportTONew.ProcessChargePerVeh = tallyReportTONew.ProcessChargePerVeh / 1000;
                    }
                    else
                    {
                        tallyReportTONew.ProcessChargePerVeh = tallyReportTONew.ProcessChargePerVeh / 100;
                    }

                    tallyReportTONew.DisplayProcessChargePerVeh = String.Format("{0:0.00}", tallyReportTONew.ProcessChargePerVeh);
                    if (Startup.IsForBRM)
                    {
                        tallyReportTONew.DisplayProcessChargePerVeh = String.Format("{0:0.000}", tallyReportTONew.ProcessChargePerVeh);
                    }

                    if (tallyReportTODT["cOrNCId"] != DBNull.Value)
                        tallyReportTONew.COrNCId = Convert.ToInt32(tallyReportTODT["cOrNCId"].ToString());
                    if (tallyReportTODT["isBoth"] != DBNull.Value)
                        tallyReportTONew.IsBoth = Convert.ToInt32(tallyReportTODT["isBoth"].ToString());
                    if (tallyReportTODT["rootScheduleId"] != DBNull.Value)
                        tallyReportTONew.RootScheduleId = Convert.ToInt32(tallyReportTODT["rootScheduleId"].ToString());
                    if (tallyReportTODT["correNarration"] != DBNull.Value) //Added by minal for Kalika
                        tallyReportTONew.Narration = Convert.ToString(tallyReportTODT["correNarration"].ToString());
                    if (Startup.IsForBRM)
                    {
                        if (tallyReportTODT["remark"] != DBNull.Value)
                            tallyReportTONew.Narration = Convert.ToString(tallyReportTODT["remark"].ToString());
                    }
                   
                    tallyReportTOTOList.Add(tallyReportTONew);
                }
            }
            return tallyReportTOTOList;
        }

        public List<PadtaReportTO> ConvertDTToListForPadta(SqlDataReader padtaReportTODT)
        {
            List<PadtaReportTO> padtaReportTOTOList = new List<PadtaReportTO>();
            if (padtaReportTODT != null)
            {
                while (padtaReportTODT.Read())
                {
                    PadtaReportTO padtaReportTONew = new PadtaReportTO();
                    if (padtaReportTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        padtaReportTONew.IdPurchaseScheduleSummary = Convert.ToInt32(padtaReportTODT["idPurchaseScheduleSummary"].ToString());
                    if (padtaReportTODT["vehicleNo"] != DBNull.Value)
                        padtaReportTONew.VehicalNo = Convert.ToString(padtaReportTODT["vehicleNo"].ToString());
                    if (padtaReportTODT["supplierName"] != DBNull.Value)
                        padtaReportTONew.SupplierName = Convert.ToString(padtaReportTODT["supplierName"].ToString());
                    if (padtaReportTODT["pm"] != DBNull.Value)
                        padtaReportTONew.Pm = Convert.ToString(padtaReportTODT["pm"].ToString());
                    if (padtaReportTODT["truckType"] != DBNull.Value)
                        padtaReportTONew.TruckType = Convert.ToString(padtaReportTODT["truckType"].ToString());
                    if (padtaReportTODT["qty"] != DBNull.Value)
                        padtaReportTONew.Qty = Convert.ToDouble(padtaReportTODT["qty"].ToString());
                    if (padtaReportTODT["grade"] != DBNull.Value)
                        padtaReportTONew.Grade = Convert.ToString(padtaReportTODT["grade"].ToString());
                    if (padtaReportTODT["gradeRate"] != DBNull.Value)
                        padtaReportTONew.GradeRate = Convert.ToDouble(padtaReportTODT["gradeRate"].ToString());
                    if (padtaReportTODT["productAomunt"] != DBNull.Value)
                        padtaReportTONew.ProductAomunt = Convert.ToDouble(padtaReportTODT["productAomunt"].ToString());
                    if (padtaReportTODT["padta_MT"] != DBNull.Value)
                        padtaReportTONew.Padta_MT = Convert.ToDouble(padtaReportTODT["padta_MT"].ToString());
                    if (padtaReportTODT["sRate"] != DBNull.Value)
                        padtaReportTONew.SRate = Convert.ToDouble(padtaReportTODT["sRate"].ToString());
                    if (padtaReportTODT["productRecovery"] != DBNull.Value)
                        padtaReportTONew.ProductRecovery = Convert.ToDouble(padtaReportTODT["productRecovery"].ToString());
                    if (padtaReportTODT["billType"] != DBNull.Value)
                        padtaReportTONew.BillType = Convert.ToString(padtaReportTODT["billType"].ToString());
                    if (padtaReportTODT["dustQty"] != DBNull.Value)
                        padtaReportTONew.DustQty = Convert.ToDouble(padtaReportTODT["dustQty"].ToString());
                    if (padtaReportTODT["baseRecovery"] != DBNull.Value)
                        padtaReportTONew.BaseItemRecovery = Convert.ToDouble(padtaReportTODT["baseRecovery"].ToString());

                    padtaReportTOTOList.Add(padtaReportTONew);
                }
            }
            return padtaReportTOTOList;
        }

        public List<purchaseSummuryReportTo> ConvertDTToListForPurschaseForOld(SqlDataReader purchaseSummuryReportTODT)
        {
            List<purchaseSummuryReportTo> purchaseSummuryReportTOTOList = new List<purchaseSummuryReportTo>();
            if (purchaseSummuryReportTODT != null)
            {
                while (purchaseSummuryReportTODT.Read())
                {
                    purchaseSummuryReportTo padtaReportTONew = new purchaseSummuryReportTo();
                    if (purchaseSummuryReportTODT["createdOn"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["createdOn"].ToString());
                        padtaReportTONew.CreatedOn = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }

                    if (purchaseSummuryReportTODT["invoiceNo"] != DBNull.Value)
                        padtaReportTONew.InvoiceNo = Convert.ToString(purchaseSummuryReportTODT["invoiceNo"].ToString());
                    if (purchaseSummuryReportTODT["invoiceDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["invoiceDate"].ToString());
                        padtaReportTONew.InvoiceDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        // padtaReportTONew.InvoiceDate = Convert.ToDateTime(purchaseSummuryReportTODT["invoiceDate"].ToString());
                    }
                    if (purchaseSummuryReportTODT["vehicleNo"] != DBNull.Value)
                        padtaReportTONew.VehicleNo = Convert.ToString(purchaseSummuryReportTODT["vehicleNo"].ToString());
                    if (purchaseSummuryReportTODT["transportorName"] != DBNull.Value)
                        padtaReportTONew.TransportorName = Convert.ToString(purchaseSummuryReportTODT["transportorName"].ToString());
                    if (purchaseSummuryReportTODT["electronicRefNo"] != DBNull.Value)
                        padtaReportTONew.ElectronicRefNo = Convert.ToString(purchaseSummuryReportTODT["electronicRefNo"].ToString());
                    if (purchaseSummuryReportTODT["ewayBillDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["ewayBillDate"].ToString());
                        padtaReportTONew.EwayBillDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        // padtaReportTONew.EwayBillDate = Convert.ToDateTime(purchaseSummuryReportTODT["ewayBillDate"].ToString());
                    }
                    if (purchaseSummuryReportTODT["ewayBillExpiryDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["ewayBillExpiryDate"].ToString());
                        padtaReportTONew.EwayBillExpiryDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        // padtaReportTONew.EwayBillExpiryDate = Convert.ToDateTime(purchaseSummuryReportTODT["ewayBillExpiryDate"].ToString());
                    }
                    if (purchaseSummuryReportTODT["supplierName"] != DBNull.Value)
                        padtaReportTONew.SupplierName = Convert.ToString(purchaseSummuryReportTODT["supplierName"].ToString());
                    if (purchaseSummuryReportTODT["voucherType"] != DBNull.Value)
                        padtaReportTONew.VoucherType = Convert.ToString(purchaseSummuryReportTODT["voucherType"].ToString());
                    if (purchaseSummuryReportTODT["purAcc"] != DBNull.Value)
                        padtaReportTONew.PurAcc = Convert.ToString(purchaseSummuryReportTODT["purAcc"].ToString());
                    if (purchaseSummuryReportTODT["cgst"] != DBNull.Value)
                        padtaReportTONew.Cgst = Convert.ToString(purchaseSummuryReportTODT["cgst"].ToString());
                    if (purchaseSummuryReportTODT["igst"] != DBNull.Value)
                        padtaReportTONew.Igst = Convert.ToString(purchaseSummuryReportTODT["igst"].ToString());
                    if (purchaseSummuryReportTODT["sgst"] != DBNull.Value)
                        padtaReportTONew.Sgst = Convert.ToString(purchaseSummuryReportTODT["sgst"].ToString());
                    if (purchaseSummuryReportTODT["otherExpAcc"] != DBNull.Value)
                        padtaReportTONew.OtherExpAcc = Convert.ToString(purchaseSummuryReportTODT["otherExpAcc"].ToString());
                    if (purchaseSummuryReportTODT["ipTransportAdvAcc"] != DBNull.Value)
                        padtaReportTONew.IpTransportAdvAcc = Convert.ToString(purchaseSummuryReportTODT["ipTransportAdvAcc"].ToString());
                    if (purchaseSummuryReportTODT["productItemDesc"] != DBNull.Value)
                        padtaReportTONew.ProductItemDesc = Convert.ToString(purchaseSummuryReportTODT["productItemDesc"].ToString());
                    if (purchaseSummuryReportTODT["invoiceQty"] != DBNull.Value)
                        padtaReportTONew.InvoiceQty = Convert.ToString(purchaseSummuryReportTODT["invoiceQty"].ToString());
                    if (purchaseSummuryReportTODT["rate"] != DBNull.Value)
                        padtaReportTONew.Rate = Convert.ToString(purchaseSummuryReportTODT["rate"].ToString());
                    if (purchaseSummuryReportTODT["basicTotal"] != DBNull.Value)
                        padtaReportTONew.BasicTotal = Convert.ToDouble(purchaseSummuryReportTODT["basicTotal"].ToString());
                    if (purchaseSummuryReportTODT["cgstAmt"] != DBNull.Value)
                        padtaReportTONew.CgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["cgstAmt"].ToString());
                    if (purchaseSummuryReportTODT["igstAmt"] != DBNull.Value)
                        padtaReportTONew.IgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["igstAmt"].ToString());
                    if (purchaseSummuryReportTODT["sgstAmt"] != DBNull.Value)
                        padtaReportTONew.SgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["sgstAmt"].ToString());
                    if (purchaseSummuryReportTODT["otherExpAmt"] != DBNull.Value)
                        padtaReportTONew.OtherExpAmt = Convert.ToDouble(purchaseSummuryReportTODT["otherExpAmt"].ToString());
                    if (purchaseSummuryReportTODT["transportorAdvAmt"] != DBNull.Value)
                        padtaReportTONew.TransportorAdvAmt = Convert.ToDouble(purchaseSummuryReportTODT["transportorAdvAmt"].ToString());
                    if (purchaseSummuryReportTODT["grandTotal"] != DBNull.Value)
                        padtaReportTONew.GrandTotal = Convert.ToDouble(purchaseSummuryReportTODT["grandTotal"].ToString());
                    if (purchaseSummuryReportTODT["narration"] != DBNull.Value)
                        padtaReportTONew.Narration = Convert.ToString(purchaseSummuryReportTODT["narration"].ToString());
                    if (purchaseSummuryReportTODT["mastervaluename"] != DBNull.Value)
                        padtaReportTONew.Grade = Convert.ToString(purchaseSummuryReportTODT["mastervaluename"].ToString());
                    if (purchaseSummuryReportTODT["costCategory"] != DBNull.Value)
                        padtaReportTONew.CostCategory = Convert.ToString(purchaseSummuryReportTODT["costCategory"].ToString());
                    if (purchaseSummuryReportTODT["costCenter"] != DBNull.Value)
                        padtaReportTONew.CostCenter = Convert.ToString(purchaseSummuryReportTODT["costCenter"].ToString());
                    //if (purchaseSummuryReportTODT["otherExpensesInsuranceInput"] != DBNull.Value)
                        //padtaReportTONew.OtherExpensesInsuranceInput = Convert.ToString(purchaseSummuryReportTODT["otherExpensesInsuranceInput"].ToString());
                    //if (purchaseSummuryReportTODT["tdsInput"] != DBNull.Value)
                      //  padtaReportTONew.TdsInput = Convert.ToString(purchaseSummuryReportTODT["tdsInput"].ToString());
                    //if (purchaseSummuryReportTODT["otherExpensesInsuranceamt"] != DBNull.Value)
                       // padtaReportTONew.OtherExpensesInsuranceamt = Convert.ToDouble(purchaseSummuryReportTODT["otherExpensesInsuranceamt"].ToString());
                    //if (purchaseSummuryReportTODT["tdsAmt"] != DBNull.Value)
                      //  padtaReportTONew.TdsAmt = Convert.ToDouble(purchaseSummuryReportTODT["tdsAmt"].ToString());
                    if (purchaseSummuryReportTODT["godown"] != DBNull.Value)
                        padtaReportTONew.Godown = Convert.ToString(purchaseSummuryReportTODT["godown"].ToString());

                    purchaseSummuryReportTOTOList.Add(padtaReportTONew);
                }
            }
            return purchaseSummuryReportTOTOList;
        }


        public List<purchaseSummuryReportTo> ConvertDTToListForPurschase(SqlDataReader purchaseSummuryReportTODT)
        {
            List<purchaseSummuryReportTo> purchaseSummuryReportTOTOList = new List<purchaseSummuryReportTo>();
            if (purchaseSummuryReportTODT != null)
            {
                while (purchaseSummuryReportTODT.Read())
                {
                    purchaseSummuryReportTo padtaReportTONew = new purchaseSummuryReportTo();
                    if (purchaseSummuryReportTODT["createdOn"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["createdOn"].ToString());
                        padtaReportTONew.CreatedOn = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }

                    if (purchaseSummuryReportTODT["invoiceNo"] != DBNull.Value)
                        padtaReportTONew.InvoiceNo = Convert.ToString(purchaseSummuryReportTODT["invoiceNo"].ToString());
                    if (purchaseSummuryReportTODT["invoiceDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["invoiceDate"].ToString());
                        padtaReportTONew.InvoiceDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        // padtaReportTONew.InvoiceDate = Convert.ToDateTime(purchaseSummuryReportTODT["invoiceDate"].ToString());
                    }
                    if (purchaseSummuryReportTODT["vehicleNo"] != DBNull.Value)
                        padtaReportTONew.VehicleNo = Convert.ToString(purchaseSummuryReportTODT["vehicleNo"].ToString());
                    if (purchaseSummuryReportTODT["transportorName"] != DBNull.Value)
                        padtaReportTONew.TransportorName = Convert.ToString(purchaseSummuryReportTODT["transportorName"].ToString());
                    if (purchaseSummuryReportTODT["electronicRefNo"] != DBNull.Value)
                        padtaReportTONew.ElectronicRefNo = Convert.ToString(purchaseSummuryReportTODT["electronicRefNo"].ToString());
                    if (purchaseSummuryReportTODT["ewayBillDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["ewayBillDate"].ToString());
                        padtaReportTONew.EwayBillDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        // padtaReportTONew.EwayBillDate = Convert.ToDateTime(purchaseSummuryReportTODT["ewayBillDate"].ToString());
                    }
                    if (purchaseSummuryReportTODT["ewayBillExpiryDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["ewayBillExpiryDate"].ToString());
                        padtaReportTONew.EwayBillExpiryDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        // padtaReportTONew.EwayBillExpiryDate = Convert.ToDateTime(purchaseSummuryReportTODT["ewayBillExpiryDate"].ToString());
                    }
                    if (purchaseSummuryReportTODT["supplierName"] != DBNull.Value)
                        padtaReportTONew.SupplierName = Convert.ToString(purchaseSummuryReportTODT["supplierName"].ToString());
                    if (purchaseSummuryReportTODT["voucherType"] != DBNull.Value)
                        padtaReportTONew.VoucherType = Convert.ToString(purchaseSummuryReportTODT["voucherType"].ToString());
                    if (purchaseSummuryReportTODT["purAcc"] != DBNull.Value)
                        padtaReportTONew.PurAcc = Convert.ToString(purchaseSummuryReportTODT["purAcc"].ToString());
                    if (purchaseSummuryReportTODT["cgst"] != DBNull.Value)
                        padtaReportTONew.Cgst = Convert.ToString(purchaseSummuryReportTODT["cgst"].ToString());
                    if (purchaseSummuryReportTODT["igst"] != DBNull.Value)
                        padtaReportTONew.Igst = Convert.ToString(purchaseSummuryReportTODT["igst"].ToString());
                    if (purchaseSummuryReportTODT["sgst"] != DBNull.Value)
                        padtaReportTONew.Sgst = Convert.ToString(purchaseSummuryReportTODT["sgst"].ToString());
                    if (purchaseSummuryReportTODT["otherExpAcc"] != DBNull.Value)
                        padtaReportTONew.OtherExpAcc = Convert.ToString(purchaseSummuryReportTODT["otherExpAcc"].ToString());
                    if (purchaseSummuryReportTODT["ipTransportAdvAcc"] != DBNull.Value)
                        padtaReportTONew.IpTransportAdvAcc = Convert.ToString(purchaseSummuryReportTODT["ipTransportAdvAcc"].ToString());
                    if (purchaseSummuryReportTODT["productItemDesc"] != DBNull.Value)
                        padtaReportTONew.ProductItemDesc = Convert.ToString(purchaseSummuryReportTODT["productItemDesc"].ToString());
                    if (purchaseSummuryReportTODT["invoiceQty"] != DBNull.Value)
                    {
                        //padtaReportTONew.InvoiceQty = Convert.ToDouble(purchaseSummuryReportTODT["invoiceQty"].ToString());
                        double qty = Convert.ToDouble(purchaseSummuryReportTODT["invoiceQty"].ToString());
                        padtaReportTONew.InvoiceQty  = String.Format("{0:0.000}", qty);
                    }
                        
                    if (purchaseSummuryReportTODT["rate"] != DBNull.Value)
                    {
                        //padtaReportTONew.Rate = Convert.ToDouble(purchaseSummuryReportTODT["rate"].ToString());
                        double rate = Convert.ToDouble(purchaseSummuryReportTODT["rate"].ToString());
                        padtaReportTONew.Rate = String.Format("{0:0.00}", rate);
                    }
                        
                    if (purchaseSummuryReportTODT["basicTotal"] != DBNull.Value)
                        padtaReportTONew.BasicTotal = Convert.ToDouble(purchaseSummuryReportTODT["basicTotal"].ToString());
                    if (purchaseSummuryReportTODT["cgstAmt"] != DBNull.Value)
                        padtaReportTONew.CgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["cgstAmt"].ToString());
                    if (purchaseSummuryReportTODT["igstAmt"] != DBNull.Value)
                        padtaReportTONew.IgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["igstAmt"].ToString());
                    if (purchaseSummuryReportTODT["sgstAmt"] != DBNull.Value)
                        padtaReportTONew.SgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["sgstAmt"].ToString());
                    if (purchaseSummuryReportTODT["otherExpAmt"] != DBNull.Value)
                        padtaReportTONew.OtherExpAmt = Convert.ToDouble(purchaseSummuryReportTODT["otherExpAmt"].ToString());
                    if (purchaseSummuryReportTODT["transportorAdvAmt"] != DBNull.Value)
                        padtaReportTONew.TransportorAdvAmt = Convert.ToDouble(purchaseSummuryReportTODT["transportorAdvAmt"].ToString());
                    if (purchaseSummuryReportTODT["grandTotal"] != DBNull.Value)
                        padtaReportTONew.GrandTotal = Convert.ToDouble(purchaseSummuryReportTODT["grandTotal"].ToString());

                    if (purchaseSummuryReportTODT["qty"] != DBNull.Value)
                        padtaReportTONew.UnloadedQty = Convert.ToDouble(purchaseSummuryReportTODT["qty"].ToString());

                    if (purchaseSummuryReportTODT["narration"] != DBNull.Value)
                    {
                        padtaReportTONew.Narration = Convert.ToString(purchaseSummuryReportTODT["narration"].ToString());
                        if (!string.IsNullOrEmpty(padtaReportTONew.Narration) && padtaReportTONew.UnloadedQty > 0)
                        {
                            padtaReportTONew.Narration += "/UW-" + String.Format("{0:0.000}", padtaReportTONew.UnloadedQty);
                        }
                    }
                    if (purchaseSummuryReportTODT["mastervaluename"] != DBNull.Value)
                        padtaReportTONew.Grade = Convert.ToString(purchaseSummuryReportTODT["mastervaluename"].ToString());
                    if (purchaseSummuryReportTODT["costCategory"] != DBNull.Value)
                        padtaReportTONew.CostCategory = Convert.ToString(purchaseSummuryReportTODT["costCategory"].ToString());
                    if (purchaseSummuryReportTODT["costCenter"] != DBNull.Value)
                        padtaReportTONew.CostCenter = Convert.ToString(purchaseSummuryReportTODT["costCenter"].ToString());
                    if (purchaseSummuryReportTODT["otherExpensesInsuranceInput"] != DBNull.Value)
                        padtaReportTONew.OtherExpensesInsuranceInput = Convert.ToString(purchaseSummuryReportTODT["otherExpensesInsuranceInput"].ToString());
                    if (purchaseSummuryReportTODT["tdsInput"] != DBNull.Value)
                        padtaReportTONew.TdsInput = Convert.ToString(purchaseSummuryReportTODT["tdsInput"].ToString());
                    if (purchaseSummuryReportTODT["otherExpensesInsuranceamt"] != DBNull.Value)
                        padtaReportTONew.OtherExpensesInsuranceamt = Convert.ToDouble(purchaseSummuryReportTODT["otherExpensesInsuranceamt"].ToString());
                    if (purchaseSummuryReportTODT["tdsAmt"] != DBNull.Value)
                        padtaReportTONew.TdsAmt = Convert.ToDouble(purchaseSummuryReportTODT["tdsAmt"].ToString());
                    if (purchaseSummuryReportTODT["godown"] != DBNull.Value)
                        padtaReportTONew.Godown = Convert.ToString(purchaseSummuryReportTODT["godown"].ToString());

                    //padtaReportTONew.TdsAmt = Math.Round(padtaReportTONew.TdsAmt);
                    padtaReportTONew.AmountToSupplier = padtaReportTONew.GrandTotal - padtaReportTONew.TdsAmt;

                    purchaseSummuryReportTOTOList.Add(padtaReportTONew);
                }
            }
            return purchaseSummuryReportTOTOList;
        }

        public List<purchaseSummuryReportTo> ConvertDTToListForPurschaseSummaryReportH(SqlDataReader purchaseSummuryReportTODT)
        {
            List<purchaseSummuryReportTo> purchaseSummuryReportTOTOList = new List<purchaseSummuryReportTo>();
            if (purchaseSummuryReportTODT != null)
            {
                while (purchaseSummuryReportTODT.Read())
                {
                    purchaseSummuryReportTo padtaReportTONew = new purchaseSummuryReportTo();


                    if (purchaseSummuryReportTODT["idInvoicePurchase"] != DBNull.Value)
                        padtaReportTONew.IdInvoicePurchase = Convert.ToInt32(purchaseSummuryReportTODT["idInvoicePurchase"].ToString());

                    if (purchaseSummuryReportTODT["invoiceNo"] != DBNull.Value)
                        padtaReportTONew.InvoiceNo = Convert.ToString(purchaseSummuryReportTODT["invoiceNo"].ToString());

                    if (purchaseSummuryReportTODT["invoiceDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["invoiceDate"].ToString());
                        padtaReportTONew.InvoiceDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }

                    if (purchaseSummuryReportTODT["createdOn"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["createdOn"].ToString());
                        padtaReportTONew.CreatedOn = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }

                    if (purchaseSummuryReportTODT["vehicleNo"] != DBNull.Value)
                        padtaReportTONew.VehicleNo = Convert.ToString(purchaseSummuryReportTODT["vehicleNo"].ToString());

                    if (purchaseSummuryReportTODT["partyName"] != DBNull.Value)
                        padtaReportTONew.SupplierName = Convert.ToString(purchaseSummuryReportTODT["partyName"].ToString());

                    if (purchaseSummuryReportTODT["partyName"] != DBNull.Value)
                        padtaReportTONew.SupplierName = Convert.ToString(purchaseSummuryReportTODT["partyName"].ToString());

                    if (purchaseSummuryReportTODT["supplierDist"] != DBNull.Value)
                        padtaReportTONew.SupplierDist = Convert.ToString(purchaseSummuryReportTODT["supplierDist"].ToString());

                    if (purchaseSummuryReportTODT["salerState"] != DBNull.Value)
                        padtaReportTONew.SalerState = Convert.ToString(purchaseSummuryReportTODT["salerState"].ToString());

                    if (purchaseSummuryReportTODT["salerGstNo"] != DBNull.Value)
                        padtaReportTONew.SalerGstin = Convert.ToString(purchaseSummuryReportTODT["salerGstNo"].ToString());


                    if (purchaseSummuryReportTODT["purchaseManager"] != DBNull.Value)
                        padtaReportTONew.PurchaseManager = Convert.ToString(purchaseSummuryReportTODT["purchaseManager"].ToString());

                    if (purchaseSummuryReportTODT["bookingRate"] != DBNull.Value)
                        padtaReportTONew.BookingRate = Convert.ToDouble(purchaseSummuryReportTODT["bookingRate"].ToString());

                    if (purchaseSummuryReportTODT["productItemDesc"] != DBNull.Value)
                        padtaReportTONew.ProductItemDesc = Convert.ToString(purchaseSummuryReportTODT["productItemDesc"].ToString());

                    if (purchaseSummuryReportTODT["invoiceQty"] != DBNull.Value)
                        padtaReportTONew.InvoiceQty = Convert.ToString(purchaseSummuryReportTODT["invoiceQty"].ToString());

                    if (purchaseSummuryReportTODT["basinAmt"] != DBNull.Value)
                        padtaReportTONew.BasicTotal = Convert.ToDouble(purchaseSummuryReportTODT["basinAmt"].ToString());

                    if (purchaseSummuryReportTODT["freightAmt"] != DBNull.Value)
                        padtaReportTONew.FreightAmt = Convert.ToDouble(purchaseSummuryReportTODT["freightAmt"].ToString());

                    if (purchaseSummuryReportTODT["cgstAmt"] != DBNull.Value)
                        padtaReportTONew.Cgst = Convert.ToString(purchaseSummuryReportTODT["cgstAmt"].ToString());

                    if (purchaseSummuryReportTODT["igstAmt"] != DBNull.Value)
                        padtaReportTONew.Igst = Convert.ToString(purchaseSummuryReportTODT["igstAmt"].ToString());

                    if (purchaseSummuryReportTODT["sgstAmt"] != DBNull.Value)
                        padtaReportTONew.Sgst = Convert.ToString(purchaseSummuryReportTODT["sgstAmt"].ToString());

                    if (purchaseSummuryReportTODT["cgstAmt"] != DBNull.Value)
                        padtaReportTONew.CgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["cgstAmt"].ToString());

                    if (purchaseSummuryReportTODT["igstAmt"] != DBNull.Value)
                        padtaReportTONew.IgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["igstAmt"].ToString());

                    if (purchaseSummuryReportTODT["sgstAmt"] != DBNull.Value)
                        padtaReportTONew.SgstAmt = Convert.ToDouble(purchaseSummuryReportTODT["sgstAmt"].ToString());

                    if (purchaseSummuryReportTODT["rate"] != DBNull.Value)
                        padtaReportTONew.Rate = Convert.ToString(purchaseSummuryReportTODT["rate"].ToString());

                    if (purchaseSummuryReportTODT["cdAmt"] != DBNull.Value)
                        padtaReportTONew.CdAmt = Convert.ToDouble(purchaseSummuryReportTODT["cdAmt"].ToString());

                    if (purchaseSummuryReportTODT["transporterName"] != DBNull.Value)
                        padtaReportTONew.TransportorName = Convert.ToString(purchaseSummuryReportTODT["transporterName"].ToString());

                    if (purchaseSummuryReportTODT["transporterMobNo"] != DBNull.Value)
                        padtaReportTONew.TransporterMobNo = Convert.ToString(purchaseSummuryReportTODT["transporterMobNo"].ToString());

                    if (purchaseSummuryReportTODT["partyPayable"] != DBNull.Value)
                        padtaReportTONew.GrandTotal = Convert.ToDouble(purchaseSummuryReportTODT["partyPayable"].ToString());

                    if (purchaseSummuryReportTODT["partyPayable"] != DBNull.Value)
                        padtaReportTONew.GrandTotal = Convert.ToDouble(purchaseSummuryReportTODT["partyPayable"].ToString());

                    if (purchaseSummuryReportTODT["supplierMobNo"] != DBNull.Value)
                        padtaReportTONew.SupplierMobNo = Convert.ToString(purchaseSummuryReportTODT["supplierMobNo"].ToString());

                    if (purchaseSummuryReportTODT["lrDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(purchaseSummuryReportTODT["lrDate"].ToString());
                        padtaReportTONew.LrDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }

                    if (purchaseSummuryReportTODT["lrNumber"] != DBNull.Value)
                        padtaReportTONew.LrNo = Convert.ToString(purchaseSummuryReportTODT["lrNumber"].ToString());

                    if (purchaseSummuryReportTODT["supplierAddress"] != DBNull.Value)
                        padtaReportTONew.SupplierAddress = Convert.ToString(purchaseSummuryReportTODT["supplierAddress"].ToString());

                    if (purchaseSummuryReportTODT["materialType"] != DBNull.Value)
                        padtaReportTONew.MaterialType = Convert.ToString(purchaseSummuryReportTODT["materialType"].ToString());

                    if (purchaseSummuryReportTODT["brokerMobNo"] != DBNull.Value)
                        padtaReportTONew.BrokerMobNo = Convert.ToString(purchaseSummuryReportTODT["brokerMobNo"].ToString());
                    if (purchaseSummuryReportTODT["tallyRefId"] != DBNull.Value)
                        padtaReportTONew.BrokerMobNo = Convert.ToString(purchaseSummuryReportTODT["tallyRefId"].ToString());
                    if (purchaseSummuryReportTODT["idPurchaseInvoiceItem"] != DBNull.Value)
                        padtaReportTONew.IdPurchaseInvoiceItem = Convert.ToInt32(purchaseSummuryReportTODT["idPurchaseInvoiceItem"].ToString());
                    if (purchaseSummuryReportTODT["firmName"] != DBNull.Value)
                        padtaReportTONew.OrgSupplierName = Convert.ToString(purchaseSummuryReportTODT["firmName"].ToString());
                    if (purchaseSummuryReportTODT["TCSAmt"] != DBNull.Value)
                        padtaReportTONew.TCSAmt = Convert.ToDouble(purchaseSummuryReportTODT["TCSAmt"].ToString());

                    purchaseSummuryReportTOTOList.Add(padtaReportTONew);
                }
            }
            return purchaseSummuryReportTOTOList;
        }

        //chetan[2019-11-01] added


        public List<GradeWiseWnloadingReportTO> ConvertDTToListGradeWiseWnloadingReport(SqlDataReader tblPurchaseUnloadingDtlReportTODT)
        {
            List<GradeWiseWnloadingReportTO> gradeWiswWnloadingReportTOList = new List<GradeWiseWnloadingReportTO>();
            if (tblPurchaseUnloadingDtlReportTODT != null)
            {
                while (tblPurchaseUnloadingDtlReportTODT.Read())
                {
                    GradeWiseWnloadingReportTO gradeWiswWnloadingReportTO = new GradeWiseWnloadingReportTO();
                    // if (tblPurchaseUnloadingDtlReportTODT["prodItemId"] != DBNull.Value)
                    // {
                    //     gradeWiswWnloadingReportTO.ProdItemId = Convert.ToInt32(tblPurchaseUnloadingDtlReportTODT["prodItemId"].ToString()); ;
                    // }

                    if (tblPurchaseUnloadingDtlReportTODT["itemName"] != DBNull.Value)
                        gradeWiswWnloadingReportTO.ItemName = Convert.ToString(tblPurchaseUnloadingDtlReportTODT["itemName"].ToString());
                    if (tblPurchaseUnloadingDtlReportTODT["qty"] != DBNull.Value)
                    {
                        gradeWiswWnloadingReportTO.QtyMT = Convert.ToDouble(tblPurchaseUnloadingDtlReportTODT["qty"].ToString());
                        //Added by minal QtyMT = display 3 decimal point for report
                        gradeWiswWnloadingReportTO.DisplayQtyMTRpt = String.Format("{0:0.000}", gradeWiswWnloadingReportTO.QtyMT);
                    }
                    gradeWiswWnloadingReportTOList.Add(gradeWiswWnloadingReportTO);
                }
            }
            return gradeWiswWnloadingReportTOList;
        }

        public List<SaudaReportTo> ConvertDTToListSaudaTO(SqlDataReader SaudaReportTODT)
        {
            List<SaudaReportTo> SaudaReportToList = new List<SaudaReportTo>();
            if (SaudaReportTODT != null)
            {
                while (SaudaReportTODT.Read())
                {
                    SaudaReportTo saudaReportTONew = new SaudaReportTo();
                    if (SaudaReportTODT["saudaCreatedOn"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(SaudaReportTODT["saudaCreatedOn"].ToString());
                        //saudaReportTONew.SaudaDate = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        saudaReportTONew.SaudaDate = Convert.ToDateTime(SaudaReportTODT["saudaCreatedOn"].ToString());
                    }

                    if (SaudaReportTODT["enqDisplayNo"] != DBNull.Value)
                        saudaReportTONew.EnquiryNo = Convert.ToString(SaudaReportTODT["enqDisplayNo"].ToString());
                    if (SaudaReportTODT["PM"] != DBNull.Value)
                        saudaReportTONew.PurchaseManager = Convert.ToString(SaudaReportTODT["PM"].ToString());
                    if (SaudaReportTODT["statusName"] != DBNull.Value)
                        saudaReportTONew.StatusName = Convert.ToString(SaudaReportTODT["statusName"].ToString());
                    if (SaudaReportTODT["supplier"] != DBNull.Value)
                        saudaReportTONew.PartyName = Convert.ToString(SaudaReportTODT["supplier"].ToString());
                    if (SaudaReportTODT["bookingRate"] != DBNull.Value)
                        saudaReportTONew.Rate = Convert.ToDouble(SaudaReportTODT["bookingRate"].ToString());
                    if (SaudaReportTODT["bookingQty"] != DBNull.Value)
                        saudaReportTONew.OrgSaudaQty = Convert.ToDouble(SaudaReportTODT["bookingQty"].ToString());
                    if (SaudaReportTODT["closingSaudaQty"] != DBNull.Value)
                        saudaReportTONew.ClosingSaudaQty = Convert.ToDouble(SaudaReportTODT["closingSaudaQty"].ToString());
                    if (SaudaReportTODT["openingSaudaQty"] != DBNull.Value)
                        saudaReportTONew.OpeningSaudaQty = Convert.ToDouble(SaudaReportTODT["openingSaudaQty"].ToString());
                    if (SaudaReportTODT["unloadingQty"] != DBNull.Value)
                        saudaReportTONew.TodaysUnloadingQty = Convert.ToDouble(SaudaReportTODT["unloadingQty"].ToString());
                    if (SaudaReportTODT["idPurchaseEnquiry"] != DBNull.Value)
                        saudaReportTONew.EnquiryId = Convert.ToInt32(SaudaReportTODT["idPurchaseEnquiry"].ToString());
                    if (SaudaReportTODT["daysElapsed"] != DBNull.Value)
                        saudaReportTONew.DaysElapsed = Convert.ToInt32(SaudaReportTODT["daysElapsed"].ToString());


                    SaudaReportToList.Add(saudaReportTONew);
                }
            }
            return SaudaReportToList;
        }

        public List<SaudaReportTo> GetSaudaChartReport(DateTime fromDate, DateTime toDate, Int32 PmId, Int32 SupplierId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select  enq.enqDisplayNo,enq.statusId,DATEDIFF(DAY,cast(enq.saudaCreatedOn as date) , cast(GETDATE() as date) )as daysElapsed,us.userDisplayName as PM,0 as openingSaudaQty,enq.idPurchaseEnquiry, usSupplier.firmName as supplier,enq.bookingRate,sta.statusName,enq.saudaCreatedOn, enq.bookingQty,enq.pendingBookingQty as closingSaudaQty,sum(unload.qtyMT) as unloadingQty"
                + " from tblPurchaseEnquiry enq " +
                " left join tblUser us on enq.userid=us.idUser"
                + " left join tblOrganization usSupplier on enq.SupplierId=usSupplier.idOrganization" +
                " left join dimStatus sta on enq.statusId = sta.idStatus" +
                " left join tblPurchaseScheduleSummary schedule on enq.idPurchaseEnquiry = schedule.purchaseEnquiryId" +
                " left join tblPurchaseUnloadingDtl unload on schedule.idPurchaseScheduleSummary = unload.purchaseScheduleSummaryId " +
                " and  cast(unload.createdOn  as date)= cast(GETDATE() as date) " +
                "where (enq.statusId not in (" + (int)Constants.TranStatusE.BOOKING_DELETE + "," + (int)Constants.TranStatusE.BOOKING_NEW_B + "," + (int)Constants.TranStatusE.BOOKING_REJECTED_BY_DIRECTOR + "," + (int)Constants.TranStatusE.COMPLETED + ","  + (int)Constants.TranStatusE.BOOKING_CANCELED + "," + (int)Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL + "," + (int)Constants.TranStatusE.PENDING_FOR_PURCHASE_MANAGER_APPROVAL + ") " +
                " and (CAST(schedule.createdOn as date)=cast(GETDATE() as date) or isnull(enq.pendingBookingQty,0) > 0) )";
                if (PmId > 0)
                {
                    cmdSelect.CommandText += "and enq.userId=" + PmId;
                }
                if (SupplierId > 0)
                {
                    cmdSelect.CommandText += " and enq.SupplierId=" + SupplierId;
                }
                cmdSelect.CommandText += "  group by enq.idPurchaseEnquiry,enq.enqDisplayNo,enq.statusId,userDisplayName,firmName,saudaCreatedOn,bookingQty,bookingRate,pendingBookingQty, " +
                  " statusName,enq.updatedOn,enq.createdOn order by enq.updatedOn desc";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<SaudaReportTo> list = ConvertDTToListSaudaTO(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<SaudaReportTo> GetSaudaChartReportComplete(DateTime fromDate, DateTime toDate, Int32 PmId, Int32 SupplierId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select  enq.enqDisplayNo,enq.statusId,DATEDIFF(DAY,cast(enq.saudaCreatedOn as date) , cast(GETDATE() as date) )as daysElapsed,us.userDisplayName as PM,0 as openingSaudaQty,enq.idPurchaseEnquiry, usSupplier.firmName as supplier,enq.bookingRate,sta.statusName,enq.saudaCreatedOn, enq.bookingQty,enq.pendingBookingQty as closingSaudaQty,sum(unload.qtyMT) as unloadingQty"
                + " from tblPurchaseEnquiry enq " +
                " left join tblUser us on enq.userid=us.idUser"
                + " left join tblOrganization usSupplier on enq.SupplierId=usSupplier.idOrganization" +
                " left join dimStatus sta on enq.statusId = sta.idStatus" +
                " left join tblPurchaseScheduleSummary schedule on enq.idPurchaseEnquiry = schedule.purchaseEnquiryId" +
                " left join tblPurchaseUnloadingDtl unload on schedule.idPurchaseScheduleSummary = unload.purchaseScheduleSummaryId " +
                " and  cast(unload.createdOn  as date)= cast(GETDATE() as date) " +
                "where  (CAST(schedule.createdOn as date)=cast(GETDATE() as date) and enq.statusId in (" + (int)Constants.TranStatusE.COMPLETED + "," + (int)Constants.TranStatusE.BOOKING_CANCELED + "))";
                if (PmId > 0)
                {
                    cmdSelect.CommandText += "and enq.userId=" + PmId;
                }
                if (SupplierId > 0)
                {
                    cmdSelect.CommandText += " and enq.SupplierId=" + SupplierId;
                }
                cmdSelect.CommandText += "  group by enq.idPurchaseEnquiry,enq.enqDisplayNo,enq.statusId,userDisplayName,firmName,saudaCreatedOn,bookingQty,bookingRate,pendingBookingQty, " +
                  " statusName,enq.updatedOn,enq.createdOn order by enq.updatedOn desc";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<SaudaReportTo> list = ConvertDTToListSaudaTO(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public double GetTotalUnloadedQty(int enquiryId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select sum(tblPurchaseUnloadingDtl.qtyMT) as unloadingQty from tblPurchaseEnquiry tblPurchaseEnquiry "
                + " left join tblPurchaseScheduleSummary tblPurchaseScheduleSummary "
                + " on tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId"
                + " left join tblPurchaseUnloadingDtl tblPurchaseUnloadingDtl "
                + " on tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseUnloadingDtl.purchaseScheduleSummaryId where idPurchaseEnquiry =" + enquiryId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                double unloadingQty = 0;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (sqlReader.Read())
                {
                    if (sqlReader["unloadingQty"] != DBNull.Value)
                        unloadingQty = Convert.ToDouble(sqlReader["unloadingQty"].ToString());
                }
                sqlReader.Dispose();
                return unloadingQty;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

            return 0;
        }


        public double GetTotalScheduledQty(int enquiryId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select sum(qty - isnull(rejectedQty,0)) as unloadingQty from tblPurchaseScheduleSummary " +
                "where purchaseEnquiryId = " + enquiryId + " and statusId = " + (int)Constants.TranStatusE.New + " and CAST(createdOn as date) = CAST(GETDATE() as date)";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                double unloadingQty = 0;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (sqlReader.Read())
                {
                    if (sqlReader["unloadingQty"] != DBNull.Value)
                        unloadingQty = Convert.ToDouble(sqlReader["unloadingQty"].ToString());
                }
                sqlReader.Dispose();
                return unloadingQty;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

            return 0;
        }

        public double GetClosingQty(int enquiryId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select  tblPurchaseEnquiry.bookingQty - sum(tblPurchaseUnloadingDtl.qtyMT) as closingQty "
                + " from tblPurchaseEnquiry tblPurchaseEnquiry left join tblPurchaseScheduleSummary tblPurchaseScheduleSummary on tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId"
                + " left join tblPurchaseUnloadingDtl tblPurchaseUnloadingDtl on tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseUnloadingDtl.purchaseScheduleSummaryId "
                + " where idPurchaseEnquiry = " + enquiryId
                + " group by bookingQty";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                double closingQty = 0;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (sqlReader.Read())
                {
                    if (sqlReader["closingQty"] != DBNull.Value)
                        closingQty = Convert.ToDouble(sqlReader["closingQty"].ToString());
                }
                sqlReader.Dispose();
                return closingQty;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

            return 0;
        }

        public double GetOpeningQty(int enquiryId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select  tblPurchaseEnquiry.bookingQty - sum(tblPurchaseUnloadingDtl.qtyMT) as openingQty "
                + " from tblPurchaseEnquiry tblPurchaseEnquiry left join tblPurchaseScheduleSummary tblPurchaseScheduleSummary on tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId"
                + " left join tblPurchaseUnloadingDtl tblPurchaseUnloadingDtl on tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseUnloadingDtl.purchaseScheduleSummaryId "
                + " and cast(tblPurchaseUnloadingDtl.createdOn  as date) < cast(GETDATE() as date) where idPurchaseEnquiry = " + enquiryId
                + " group by bookingQty";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                double openingQty = 0;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (sqlReader.Read())
                {
                    if (sqlReader["openingQty"] != DBNull.Value)
                        openingQty = Convert.ToDouble(sqlReader["openingQty"].ToString());
                }
                sqlReader.Dispose();
                return openingQty;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

            return 0;
        }

        public double GetOpeningScheduledQty(int enquiryId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select tblPurchaseEnquiry.bookingQty- sum(isnull(tblPurchaseScheduleSummary.qty,0) - isnull(rejectedQty,0)) as openingQty from tblPurchaseScheduleSummary tblPurchaseScheduleSummary "
                + " left join tblPurchaseEnquiry tblPurchaseEnquiry on tblPurchaseEnquiry.idPurchaseEnquiry=tblPurchaseScheduleSummary.purchaseEnquiryId "
                + " where purchaseEnquiryId = " + enquiryId + " and tblPurchaseScheduleSummary.statusId = " + (int)Constants.TranStatusE.New
                + " and CAST(tblPurchaseScheduleSummary.createdOn as date) < CAST(GETDATE() as date) "
                + " group by tblPurchaseScheduleSummary.qty,tblPurchaseEnquiry.bookingQty";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                double openingQty = 0;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (sqlReader.Read())
                {
                    if (sqlReader["openingQty"] != DBNull.Value)
                        openingQty = Convert.ToDouble(sqlReader["openingQty"].ToString());
                }
                sqlReader.Dispose();
                return openingQty;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

            return 0;
        }

        //public List<PartyWiseUnldReportTO> GetPartyWiseUnldReport(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    SqlDataReader reader = null;

        //    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(tblPurSchSummaryFilterTO.FromDate);
        //    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(tblPurSchSummaryFilterTO.ToDate);
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = " SELECT  summary.idPurchaseScheduleSummary,summary.supplierId,summary.vehicleNo,tblOrganization.firmName AS supplierName,scheduleDetails.prodItemId, SUM(scheduleDetails.qty) AS qty,scheduleDetails.rate, tblProductItem.itemName  from tblPurchaseScheduleSummary summary " +
        //                                " LEFT JOIN tblPurchaseScheduleDetails scheduleDetails ON scheduleDetails.purchaseScheduleSummaryId = summary.idPurchaseScheduleSummary " +
        //                                " LEFT JOIN tblProductItem tblProductItem ON tblProductItem.idProdItem = scheduleDetails.prodItemId " +
        //                                " LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization = summary.supplierId " +
        //                                " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = summary.purchaseEnquiryId " +
        //                                " WHERE summary.corretionCompletedOn BETWEEN @fromDate AND @toDate " +
        //                                " AND summary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED + " AND isnull(summary.isCorrectionCompleted,0) =1 " +
        //                                " summary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS);


        //        if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.UserId))
        //        {
        //            cmdSelect.CommandText += " tblPurchaseEnquiry.userId IN ( " + tblPurSchSummaryFilterTO.UserId + " ) ";
        //        }

        //        if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.COrNcId))
        //        {
        //            cmdSelect.CommandText += " summary.cOrNCId IN ( " + tblPurSchSummaryFilterTO.COrNcId + " ) ";
        //        }


        //        if (tblPurSchSummaryFilterTO.ProdClassId > 0)
        //        {
        //            cmdSelect.CommandText += " tblPurchaseEnquiry.prodClassId =  " + tblPurSchSummaryFilterTO.ProdClassId + "  ";
        //        }

        //        if (tblPurSchSummaryFilterTO.SupplierId > 0)
        //        {
        //            cmdSelect.CommandText += " summary.supplierId =  " + tblPurSchSummaryFilterTO.SupplierId + "  ";
        //        }

        //        string groupByStr = " GROUP BY summary.idPurchaseScheduleSummary ,summary.supplierId,summary.vehicleNo,tblOrganization.firmName,scheduleDetails.prodItemId,scheduleDetails.qty,scheduleDetails.rate,tblProductItem.itemName ";
        //        string orderByStr = " ORDER BY summary.idPurchaseScheduleSummary ";

        //        cmdSelect.CommandText += groupByStr + orderByStr;

        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<PartyWiseUnldReportTO> list = ConvertDTToListForPartyWiseUnldReport(reader);
        //        reader.Close();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        reader.Dispose();
        //        cmdSelect.Dispose();
        //    }
        //}

        // public List<PartyWiseUnldReportTO> ConvertDTToListForPartyWiseUnldReport(SqlDataReader partyWiseUnldReportDT)
        // {
        //     List<PartyWiseUnldReportTO> partyWiseUnldReportTOList = new List<PartyWiseUnldReportTO>();
        //     if (partyWiseUnldReportDT != null)
        //     {
        //         while (partyWiseUnldReportDT.Read())
        //         {
        //             PartyWiseUnldReportTO partyWiseUnldReportTO = new PartyWiseUnldReportTO();

        //             // if (partyWiseUnldReportDT["idPurchaseScheduleSummary"] != DBNull.Value)
        //             //     partyWiseUnldReportTO.IdPurchaseScheduleSummary = Convert.ToInt32(partyWiseUnldReportDT["idPurchaseScheduleSummary"].ToString());
        //             if (partyWiseUnldReportDT["vehicleNo"] != DBNull.Value)
        //                 partyWiseUnldReportTO.VehicleNo = Convert.ToString(partyWiseUnldReportDT["vehicleNo"].ToString());
        //             if (partyWiseUnldReportDT["supplierName"] != DBNull.Value)
        //                 partyWiseUnldReportTO.SupplierName = Convert.ToString(partyWiseUnldReportDT["supplierName"].ToString());
        //             if (partyWiseUnldReportDT["qty"] != DBNull.Value)
        //                 partyWiseUnldReportTO.Qty = Convert.ToDouble(partyWiseUnldReportDT["qty"].ToString());
        //             if (partyWiseUnldReportDT["rate"] != DBNull.Value)
        //                 partyWiseUnldReportTO.Rate = Convert.ToDouble(partyWiseUnldReportDT["rate"].ToString());
        //             if (partyWiseUnldReportDT["itemName"] != DBNull.Value)
        //                 partyWiseUnldReportTO.ItemName = Convert.ToString(partyWiseUnldReportDT["itemName"].ToString());

        //             partyWiseUnldReportTOList.Add(partyWiseUnldReportTO);
        //         }
        //     }
        //     return partyWiseUnldReportTOList;
        // }

        public List<TblPurchaseWeighingStageSummaryTO> GetPartyWeightComparisonReport(DateTime fromDate, DateTime toDate,Int32 statusId,String purchaseManagerIds)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
            cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select tblPurchaseScheduleSummary.idpurchaseschedulesummary as correctionRecId,tblPartyWeighingMeasures.netWt,tblPartyWeighingMeasures.grossWt,tblPartyWeighingMeasures.tareWt,"
                + " tblOrganization.firmName,"
                + " tblPurchaseVehFreightDtls.freight,tblPurchaseVehFreightDtls.advance,tblPurchaseVehFreightDtls.unloadingQtyAmt as unloadingQty,tblPurchaseVehFreightDtls.shortage,tblPurchaseVehFreightDtls.amount, "
                + " case when tblPurchaseVehFreightDtls.isValidInfo = 1 then 'Yes' when tblPurchaseVehFreightDtls.isValidInfo = 0 then 'No'end as isValidInfo, "
                + " case when tblPurchaseVehFreightDtls.isStorageExcess = 1 then 'Yes'when tblPurchaseVehFreightDtls.isStorageExcess = 0 then 'No' end as isStorageExcess,"
                + " * from tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary "
                + " inner join tblPurchaseScheduleSummary tblPurchaseScheduleSummary  on "
                + " ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary .idPurchaseScheduleSummary) = tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId ";
                
                if(statusId > 0)
                {
                    cmdSelect.CommandText += " and tblPurchaseScheduleSummary.statusId = " + statusId;
                }

                cmdSelect.CommandText += " and tblPurchaseScheduleSummary.isCorrectionCompleted = 1 "
                + " and tblPurchaseScheduleSummary.vehiclePhaseId = " + (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS
                + " left join tblPartyWeighingMeasures tblPartyWeighingMeasures on tblPartyWeighingMeasures.purchaseScheduleSummaryId = tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId "
                + " left join tblOrganization tblOrganization on tblOrganization.idOrganization = tblPurchaseWeighingStageSummary.supplierId "
                + " left join tblPurchaseVehFreightDtls tblPurchaseVehFreightDtls on tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId "
                + " where  tblPurchaseScheduleSummary.corretionCompletedOn between @fromDate and @toDate ";
                if (!String.IsNullOrEmpty(purchaseManagerIds))
                {
                    cmdSelect.CommandText += " AND tblPurchaseScheduleSummary.purchaseEnquiryId IN (SELECT tblPurchaseEnquiry.idPurchaseEnquiry " +
                                             "                                                      FROM tblPurchaseEnquiry tblPurchaseEnquiry" +
                                             "                                                      WHERE tblPurchaseEnquiry.userId IN ( " + purchaseManagerIds + "))";
                }
                //+ " order by tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId";
                cmdSelect.CommandText += " order by tblPurchaseScheduleSummary.corretionCompletedOn desc";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToListForWeighingSummary(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<CorrectionUnloadingReportTO> GetCorrectionUnloadingReports(DateTime fromDate, DateTime toDate,int isCorrectionCompleted,int vehiclePhaseId,int statusId,String purchaseManagerIds)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
           
            try
            {
                conn.Open();
                cmdSelect.CommandText = " select corretionCompletedOn ,tblPurchaseScheduleDetails.qty,rate,productAomunt,supervisorId,userDisplayName " +
                                        " ,tblProductItem.itemName from tblPurchaseScheduleDetails " +
                                        " Left Join  tblPurchaseScheduleSummary tblPurchaseScheduleSummary on " +
                                        " tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId " +
                                        " LEFT JOIN tblProductItem tblProductItem " +
                                        " on tblProductItem.idProdItem = tblPurchaseScheduleDetails.prodItemId " +
                                        " Left Join tblUser tblUser on tblUser.idUser = tblPurchaseScheduleSummary.supervisorId " +
                                        " where isCorrectionCompleted = @isCorrectionCompleted and vehiclePhaseId = @vehiclePhaseId  and statusId = @statusId AND corretionCompletedOn between @fromDate and @toDate";
                if (!String.IsNullOrEmpty(purchaseManagerIds))
                {
                    cmdSelect.CommandText += " AND tblPurchaseScheduleSummary.purchaseEnquiryId IN (SELECT tblPurchaseEnquiry.idPurchaseEnquiry" +
                        "                                                                           FROM tblPurchaseEnquiry tblPurchaseEnquiry " +
                        "                                                                           WHERE tblPurchaseEnquiry.userId IN (" + purchaseManagerIds +"))";
                        
                }
                cmdSelect.CommandText += " order by tblPurchaseScheduleSummary.corretionCompletedOn desc ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value =fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value =toDate;
                cmdSelect.Parameters.Add("@isCorrectionCompleted", System.Data.SqlDbType.Int).Value = isCorrectionCompleted;
                cmdSelect.Parameters.Add("@vehiclePhaseId", System.Data.SqlDbType.Int).Value =vehiclePhaseId;
                cmdSelect.Parameters.Add("@statusId", System.Data.SqlDbType.Int).Value =statusId;


                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<CorrectionUnloadingReportTO> list = ConvertDTGetCorrectionUnloadingReportsTO(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<CorrectionUnloadingReportTO> ConvertDTGetCorrectionUnloadingReportsTO(SqlDataReader reader)
        {
            List<CorrectionUnloadingReportTO> correctionUnloadingReportTOList = new List<CorrectionUnloadingReportTO> ();
            if (reader != null)
            {
                while (reader.Read())
                {
                    CorrectionUnloadingReportTO CorrectionUnloadingReportsTONew = new CorrectionUnloadingReportTO();

                    if (reader["corretionCompletedOn"] != DBNull.Value)
                        CorrectionUnloadingReportsTONew.CorretionCompletedOn = Convert.ToDateTime(reader["corretionCompletedOn"].ToString());
                    if (reader["productAomunt"] != DBNull.Value)
                        CorrectionUnloadingReportsTONew.CorrectionAmt = Convert.ToDouble(reader["productAomunt"].ToString());
                    if (reader["qty"] != DBNull.Value)
                        CorrectionUnloadingReportsTONew.CorrectionQty = Convert.ToDouble(reader["qty"].ToString());
                    if (reader["rate"] != DBNull.Value)
                        CorrectionUnloadingReportsTONew.CorrectionRate = Convert.ToDouble(reader["rate"].ToString());
                    if (reader["supervisorId"] != DBNull.Value)
                        CorrectionUnloadingReportsTONew.SupervisorId = Convert.ToInt32(reader["supervisorId"].ToString());
                    if (reader["userDisplayName"] != DBNull.Value)
                        CorrectionUnloadingReportsTONew.SupervisorName = Convert.ToString(reader["userDisplayName"].ToString());
                    if (reader["itemName"] != DBNull.Value)
                        CorrectionUnloadingReportsTONew.ItemName = Convert.ToString(reader["itemName"].ToString());
                    correctionUnloadingReportTOList.Add(CorrectionUnloadingReportsTONew);
                }
            }
            return correctionUnloadingReportTOList;
        }
        public List<TblPurchaseWeighingStageSummaryTO> ConvertDTToListForWeighingSummary(SqlDataReader tblPurchaseWeighingStageSummaryTODT)
        {
            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = new List<TblPurchaseWeighingStageSummaryTO>();
            if (tblPurchaseWeighingStageSummaryTODT != null)
            {
                while (tblPurchaseWeighingStageSummaryTODT.Read())
                {
                    TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTONew = new TblPurchaseWeighingStageSummaryTO();
                    if (tblPurchaseWeighingStageSummaryTODT["idPurchaseWeighingStage"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IdPurchaseWeighingStage = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["idPurchaseWeighingStage"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["weighingMachineId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeighingMachineId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weighingMachineId"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["createdBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["updatedBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["grossWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.GrossWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["grossWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["actualWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.ActualWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["actualWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["netWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.NetWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["netWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["rstNumber"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RstNumber = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["rstNumber"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["purchaseScheduleSummaryId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["weightMeasurTypeId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeightMeasurTypeId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weightMeasurTypeId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["machineCalibrationId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.MachineCalibrationId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["machineCalibrationId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["correctionRecId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.CorrectionRecId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["correctionRecId"].ToString());
                    
                    if (tblPurchaseWeighingStageSummaryTODT["weightStageId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeightStageId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weightStageId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["recoveryPer"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryPer = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["recoveryPer"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["recoveryBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["recoveryOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["isRecConfirm"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IsRecConfirm = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["isRecConfirm"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["isValid"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IsValid = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["isValid"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["tareWt"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.PartyTareWt = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["tareWt"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["netWt"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.PartyNetWt = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["netWt"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["grossWt"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.PartyGrossWt = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["grossWt"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["firmName"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.SupplierName = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["firmName"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.Freight = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["freight"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["advance"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.Advance = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["advance"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["unloadingQty"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.UnloadingQty = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["unloadingQty"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["isValidInfo"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IsValidInfo = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["isValidInfo"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["isStorageExcess"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IsStorageExcess = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["isStorageExcess"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["amount"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.Amount = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["amount"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["shortage"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.Shortage = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["shortage"].ToString());

                    tblPurchaseWeighingStageSummaryTOList.Add(tblPurchaseWeighingStageSummaryTONew);
                }
            }
            return tblPurchaseWeighingStageSummaryTOList;
        }

        //Added by minal 19 May 2021
        public List<TallyTransportEnquiryTO> SelectTallyTransportEnquiryDetails(string vehicleIds, int cOrNcId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string sql1 = " WITH cte_TallyTransportEnquiry AS   " +
                                        " ( " +
                                        "    SELECT tblPurchaseScheduleSummary.corretionCompletedOn AS date," +
                                        "   'Cash Payment'  AS voucherType,tblOrganization.firmName AS partyName,'Cash' AS cash," +
                                        "    CAST(ROUND((ISNULL(tblPurchaseVehFreightDtls.amount,0) /1000),3) AS NUMERIC(36,3)) AS tranportAmountRS," +
                                        "    CAST(ROUND((ISNULL(tblPurchaseVehFreightDtls.amount,0) /1000),3) AS NUMERIC(36,3)) AS manojSrpPettyCsh, " +
                                        "    tblPurchaseScheduleSummary.vehicleNo AS narration" +
                                        "    FROM   tblPurchaseScheduleSummary tblPurchaseScheduleSummary" +
                                        "    LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId" +
                                        "    LEFT JOIN tblPurchaseVehFreightDtls tblPurchaseVehFreightDtls ON tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                        "    WHERE and ISNULL(tblPurchaseScheduleSummary.iscorrectionCompleted,0) = 1  tblPurchaseScheduleSummary.vehiclePhaseId =" + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS);
                //+ " AND tblPurchaseScheduleSummary.isActive = 1";

                if (cOrNcId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                {
                    sql1 += " AND tblPurchaseScheduleSummary.cOrNCId=" + cOrNcId;
                    sql1 += " AND (tblPurchaseScheduleSummary.isBoth = 0 or tblPurchaseScheduleSummary.isBoth IS NULL)  ";
                }
                if (!String.IsNullOrEmpty(vehicleIds))
                {
                    sql1 += " AND tblPurchaseScheduleSummary.rootScheduleId IN (" + vehicleIds + ")";
                }

                string sql2 = " ) " +
                                        "" +
                                        " SELECT FORMAT(date,'dd/MM/yyyy') AS Date,voucherType,partyName,cash,CAST(tranportAmountRS AS NVARCHAR) AS tranportAmountRS,CAST(manojSrpPettyCsh AS NVARCHAR) AS manojSrpPettyCsh, narration" +
                                        " FROM cte_TallyTransportEnquiry";

                cmdSelect.CommandText = sql1 + sql2;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TallyTransportEnquiryTO> list = ConvertDTTallyTransportEnquiry(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TallyTransportEnquiryTO> SelectTallyTransportEnquiryDetailsForCopy(DateTime fromDate, DateTime toDate,Int32 cOrNcId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            String sql1 = String.Empty;            
            try
            {
                conn.Open();
                sql1 = " WITH cte_TallyTransportEnquiry AS   " +
                                        " ( " +
                                        "    SELECT tblPurchaseScheduleSummary.corretionCompletedOn AS date," +
                                        "   'Journal' AS voucherType,tblOrganization.firmName AS partyName,'Manoj Srp Petty Csh' AS cash," +
                                        "    CAST(ROUND((ISNULL(tblPurchaseVehFreightDtls.amount,0) /1000),3) AS NUMERIC(36,3)) AS tranportAmountRS," +
                                        "    CAST(ROUND((ISNULL(tblPurchaseVehFreightDtls.amount,0) /1000),3) AS NUMERIC(36,3)) AS manojSrpPettyCsh, " +
                                        "    tblPurchaseScheduleSummary.vehicleNo AS narration" +
                                        "    FROM   tblPurchaseScheduleSummary tblPurchaseScheduleSummary" +
                                        "    LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId" +
                                        "    LEFT JOIN tblPurchaseVehFreightDtls tblPurchaseVehFreightDtls ON tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                        "    WHERE tblPurchaseScheduleSummary.vehiclePhaseId =" + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS) +
                                        //"    AND tblPurchaseScheduleSummary.isActive = 1" +
                                        "    AND tblPurchaseScheduleSummary.statusId = " + Convert.ToInt32(Constants.TranStatusE.UNLOADING_COMPLETED) +
                                        "    AND ISNULL(tblPurchaseScheduleSummary.isCorrectionCompleted,0) =1 " +
                                        //"    AND CAST(tblPurchaseScheduleSummary.corretionCompletedOn AS DATE) BETWEEN @fromDate AND @toDate ";
                                        "    AND tblPurchaseScheduleSummary.corretionCompletedOn BETWEEN @fromDate AND @toDate ";

                if (cOrNcId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                {
                    sql1 += " AND tblPurchaseScheduleSummary.cOrNCId=" + cOrNcId;
                    sql1 += " AND (tblPurchaseScheduleSummary.isBoth = 0 or tblPurchaseScheduleSummary.isBoth IS NULL)  ";
                }
                  
                    sql1 +=   " )" +
                                        " SELECT FORMAT(date,'dd/MM/yyyy') AS date,voucherType,partyName,cash,CAST(tranportAmountRS AS NVARCHAR) AS tranportAmountRS,CAST(manojSrpPettyCsh AS NVARCHAR) AS manojSrpPettyCsh, narration" +
                                        " FROM cte_TallyTransportEnquiry" +
                                        " WHERE ISNULL(tranportAmountRS,0) <> 0 ";

                cmdSelect.CommandText = sql1;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TallyTransportEnquiryTO> list = ConvertDTTallyTransportEnquiry(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TallyTransportEnquiryTO> ConvertDTTallyTransportEnquiry(SqlDataReader reader)
        {
            List<TallyTransportEnquiryTO> tallyTransportEnquiryTOList = new List<TallyTransportEnquiryTO>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    TallyTransportEnquiryTO TallyTransportEnquiryTONew = new TallyTransportEnquiryTO();

                    if (reader["date"] != DBNull.Value)
                        TallyTransportEnquiryTONew.Date = Convert.ToString(reader["date"].ToString());
                    if (reader["voucherType"] != DBNull.Value)
                        TallyTransportEnquiryTONew.VoucherType = Convert.ToString(reader["voucherType"].ToString());
                    if (reader["partyName"] != DBNull.Value)
                        TallyTransportEnquiryTONew.PartyName = Convert.ToString(reader["partyName"].ToString());
                    if (reader["cash"] != DBNull.Value)
                        TallyTransportEnquiryTONew.Cash = Convert.ToString(reader["cash"].ToString());
                    if (reader["tranportAmountRS"] != DBNull.Value)                        
                    TallyTransportEnquiryTONew.TranportAmountRS = Convert.ToDouble(reader["tranportAmountRS"].ToString());
                    if (reader["manojSrpPettyCsh"] != DBNull.Value)
                        TallyTransportEnquiryTONew.ManojSrpPettyCsh = Convert.ToDouble(reader["manojSrpPettyCsh"].ToString());
                    if (reader["narration"] != DBNull.Value)
                        TallyTransportEnquiryTONew.Narration = Convert.ToString(reader["narration"].ToString());
                    tallyTransportEnquiryTOList.Add(TallyTransportEnquiryTONew);
                }
            }
            return tallyTransportEnquiryTOList;
        }

        public List<CCTransportEnquiryTO> SelectCCTransportEnquiryDetails(string vehicleIds, int cOrNcId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string sql1 = " WITH cte_ccTransportEnquiry AS   " +
                                        "  ( " +
                                        "    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NEWID())) AS srNo," +
                                        "           FORMAT(tblPurchaseScheduleSummary.corretionCompletedOn,'dd/MM/yyyy') AS date," +
                                        "           tblOrganization.firmName AS partyName,tblPurchaseScheduleSummary.vehicleNo AS vehicleNumber," +
                                        "           CAST(ROUND((ISNULL(tblPurchaseVehFreightDtls.amount,0) /1000),3) AS NUMERIC(36,3)) AS transportPayment " +
                                        "    FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary" +
                                        "    LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId  " +
                                        "    LEFT JOIN tblPurchaseVehFreightDtls tblPurchaseVehFreightDtls ON tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                        "	 WHERE" +
                                        //" CAST((tblPurchaseScheduleSummary.corretionCompletedOn) AS DATE) <= @toDate AND " +
                                        "    tblPurchaseScheduleSummary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS) +
                                        " AND tblPurchaseScheduleSummary.isActive = 1";
                if (cOrNcId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                {
                    sql1 += " AND tblPurchaseScheduleSummary.cOrNCId=" + cOrNcId;
                    sql1 += " AND (tblPurchaseScheduleSummary.isBoth = 0 or tblPurchaseScheduleSummary.isBoth IS NULL)  ";
                }

                if (!String.IsNullOrEmpty(vehicleIds))
                {
                    sql1 += " AND tblPurchaseScheduleSummary.rootScheduleId IN (" + vehicleIds + ")";
                }
                string sql2 = "  ) " +
                                        " " +
                                        " SELECT srNo AS srNo,date AS date,partyName,vehicleNumber,CAST(transportPayment AS NVARCHAR) AS transportPayment " +
                                        " FROM cte_ccTransportEnquiry";

                cmdSelect.CommandText = sql1 + sql2;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<CCTransportEnquiryTO> list = ConvertDTCCTransportEnquiry(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<CCTransportEnquiryTO> SelectCCTransportEnquiryDetailsForCopy(DateTime fromDate, DateTime toDate,Int32 cOrNcId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            String sql1 = String.Empty;
            try
            {
                conn.Open();
                sql1 = " WITH cte_ccTransportEnquiry AS   " +
                       "  ( " +
                       "    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NEWID())) AS srNo," +
                       "           FORMAT(tblPurchaseScheduleSummary.corretionCompletedOn,'dd/MM/yyyy') AS date," +
                       "           tblOrganization.firmName AS partyName,tblPurchaseScheduleSummary.vehicleNo AS vehicleNumber," +
                       "           CAST(ROUND((ISNULL(tblPurchaseVehFreightDtls.amount,0) /1000),3) AS NUMERIC(36,3)) AS transportPayment " +
                       "    FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary" +
                       "    LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId  " +
                       "    LEFT JOIN tblPurchaseVehFreightDtls tblPurchaseVehFreightDtls ON tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                       "	 WHERE" +
                       //"  CAST((tblPurchaseScheduleSummary.corretionCompletedOn) AS DATE) BETWEEN @fromDate AND @toDate AND " +
                       "  tblPurchaseScheduleSummary.corretionCompletedOn BETWEEN @fromDate AND @toDate AND " +
                       "    tblPurchaseScheduleSummary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS) +
                       //" AND tblPurchaseScheduleSummary.isActive = 1 " +
                       "    AND tblPurchaseScheduleSummary.statusId = " + Convert.ToInt32(Constants.TranStatusE.UNLOADING_COMPLETED) +
                        "    AND ISNULL(tblPurchaseScheduleSummary.isCorrectionCompleted,0) =1 " +
                       " AND ISNULL(tblPurchaseVehFreightDtls.amount,0) <> 0";

                if (cOrNcId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                {
                    sql1 += " AND tblPurchaseScheduleSummary.cOrNCId=" + cOrNcId;
                    sql1 += " AND (tblPurchaseScheduleSummary.isBoth = 0 or tblPurchaseScheduleSummary.isBoth IS NULL)  ";
                }
                   
                    sql1 += "  ) " +
                                        " " +
                                        " SELECT srNo AS srNo,date AS date,partyName,vehicleNumber,CAST(transportPayment AS NVARCHAR) AS transportPayment " +
                                        " FROM cte_ccTransportEnquiry";

                cmdSelect.CommandText = sql1;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<CCTransportEnquiryTO> list = ConvertDTCCTransportEnquiry(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<CCTransportEnquiryTO> ConvertDTCCTransportEnquiry(SqlDataReader reader)
        {
            List<CCTransportEnquiryTO> cCTransportEnquiryTOList = new List<CCTransportEnquiryTO>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    CCTransportEnquiryTO cCTransportEnquiryTONew = new CCTransportEnquiryTO();

                    if (reader["srNo"] != DBNull.Value)
                        cCTransportEnquiryTONew.SrNo = Convert.ToString(reader["srNo"].ToString());
                    if (reader["date"] != DBNull.Value)
                        cCTransportEnquiryTONew.Date = Convert.ToString(reader["date"].ToString());
                    if (reader["partyName"] != DBNull.Value)
                        cCTransportEnquiryTONew.PartyName = Convert.ToString(reader["partyName"].ToString());
                    if (reader["vehicleNumber"] != DBNull.Value)
                        cCTransportEnquiryTONew.VehicleNumber = Convert.ToString(reader["vehicleNumber"].ToString());
                    if (reader["transportPayment"] != DBNull.Value)
                        cCTransportEnquiryTONew.TransportPayment = Convert.ToDouble(reader["transportPayment"].ToString());

                    cCTransportEnquiryTOList.Add(cCTransportEnquiryTONew);
                }
            }
            return cCTransportEnquiryTOList;
        }

        public List<TallyReportTO> SelectTallyPREnquiryDetails(string vehicleIds, int cOrNcId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT summary.idPurchaseScheduleSummary,  " +
                    //"summary.createdOn AS date,summary.vehicleNo AS truckNo," +
                    " summary.corretionCompletedOn AS date,summary.vehicleNo AS truckNo," +
                    " userdetails.userDisplayName AS pm," +
                    " proditem.itemName AS grade,details.qty AS gradeQty," +
                    " case when  proditem.isNonCommercialItem =1 then details.qty end as dustQty ," +
                    " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  CAST(((details.rate)/1000) AS DECIMAL(10,2)) end as gradeRate," +
                    " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  cast(((details.qty * details.rate)/1000) as decimal(10, 3)) end as total" +
                    " , CASE WHEN summary.cOrNCId = 1 THEN  ISNULL(invoiceAddr.billingPartyName,org.firmName)  WHEN summary.cOrNCId = 0 THEN org.firmName   END AS supplierName," +
                    " CASE WHEN summary.cOrNCId = 1 THEN 'ORDER' WHEN summary.cOrNCId = 0 THEN 'ENQUIRY' END AS billType,classifictaion.prodClassDesc AS materialType,summary.containerNo AS containerNo,tblPurchaseVehicleSpotEntry.location AS location,purchaseWeighing.godown," +
                    //" CASE WHEN ISNULL(summary.processChargePerVeh,0) >= 0 THEN CAST(ROUND((ISNULL(summary.processChargePerVeh,0) /1000),2) AS NUMERIC(36,2))  " +
                    //" ELSE CAST(ROUND(ISNULL(summary.processChargePerVeh,0),2) AS NUMERIC(36,2)) END AS processChargePerVeh, " +
                    " (ISNULL(summary.processChargePerVeh,0) /1000) AS processChargePerVeh,summary.cOrNCId,summary.isBoth,summary.rootScheduleId " +
                    " FROM dbo.tblPurchaseScheduleSummary summary " +
                    " LEFT JOIN tblPurchaseEnquiry PurchaseEnquiry ON summary.purchaseEnquiryId = PurchaseEnquiry.idPurchaseEnquiry " +
                    " LEFT JOIN tblProdClassification classifictaion ON PurchaseEnquiry.prodClassId = classifictaion.idProdClass " +
                    " LEFT JOIN dbo.tblUser userdetails ON PurchaseEnquiry.userId = userdetails.idUser " +
                    " LEFT JOIN tblPurchaseScheduleDetails details ON summary.idPurchaseScheduleSummary = details.purchaseScheduleSummaryId " +
                    " LEFT JOIN tblOrganization org ON summary.supplierId = org.idOrganization " +
                    " left join tblPurchaseInvoice invoice on invoice.purSchSummaryId = summary.rootScheduleId " +
                    " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    " LEFT JOIN tblProductItem proditem ON details.prodItemId = proditem.idProdItem " +
                    " LEFT JOIN tblPurchaseVehicleSpotEntry tblPurchaseVehicleSpotEntry on tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId = isnull(summary.rootScheduleId,summary.idPurchaseScheduleSummary)" +
                    " LEFT JOIN " +
                    " ( " +
                    "  SELECT purchaseWeighingStageSummary.purchaseScheduleSummaryId,tblWeighingMachine.machineName AS godown" +
                    "  FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary" +
                    "  LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = purchaseWeighingStageSummary.weighingMachineId" +
                    "  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                    "  ) AS purchaseWeighing" +
                    " ON purchaseWeighing.purchaseScheduleSummaryId = ISNULL(summary.rootScheduleId,summary.idPurchaseScheduleSummary)" +
                    " WHERE summary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS)
                    //Prajakta[2019-03-06] Commented and added
                    //+ " AND summary.createdOn BETWEEN @fromDate AND @toDate " 
                    //+ " AND  CAST(summary.corretionCompletedOn AS DATE) <= @toDate "
                    + " AND isnull(summary.isCorrectionCompleted,0) =1 AND summary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
                if (cOrNcId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                    cmdSelect.CommandText += " AND summary.cOrNCId=" + cOrNcId;

                if (!String.IsNullOrEmpty(vehicleIds))
                {
                    cmdSelect.CommandText += " AND summary.rootScheduleId IN (" + vehicleIds + ")";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TallyReportTO> list = ConvertDTToList(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TallyReportTO> SelectTallyPREnquiryDetailsForCopy(DateTime fromDate, DateTime toDate,String materialIds,Int32 cOrNcId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT summary.idPurchaseScheduleSummary,  " +
                    //"summary.createdOn AS date,summary.vehicleNo AS truckNo," +
                    " summary.corretionCompletedOn AS date,summary.vehicleNo AS truckNo," +
                    " userdetails.userDisplayName AS pm," +
                    " proditem.itemName AS grade,details.qty AS gradeQty," +
                    " case when  proditem.isNonCommercialItem =1 then details.qty end as dustQty ," +
                    " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  (details.rate) end as gradeRate," +
                    " case when  proditem.isNonCommercialItem =1 then 0 when isnull(proditem.isNonCommercialItem,0) =0 then  (details.qty * details.rate) end as total" +
                    " , CASE WHEN summary.cOrNCId = 1 THEN  ISNULL(invoiceAddr.billingPartyName,org.firmName)  WHEN summary.cOrNCId = 0 THEN org.firmName   END AS supplierName," +
                    " CASE WHEN summary.cOrNCId = 1 THEN 'ORDER' WHEN summary.cOrNCId = 0 THEN 'ENQUIRY' END AS billType," +
                    //" classifictaion.prodClassDesc AS materialType," +
                    " CASE WHEN classifictaion.prodClassDesc = 'Local Scrap' THEN 'SRP' ELSE classifictaion.prodClassDesc END AS materialType," +
                    " summary.containerNo AS containerNo,tblPurchaseVehicleSpotEntry.location AS location,LEFT(purchaseWeighing.godown,3) AS godown," +
                    //" CASE WHEN ISNULL(summary.processChargePerVeh,0) >= 0 THEN CAST(ROUND((ISNULL(summary.processChargePerVeh,0) /1000),2) AS NUMERIC(36,2))  " +
                    //" ELSE CAST(ROUND(ISNULL(summary.processChargePerVeh,0),2) AS NUMERIC(36,2)) END AS processChargePerVeh, " +
                    " (ISNULL(summary.processChargePerVeh,0)) AS processChargePerVeh,summary.cOrNCId,summary.isBoth,summary.rootScheduleId,PurchaseEnquiry.remark,summary.correNarration" +
                    " FROM dbo.tblPurchaseScheduleSummary summary " +
                    " LEFT JOIN tblPurchaseEnquiry PurchaseEnquiry ON summary.purchaseEnquiryId = PurchaseEnquiry.idPurchaseEnquiry " +
                    " LEFT JOIN tblProdClassification classifictaion ON PurchaseEnquiry.prodClassId = classifictaion.idProdClass " +
                    " LEFT JOIN dbo.tblUser userdetails ON PurchaseEnquiry.userId = userdetails.idUser " +
                    " LEFT JOIN tblPurchaseScheduleDetails details ON summary.idPurchaseScheduleSummary = details.purchaseScheduleSummaryId " +
                    " LEFT JOIN tblOrganization org ON summary.supplierId = org.idOrganization " +
                    " left join tblPurchaseInvoice invoice on invoice.purSchSummaryId = summary.rootScheduleId " +
                    " LEFT JOIN tblPurchaseInvoiceAddr invoiceAddr ON invoiceAddr.purchaseInvoiceId = invoice.idInvoicePurchase " +
                    " LEFT JOIN tblProductItem proditem ON details.prodItemId = proditem.idProdItem " +
                    " LEFT JOIN tblPurchaseVehicleSpotEntry tblPurchaseVehicleSpotEntry on tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId = isnull(summary.rootScheduleId,summary.idPurchaseScheduleSummary)" +
                    " LEFT JOIN " +
                    " ( " +
                    "  SELECT purchaseWeighingStageSummary.purchaseScheduleSummaryId,tblWeighingMachine.machineName AS godown" +
                    "  FROM tblPurchaseWeighingStageSummary purchaseWeighingStageSummary" +
                    "  LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = purchaseWeighingStageSummary.weighingMachineId" +
                    "  WHERE purchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                    "  ) AS purchaseWeighing" +
                    " ON purchaseWeighing.purchaseScheduleSummaryId = ISNULL(summary.rootScheduleId,summary.idPurchaseScheduleSummary)" +
                    " WHERE summary.vehiclePhaseId = " + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS)
                    //Prajakta[2019-03-06] Commented and added
                    //+ " AND summary.createdOn BETWEEN @fromDate AND @toDate " 
                    //+ " AND  CAST(summary.corretionCompletedOn AS DATE) BETWEEN @fromDate AND @toDate "
                    + " AND  summary.corretionCompletedOn BETWEEN @fromDate AND @toDate "
                    + " AND isnull(summary.isCorrectionCompleted,0) =1 AND summary.statusId =" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
                if (!String.IsNullOrEmpty(materialIds))
                {
                    cmdSelect.CommandText += " AND classifictaion.idProdClass IN (" + materialIds + ")";
                }
                if (cOrNcId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                    cmdSelect.CommandText += " AND summary.cOrNCId=" + cOrNcId;

                cmdSelect.CommandText += "  ORDER BY summary.rootScheduleId";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TallyReportTO> list = ConvertDTToListForTallyPREnquiryDropbox(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblWBRptTO> SelectWBForPurchaseReportListForCopy(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;            
            try
            {
                conn.Open();

                cmdSelect.CommandTimeout = 300;
                cmdSelect.CommandText = "GetWBReportForPurchase";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.StoredProcedure;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblWBRptTO> SelectWBForLoadReportListForCopy(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                cmdSelect.CommandTimeout = 300;
                cmdSelect.CommandText = "GetWBReportForLoad";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.StoredProcedure;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblWBRptTO> SelectWBForUnloadReportListForCopy(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                cmdSelect.CommandTimeout = 300;
                cmdSelect.CommandText = "GetWBReportForunLoad";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.StoredProcedure;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblWBRptTO> SelectWBForPurchaseReportList(string vehicleIds, int cOrNcId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string sql1 = String.Empty;
            string sql2 = String.Empty;
            //DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                sql1 = " DECLARE @Temp TABLE(idR INT IDENTITY NOT NULL,wBID NVARCHAR(100),userID NVARCHAR(100),orignalRSTNo NVARCHAR(200),additionalRSTNo NVARCHAR(200),date NVARCHAR(20),time NVARCHAR(20),materialType NVARCHAR(500)," +
                    " materialSubType NVARCHAR(1000),grossWeight DECIMAL(18,2),firstWeight DECIMAL(18,2),secondWeight DECIMAL(18,2),thirdWeight DECIMAL(18,2),forthWeight DECIMAL(18,2),fifthWeight DECIMAL(18,2),sixthWeight DECIMAL(18,2)," +
                    " seventhWeight DECIMAL(18,2),tareWeight DECIMAL(18,2),netWeight DECIMAL(18,2),loadOrUnload NVARCHAR(50),fromLocation NVARCHAR(100),toLocation NVARCHAR(100),transactionType NVARCHAR(100),vehicleNumber NVARCHAR(100),vehicleStatus NVARCHAR(100),billType NVARCHAR(100),vehicleID NVARCHAR(100)," +
                    " statusId INT,isActive INT,rootScheduleId INT,idPurchaseScheduleSummary INT)" +
                    " DECLARE @Temp1 TABLE (  idR1 INT IDENTITY NOT NULL,rootScheduleId INT)" +
                    " INSERT INTO @Temp " +
                    " SELECT purchaseWeighingStageSummary.machineName AS wBID,purchaseWeighingStageSummary.userDisplayName AS userID,'-' AS orignalRSTNo, " +
                    " '-' AS additionalRSTNo,FORMAT(tareWt.createdOn,'dd/MM/yyyy') AS date,CONVERT(CHAR(5),tareWt.createdOn, 108) AS time," +
                    " CASE WHEN prodClassification.prodClassDesc = 'Local Scrap' THEN 'SRP' ELSE prodClassification.prodClassDesc END AS materialType," +
                    " materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName FROM tblProductItem productItem " +
                    " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem " +
                    " WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = purchaseScheduleSummary.idPurchaseScheduleSummary" +
                    " FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')),grossWt.grossWeightMT AS grossWeight," +
                    " wtStage1.actualWeightMT AS firstWeight,wtStage2.actualWeightMT AS secondWeight,wtStage3.actualWeightMT AS thirdWeight," +
                    " wtStage4.actualWeightMT AS forthWeight,wtStage5.actualWeightMT AS fifthWeight,wtStage6.actualWeightMT AS sixthWeight," +
                    " wtStage7.actualWeightMT AS seventhWeight,tareWt.actualWeightMT AS tareWeight," +
                    " CASE WHEN ((ISNULL(grossWt.grossWeightMT,0)) - (ISNULL(tareWt.actualWeightMT,0))) < 0 THEN 0" +
                    " ELSE ((ISNULL(grossWt.grossWeightMT,0)) - (ISNULL(tareWt.actualWeightMT,0))) END AS netWeight ," +
                    " 'Unload' AS loadOrUnload, " +
                    " CASE WHEN purchaseVehicleSpotEntry.location IS NOT NULL THEN purchaseVehicleSpotEntry.location ELSE purchaseScheduleSummary.location END AS fromLocation, " +
                    " 'Jalna' AS toLocation,'Purchase' AS transactionType, " +
                    " purchaseScheduleSummary.vehicleNo AS vehicleNumber,dimStatus.statusDesc AS vehicleStatus," +
                    " CASE WHEN purchaseScheduleSummary.cOrNCId = 1 THEN 'Order' WHEN purchaseScheduleSummary.cOrNCId = 0 THEN 'Enquiry' ELSE '' END AS billType," +
                    " purchaseScheduleSummary.rootScheduleId AS vehicleID,purchaseScheduleSummary.statusId,purchaseScheduleSummary.isActive,purchaseScheduleSummary.rootScheduleId,purchaseScheduleSummary.idPurchaseScheduleSummary  " +
                    " FROM tblPurchaseScheduleSummary purchaseScheduleSummary" +
                    " LEFT JOIN " +
                    "           (" +
                    "               SELECT tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId,weighingMachine.machineName,tblUser.userDisplayName " +
                    "               FROM tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
                    "               LEFT JOIN tblWeighingMachine weighingMachine ON weighingMachine.idWeighingMachine = tblPurchaseWeighingStageSummary.weighingMachineId " +
                    "               LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseWeighingStageSummary.createdBy " +
                    "               WHERE tblPurchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                    "           ) AS purchaseWeighingStageSummary  " +
                    " ON ISNULL(purchaseScheduleSummary.rootScheduleId,purchaseScheduleSummary.idPurchaseScheduleSummary) = purchaseWeighingStageSummary.purchaseScheduleSummaryId " +
                    " LEFT JOIN tblPurchaseEnquiry purchaseEnquiry ON purchaseEnquiry.idPurchaseEnquiry = purchaseScheduleSummary.purchaseEnquiryId " +
                    " LEFT JOIN tblProdClassification prodClassification ON prodClassification.idProdClass = purchaseEnquiry.prodClassId " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary grossWt ON grossWt.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND grossWt.weightStageId = 0 AND grossWt.weightMeasurTypeId = 3 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage1 ON wtStage1.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage1.weightStageId = 1 AND wtStage1.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage2 ON wtStage2.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage2.weightStageId = 2 AND wtStage2.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage3 ON wtStage3.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage3.weightStageId = 3 AND wtStage3.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage4 ON wtStage4.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage4.weightStageId = 4 AND wtStage4.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage5 ON wtStage5.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage5.weightStageId = 5 AND wtStage5.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage6 ON wtStage6.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage6.weightStageId = 6 AND wtStage6.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage7 ON wtStage7.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage7.weightStageId = 7 AND wtStage7.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary tareWt ON tareWt.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND tareWt.weightMeasurTypeId = 1 " +
                    " LEFT JOIN tblPurchaseVehicleSpotEntry purchaseVehicleSpotEntry ON purchaseVehicleSpotEntry.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary)" +
                    " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus = purchaseScheduleSummary.statusId" +
                    " WHERE purchaseScheduleSummary.isActive = 1";

                if (!String.IsNullOrEmpty(vehicleIds))
                {
                    sql1 += " AND purchaseScheduleSummary.rootScheduleId IN (" + vehicleIds + ")";
                }

                sql2 =   ""+
                    "" +
                    " INSERT INTO @Temp1 (rootScheduleId)" +
                    " SELECT rootScheduleId FROM @Temp" +
                    "" +
                    " DECLARE @VarID INT" +
                    " SET     @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp1)" +
                    " DECLARE @VarRid INT" +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp1" +
                    " WHILE @VarID !=0" +
                    " BEGIN" +
                    "" +
                    "  DECLARE @statusId INT" +
                    "  DECLARE @vehiclePhaseId INT" +
                    "  DECLARE @rootScheduleId  INT" +
                    "  SELECT  @rootScheduleId = rootScheduleId FROM @Temp1 WHERE   IdR1 = @VarRid" +
                    "" +
                    " SELECT @statusId = tblPurchaseScheduleSummary.statusId,@vehiclePhaseId = tblPurchaseScheduleSummary.vehiclePhaseId " +
                    " FROM  tblPurchaseScheduleSummary tblPurchaseScheduleSummary" +
                    " WHERE tblPurchaseScheduleSummary.idPurchaseScheduleSummary = (SELECT MAX(purchaseScheduleSummarytbl.idPurchaseScheduleSummary) FROM tblPurchaseScheduleSummary purchaseScheduleSummarytbl WHERE purchaseScheduleSummarytbl.rootScheduleId = @rootScheduleId)" +
                    "" +
                    "  IF @statusId = 509 AND @vehiclePhaseId = 2 " +
                    "  BEGIN" +
                    "  UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    "  FROM tblProductItem productItem " +
                    "  LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    "  WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 2) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    "  WHERE rootScheduleId = @rootScheduleId " +
                    "" +
                    " UPDATE @Temp SET vehicleStatus = 'Grading Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    "" +
                    " IF @statusId = 510 AND @vehiclePhaseId = 2" +
                    " BEGIN" +
                    "  UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END" +
                    "" +
                    "  IF @statusId = 509 AND @vehiclePhaseId = 3 " +
                    "  BEGIN" +
                    "  UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    "  FROM tblProductItem productItem " +
                    "  LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    "  WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 3) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    "  WHERE rootScheduleId = @rootScheduleId " +
                    "" +
                    " UPDATE @Temp SET vehicleStatus = 'Recovery Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    "" +
                    " IF @statusId = 510 AND @vehiclePhaseId = 3" +
                    " BEGIN" +
                    "   UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END" +
                    "" +
                    " IF @statusId = 509 AND @vehiclePhaseId = 4 " +
                    " BEGIN" +
                    " UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    " FROM tblProductItem productItem " +
                    " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    " WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 4) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    " WHERE rootScheduleId = @rootScheduleId " +
                    " " +
                    " UPDATE @Temp SET vehicleStatus = 'Correction Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    "" +
                    " IF @statusId = 510 AND @vehiclePhaseId = 4" +
                    " BEGIN" +
                    "  UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END" +
                    "" +
                    " " +
                    " DELETE @Temp1 WHERE IdR1 = @VarRid    " +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp1) " +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp1" +
                    " END" +
                    "" +
                    " SELECT wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,firstWeight,secondWeight,thirdWeight,forthWeight," +
                    " fifthWeight,sixthWeight,seventhWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,UPPER(vehicleNumber) AS vehicleNumber,vehicleStatus,billType,vehicleID" +
                    " FROM @Temp";
                cmdSelect.CommandText = sql1 + sql2;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                //cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt.Date;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblWBRptTO> ConvertDTToListForRPTWBReport(SqlDataReader tblWBRptTOTODT)
        {
            List<TblWBRptTO> TblWBRptTOList = new List<TblWBRptTO>();
            try
            {
                if (tblWBRptTOTODT != null)
                {

                    while (tblWBRptTOTODT.Read())
                    {
                        TblWBRptTO tblWBRptTONew = new TblWBRptTO();
                        
                        if (tblWBRptTOTODT["wBID"] != DBNull.Value)
                            tblWBRptTONew.WBID = Convert.ToString(tblWBRptTOTODT["wBID"].ToString());
                        
                        if (tblWBRptTOTODT["userID"] != DBNull.Value)
                            tblWBRptTONew.UserID = Convert.ToString(tblWBRptTOTODT["userID"].ToString());
                                                
                        if (tblWBRptTOTODT["date"] != DBNull.Value)
                            tblWBRptTONew.Date = Convert.ToString(tblWBRptTOTODT["date"].ToString());
                        
                        if (tblWBRptTOTODT["time"] != DBNull.Value)
                            tblWBRptTONew.Time = Convert.ToString(tblWBRptTOTODT["time"].ToString());
                        
                        if (tblWBRptTOTODT["vehicleNo"] != DBNull.Value)
                        tblWBRptTONew.VehicleNumber = Convert.ToString(tblWBRptTOTODT["vehicleNo"].ToString());
                                                
                        if (tblWBRptTOTODT["grossWeight"] != DBNull.Value)
                            tblWBRptTONew.GrossWeight = Convert.ToDecimal(tblWBRptTOTODT["grossWeight"].ToString());
                        
                        if (tblWBRptTOTODT["firstWeight"] != DBNull.Value)
                            tblWBRptTONew.FirstWeight = Convert.ToDecimal(tblWBRptTOTODT["firstWeight"].ToString());
                        
                        if (tblWBRptTOTODT["secondWeight"] != DBNull.Value)
                            tblWBRptTONew.SecondWeight = Convert.ToDecimal(tblWBRptTOTODT["secondWeight"].ToString());
                        
                        if (tblWBRptTOTODT["thirdWeight"] != DBNull.Value)
                            tblWBRptTONew.ThirdWeight = Convert.ToDecimal(tblWBRptTOTODT["thirdWeight"].ToString());
                        
                        if (tblWBRptTOTODT["forthWeight"] != DBNull.Value)
                            tblWBRptTONew.ForthWeight = Convert.ToDecimal(tblWBRptTOTODT["forthWeight"].ToString());
                        
                        if (tblWBRptTOTODT["fifthWeight"] != DBNull.Value)
                            tblWBRptTONew.FifthWeight = Convert.ToDecimal(tblWBRptTOTODT["fifthWeight"].ToString());
                        
                        if (tblWBRptTOTODT["sixthWeight"] != DBNull.Value)
                            tblWBRptTONew.SixthWeight = Convert.ToDecimal(tblWBRptTOTODT["sixthWeight"].ToString());
                        
                        if (tblWBRptTOTODT["seventhWeight"] != DBNull.Value)
                            tblWBRptTONew.SeventhWeight = Convert.ToDecimal(tblWBRptTOTODT["seventhWeight"].ToString());
                        
                        if (tblWBRptTOTODT["tareWeight"] != DBNull.Value)
                            tblWBRptTONew.TareWeight = Convert.ToDecimal(tblWBRptTOTODT["tareWeight"].ToString());
                        
                        if (tblWBRptTOTODT["netWeight"] != DBNull.Value)
                            tblWBRptTONew.NetWeight = Convert.ToDecimal(tblWBRptTOTODT["netWeight"].ToString());
                        
                        if (tblWBRptTOTODT["loadOrUnload"] != DBNull.Value)
                            tblWBRptTONew.LoadOrUnload = Convert.ToString(tblWBRptTOTODT["loadOrUnload"].ToString());
                                                
                        if (tblWBRptTOTODT["transactionType"] != DBNull.Value)
                            tblWBRptTONew.TransactionType = Convert.ToString(tblWBRptTOTODT["transactionType"].ToString());
                        
                        if (tblWBRptTOTODT["vehicleID"] != DBNull.Value)
                            tblWBRptTONew.VehicleID = Convert.ToString(tblWBRptTOTODT["vehicleID"].ToString());

                        if (tblWBRptTOTODT["status"] != DBNull.Value)
                            tblWBRptTONew.VehicleStatus = Convert.ToString(tblWBRptTOTODT["status"].ToString());

                        if (tblWBRptTOTODT["id"] != DBNull.Value)
                            tblWBRptTONew.Id = Convert.ToInt64(tblWBRptTOTODT["id"].ToString());

                        if (tblWBRptTOTODT["MaterialType"] != DBNull.Value)
                            tblWBRptTONew.MaterialType = tblWBRptTOTODT["MaterialType"].ToString();

                        if (tblWBRptTOTODT["MaterialSubType"] != DBNull.Value)
                            tblWBRptTONew.MaterialSubType = tblWBRptTOTODT["MaterialSubType"].ToString();

                        if (tblWBRptTOTODT["FromLocation"] != DBNull.Value)
                            tblWBRptTONew.FromLocation = tblWBRptTOTODT["FromLocation"].ToString();

                        if (tblWBRptTOTODT["ToLocation"] != DBNull.Value)
                            tblWBRptTONew.ToLocation = tblWBRptTOTODT["ToLocation"].ToString();

                        if (tblWBRptTOTODT["BillType"] != DBNull.Value)
                            tblWBRptTONew.BillType = tblWBRptTOTODT["BillType"].ToString();

                        TblWBRptTOList.Add(tblWBRptTONew);

                    }
                }
                // return TblWBRptTOList;
                return TblWBRptTOList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}