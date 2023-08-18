using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using PurchaseTrackerAPI.Controllers;
using PurchaseTrackerAPI.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.BL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.IoT;
using PurchaseTrackerAPI.IoT.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using Serilog;
using BL;


namespace PurchaseTrackerAPI
{
    public class Startup
    {
        //private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        //public Startup(ITblConfigParamsDAO iTblConfigParamsDAO)
        //{
        //    _iTblConfigParamsDAO = iTblConfigParamsDAO;
        //}

        public static string ConnectionString { get; private set; }
        public static string NewConnectionString { get; private set; }

        public static string RequestOriginString { get; set; }
        public static string AzureConnectionStr { get; set; }

        public static string RecycleUrl { get; set; }

        public static string CommonUrl { get; private set; }

        public static string StockAPIUrl { get; private set; }

        public static Boolean isLive { get; private set; }

        public static Boolean IsForBRM { get; private set; }
         

        //   public static Boolean isForBRM { get; private set; }
        //For IoT
        public static Int32 WeighingSrcConfig { get; private set; }

        public static string IoTBackUpConnectionString { get; private set; }
        //public static List<int> AvailableModbusRefList { get; set; }
        public static string GateIotApiURL { get; set; }

        public static string SERVER_DATETIME_QUERY_STRING { get; private set; }
        public static string MIGRATION_CONNECTION_STRING { get; private set; }
        public static Boolean IsLocalAPI { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
#if DEBUG
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.RollingFile("../logs/error_log-{Date}.txt")
            .WriteTo.Logger(l => l
            .MinimumLevel.Warning()
            .MinimumLevel.Information()
            .WriteTo.RollingFile("../logs/warling_log-{Date}.log")).CreateLogger();
#else
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.RollingFile("../logs/log-{Date}.txt").CreateLogger();
#endif


        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddSingleton<IConnectionString, ConnectionString>();
            services.AddSingleton<Idimensionbl, DimensionBL>();
            services.AddSingleton<Idimensiondao, DimensionDAO>();
            services.AddSingleton<IDimPageElementTypesBL, DimPageElementTypesBL>();
            services.AddSingleton<IDimPageElementTypesDAO, DimPageElementTypesDAO>();
            services.AddSingleton<IDimQualitySampleTypeBL, DimQualitySampleTypeBL>();
            services.AddSingleton<IDimQualitySampleTypeDAO, DimQualitySampleTypeDAO>();
            services.AddSingleton<Icommondao, CommonDAO>();
            services.AddSingleton<IDimStateDAO, DimStateDAO>();
            services.AddSingleton<IDimStateBL, DimStateBL>();
            services.AddSingleton<IDimStatusDAO, DimStatusDAO>();
            services.AddSingleton<IDimStatusBL, DimStatusBL>();
            services.AddSingleton<IDimVehicleCategoryBL, DimVehicleCategoryBL>();
            services.AddSingleton<IDimVehicleCategoryDAO, DimVehicleCategoryDAO>();
            services.AddSingleton<IDimVehiclePhaseBL, DimVehiclePhaseBL>();
            services.AddSingleton<IDimVehiclePhaseDAO, DimVehiclePhaseDAO>();
            services.AddSingleton<ISendMailDAO, SendMailDAO>();
            services.AddSingleton<IDimVehicleTypeDAO, DimVehicleTypeDAO>();
            services.AddSingleton<ISQLHelper, SQLHelper>();
            services.AddSingleton<INotification, Notification>();
            services.AddSingleton<IReportBL, ReportBL>();
            services.AddSingleton<IReportDAO, ReportDAO>();
            services.AddSingleton<ITblAddonsFunDtlsDAO, TblAddonsFunDtlsDAO>();
            services.AddSingleton<ITblAddonsFunDtlsBL, TblAddonsFunDtlsBL>();
            services.AddSingleton<ITblAddressDAO, TblAddressDAO>();
            services.AddSingleton<ITblAddressBL, TblAddressBL>();
            services.AddSingleton<ITblAlertActionDtlBL, TblAlertActionDtlBL>();
            services.AddSingleton<ITblAlertActionDtlDAO, TblAlertActionDtlDAO>();
            services.AddSingleton<ITblAlertDefinitionDAO, TblAlertDefinitionDAO>();
            services.AddSingleton<ITblAlertDefinitionBL, TblAlertDefinitionBL>();
            services.AddSingleton<ITblAlertEscalationSettingsBL, TblAlertEscalationSettingsBL>();
            services.AddSingleton<ITblAlertEscalationSettingsDAO, TblAlertEscalationSettingsDAO>();
            services.AddSingleton<ITblAlertInstanceDAO, TblAlertInstanceDAO>();
            services.AddSingleton<Itblalertinstancebl, TblAlertInstanceBL>();
            services.AddSingleton<ITblAlertSubscribersBL, TblAlertSubscribersBL>();
            services.AddSingleton<ITblAlertSubscribersDAO, TblAlertSubscribersDAO>();
            services.AddSingleton<ITblAlertSubscribersBL, TblAlertSubscribersBL>();
            services.AddSingleton<ITblAlertSubscriptSettingsDAO, TblAlertSubscriptSettingsDAO>();
            services.AddSingleton<TblAlertSubscriptSettingsBL, TblAlertSubscriptSettingsBL>();
            services.AddSingleton<ITblAlertUsersDAO, TblAlertUsersDAO>();
            services.AddSingleton<ITblAlertUsersBL, TblAlertUsersBL>();
            services.AddSingleton<ITblBaseItemMetalCostDAO, TblBaseItemMetalCostDAO>();
            services.AddSingleton<ITblBaseItemMetalCostBL, TblBaseItemMetalCostBL>();
            services.AddSingleton<ITblCnfDealersBL, TblCnfDealersBL>();
            services.AddSingleton<ITblCnfDealersDAO, TblCnfDealersDAO>();
            services.AddSingleton<ITblCompetitorExtDAO, TblCompetitorExtDAO>();
            services.AddSingleton<ITblCompetitorExtBL, TblCompetitorExtBL>();
            services.AddSingleton<ITblConfigParamHistoryDAO, TblConfigParamHistoryDAO>();
            services.AddSingleton<ITblConfigParamHistoryBL, TblConfigParamHistoryBL>();
            services.AddSingleton<ITblConfigParamsDAO, TblConfigParamsDAO>();
            services.AddSingleton<ITblConfigParamsBL, TblConfigParamsBL>();
            services.AddSingleton<ITblDocumentDetailsDAO, TblDocumentDetailsDAO>();
            services.AddSingleton<ITblDocumentDetailsBL, TblDocumentDetailsBL>();
            services.AddSingleton<ITblEmailConfigrationDAO, TblEmailConfigrationDAO>();
            services.AddSingleton<ITblEmailConfigrationBL, TblEmailConfigrationBL>();
            services.AddSingleton<ITblEnquiryDtlBL, TblEnquiryDtlBL>();
            services.AddSingleton<ITblEnquiryDtlDAO, TblEnquiryDtlDAO>();
            services.AddSingleton<ITblEntityRangeBL, TblEntityRangeBL>();
            services.AddSingleton<ITblEntityRangeDAO, TblEntityRangeDAO>();
            services.AddSingleton<ITblExpressionDtlsBL, TblExpressionDtlsBL>();
            services.AddSingleton<ITblExpressionDtlsDAO, TblExpressionDtlsDAO>();
            services.AddSingleton<ITblFeedbackDAO, TblFeedbackDAO>();
            services.AddSingleton<ITblFeedbackBL, TblFeedbackBL>();
            services.AddSingleton<ITblGlobalRateBL, TblGlobalRateBL>();
            services.AddSingleton<ITblGlobalRateDAO, TblGlobalRateDAO>();
            services.AddSingleton<ITblGlobalRatePurchaseBL, TblGlobalRatePurchaseBL>();
            services.AddSingleton<ITblGlobalRatePurchaseDAO, TblGlobalRatePurchaseDAO>();
            services.AddSingleton<ITblGradeExpressionDtlsBL, TblGradeExpressionDtlsBL>();
            services.AddSingleton<ITblGradeExpressionDtlsDAO, TblGradeExpressionDtlsDAO>();
            services.AddSingleton<ITblGradeWiseTargetQtyBL, TblGradeWiseTargetQtyBL>();
            services.AddSingleton<ITblGradeWiseTargetQtyDAO, TblGradeWiseTargetQtyDAO>();
            services.AddSingleton<ITblGstCodeDtlsDAO, TblGstCodeDtlsDAO>();
            services.AddSingleton<ITblGstCodeDtlsBL, TblGstCodeDtlsBL>();
            services.AddSingleton<Itblloadingbl, TblLoadingBL>();
            services.AddSingleton<Itblloadingdao, TblLoadingDAO>();
            services.AddSingleton<ITblLocationDAO, TblLocationDAO>();
            services.AddSingleton<ITblLocationBL, TblLocationBL>();
            services.AddSingleton<ITblLoginBL, TblLoginBL>();
            services.AddSingleton<ITblLoginDAO, TblLoginDAO>();
            services.AddSingleton<ITblMachineCalibrationBL, TblMachineCalibrationBL>();
            services.AddSingleton<ITblMachineCalibrationDAO, TblMachineCalibrationDAO>();
            services.AddSingleton<ITblMaterialBL, TblMaterialBL>();
            services.AddSingleton<ITblMaterialDAO, TblMaterialDAO>();
            services.AddSingleton<ITblMenuStructureBL, TblMenuStructureBL>();
            services.AddSingleton<ITblMenuStructureDAO, TblMenuStructureDAO>();
            services.AddSingleton<ITblModuleBL, TblModuleBL>();
            services.AddSingleton<ITblModuleDAO, TblModuleDAO>();
            services.AddSingleton<ITblOrganizationBL, TblOrganizationBL>();
            services.AddSingleton<ITblOrganizationDAO, TblOrganizationDAO>();
            services.AddSingleton<ITblOtherSourceBL, TblOtherSourceBL>();
            services.AddSingleton<ITblOtherSourceDAO, TblOtherSourceDAO>();
            services.AddSingleton<ITblOtherTaxesDAO, TblOtherTaxesDAO>();
            services.AddSingleton<ITblPageElementsBL, TblPageElementsBL>();
            services.AddSingleton<ITblPageElementsDAO, TblPageElementsDAO>();
            services.AddSingleton<ITblOverdueDtlDAO, TblOverdueDtlDAO>();
            services.AddSingleton<ITblPagesBL, TblPagesBL>();
            services.AddSingleton<ITblPagesDAO, TblPagesDAO>();
            services.AddSingleton<ITblPartyWeighingMeasuresBL, TblPartyWeighingMeasuresBL>();
            services.AddSingleton<ITblPartyWeighingMeasuresDAO, TblPartyWeighingMeasuresDAO>();
            services.AddSingleton<ITblPersonDAO, TblPersonDAO>();
            services.AddSingleton<ITblPersonBL, TblPersonBL>();
            services.AddSingleton<ITblPmUserDAO, TblPmUserDAO>();
            services.AddSingleton<ITblPmUserBL, TblPmUserBL>();
            services.AddSingleton<ITblProdClassificationDAO, TblProdClassificationDAO>();
            services.AddSingleton<ITblProdClassificationBL, TblProdClassificationBL>();
            services.AddSingleton<ITblProdGstCodeDtlsBL, TblProdGstCodeDtlsBL>();
            services.AddSingleton<ITblProdGstCodeDtlsDAO, TblProdGstCodeDtlsDAO>();
            services.AddSingleton<ITblProductInfoBL, TblProductInfoBL>();
            services.AddSingleton<ITblProductInfoDAO, TblProductInfoDAO>();
            services.AddSingleton<ITblProductItemBL, TblProductItemBL>();
            services.AddSingleton<ITblProductItemDAO, TblProductItemDAO>();
            services.AddSingleton<ITblPurchaseBookingActionsBL, TblPurchaseBookingActionsBL>();
            services.AddSingleton<ITblPurchaseBookingActionsDAO, TblPurchaseBookingActionsDAO>();
            services.AddSingleton<ITblPurchaseBookingBeyondQuotaDAO, TblPurchaseBookingBeyondQuotaDAO>();
            services.AddSingleton<ITblPurchaseBookingBeyondQuotaBL, TblPurchaseBookingBeyondQuotaBL>();
            services.AddSingleton<ITblPurchaseCompetitorExtDAO, TblPurchaseCompetitorExtDAO>();
            services.AddSingleton<ITblPurchaseCompetitorExtBL, TblPurchaseCompetitorExtBL>();
            services.AddSingleton<Itblpurchasecompetitorupdatesdao, tblpurchasecompetitorupdatesdao>();
            services.AddSingleton<ITblPurchaseCompetitorUpdatesBL, TblPurchaseCompetitorUpdatesBL>();
            services.AddSingleton<ITblPurchaseDocToVerifyDAO, TblPurchaseDocToVerifyDAO>();
            services.AddSingleton<ITblPurchaseDocToVerifyBL, TblPurchaseDocToVerifyBL>();
            services.AddSingleton<ITblPurchaseEnquiryBL, TblPurchaseEnquiryBL>();
            services.AddSingleton<ITblPurchaseEnquiryDAO, TblPurchaseEnquiryDAO>();
            services.AddSingleton<ITblPurchaseEnquiryDetailsBL, TblPurchaseEnquiryDetailsBL>();
            services.AddSingleton<ITblPurchaseEnquiryDetailsDAO, TblPurchaseEnquiryDetailsDAO>();
            services.AddSingleton<ITblPurchaseEnquiryHistoryBL, TblPurchaseEnquiryHistoryBL>();
            services.AddSingleton<ITblPurchaseEnquiryHistoryDAO, TblPurchaseEnquiryHistoryDAO>();
            services.AddSingleton<ITblPurchaseEnquiryScheduleDAO, TblPurchaseEnquiryScheduleDAO>();
            services.AddSingleton<ITblPurchaseEnquiryScheduleBL, TblPurchaseEnquiryScheduleBL>();
            services.AddSingleton<ITblPurchaseGradingDtlsBL, TblPurchaseGradingDtlsBL>();
            services.AddSingleton<ITblPurchaseGradingDtlsDAO, TblPurchaseGradingDtlsDAO>();
            services.AddSingleton<ITblPurchaseInvoiceAddrBL, TblPurchaseInvoiceAddrBL>();
            services.AddSingleton<ITblPurchaseInvoiceAddrDAO, TblPurchaseInvoiceAddrDAO>();
            services.AddSingleton<ITblPurchaseInvoiceBL, TblPurchaseInvoiceBL>();
            services.AddSingleton<ITblPurchaseInvoiceDAO, TblPurchaseInvoiceDAO>();
            services.AddSingleton<ITblPurchaseDocToVerifyBL, TblPurchaseDocToVerifyBL>();
            services.AddSingleton<ITblPurchaseDocToVerifyDAO, TblPurchaseDocToVerifyDAO>();
            services.AddSingleton<ITblPurchaseInvoiceHistoryBL, TblPurchaseInvoiceHistoryBL>();
            services.AddSingleton<ITblPurchaseInvoiceHistoryDAO, TblPurchaseInvoiceHistoryDAO>();
            services.AddSingleton<ITblPurchaseInvoiceDocumentsBL, TblPurchaseInvoiceDocumentsBL>();
            services.AddSingleton<ITblPurchaseInvoiceDocumentsDAO, TblPurchaseInvoiceDocumentsDAO>();
            services.AddSingleton<ITblPurchaseInvoiceInterfacingDtlBL, TblPurchaseInvoiceInterfacingDtlBL>();
            services.AddSingleton<ITblPurchaseInvoiceInterfacingDtlDAO, TblPurchaseInvoiceInterfacingDtlDAO>();
            services.AddSingleton<ITblPurchaseInvoiceItemDetailsBL, TblPurchaseInvoiceItemDetailsBL>();
            services.AddSingleton<ITblPurchaseInvoiceItemDetailsDAO, TblPurchaseInvoiceItemDetailsDAO>();
            services.AddSingleton<ITblPurchaseInvoiceItemTaxDetailsBL, TblPurchaseInvoiceItemTaxDetailsBL>();
            services.AddSingleton<ITblPurchaseInvoiceItemTaxDetailsDAO, TblPurchaseInvoiceItemTaxDetailsDAO>();
            services.AddSingleton<ITblPurchaseManagerSupplierBL, TblPurchaseManagerSupplierBL>();
            services.AddSingleton<ITblPurchaseManagerSupplierDAO, TblPurchaseManagerSupplierDAO>();
            services.AddSingleton<ITblPurchaseParityDetailsBL, TblPurchaseParityDetailsBL>();
            services.AddSingleton<ITblPurchaseParityDetailsDAO, TblPurchaseParityDetailsDAO>();
            services.AddSingleton<ITblPurchaseParitySummaryBL, TblPurchaseParitySummaryBL>();
            services.AddSingleton<ITblPurchaseParitySummaryDAO, TblPurchaseParitySummaryDAO>();
            services.AddSingleton<ITblPurchaseScheduleStatusHistoryBL, TblPurchaseScheduleStatusHistoryBL>();
            services.AddSingleton<ITblPurchaseScheduleStatusHistoryDAO, TblPurchaseScheduleStatusHistoryDAO>();
            services.AddSingleton<ITblPurchaseScheduleSummaryBL, TblPurchaseScheduleSummaryBL>();
            services.AddSingleton<ITblPurchaseScheduleSummaryDAO, TblPurchaseScheduleSummaryDAO>();
            services.AddSingleton<ITblPurchaseUnloadingDtlBL, TblPurchaseUnloadingDtlBL>();
            services.AddSingleton<ITblPurchaseUnloadingDtlDAO, TblPurchaseUnloadingDtlDAO>();
            services.AddSingleton<ITblPurchaseVehicleDetailsBL, TblPurchaseVehicleDetailsBL>();
            services.AddSingleton<ITblPurchaseVehicleDetailsDAO, TblPurchaseVehicleDetailsDAO>();
            services.AddSingleton<ITblPurchaseVehicleOtherEntryBL, TblPurchaseVehicleOtherEntryBL>();
            services.AddSingleton<ITblPurchaseVehicleOtherEntryDAO, TblPurchaseVehicleOtherEntryDAO>();
            services.AddSingleton<ITblPurchaseVehicleSpotEntryBL, TblPurchaseVehicleSpotEntryBL>();
            services.AddSingleton<ITblPurchaseVehicleSpotEntryDAO, TblPurchaseVehicleSpotEntryDAO>();
            services.AddSingleton<ITblPurchaseVehicleStageCntBL, TblPurchaseVehicleStageCntBL>();
            services.AddSingleton<ITblPurchaseVehicleStageCntDAO, TblPurchaseVehicleStageCntDAO>();
            services.AddSingleton<ITblPurchaseWeighingStageSummaryBL, TblPurchaseWeighingStageSummaryBL>();
            services.AddSingleton<ITblPurchaseWeighingStageSummaryDAO, TblPurchaseWeighingStageSummaryDAO>();
            services.AddSingleton<ITblQualityPhaseBL, TblQualityPhaseBL>();
            services.AddSingleton<ITblQualityPhaseDAO, TblQualityPhaseDAO>();
            services.AddSingleton<ITblQualityPhaseDtlsBL, TblQualityPhaseDtlsBL>();
            services.AddSingleton<ITblQualityPhaseDtlsDAO, TblQualityPhaseDtlsDAO>();
            services.AddSingleton<ITblQualityPhaseDtlsDAO, TblQualityPhaseDtlsDAO>();
            services.AddSingleton<Itblquotadeclarationbl, TblQuotaDeclarationBL>();
            services.AddSingleton<ITblQuotaDeclarationDAO, TblQuotaDeclarationDAO>();
            services.AddSingleton<ITblRateBandDeclarationPurchaseBL, TblRateBandDeclarationPurchaseBL>();
            services.AddSingleton<ITblRateBandDeclarationPurchaseDAO, TblRateBandDeclarationPurchaseDAO>();
            services.AddSingleton<ITblRateDeclareReasonsBL, TblRateDeclareReasonsBL>();
            services.AddSingleton<ITblRateDeclareReasonsDAO, TblRateDeclareReasonsDAO>();
            services.AddSingleton<ITblRoleBL, TblRoleBL>();
            services.AddSingleton<ITblRoleDAO, TblRoleDAO>();
            services.AddSingleton<ITblSmsBL, TblSmsBL>();
            services.AddSingleton<ITblSmsDAO, TblSmsDAO>();
            services.AddSingleton<ITblSpotVehicleMaterialDtlsBL, TblSpotVehicleMaterialDtlsBL>();
            services.AddSingleton<ITblSpotVehicleMaterialDtlsDAO, TblSpotVehicleMaterialDtlsDAO>();
            services.AddSingleton<ITblSysElementsBL, TblSysElementsBL>();
            services.AddSingleton<ITblSysElementsDAO, TblSysElementsDAO>();
            services.AddSingleton<ITblSysEleRoleEntitlementsBL, TblSysEleRoleEntitlementsBL>();
            services.AddSingleton<ITblSysEleRoleEntitlementsDAO, TblSysEleRoleEntitlementsDAO>();
            services.AddSingleton<ITblSysEleUserEntitlementsDAO, TblSysEleUserEntitlementsDAO>();
            services.AddSingleton<ITblSysEleUserEntitlementsBL, TblSysEleUserEntitlementsBL>();
            services.AddSingleton<ITblTaxRatesBL, TblTaxRatesBL>();
            services.AddSingleton<ITblTaxRatesDAO, TblTaxRatesDAO>();
            services.AddSingleton<ITblUserBL, TblUserBL>();
            services.AddSingleton<ITblUserDAO, TblUserDAO>();
            services.AddSingleton<ITblUserExtBL, TblUserExtBL>();
            services.AddSingleton<ITblUserExtDAO, TblUserExtDAO>();
            services.AddSingleton<ITblUserRoleBL, TblUserRoleBL>();
            services.AddSingleton<ITblUserRoleDAO, TblUserRoleDAO>();
            services.AddSingleton<ITblUserVerBL, TblUserVerBL>();
            services.AddSingleton<ITblVariablesBL, TblVariablesBL>();
            services.AddSingleton<ITblVariablesDAO, TblVariablesDAO>();
            services.AddSingleton<ITblWeighingBL, TblWeighingBL>();
            services.AddSingleton<ITblWeighingDAO, TblWeighingDAO>();
            services.AddSingleton<ITblWeighingMachineBL, TblWeighingMachineBL>();
            services.AddSingleton<ITblWeighingMachineDAO, TblWeighingMachineDAO>();
            services.AddSingleton<Ivitplnotify, VitplNotify>();
            services.AddSingleton<Ivitplsms, VitplSMS>();
            services.AddSingleton<ITblAlertSubscriptSettingsBL, TblAlertSubscriptSettingsBL>();
            services.AddSingleton<ITblAlertSubscriptSettingsDAO, TblAlertSubscriptSettingsDAO>();


            services.AddSingleton<ITblRecyclePreferenceDAO, TblRecyclePreferenceDAO>();
            services.AddSingleton<ITblRecyclePreferenceBL, TblRecyclePreferenceBL>();
            services.AddSingleton<ITblRecycleDocumentBL, TblRecycleDocumentBL>();
            services.AddSingleton<ITblRecycleDocumentDAO, TblRecycleDocumentDAO>();
            services.AddSingleton<IDimVehicleTypeBL, DimVehicleTypeBL>();
            services.AddSingleton<IDimVehicleTypeDAO, DimVehicleTypeDAO>();
            services.AddSingleton<ITblPurchaseVehicleMaterialSampleDAO, TblPurchaseVehicleMaterialSampleDAO>();
            services.AddSingleton<ICircularDependancyBL, CircularDependancyBL>();
            services.AddSingleton<IPurchaseScheduleSummerycircularBL, PurchaseScheduleSummerycircularBL>();
            services.AddSingleton<ITblProdItemDescBL, TblProdItemDescBL>();
            services.AddSingleton<ITblPurchaseItemDescBL, TblPurchaseItemDescBL>();


            services.AddSingleton<ITblProdItemDescBL, TblProdItemDescBL>();
            services.AddSingleton<ITblPurchaseItemDescBL, TblPurchaseItemDescBL>();

            services.AddSingleton<ITblProdItemDescDAO, TblProdItemDescDAO>();
            services.AddSingleton<ITblPurchaseItemDescDAO, TblPurchaseItemDescDAO>();

            services.AddSingleton<ITblPurchaseBookingOpngBalBL, TblPurchaseBookingOpngBalBL>();
            services.AddSingleton<ITblPurchaseBookingOpngBalDAO, TblPurchaseBookingOpngBalDAO>();

            services.AddSingleton<ITblPurchaseEnquiryQtyConsumptionBL, TblPurchaseEnquiryQtyConsumptionBL>();
            services.AddSingleton<ITblPurchaseEnquiryQtyConsumptionDAO, TblPurchaseEnquiryQtyConsumptionDAO>();

            services.AddSingleton<ITblPurchaseBookingOpngBalBL, TblPurchaseBookingOpngBalBL>();
            services.AddSingleton<ITblPurchaseBookingOpngBalDAO, TblPurchaseBookingOpngBalDAO>();

            services.AddSingleton<ITblPurchaseVehFreightDtlsBL, TblPurchaseVehFreightDtlsBL>();
            services.AddSingleton<ITblPurchaseVehFreightDtlsDAO, TblPurchaseVehFreightDtlsDAO>();

            services.AddSingleton<ITblDashboardEntityBL, TblDashboardEntityBL>();
            services.AddSingleton<ITblDashboardEntityDAO, TblDashboardEntityDAO>();

            services.AddSingleton<ITblDashboardEntityHistoryBL, TblDashboardEntityHistoryBL>();
            services.AddSingleton<ITblDashboardEntityHistoryDAO, TblDashboardEntityHistoryDAO>();


            services.AddSingleton<ITblPurchaseVehicleStatusHistoryBL, TblPurchaseVehicleStatusHistoryBL>();
            services.AddSingleton<ITblPurchaseVehicleStatusHistoryDAO, TblPurchaseVehicleStatusHistoryDAO>();

            services.AddSingleton<IDimReportTemplateDAO,DimReportTemplateDAO>();
            services.AddSingleton<IDimReportTemplateBL, DimReportTemplateBL>();
            services.AddSingleton<IRunVegaFlexCelReport,RunVegaFlexCelReport>();
            services.AddSingleton<IRunReport,RunReport>();

            services.AddSingleton<ITblScheduleDensityBL, TblScheduleDensityBL>();
            services.AddSingleton<ITblScheduleDensityDAO, TblScheduleDensityDAO>();
            //services.AddSingleton<ITblConfigParamsDAO, ITblConfigParamsDAO>();

            //services.AddScoped<IDimReportTemplateDAO,DimReportTemplateDAO>();
            //services.AddScoped<IDimReportTemplateBL, DimReportTemplateBL>();
            //services.AddScoped<IRunVegaFlexCelReport,RunVegaFlexCelReport>();
            //services.AddScoped<IRunReport,RunReport>();

            services.AddSingleton<ITblPurchaseEnqVehDescBL, TblPurchaseEnqVehDescBL>();
            services.AddSingleton<ITblPurchaseEnqVehDescDAO, TblPurchaseEnqVehDescDAO>();
            services.AddSingleton<IDimPurchaseTcTypeElementBL, DimPurchaseTcTypeElementBL>();
            services.AddSingleton<IDimPurchaseTcTypeElementDAO, DimPurchaseTcTypeElementDAO>();

            services.AddSingleton<ITblPurchaseSchStatusHistoryBL, TblPurchaseSchStatusHistoryBL>();
            services.AddSingleton<ITblPurchaseSchStatusHistoryDAO, TblPurchaseSchStatusHistoryDAO>();
            services.AddSingleton<ITblPurchaseSchTcDtlsBL, TblPurchaseSchTcDtlsBL>();
            services.AddSingleton<ITblPurchaseSchTcDtlsDAO, TblPurchaseSchTcDtlsDAO>();
            services.AddSingleton<IDimPurchaseGradeQtyTypeBL, DimPurchaseGradeQtyTypeBL>();
            services.AddSingleton<ITblGradeQtyDtlsBL, TblGradeQtyDtlsBL>();
            services.AddSingleton<IDimPurchaseGradeQtyTypeDAO, DimPurchaseGradeQtyTypeDAO>();
            services.AddSingleton<ITblGradeQtyDtlsDAO, TblGradeQtyDtlsDAO>();

            services.AddSingleton<ITblPurchaseVehLinkSaudaDAO, TblPurchaseVehLinkSaudaDAO>();
            services.AddSingleton<ITblPurchaseVehLinkSaudaBL, TblPurchaseVehLinkSaudaBL>();

            services.AddSingleton<ITblpurchaseEnqShipmemtDtlsBL, TblpurchaseEnqShipmemtDtlsBL>();
            services.AddSingleton<ITblpurchaseEnqShipmemtDtlsDAO, TblpurchaseEnqShipmemtDtlsDAO>();
            services.AddSingleton<ITblpurchaseEnqShipmemtDtlsExtBL, TblpurchaseEnqShipmemtDtlsExtBL>();
            services.AddSingleton<ITblpurchaseEnqShipmemtDtlsExtDAO, TblpurchaseEnqShipmemtDtlsExtDAO>();

            services.AddSingleton<ITblReportsBackupDtlsBL, TblReportsBackupDtlsBL>();
            services.AddSingleton<ITblReportsBackupDtlsDAO, TblReportsBackupDtlsDAO>();

            services.AddSingleton<ITblSpotEntryContainerDtlsBL, TblSpotEntryContainerDtlsBL>();
            services.AddSingleton<ITblSpotEntryContainerDtlsDAO, TblSpotEntryContainerDtlsDAO>();

            //Internal Transfer
            services.AddSingleton<ITblVehicleBL , TblVehicleBL >();
            services.AddSingleton<ITblVehicleDAO , TblVehicleDAO>();
            services.AddSingleton<ITblTransferRequestBL , TblTransferRequestBL>();
            services.AddSingleton<ITblTransferRequestDAO , TblTransferRequestDAO>(); 
            services.AddSingleton<ITblTRSLABL , TblTRSLABL>();
            services.AddSingleton<ITblTRSLADAO , TblTRSLADAO>();
            services.AddSingleton<ILoadingBL, LoadingBL>();
            services.AddSingleton<ILoadingDAO, LoadingDAO>();


            #region DI Of IoT MiddleWare @KKM 
            services.AddSingleton<IModbusRefConfig, ModbusRefConfig>();
            services.AddSingleton<IGateCommunication, GateCommunication>();
            services.AddSingleton<IIotCommunication, IotCommunication>();
            services.AddSingleton<IWeighingCommunication, WeighingCommunication>();
            services.AddSingleton<ITblGateBL, TblGateBL>();
            services.AddSingleton<ITblGateDAO, TblGateDAO>();
            #endregion


            //APG[2022-Jan-20]
            services.AddSingleton<ITblTRLoadingBL, TblTRLoadingBL>();
            services.AddSingleton<ITblTRLoadingDAO, TblTRLoadingDAO>();
            services.AddSingleton<ITblTRLoadingHistoryBL, TblTRLoadingHistoryBL>();
            services.AddSingleton<ITblTRLoadingHistoryDAO, TblTRLoadingHistoryDAO>();





            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder.Build());
            });
            services.AddMvc();

            // swagger implementation 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Transfer API", Version = "v1" });
            });

            services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented; });
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddMvc();
            ConnectionString = Configuration.GetSection("Data:DefaultConnection").Value.ToString();
            CommonUrl = Convert.ToString(Configuration.GetSection("Data:CommonUrl").Value);
            StockAPIUrl = Convert.ToString(Configuration.GetSection("Data:StockAPIUrl").Value);
            RequestOriginString = Convert.ToString(Configuration.GetSection("Data:RequestOriginString").Value);
            NewConnectionString = Configuration.GetSection("Data:NewDefaultConnection").Value.ToString();
            AzureConnectionStr = Configuration.GetSection("Data:AzureConnectionStr").Value.ToString();
            MIGRATION_CONNECTION_STRING = Configuration.GetSection("Data:DefaultMirgrationConnection").Value.ToString();
            RecycleUrl = Configuration.GetSection("Data:RecycleUrl").Value.ToString();

            //GetDateTimeQueryString();
            //IsLocalApi();
            //Prajakta[2019-04-11] Added
            //TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(StaticStuff.Constants.CP_SCRAP_IS_FOR_BHAGYALAXMI);
            //if (tblConfigParamsTO != null)
            //{
            //    if(tblConfigParamsTO.ConfigParamVal=="1")
            //    {
            //        isForBRM=true;
            //    }
            //    else
            //    {
            //        isForBRM=false;
            //    }
            //}
            GetDateTimeQueryString();
            IsLocalApi();
            IsForBRMConfigVal();

        }

        public void GetDateTimeQueryString()
        {
            TblConfigParamsDAO a = new TblConfigParamsDAO();
            string sqlQuery = "SELECT CURRENT_TIMESTAMP AS ServerDate";
            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamValByName(Constants.SERVER_DATETIME_QUERY_STRING);
            if (tblConfigParamsTO != null)
            {
                sqlQuery = tblConfigParamsTO.ConfigParamVal;
            }
            SERVER_DATETIME_QUERY_STRING = sqlQuery;

        }

        public void IsForBRMConfigVal()
        {
            IsForBRM = false;
            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamValByName(StaticStuff.Constants.CP_SCRAP_IS_FOR_BHAGYALAXMI);
            if (tblConfigParamsTO != null)
            {
                if (tblConfigParamsTO.ConfigParamVal == "1")
                {
                    IsForBRM = true;
                }
                else
                {
                    IsForBRM = false;
                }
            }
        }

        public void IsLocalApi()
        {
            IsLocalAPI = false;
            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamValByName(StaticStuff.Constants.IS_LOCAL_API);
            if (tblConfigParamsTO != null)
            {
                Int32 isLocalAPI = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                if (isLocalAPI == 1)
                    IsLocalAPI = true;
                else
                    IsLocalAPI = false;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug(LogLevel.Information).AddSerilog();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error).AddSerilog();
            }

            app.UseCors("AllowAll");
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Purchase API V1");
            });

            app.UseMvc();
            //Add For init of modbusref list (IoT) @KKM 
           // ModbusRefConfig.setModbusRef();
        }
    }
}
