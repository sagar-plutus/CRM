using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseScheduleSummaryDAO : ITblPurchaseScheduleSummaryDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly Icommondao _iCommonDAO;
        public TblPurchaseScheduleSummaryDAO(IConnectionString iConnectionString, ITblConfigParamsDAO iTblConfigParamsDAO, ITblConfigParamsBL iTblConfigParamsBL, Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iCommonDAO = icommondao;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseScheduleSummary]";
            return sqlSelectQry;
        }
        public String SqlSelectQueryNew()
        {
            String sqlSelectQry = " Select tblOrganization.firmName,tblUser.userDisplayName,dimStatus.statusName, tblPurchaseEnquiry.cOrNCId, tblPurchaseEnquiry.bookingRate, tblProdClassification.displayName, tblPurchaseEnquiry.globalRatePurchaseId, tblRateBandDeclarationPurchase.rate_band_costing, " +
                " tblPurchaseEnquiry.bookingQty, dimState.stateName, tblAddress.areaName, tblPurchaseEnquiry.prodClassId, tblAddress.stateId, " +
                " tblPurchaseScheduleSummary.*, vehDimState.stateName AS vehicleStateName, dimVehicleType.vehicleTypeDesc, " +
                " tbluser1.userDisplayName as purchaseManager" +
                " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                " INNER JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId " +
                " INNER JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry=tblPurchaseScheduleSummary.purchaseEnquiryId " +
                " INNER JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass=tblPurchaseEnquiry.prodClassId " +
                " INNER JOIN tblRateBandDeclarationPurchase tblRateBandDeclarationPurchase " +
                " ON tblRateBandDeclarationPurchase.idRateBandDeclarationPurchase=tblPurchaseEnquiry.rateBandDeclarationPurchaseId " +
                " INNER JOIN tblAddress tblAddress ON tblAddress.idAddr=tblOrganization.addrId " +
                " INNER JOIN dimState dimState ON dimState.idState=tblAddress.stateId " +
                " INNER JOIN dimStatus dimStatus ON dimStatus.idStatus= tblPurchaseScheduleSummary.statusId " +
                // " LEFT JOIN tblLocation tblLocation ON tblLocation.idLocation= tblPurchaseScheduleSummary.locationId " +
                " LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseScheduleSummary.supervisorId " +
                " LEFT JOIN dimState vehDimState ON vehDimState.idState = tblPurchaseScheduleSummary.vehicleStateId " +
                " LEFT JOIN dimVehicleType ON dimVehicleType.idVehicleType = tblPurchaseScheduleSummary.vehicleTypeId" +
                //Priyanka [28-01-2019]
                " LEFT JOIN tblUser tbluser1 ON tbluser1.iduser = tblPurchaseEnquiry.userId ";

            return sqlSelectQry;
        }

        public String SelectQuery()
        {
            //tblLocation.locationDesc,
            String sqlSelectQry = " select * from (Select tblOrganization.firmName,orgSaudaSupplier.firmName AS saudaSupplierName,countComplete.unloadingCompCnt,countComplete.wtStageCompCnt,countComplete.gradingCompCnt,countComplete.recoveryCompCnt,tblUserphoto.userDisplayName as photographer,dimVehiclePhase.sequanceNo,dimVehiclePhase.phaseName, tblUser.userDisplayName,dimStatus.statusName,dimStatus.colorCode, dimStatusTemp.idStatus as prevStatusId,dimStatusTemp.statusName as previousStatusName, tblPurchaseEnquiry.isAutoSpotVehSauda,tblPurchaseEnquiry.bookingRate, tblProdClassification.displayName,tblProdClassification.prodClassDesc, tblPurchaseEnquiry.globalRatePurchaseId, tblRateBandDeclarationPurchase.rate_band_costing, " +
                                                            " tblPurchaseEnquiry.saudaTypeId,tblPurchaseEnquiry.bookingQty,tblPurchaseEnquiry.refRateofV48Var as refRateForSaudaNC,tblPurchaseEnquiry.refRateC as refRateForSaudaC,tblPurchaseEnquiry.rateForC,tblPurchaseEnquiry.rateForNC,dimMasterValue.masterValueName as narration,tblPurchaseEnquiry.isConvertToSauda, dimState.stateName, tblAddress.areaName, tblPurchaseEnquiry.prodClassId, tblAddress.stateId, tblPurchaseEnquiry.remark as saudaRemark," +
                                                            "tblPurchaseScheduleStatusHistory.acceptStatusId," +
                                                            "tblPurchaseScheduleStatusHistory.acceptPhaseId,tblPurchaseScheduleStatusHistory.rejectPhaseId ,tblPurchaseScheduleStatusHistory.rejectStatusId ," +
                                                            "tblPurchaseScheduleStatusHistory.isIgnoreApproval,tblPurchaseScheduleStatusHistory.isLatest,tblPurchaseScheduleStatusHistory.isApproved,tblPurchaseScheduleStatusHistory.idScheduleAuthHistory as scheduleHistoryId,tblPurchaseScheduleStatusHistory.navigationUrl,tblPurchaseScheduleStatusHistory.isActive as historyIsActive,tblPurchaseScheduleStatusHistory.statusRemark,tblPurchaseScheduleStatusHistory.phaseId as historyPhaseId ," +
                                                            " tblPurchaseScheduleSummary.*,dimStatus.statusDesc, vehDimState.stateName AS vehicleStateName, dimVehicleType.vehicleTypeDesc,dimVehicleType.targetPadta,tblPurchaseEnquiry.userId,tblPurchaseEnquiry.enqDisplayNo,   " +
                                                            " tbluser1.userDisplayName as purchaseManager,tblUserForGrader.userDisplayName as GreaderName,tbluserForRejectedBy.userDisplayName as rejectedByUserName, tblUserForRecovery.userDisplayName as EngineerName,tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP,userCreated.userDisplayName as createdByName,userUpdated.userDisplayName as updatedByName, ISNULL(tblPurchaseVehFreightDtls.amount,0) as freightAmount,linkSaudaNo,saudaNarration " +
                                                            //" ,ISNULL(gradingCompleSch.updatedOn ,gradingCompleSch.createdOn ) as 'GradingComplOn', " + //Reshma[01-03-21] Added For gradimg details
                                                            " ,ISNULL(tblPurchaseScheduleSummary.gradingDate ,tblPurchaseScheduleSummary.gradingDate  ) as 'GradingComplOn', " +
                                                            " correctionCompleSch.unldDatePadtaPerTon AS correcUnldDatePadtaForOrder,correctionCompleSch.unldDatePadtaPerTonForNC AS correcUnldDatePadtaForEnquiry " +
                                                            " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                                            " LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId " +
                                                            " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry=tblPurchaseScheduleSummary.purchaseEnquiryId " +
                                                            " LEFT JOIN tblOrganization orgSaudaSupplier ON orgSaudaSupplier.idOrganization=tblPurchaseEnquiry.supplierId " +
                                                            " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass=tblPurchaseEnquiry.prodClassId " +
                                                            " LEFT JOIN tblRateBandDeclarationPurchase tblRateBandDeclarationPurchase " +
                                                            " ON tblRateBandDeclarationPurchase.idRateBandDeclarationPurchase=tblPurchaseEnquiry.rateBandDeclarationPurchaseId " +
                                                            " LEFT JOIN tblAddress tblAddress ON tblAddress.idAddr=tblOrganization.addrId  " +
                                                            " LEFT JOIN dimState dimState ON dimState.idState=tblAddress.stateId " +
                                                            " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus= tblPurchaseScheduleSummary.statusId " +
                                                            " LEFT JOIN dimStatus dimStatusTemp  on dimStatus.idStatus=dimStatusTemp.prevStatusId " +
                                                            "LEFT JOIN" +
                                                            "         (" +
                                                            "                   SELECT tblPurchaseScheduleStatusHistory.acceptStatusId,tblPurchaseScheduleStatusHistory.acceptPhaseId,tblPurchaseScheduleStatusHistory.rejectPhaseId ,tblPurchaseScheduleStatusHistory.rejectStatusId, " +
                                                            "                          tblPurchaseScheduleStatusHistory.isIgnoreApproval,tblPurchaseScheduleStatusHistory.isLatest,tblPurchaseScheduleStatusHistory.isApproved,tblPurchaseScheduleStatusHistory.idScheduleAuthHistory," +
                                                            "                          tblPurchaseScheduleStatusHistory.navigationUrl,tblPurchaseScheduleStatusHistory.isActive,tblPurchaseScheduleStatusHistory.statusRemark,tblPurchaseScheduleStatusHistory.phaseId,tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId" +
                                                            "                   FROM tblPurchaseScheduleStatusHistory  " +
                                                            "                   WHERE isLatest=1" +
                                                            "         ) AS tblPurchaseScheduleStatusHistory" +
                                                            "  ON tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId" +
                                                            //" LEFT JOIN tblPurchaseScheduleStatusHistory ON tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId and tblPurchaseScheduleStatusHistory.isLatest=1" +
                                                            " LEFT JOIN dimState vehDimState ON vehDimState.idState = tblPurchaseScheduleSummary.vehicleStateId " +
                                                            " LEFT JOIN tblPurchaseVehFreightDtls tblPurchaseVehFreightDtls ON tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            // " LEFT JOIN tblLocation tblLocation ON tblLocation.idLocation= tblPurchaseScheduleSummary.locationId " +
                                                            "  LEFT JOIN tblUser tblUserphoto ON tblUserphoto.idUser = tblPurchaseScheduleSummary.photographerId" +
                                                            " LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseScheduleSummary.supervisorId " +
                                                            " LEFT JOIN tblUser tblUserForGrader ON tblUserForGrader.idUser = tblPurchaseScheduleSummary.graderId " +
                                                            " LEFT JOIN tblUser tblUserForRecovery ON tblUserForRecovery.idUser = tblPurchaseScheduleSummary.engineerId " +
                                                            " LEFT JOIN dimMasterValue dimMasterValue ON tblPurchaseScheduleSummary.narrationId = dimMasterValue.idMasterValue " +
                                                            " LEFT JOIN dimVehiclePhase dimVehiclePhase ON dimVehiclePhase.idVehiclePhase = tblPurchaseScheduleSummary.vehiclePhaseId " +
                                                              " LEFT JOIN dimVehicleType ON dimVehicleType.idVehicleType = tblPurchaseScheduleSummary.vehicleTypeId" +
                                                            //Priyanka [28-01-2019]
                                                            " LEFT JOIN tblUser tbluser1 ON tbluser1.iduser = tblPurchaseEnquiry.userId  " +
                                                            " LEFT JOIN tblUser tbluserForRejectedBy ON tbluserForRejectedBy.iduser = tblPurchaseScheduleSummary.rejectedBy  " + //Prajakta[2019-05-09]Added
                                                            " LEFT JOIN tblGate tblGate ON tblGate.idGate = tblPurchaseScheduleSummary.gateId" +
                                                            " LEFT JOIN tblUser userCreated ON userCreated.idUser = tblPurchaseScheduleSummary.createdBy " +
                                                            " LEFT JOIN tblUser userUpdated ON userUpdated.idUser = tblPurchaseScheduleSummary.updatedBy " +
                                                            //Deepali [20-03-2019]
                                                            " left join tblPurchaseVehicleStageCnt countComplete on tblPurchaseScheduleSummary.rootScheduleId = countComplete.rootScheduleId  " +
                                                            " LEFT JOIN ( " +
                                                            " SELECT tblPurchaseScheduleSummary.rootScheduleId,STUFF((SELECT '; ' + tblPurchaseEnquiry.enqDisplayNo  " +
                                                            " FROM tblPurchaseVehLinkSauda tblPurchaseVehLinkSauda  "+
                                                            " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry= tblPurchaseVehLinkSauda.purchaseEnquiryId  " +
                                                            " where tblPurchaseVehLinkSauda.isActive = 1 AND tblPurchaseVehLinkSauda.rootScheduleId = tblPurchaseScheduleSummary.rootScheduleId " +
                                                            " FOR XML PATH('')), 1, 1, '')[linkSaudaNo] " +
                                                            " FROM tblPurchaseScheduleSummary " +
                                                            " GROUP BY tblPurchaseScheduleSummary.rootScheduleId) AS tblLinkVehSauda "+
                                                            " ON tblLinkVehSauda.rootScheduleId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            
                                                            " LEFT JOIN(SELECT tblPurchaseScheduleSummary.rootScheduleId, " +
                                                            " STUFF((SELECT '; ' + tblPurchaseEnquiry.remark FROM tblPurchaseVehLinkSauda tblPurchaseVehLinkSauda " +
                                                            " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseVehLinkSauda.purchaseEnquiryId   where tblPurchaseVehLinkSauda.isActive = 1 " +
                                                            " AND tblPurchaseVehLinkSauda.rootScheduleId = tblPurchaseScheduleSummary.rootScheduleId " +
                                                            " FOR XML PATH('')), 1, 1, '')[saudaNarration] " +
                                                            " FROM tblPurchaseScheduleSummary " +
                                                            " GROUP BY tblPurchaseScheduleSummary.rootScheduleId) AS tblSaudaMultiNarration  ON tblSaudaMultiNarration.rootScheduleId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId, tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +

                                                            //Reshma[03-03-21] Added FOr Grading Details
            " LEFT JOIN( select gradingCompleSch.* from tblPurchaseScheduleSummary gradingCompleSch " +
                                                            " where gradingCompleSch.isGradingCompleted = 1 and gradingCompleSch.vehiclePhaseId = " + (int)Constants.PurchaseVehiclePhasesE.GRADING + " and gradingCompleSch.statusId = " + (int)Constants.TranStatusE.UNLOADING_COMPLETED +
                                                            " ) AS gradingCompleSch on ISNULL(gradingCompleSch.rootScheduleId, gradingCompleSch.idPurchaseScheduleSummary) = isnull(tblPurchaseScheduleSummary.rootScheduleId, tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            " AND gradingCompleSch.cOrNCId = tblPurchaseScheduleSummary.cOrNCId " +
                                                            " LEFT JOIN( select correctionCompleSch.* from tblPurchaseScheduleSummary correctionCompleSch " +
                                                            " where correctionCompleSch.isCorrectionCompleted = 1 and correctionCompleSch.vehiclePhaseId = " + (int)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " and correctionCompleSch.statusId = " + (int)Constants.TranStatusE.UNLOADING_COMPLETED +
                                                            " ) AS correctionCompleSch on ISNULL(correctionCompleSch.rootScheduleId, correctionCompleSch.idPurchaseScheduleSummary) = isnull(tblPurchaseScheduleSummary.rootScheduleId, tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            " AND correctionCompleSch.cOrNCId = tblPurchaseScheduleSummary.cOrNCId " +
            " ) SRC ";
            //  WHERE  SRC.statusId in (" + statusId + ")" +
            //  " AND RN = 1 ";

            return sqlSelectQry;

        }

        public String SelectQueryForEnqVehDisplay()
        {
            //tblLocation.locationDesc,
            String sqlSelectQry = " select * from (Select tblOrganization.firmName,countComplete.unloadingCompCnt,countComplete.wtStageCompCnt,countComplete.gradingCompCnt,countComplete.recoveryCompCnt,tblUserphoto.userDisplayName as photographer,dimVehiclePhase.sequanceNo,dimVehiclePhase.phaseName, tblUser.userDisplayName,dimStatus.statusName,dimStatus.colorCode, dimStatusTemp.idStatus as prevStatusId,dimStatusTemp.statusName as previousStatusName, tblPurchaseEnquiry.isAutoSpotVehSauda,tblPurchaseEnquiry.bookingRate, tblProdClassification.displayName,tblProdClassification.prodClassDesc, tblPurchaseEnquiry.globalRatePurchaseId, tblRateBandDeclarationPurchase.rate_band_costing, " +
                                                            " tblPurchaseEnquiry.saudaTypeId,tblPurchaseEnquiry.bookingQty,tblPurchaseEnquiry.refRateofV48Var as refRateForSaudaNC,tblPurchaseEnquiry.refRateC as refRateForSaudaC,tblPurchaseEnquiry.rateForC,tblPurchaseEnquiry.rateForNC,dimMasterValue.masterValueName as narration,tblPurchaseEnquiry.isConvertToSauda, dimState.stateName, tblAddress.areaName, tblPurchaseEnquiry.prodClassId, tblAddress.stateId, " +
                                                            "tblPurchaseScheduleStatusHistory.acceptStatusId," +
                                                            "tblPurchaseScheduleStatusHistory.acceptPhaseId,tblPurchaseScheduleStatusHistory.rejectPhaseId ,tblPurchaseScheduleStatusHistory.rejectStatusId ," +
                                                            "tblPurchaseScheduleStatusHistory.isIgnoreApproval,tblPurchaseScheduleStatusHistory.isLatest,tblPurchaseScheduleStatusHistory.isApproved,tblPurchaseScheduleStatusHistory.idScheduleAuthHistory as scheduleHistoryId,tblPurchaseScheduleStatusHistory.navigationUrl,tblPurchaseScheduleStatusHistory.isActive as historyIsActive,tblPurchaseScheduleStatusHistory.statusRemark,tblPurchaseScheduleStatusHistory.phaseId as historyPhaseId ," +
                                                            " tblPurchaseScheduleSummary.*,dimStatus.statusDesc, vehDimState.stateName AS vehicleStateName, dimVehicleType.vehicleTypeDesc,dimVehicleType.targetPadta,tblPurchaseEnquiry.userId,tblPurchaseEnquiry.enqDisplayNo,   " +
                                                            " tbluser1.userDisplayName as purchaseManager,tblUserForGrader.userDisplayName as GreaderName,tbluserForRejectedBy.userDisplayName as rejectedByUserName, tblUserForRecovery.userDisplayName as EngineerName,tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP,userCreated.userDisplayName as createdByName,userUpdated.userDisplayName as updatedByName, ISNULL(tblPurchaseVehFreightDtls.amount,0) as freightAmount,linkSaudaNo " +
                                                            //" ,ISNULL(gradingCompleSch.updatedOn ,gradingCompleSch.createdOn ) as 'GradingComplOn', " + //Reshma[01-03-21] Added For gradimg details
                                                            " , ISNULL(tblPurchaseScheduleSummary.gradingDate, tblPurchaseScheduleSummary.gradingDate) as 'GradingComplOn', " + 
                                                            " correctionCompleSch.unldDatePadtaPerTon AS correcUnldDatePadtaForOrder,correctionCompleSch.unldDatePadtaPerTonForNC AS correcUnldDatePadtaForEnquiry " +
                                                            " ,tblPurchaseVehLinkSauda.purchaseEnquiryId as purLinkVehEnquiryId " +
                                                            " ,tblPurchaseVehLinkSauda.linkedQty " +
                                                            " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                                            " LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId " +
                                                            " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry=tblPurchaseScheduleSummary.purchaseEnquiryId " +
                                                            //" LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseEnquiry.supplierId " +
                                                            " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass=tblPurchaseEnquiry.prodClassId " +
                                                            " LEFT JOIN tblRateBandDeclarationPurchase tblRateBandDeclarationPurchase " +
                                                            " ON tblRateBandDeclarationPurchase.idRateBandDeclarationPurchase=tblPurchaseEnquiry.rateBandDeclarationPurchaseId " +
                                                            " LEFT JOIN tblAddress tblAddress ON tblAddress.idAddr=tblOrganization.addrId  " +
                                                            " LEFT JOIN dimState dimState ON dimState.idState=tblAddress.stateId " +
                                                            " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus= tblPurchaseScheduleSummary.statusId " +
                                                            " LEFT JOIN dimStatus dimStatusTemp  on dimStatus.idStatus=dimStatusTemp.prevStatusId " +
                                                            "LEFT JOIN" +
                                                            "         (" +
                                                            "                   SELECT tblPurchaseScheduleStatusHistory.acceptStatusId,tblPurchaseScheduleStatusHistory.acceptPhaseId,tblPurchaseScheduleStatusHistory.rejectPhaseId ,tblPurchaseScheduleStatusHistory.rejectStatusId, " +
                                                            "                          tblPurchaseScheduleStatusHistory.isIgnoreApproval,tblPurchaseScheduleStatusHistory.isLatest,tblPurchaseScheduleStatusHistory.isApproved,tblPurchaseScheduleStatusHistory.idScheduleAuthHistory," +
                                                            "                          tblPurchaseScheduleStatusHistory.navigationUrl,tblPurchaseScheduleStatusHistory.isActive,tblPurchaseScheduleStatusHistory.statusRemark,tblPurchaseScheduleStatusHistory.phaseId,tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId" +
                                                            "                   FROM tblPurchaseScheduleStatusHistory  " +
                                                            "                   WHERE isLatest=1" +
                                                            "         ) AS tblPurchaseScheduleStatusHistory" +
                                                            "  ON tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId" +
                                                            //" LEFT JOIN tblPurchaseScheduleStatusHistory ON tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId and tblPurchaseScheduleStatusHistory.isLatest=1" +
                                                            " LEFT JOIN dimState vehDimState ON vehDimState.idState = tblPurchaseScheduleSummary.vehicleStateId " +
                                                            " LEFT JOIN tblPurchaseVehFreightDtls tblPurchaseVehFreightDtls ON tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            // " LEFT JOIN tblLocation tblLocation ON tblLocation.idLocation= tblPurchaseScheduleSummary.locationId " +
                                                            "  LEFT JOIN tblUser tblUserphoto ON tblUserphoto.idUser = tblPurchaseScheduleSummary.photographerId" +
                                                            " LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseScheduleSummary.supervisorId " +
                                                            " LEFT JOIN tblUser tblUserForGrader ON tblUserForGrader.idUser = tblPurchaseScheduleSummary.graderId " +
                                                            " LEFT JOIN tblUser tblUserForRecovery ON tblUserForRecovery.idUser = tblPurchaseScheduleSummary.engineerId " +
                                                            " LEFT JOIN dimMasterValue dimMasterValue ON tblPurchaseScheduleSummary.narrationId = dimMasterValue.idMasterValue " +
                                                            " LEFT JOIN dimVehiclePhase dimVehiclePhase ON dimVehiclePhase.idVehiclePhase = tblPurchaseScheduleSummary.vehiclePhaseId " +
                                                              " LEFT JOIN dimVehicleType ON dimVehicleType.idVehicleType = tblPurchaseScheduleSummary.vehicleTypeId" +
                                                            //Priyanka [28-01-2019]
                                                            " LEFT JOIN tblUser tbluser1 ON tbluser1.iduser = tblPurchaseEnquiry.userId  " +
                                                            " LEFT JOIN tblUser tbluserForRejectedBy ON tbluserForRejectedBy.iduser = tblPurchaseScheduleSummary.rejectedBy  " + //Prajakta[2019-05-09]Added
                                                            " LEFT JOIN tblGate tblGate ON tblGate.idGate = tblPurchaseScheduleSummary.gateId" +
                                                            " LEFT JOIN tblUser userCreated ON userCreated.idUser = tblPurchaseScheduleSummary.createdBy " +
                                                            " LEFT JOIN tblUser userUpdated ON userUpdated.idUser = tblPurchaseScheduleSummary.updatedBy " +
                                                            //Deepali [20-03-2019]
                                                            " left join tblPurchaseVehicleStageCnt countComplete on tblPurchaseScheduleSummary.rootScheduleId = countComplete.rootScheduleId  " +
                                                            //Prajakta[2021-05-13] Added
                                                            " left join tblPurchaseVehLinkSauda on tblPurchaseVehLinkSauda.rootScheduleId = tblPurchaseScheduleSummary.rootScheduleId " +
                                                            " LEFT JOIN ( " +
                                                            " SELECT tblPurchaseScheduleSummary.rootScheduleId,STUFF((SELECT '; ' + tblPurchaseEnquiry.enqDisplayNo  " +
                                                            " FROM tblPurchaseVehLinkSauda tblPurchaseVehLinkSauda  " +
                                                            " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry= tblPurchaseVehLinkSauda.purchaseEnquiryId  " +
                                                            " where tblPurchaseVehLinkSauda.isActive = 1 AND tblPurchaseVehLinkSauda.rootScheduleId = tblPurchaseScheduleSummary.rootScheduleId " +
                                                            " FOR XML PATH('')), 1, 1, '')[linkSaudaNo] " +
                                                            " FROM tblPurchaseScheduleSummary " +
                                                            " GROUP BY tblPurchaseScheduleSummary.rootScheduleId) AS tblLinkVehSauda " +
                                                            " ON tblLinkVehSauda.rootScheduleId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +

                                                            //" LEFT JOIN(SELECT tblPurchaseScheduleSummary.rootScheduleId, " +
                                                            //" STUFF((SELECT '; ' + tblPurchaseEnquiry.remark FROM tblPurchaseVehLinkSauda tblPurchaseVehLinkSauda " +
                                                            //" LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseVehLinkSauda.purchaseEnquiryId   where tblPurchaseVehLinkSauda.isActive = 1 " +
                                                            //" AND tblPurchaseVehLinkSauda.rootScheduleId = tblPurchaseScheduleSummary.rootScheduleId " +
                                                            //" FOR XML PATH('')), 1, 1, '')[saudaNarration] " +
                                                            //" FROM tblPurchaseScheduleSummary " +
                                                            //" GROUP BY tblPurchaseScheduleSummary.rootScheduleId) AS tblSaudaMultiNarration  ON tblSaudaMultiNarration.rootScheduleId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId, tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +

                                                            //Reshma[03-03-21] Added FOr Grading Details
                                                            " LEFT JOIN( select gradingCompleSch.* from tblPurchaseScheduleSummary gradingCompleSch " +
                                                            " where gradingCompleSch.isGradingCompleted = 1 and gradingCompleSch.vehiclePhaseId = " + (int)Constants.PurchaseVehiclePhasesE.GRADING + " and gradingCompleSch.statusId = " + (int)Constants.TranStatusE.UNLOADING_COMPLETED +
                                                            " ) AS gradingCompleSch on ISNULL(gradingCompleSch.rootScheduleId, gradingCompleSch.idPurchaseScheduleSummary) = isnull(tblPurchaseScheduleSummary.rootScheduleId, tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            " AND gradingCompleSch.cOrNCId = tblPurchaseScheduleSummary.cOrNCId " +
                                                            " LEFT JOIN( select correctionCompleSch.* from tblPurchaseScheduleSummary correctionCompleSch " +
                                                            " where correctionCompleSch.isCorrectionCompleted = 1 and correctionCompleSch.vehiclePhaseId = " + (int)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " and correctionCompleSch.statusId = " + (int)Constants.TranStatusE.UNLOADING_COMPLETED +
                                                            " ) AS correctionCompleSch on ISNULL(correctionCompleSch.rootScheduleId, correctionCompleSch.idPurchaseScheduleSummary) = isnull(tblPurchaseScheduleSummary.rootScheduleId, tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            " AND correctionCompleSch.cOrNCId = tblPurchaseScheduleSummary.cOrNCId " +
            " ) SRC ";
            //  WHERE  SRC.statusId in (" + statusId + ")" +
            //  " AND RN = 1 ";

            return sqlSelectQry;

        }

        public String SelectQueryForMasterReport()
        {
            //tblLocation.locationDesc,
            String sqlSelectQry = " select * from (Select CASE WHEN tblPurchaseInvoiceAddr.billingPartyName IS NOT NULL THEN tblPurchaseInvoiceAddr.billingPartyName ELSE tblOrganization.firmName END AS firmName " +
                                                            ",countComplete.unloadingCompCnt,countComplete.wtStageCompCnt,countComplete.gradingCompCnt,countComplete.recoveryCompCnt,tblUserphoto.userDisplayName as photographer,dimVehiclePhase.sequanceNo,dimVehiclePhase.phaseName, tblUser.userDisplayName,dimStatus.statusName,dimStatus.colorCode, dimStatusTemp.idStatus as prevStatusId,dimStatusTemp.statusName as previousStatusName, tblPurchaseEnquiry.isAutoSpotVehSauda,tblPurchaseEnquiry.bookingRate, tblProdClassification.displayName,tblProdClassification.prodClassDesc, tblPurchaseEnquiry.globalRatePurchaseId, tblRateBandDeclarationPurchase.rate_band_costing, " +
                                                            " tblPurchaseEnquiry.bookingQty,tblPurchaseEnquiry.refRateofV48Var as refRateForSaudaNC,tblPurchaseEnquiry.refRateC as refRateForSaudaC,tblPurchaseEnquiry.rateForC,tblPurchaseEnquiry.rateForNC,dimMasterValue.masterValueName as narration,tblPurchaseEnquiry.isConvertToSauda, dimState.stateName, tblAddress.areaName, tblPurchaseEnquiry.prodClassId, tblAddress.stateId," +
                                                            "tblPurchaseScheduleStatusHistory.acceptStatusId," +
                                                            "tblPurchaseScheduleStatusHistory.acceptPhaseId,tblPurchaseScheduleStatusHistory.rejectPhaseId ,tblPurchaseScheduleStatusHistory.rejectStatusId ," +
                                                            "tblPurchaseScheduleStatusHistory.isIgnoreApproval,tblPurchaseScheduleStatusHistory.isLatest,tblPurchaseScheduleStatusHistory.isApproved,tblPurchaseScheduleStatusHistory.idScheduleAuthHistory as scheduleHistoryId,tblPurchaseScheduleStatusHistory.navigationUrl,tblPurchaseScheduleStatusHistory.isActive as historyIsActive,tblPurchaseScheduleStatusHistory.statusRemark,tblPurchaseScheduleStatusHistory.phaseId as historyPhaseId ," +
                                                            " tblPurchaseScheduleSummary.*,dimStatus.statusDesc, vehDimState.stateName AS vehicleStateName, dimVehicleType.vehicleTypeDesc,dimVehicleType.targetPadta,tblPurchaseEnquiry.userId,tblPurchaseEnquiry.enqDisplayNo,   " +
                                                            " tbluser1.userDisplayName as purchaseManager,tblUserForGrader.userDisplayName as GreaderName,tbluserForRejectedBy.userDisplayName as rejectedByUserName, tblUserForRecovery.userDisplayName as EngineerName,tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP,userCreated.userDisplayName as createdByName,userUpdated.userDisplayName as updatedByName,ISNULL(tblPurchaseVehFreightDtls.amount,0) as freightAmount" +
                                                            //" ,ISNULL(gradingCompleSch.updatedOn ,gradingCompleSch.createdOn ) as 'GradingComplOn' " + //Reshma[01-03-21] Added For gradimg details
                                                            //" correctionCompleSch.unldDatePadtaPerTon AS correcUnldDatePadtaForOrder,correctionCompleSch.unldDatePadtaPerTonForNC AS correcUnldDatePadtaForEnquiry " +
                                                            " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                                            " LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId " +
                                                            " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry=tblPurchaseScheduleSummary.purchaseEnquiryId " +
                                                            //" LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseEnquiry.supplierId " +
                                                            " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass=tblPurchaseEnquiry.prodClassId " +
                                                            " LEFT JOIN tblRateBandDeclarationPurchase tblRateBandDeclarationPurchase " +
                                                            " ON tblRateBandDeclarationPurchase.idRateBandDeclarationPurchase=tblPurchaseEnquiry.rateBandDeclarationPurchaseId " +
                                                            " LEFT JOIN tblAddress tblAddress ON tblAddress.idAddr=tblOrganization.addrId  " +
                                                            " LEFT JOIN dimState dimState ON dimState.idState=tblAddress.stateId " +
                                                            " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus= tblPurchaseScheduleSummary.statusId " +
                                                            " LEFT JOIN dimStatus dimStatusTemp  on dimStatus.idStatus=dimStatusTemp.prevStatusId " +
                                                            "LEFT JOIN" +
                                                            "         (" +
                                                            "                   SELECT tblPurchaseScheduleStatusHistory.acceptStatusId,tblPurchaseScheduleStatusHistory.acceptPhaseId,tblPurchaseScheduleStatusHistory.rejectPhaseId ,tblPurchaseScheduleStatusHistory.rejectStatusId, " +
                                                            "                          tblPurchaseScheduleStatusHistory.isIgnoreApproval,tblPurchaseScheduleStatusHistory.isLatest,tblPurchaseScheduleStatusHistory.isApproved,tblPurchaseScheduleStatusHistory.idScheduleAuthHistory," +
                                                            "                          tblPurchaseScheduleStatusHistory.navigationUrl,tblPurchaseScheduleStatusHistory.isActive,tblPurchaseScheduleStatusHistory.statusRemark,tblPurchaseScheduleStatusHistory.phaseId,tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId" +
                                                            "                   FROM tblPurchaseScheduleStatusHistory  " +
                                                            "                   WHERE isLatest=1" +
                                                            "         ) AS tblPurchaseScheduleStatusHistory" +
                                                            "  ON tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId" +
                                                            //" LEFT JOIN tblPurchaseScheduleStatusHistory ON tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId and tblPurchaseScheduleStatusHistory.isLatest=1" +
                                                            " LEFT JOIN dimState vehDimState ON vehDimState.idState = tblPurchaseScheduleSummary.vehicleStateId " +
                                                            " LEFT JOIN tblPurchaseVehFreightDtls tblPurchaseVehFreightDtls ON tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            // " LEFT JOIN tblLocation tblLocation ON tblLocation.idLocation= tblPurchaseScheduleSummary.locationId " +
                                                            "  LEFT JOIN tblUser tblUserphoto ON tblUserphoto.idUser = tblPurchaseScheduleSummary.photographerId" +
                                                            " LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseScheduleSummary.supervisorId " +
                                                            " LEFT JOIN tblUser tblUserForGrader ON tblUserForGrader.idUser = tblPurchaseScheduleSummary.graderId " +
                                                            " LEFT JOIN tblUser tblUserForRecovery ON tblUserForRecovery.idUser = tblPurchaseScheduleSummary.engineerId " +
                                                            " LEFT JOIN dimMasterValue dimMasterValue ON tblPurchaseScheduleSummary.narrationId = dimMasterValue.idMasterValue " +
                                                            " LEFT JOIN dimVehiclePhase dimVehiclePhase ON dimVehiclePhase.idVehiclePhase = tblPurchaseScheduleSummary.vehiclePhaseId " +
                                                              " LEFT JOIN dimVehicleType ON dimVehicleType.idVehicleType = tblPurchaseScheduleSummary.vehicleTypeId" +
                                                            //Priyanka [28-01-2019]
                                                            " LEFT JOIN tblUser tbluser1 ON tbluser1.iduser = tblPurchaseEnquiry.userId  " +
                                                            " LEFT JOIN tblUser tbluserForRejectedBy ON tbluserForRejectedBy.iduser = tblPurchaseScheduleSummary.rejectedBy  " + //Prajakta[2019-05-09]Added
                                                            " LEFT JOIN tblGate tblGate ON tblGate.idGate = tblPurchaseScheduleSummary.gateId" +
                                                            " LEFT JOIN tblUser userCreated ON userCreated.idUser = tblPurchaseScheduleSummary.createdBy " +
                                                            " LEFT JOIN tblUser userUpdated ON userUpdated.idUser = tblPurchaseScheduleSummary.updatedBy " +
                                                            //Deepali [20-03-2019]
                                                            " left join tblPurchaseVehicleStageCnt countComplete on tblPurchaseScheduleSummary.rootScheduleId = countComplete.rootScheduleId  " +
                                                            " LEFT JOIN tblPurchaseInvoice tblPurchaseInvoice ON tblPurchaseInvoice.purSchSummaryId = ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            " LEFT JOIN tblPurchaseInvoiceAddr tblPurchaseInvoiceAddr ON tblPurchaseInvoiceAddr.purchaseInvoiceId = tblPurchaseInvoice.idInvoicePurchase " +
                                                            //Reshma[03-03-21] Added FOr Grading Details
                                                            //" LEFT JOIN( select gradingCompleSch.* from tblPurchaseScheduleSummary gradingCompleSch " +
                                                            //" where gradingCompleSch.isGradingCompleted = 1 and gradingCompleSch.vehiclePhaseId = " + (int)Constants.PurchaseVehiclePhasesE.GRADING + " and gradingCompleSch.statusId = " + (int)Constants.TranStatusE.UNLOADING_COMPLETED +
                                                            //" ) AS gradingCompleSch on ISNULL(gradingCompleSch.rootScheduleId, gradingCompleSch.idPurchaseScheduleSummary) = isnull(tblPurchaseScheduleSummary.rootScheduleId, tblPurchaseScheduleSummary.idPurchaseScheduleSummary) "+
                                                            ////" LEFT JOIN( select correctionCompleSch.* from tblPurchaseScheduleSummary correctionCompleSch " +
                                                            //" where correctionCompleSch.isCorrectionCompleted = 1 and correctionCompleSch.vehiclePhaseId = " + (int)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " and correctionCompleSch.statusId = " + (int)Constants.TranStatusE.UNLOADING_COMPLETED +
                                                            //" ) AS correctionCompleSch on ISNULL(correctionCompleSch.rootScheduleId, correctionCompleSch.idPurchaseScheduleSummary) = isnull(tblPurchaseScheduleSummary.rootScheduleId, tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                                            " ) SRC ";
            //  WHERE  SRC.statusId in (" + statusId + ")" +
            //  " AND RN = 1 ";

            return sqlSelectQry;

        }


        public String SelectQueryForPendingQualityFlag()
        {
            //tblLocation.locationDesc,
            String sqlSelectQry = " Select distinct tblPurchaseScheduleSummary.rootScheduleId, tblOrganization.firmName,dimVehiclePhase.sequanceNo,dimVehiclePhase.phaseName, tblUser.userDisplayName,dimStatus.statusDesc,dimStatus.statusName,dimStatus.colorCode,dimStatus.prevStatusId,dimStatusTemp.statusName as previousStatusName, tblPurchaseEnquiry.bookingRate, tblProdClassification.displayName,tblProdClassification.prodClassDesc, tblPurchaseEnquiry.globalRatePurchaseId, tblRateBandDeclarationPurchase.rate_band_costing, " +
                                                            " tblPurchaseEnquiry.bookingQty, dimState.stateName, tblAddress.areaName, tblPurchaseEnquiry.prodClassId, tblAddress.stateId," +
                                                            " tblPurchaseScheduleSummary.*, vehDimState.stateName AS vehicleStateName, dimVehicleType.vehicleTypeDesc,tblPurchaseEnquiry.UserId,tblPurchaseEnquiry.enqDisplayNo,tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP ," +
                                                            " tbluser1.userDisplayName as purchaseManager " +
                                                            // " ROW_NUMBER() over(partition by purchaseEnquiryId, vehicleNo, CAST(scheduleDate AS DATE)  order by idPurchaseScheduleSummary desc) AS RN " +
                                                            " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                                            " LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId " +
                                                            " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry=tblPurchaseScheduleSummary.purchaseEnquiryId " +
                                                            " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass=tblPurchaseEnquiry.prodClassId " +
                                                            " LEFT JOIN tblRateBandDeclarationPurchase tblRateBandDeclarationPurchase " +
                                                            " ON tblRateBandDeclarationPurchase.idRateBandDeclarationPurchase=tblPurchaseEnquiry.rateBandDeclarationPurchaseId " +
                                                            " LEFT JOIN tblAddress tblAddress ON tblAddress.idAddr=tblOrganization.addrId  " +
                                                            " LEFT JOIN dimState dimState ON dimState.idState=tblAddress.stateId " +
                                                            " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus= tblPurchaseScheduleSummary.statusId " +
                                                            " LEFT JOIN dimStatus dimStatusTemp  on dimStatus.idStatus=dimStatusTemp.prevStatusId " +
                                                            " LEFT JOIN dimState vehDimState ON vehDimState.idState = tblPurchaseScheduleSummary.vehicleStateId " +
                                                            // " LEFT JOIN tblLocation tblLocation ON tblLocation.idLocation= tblPurchaseScheduleSummary.locationId " +
                                                            " LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseScheduleSummary.supervisorId" +

                                                            //Priyanka [28-01-2019]
                                                            " LEFT JOIN tblUser tbluser1 ON tbluser1.iduser = tblPurchaseEnquiry.userId " +
                                                            " LEFT JOIN tblGate tblGate ON tblGate.idGate = tblPurchaseScheduleSummary.gateId" +

                                                            " LEFT JOIN dimVehiclePhase dimVehiclePhase ON dimVehiclePhase.idVehiclePhase = tblPurchaseScheduleSummary.vehiclePhaseId " +
                                                             " LEFT JOIN dimVehicleType ON dimVehicleType.idVehicleType = tblPurchaseScheduleSummary.vehicleTypeId " +
                                                            "left join tblQualityPhase tblQualityPhase on tblQualityPhase.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId " +
                                                              "where isnull(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) in" +
                                                              "(select PurchaseScheduleSummaryId from tblQualityPhase where idTblQualityPhase in" +
                                                              "(select tblQualityPhaseId from tblQualityPhaseDtls where isnull(flagStatusId,0)= 0) and isActive =1 )" +
                                                              " and tblPurchaseScheduleSummary.isActive =1";


            return sqlSelectQry;

        }


        public String SelectQueryForApprovals()
        {
            //tblLocation.locationDesc,
            String sqlSelectQry = " select * from (Select tblOrganization.firmName,dimVehiclePhase.sequanceNo,dimVehiclePhase.phaseName, tblUser.userDisplayName, PmUser.userDisplayName AS purchaseManagerName  ,dimStatus.statusDesc,dimStatus.statusName,dimStatus.colorCode,dimStatus.prevStatusId,dimStatusTemp.statusName as previousStatusName, tblPurchaseEnquiry.bookingRate,tblPurchaseEnquiry.wtRateApprovalDiff, tblProdClassification.displayName,tblProdClassification.prodClassDesc, tblPurchaseEnquiry.globalRatePurchaseId, tblRateBandDeclarationPurchase.rate_band_costing, " +
                                                            " tblPurchaseEnquiry.bookingQty,dimMasterValue.masterValueName as narration, dimState.stateName, tblAddress.areaName, tblPurchaseEnquiry.prodClassId, tblAddress.stateId,tblPurchaseScheduleStatusHistory.approvalType,tblPurchaseScheduleStatusHistory.acceptStatusId," +
                                                            "tblPurchaseScheduleStatusHistory.acceptPhaseId,tblPurchaseScheduleStatusHistory.rejectPhaseId ,tblPurchaseScheduleStatusHistory.rejectStatusId ," +
                                                            "tblPurchaseScheduleStatusHistory.isIgnoreApproval,tblPurchaseScheduleStatusHistory.isLatest,tblPurchaseScheduleStatusHistory.isApproved,tblPurchaseScheduleStatusHistory.idScheduleAuthHistory as scheduleHistoryId,tblPurchaseScheduleStatusHistory.navigationUrl,tblPurchaseScheduleStatusHistory.isActive as historyIsActive,tblPurchaseScheduleStatusHistory.statusRemark,tblPurchaseScheduleStatusHistory.phaseId as historyPhaseId ," +
                                                            " tblPurchaseScheduleSummary.*,tblUserForGrader.userDisplayName as GreaderName, tblUserForRecovery.userDisplayName as EngineerName , vehDimState.stateName AS vehicleStateName, dimVehicleType.vehicleTypeDesc,tblPurchaseEnquiry.UserId,tblPurchaseEnquiry.enqDisplayNo   " +
                                                            // " ROW_NUMBER() over(partition by purchaseEnquiryId, vehicleNo, CAST(scheduleDate AS DATE)  order by idPurchaseScheduleSummary desc) AS RN " +
                                                            " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                                            " LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization=tblPurchaseScheduleSummary.supplierId " +
                                                            " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry=tblPurchaseScheduleSummary.purchaseEnquiryId " +
                                                            " LEFT JOIN tbluser PmUser ON PmUser.idUser = tblPurchaseEnquiry.userId " +
                                                            " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass=tblPurchaseEnquiry.prodClassId " +
                                                            " LEFT JOIN tblRateBandDeclarationPurchase tblRateBandDeclarationPurchase " +
                                                            " ON tblRateBandDeclarationPurchase.idRateBandDeclarationPurchase=tblPurchaseEnquiry.rateBandDeclarationPurchaseId " +
                                                            " LEFT JOIN tblAddress tblAddress ON tblAddress.idAddr=tblOrganization.addrId  " +
                                                            " LEFT JOIN dimState dimState ON dimState.idState=tblAddress.stateId " +
                                                            " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus= tblPurchaseScheduleSummary.statusId " +
                                                            " LEFT JOIN dimStatus dimStatusTemp  on dimStatus.idStatus=dimStatusTemp.prevStatusId " +
                                                            " LEFT JOIN tblUser tblUserForRecovery ON tblUserForRecovery.idUser = tblPurchaseScheduleSummary.engineerId " +
                                                            " LEFT JOIN tblUser tblUserForGrader ON tblUserForGrader.idUser = tblPurchaseScheduleSummary.graderId " +
                                                            " LEFT JOIN dimState vehDimState ON vehDimState.idState = tblPurchaseScheduleSummary.vehicleStateId " +
                                                            " LEFT JOIN dimMasterValue dimMasterValue ON tblPurchaseScheduleSummary.narrationId = dimMasterValue.idMasterValue " +

                                                            // " LEFT JOIN tblLocation tblLocation ON tblLocation.idLocation= tblPurchaseScheduleSummary.locationId " +
                                                            " LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseScheduleSummary.supervisorId " +
                                                            " LEFT JOIN dimVehiclePhase dimVehiclePhase ON dimVehiclePhase.idVehiclePhase = tblPurchaseScheduleSummary.vehiclePhaseId " +
                                                             "LEFT JOIN" +
                                                            "         (" +
                                                            "                   SELECT tblPurchaseScheduleStatusHistory.approvalType,tblPurchaseScheduleStatusHistory.acceptStatusId,tblPurchaseScheduleStatusHistory.acceptPhaseId,tblPurchaseScheduleStatusHistory.rejectPhaseId ,tblPurchaseScheduleStatusHistory.rejectStatusId, " +
                                                            "                          tblPurchaseScheduleStatusHistory.isIgnoreApproval,tblPurchaseScheduleStatusHistory.isLatest,tblPurchaseScheduleStatusHistory.isApproved,tblPurchaseScheduleStatusHistory.idScheduleAuthHistory," +
                                                            "                          tblPurchaseScheduleStatusHistory.navigationUrl,tblPurchaseScheduleStatusHistory.isActive,tblPurchaseScheduleStatusHistory.statusRemark,tblPurchaseScheduleStatusHistory.phaseId,tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId" +
                                                            "                   FROM tblPurchaseScheduleStatusHistory  " +
                                                            "                   WHERE isLatest=1" +
                                                            "         ) AS tblPurchaseScheduleStatusHistory" +
                                                            "  ON tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId" +
                                                            //" LEFT JOIN tblPurchaseScheduleStatusHistory ON tblPurchaseScheduleStatusHistory.PurchaseScheduleSummaryId = tblPurchaseScheduleSummary.rootScheduleId" +
                                                            " LEFT JOIN dimVehicleType ON dimVehicleType.idVehicleType = tblPurchaseScheduleSummary.vehicleTypeId ) " +
                                                            " SRC ";
            //  WHERE  SRC.statusId in (" + statusId + ")" +
            //  " AND RN = 1 ";

            return sqlSelectQry;

        }
        public String SelectVehicleTrackingDtlsQry()
        {
            String sqlSelectQry = " SELECT ISNULL(tblpurchaseschedulesummary.rootScheduleId,tblpurchaseschedulesummary.idpurchaseschedulesummary) AS vehicleId,tblpurchaseschedulesummary.cOrNCId , " +
                                  " CASE WHEN tblpurchaseschedulesummary.cOrNCId = 1 THEN 'Order' WHEN tblpurchaseschedulesummary.cOrNCId = 0 THEN 'Enquiry' ELSE ''  END AS bookingType, " +
                                  " pmUser.userDisplayName AS purchaseManager,supplierUser.firmName AS partyName,tblpurchaseschedulesummary.vehicleNo, " +
                                  " newVehSchedule.createdOn AS scheduleOn ,newVevScheduleByUser.userDisplayName AS scheduleBy, " +
                                  " ISNULL(tblPurchaseInvoice.updatedOn,tblPurchaseInvoice.createdOn) AS commercialApprovalOn,commercialApproByUser.userDisplayName AS commercialApprovalBy, " +
                                  " outSideInsp.createdOn AS outSideInspectionOn, outInspeByUser.userDisplayName AS outsideInspeBy, " +
                                  " vehReported.createdOn  AS vehicleRepotedOn, vehReportedByUser.userDisplayName AS vehicleReportedBy," +
                                  " vehRequested.createdOn  AS vehRequestedOn, vehRequestedByUser.userDisplayName AS vehRequestedBy," +
                                  " sendIn.createdOn AS sendInOn, sendInByUser.userDisplayName AS sendInBy," +
                                  " grossWt.createdOn AS grossWtTakenOn, grossWtByUser.userDisplayName AS grossWtTakenBy," +
                                  " wtStage1.createdOn AS wtStage1TakenOn, wtStage1ByUser.userDisplayName AS wtStage1TakenBy," +
                                  " wtStage2.createdOn AS wtStage2TakenOn, wtStage2ByUser.userDisplayName AS wtStage2TakenBy," +
                                  " wtStage3.createdOn AS wtStage3TakenOn, wtStage3ByUser.userDisplayName AS wtStage3TakenBy," +
                                  " wtStage4.createdOn AS wtStage4TakenOn, wtStage4ByUser.userDisplayName AS wtStage4TakenBy," +
                                  " wtStage5.createdOn AS wtStage5TakenOn, wtStage5ByUser.userDisplayName AS wtStage5TakenBy," +
                                  " wtStage6.createdOn AS wtStage6TakenOn, wtStage6ByUser.userDisplayName AS wtStage6TakenBy," +
                                  " wtStage7.createdOn AS wtStage7TakenOn, wtStage7ByUser.userDisplayName AS wtStage7TakenBy," +
                                  " tareWt.createdOn AS tareWtTakenOn, tareWtByUser.userDisplayName AS tareWtTakenBy," +
                                  " weighingComplete.createdOn AS weighingCompletedOn, weighingCompleByUser.userDisplayName AS weighingCompletedBy," +
                                  " vehicleOut.createdOn AS vehicleOutOn,vehicleOutByUser.userDisplayName AS vehicleOutBy," +
                                  " unloading.createdOn AS unloadingCompleOn, unloadingCompleByUser.userDisplayName AS unloadingCompleBy," +
                                  " grading.createdOn AS gradingCompleOn, gradingCompleByUser.userDisplayName AS gradingCompleBy ," +
                                  " recovery.createdOn AS recoveryCompleOn, recoveryCompleByUser.userDisplayName AS recoveryCompleBy," +
                                  " correction.createdOn AS correctionCompleOn, correctionCompleByUser.userDisplayName AS correctionCompleBy," +
                                  " unloadingSupervisorName.userDisplayName AS supervisorName," +
                                  " graderName.userDisplayName AS graderName," +
                                  " recEnggName.userDisplayName AS engineerName," +
                                  " tblpurchaseschedulesummary.isCorrectionCompleted" +
                                  " FROM tblpurchaseschedulesummary tblpurchaseschedulesummary" +
                                  " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblPurchaseEnquiry.idpurchaseEnquiry = tblpurchaseschedulesummary.purchaseEnquiryId" +
                                  " LEFT JOIN tblUser pmUser ON pmUser.idUser = tblPurchaseEnquiry.userId" +
                                  " LEFT JOIN tblOrganization supplierUser ON supplierUser.idOrganization = tblpurchaseschedulesummary.supplierId" +
                                  " LEFT JOIN( SELECT purchaseScheduleSummaryId AS purchaseScheduleSummaryId,MAX(createdOn) AS createdOn,MAX(statusId) AS statusId, MAX(createdBy )AS createdBy FROM tblPurchaseSchStatusHistory WHERE statusId = 501 GROUP BY purchaseScheduleSummaryId ) AS newVehSchedule ON newVehSchedule.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND newVehSchedule.statusId = 501 " +
                                  " LEFT JOIN tblUser newVevScheduleByUser ON newVevScheduleByUser.idUser = newVehSchedule.createdBy" +
                                  " LEFT JOIN tblPurchaseInvoice ON tblPurchaseInvoice.purSchSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary)" +
                                  " LEFT JOIN tblUser commercialApproByUser ON commercialApproByUser.idUser = tblPurchaseInvoice.createdBy" +
                                  " LEFT JOIN tblPurchaseSchStatusHistory outSideInsp ON outSideInsp.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND outSideInsp.vehiclePhaseId = 1" +
                                  " LEFT JOIN tblUser outInspeByUser ON outInspeByUser.idUser = outSideInsp.createdBy" +
                                  " LEFT JOIN tblPurchaseSchStatusHistory vehReported ON vehReported.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND vehReported.statusId = 502" +
                                  " LEFT JOIN tblUser vehReportedByUser ON vehReportedByUser.idUser = vehReported.createdBy" +
                                  " LEFT JOIN tblPurchaseSchStatusHistory vehRequested ON vehRequested.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND vehRequested.statusId = 504" +
                                  " LEFT JOIN tblUser vehRequestedByUser ON vehRequestedByUser.idUser = vehRequested.createdBy" +
                                  " LEFT JOIN tblPurchaseSchStatusHistory sendIn ON sendIn.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND sendIn.statusId = 505" +
                                  " LEFT JOIN tblUser sendInByUser ON sendInByUser.idUser = sendIn.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary grossWt ON grossWt.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND grossWt.weightStageId = 0 AND grossWt.weightMeasurTypeId = 3" +
                                  " LEFT JOIN tblUser grossWtByUser ON grossWtByUser.idUser = grossWt.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary wtStage1 ON wtStage1.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND wtStage1.weightStageId = 1 AND wtStage1.weightMeasurTypeId = 2" +
                                  " LEFT JOIN tblUser wtStage1ByUser ON wtStage1ByUser.idUser = wtStage1.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary wtStage2 ON wtStage2.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND wtStage2.weightStageId = 2 AND wtStage2.weightMeasurTypeId = 2" +
                                  " LEFT JOIN tblUser wtStage2ByUser ON wtStage2ByUser.idUser = wtStage2.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary wtStage3 ON wtStage3.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND wtStage3.weightStageId = 3 AND wtStage3.weightMeasurTypeId = 2" +
                                  " LEFT JOIN tblUser wtStage3ByUser ON wtStage3ByUser.idUser = wtStage3.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary wtStage4 ON wtStage4.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND wtStage4.weightStageId = 4 AND wtStage4.weightMeasurTypeId = 2" +
                                  " LEFT JOIN tblUser wtStage4ByUser ON wtStage4ByUser.idUser = wtStage4.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary wtStage5 ON wtStage5.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND wtStage5.weightStageId = 5 AND wtStage5.weightMeasurTypeId = 2" +
                                  " LEFT JOIN tblUser wtStage5ByUser ON wtStage5ByUser.idUser = wtStage5.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary wtStage6 ON wtStage6.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND wtStage6.weightStageId = 6 AND wtStage6.weightMeasurTypeId = 2" +
                                  " LEFT JOIN tblUser wtStage6ByUser ON wtStage6ByUser.idUser = wtStage6.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary wtStage7 ON wtStage7.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND wtStage7.weightStageId = 7 AND wtStage7.weightMeasurTypeId = 2" +
                                  " LEFT JOIN tblUser wtStage7ByUser ON wtStage7ByUser.idUser = wtStage7.createdBy" +
                                  " LEFT JOIN tblPurchaseWeighingStageSummary tareWt ON tareWt.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND tareWt.weightMeasurTypeId = 1" +
                                  " LEFT JOIN tblUser tareWtByUser ON tareWtByUser.idUser = tareWt.createdBy" +
                                  " LEFT JOIN tblPurchaseSchStatusHistory weighingComplete ON weighingComplete.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND weighingComplete.statusId = 533" +
                                  " LEFT JOIN tblUser weighingCompleByUser ON weighingCompleByUser.idUser = weighingComplete.createdBy" +
                                  " LEFT JOIN tblPurchaseSchStatusHistory vehicleOut ON vehicleOut.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId,tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND vehicleOut.statusId = 510" +
                                  " LEFT JOIN tblUser vehicleOutByUser ON vehicleOutByUser.idUser = vehicleOut.createdBy"+
                                  " LEFT JOIN tblPurchaseSchStatusHistory unloading ON unloading.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND unloading.vehiclePhaseId = 5" +
                                  " LEFT JOIN tblUser unloadingSupervisorName ON unloadingSupervisorName.idUser = tblpurchaseschedulesummary.supervisorId" +
                                  " LEFT JOIN tblUser unloadingCompleByUser ON unloadingCompleByUser.idUser = unloading.createdBy" +
                                  " LEFT JOIN tblPurchaseSchStatusHistory grading ON grading.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND grading.vehiclePhaseId = 2" +
                                  " LEFT JOIN tblUser graderName ON graderName.idUser = tblpurchaseschedulesummary.graderId" +
                                  " LEFT JOIN tblUser gradingCompleByUser on gradingCompleByUser.idUser = grading.createdBy" +
                                  " LEFT JOIN tblPurchaseSchStatusHistory recovery ON recovery.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND recovery.vehiclePhaseId = 3" +
                                  " LEFT JOIN tblUser recEnggName ON recEnggName.idUser = tblpurchaseschedulesummary.engineerId" +
                                  " LEFT JOIN tblUser recoveryCompleByUser ON recoveryCompleByUser.idUser = recovery.createdBy" +
                                  " LEFT JOIN(SELECT purchaseScheduleSummaryId AS purchaseScheduleSummaryId,MAX(createdOn) AS createdOn, MAX(vehiclePhaseId) AS vehiclePhaseId, MAX(createdBy )AS createdBy FROM tblPurchaseSchStatusHistory WHERE vehiclePhaseId = 4 GROUP BY purchaseScheduleSummaryId) AS correction ON correction.purchaseScheduleSummaryId = ISNULL(tblpurchaseschedulesummary.rootScheduleId, tblpurchaseschedulesummary.idPurchaseScheduleSummary) AND correction.vehiclePhaseId = 4" +
                                  " LEFT JOIN tblUser correctionCompleByUser ON correctionCompleByUser.idUser = correction.createdBy" +
                                  " WHERE ISNULL(tblpurchaseschedulesummary.isActive,0) = 1 AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0)= 1";




            return sqlSelectQry;
        }
        public String SelectQueryLight()
        {
            //tblLocation.locationDesc,
            String sqlSelectQry = " select * from tblPurchaseScheduleSummary ";
            return sqlSelectQry;

        }

        public String UnloadingRateScrapQuery()
        {
            String sqlSelectQry = " SELECT tblPurchaseScheduleSummary.cOrNcId,CAST(corretionCompletedOn AS DATE) AS unloadingdate , sum(tblPurchaseScheduleDetails.productAomunt)/sum(tblPurchaseScheduleDetails.qty) AS total " +
                                  " ,tblpurchaseEnquiry.prodClassId,tblProdClassification.prodClassDesc" +
                                  " from tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                  " LEFT JOIN tblpurchaseEnquiry tblpurchaseEnquiry ON tblpurchaseEnquiry.IdpurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
                                  " LEFT JOIN tblPurchaseScheduleDetails tblPurchaseScheduleDetails " +
                                  " ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId " +
                                  " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass = tblpurchaseEnquiry.prodClassId ";
            return sqlSelectQry;
        }



        #endregion

        #region Selection
        public List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummary()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        //Priyanka [28-01-2019]
        public List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummaryForCommercialApp(string ignoreStatusIds,Int32 approvalType, Int32 idPurchaseScheduleSummary, Int32 MaterialTypeId)
        {
           

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String approvalTypeStr = string.Empty;
            try
            {
                conn.Open();
                if (approvalType == 1)
                {
                    approvalTypeStr = " AND ISNULL(SRC.commercialVerified,0) = 0 ";
                }
                if (idPurchaseScheduleSummary > 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE SRC.cOrNCId =1 AND SRC.isActive = 1 AND ISNULL(SRC.commercialApproval,0) = 0 " + approvalTypeStr
                                                      + " AND SRC.statusId NOT IN (" + ignoreStatusIds + ") " + "AND SRC.idPurchaseScheduleSummary = " + idPurchaseScheduleSummary + " and SRC.prodClassId not in (" + MaterialTypeId + ")  order by SRC.createdOn desc";
                }
                else
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE SRC.cOrNCId =1 AND SRC.isActive = 1 AND ISNULL(SRC.commercialApproval,0) = 0 " + approvalTypeStr
                                                      + " AND SRC.statusId NOT IN (" + ignoreStatusIds + ") " + " and SRC.prodClassId not in (" + MaterialTypeId + ") order by SRC.createdOn desc";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        //Priyanka [01-03-2019]
        public List<TblPurchaseScheduleSummaryTO> SelectPurchaseScheduleSummaryTOByVehicleNo(String vehicleNo, Int32 actualRootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String approvalTypeStr = string.Empty;
            String statusIdStr = Convert.ToInt32(Constants.TranStatusE.New) + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_OUT) + "," + Convert.ToInt32(Constants.TranStatusE.DELETE_VEHICLE)
            //Prajakta[2019-05-13] Added
            + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_REJECTED_AFTER_WEIGHING)
            + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_REJECTED_BEFORE_WEIGHING)
            + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_REJECTED_AFTER_GROSS_WEIGHT)
            + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_CANCELED);
            String spotEntryCond = string.Empty;
            try
            {
                conn.Open();

                if (actualRootScheduleId != 0)
                {
                    spotEntryCond = "AND rootScheduleId NOT IN (" + actualRootScheduleId + ")";
                }

                // cmdSelect.CommandText = SelectQuery() + " WHERE vehicleNo = '" + vehicleNo + "' AND statusId NOT IN(" + statusIdStr + ") " +
                //                         "AND isActive =1 " + spotEntryCond;

                cmdSelect.CommandText = SelectQueryLight() + " WHERE vehicleNo = '" + vehicleNo + "' AND statusId NOT IN(" + statusIdStr + ") " +
                                       "AND isActive =1 " + spotEntryCond;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToListLight(sqlReader);

                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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



        public string GetDisplayNameFromUserID(int createdBy)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                string userDisplayName = "";
                cmdSelect.CommandText = "select * from tblUser where idUser =" + createdBy;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (sqlReader.Read())
                {
                    if (sqlReader["userDisplayName"] != DBNull.Value)
                        userDisplayName = (sqlReader["userDisplayName"].ToString());
                }
                sqlReader.Dispose();
                return userDisplayName;
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

        public int SelectNextStatusOfCurrentStatus(int statusId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                int idstatus = 0;
                cmdSelect.CommandText = "select * from dimStatus where prevStatusId =" + statusId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (sqlReader.Read())
                {
                    if (sqlReader["idStatus"] != DBNull.Value)
                        idstatus = Convert.ToInt32(sqlReader["idStatus"].ToString());
                }
                sqlReader.Dispose();
                return idstatus;
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
        }



        //Nikhil[2018-05-25] Add
        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummary(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SelectQuery() + " WHERE ISNULL(SRC.isActive,0)=1 and purchaseEnquiryId = " + purchaseEnquiryId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 idPurchaseScheduleSummary, Boolean isActive, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                if (isActive)
                    cmdSelect.CommandText = SelectQuery() + " WHERE ISNULL(SRC.isActive,0)=1 and  SRC.idPurchaseScheduleSummary = " + idPurchaseScheduleSummary;
                else
                    cmdSelect.CommandText = SelectQuery() + " WHERE  SRC.idPurchaseScheduleSummary = " + idPurchaseScheduleSummary;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Close();
                sqlReader.Dispose();

                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByRootID(Int32 RootScheduleId, Boolean isActive, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                if (isActive)
                    cmdSelect.CommandText = SelectQuery() + " WHERE ISNULL(SRC.isActive,0)=1 and  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + RootScheduleId;
                else
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + RootScheduleId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Close();
                sqlReader.Dispose();

                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootID(Int32 RootScheduleId, Boolean isActive)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;


            try
            {
                conn.Open();
                if (isActive)
                    cmdSelect.CommandText = SelectQuery() + " WHERE ISNULL(SRC.isActive,0)=1 and  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + RootScheduleId;
                else
                    cmdSelect.CommandText = SelectQuery() + " WHERE ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + RootScheduleId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Close();

                if (list != null && list.Count > 0)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        public TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 parentScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() + " WHERE ISNULL(SRC.isActive,0)=1 and SRC.parentPurchaseScheduleSummaryId = " + parentScheduleId + "";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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
        public TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByParentScheduleId(Int32 parentScheduleId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SelectQuery() + " WHERE ISNULL(SRC.isActive,0)=1 and SRC.parentPurchaseScheduleSummaryId = " + parentScheduleId + "";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }




        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootId(Int32 rootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() + " WHERE  isnull(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " order by idPurchaseScheduleSummary desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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



        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootId(Int32 rootScheduleId, SqlConnection con, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            cmdSelect.Connection = con;
            cmdSelect.Transaction = tran;
            try
            {
                cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + "  ORDER BY SRC.idPurchaseScheduleSummary DESC";
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId, SqlConnection con, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Connection = con;
            cmdSelect.Transaction = tran;
            try
            {
                // if (VehiclePhaseId == 0)
                //     cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId;
                // else
                //     cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId + " AND SRC.vehiclePhaseId=" + VehiclePhaseId;


                if (rootScheduleId > 0 && statusId > 0 && VehiclePhaseId > 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId + " AND SRC.vehiclePhaseId=" + VehiclePhaseId;
                }
                else if (rootScheduleId > 0 && statusId > 0 && VehiclePhaseId == 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId;
                }
                else if (rootScheduleId > 0 && statusId == 0 && VehiclePhaseId > 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.vehiclePhaseId=" + VehiclePhaseId;
                }
                else if (rootScheduleId > 0 && statusId == 0 && VehiclePhaseId == 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId;
                }

                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public Int32 SelectVehicleScheduleByRootAndStatusIdCount(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId, SqlConnection con, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Connection = con;
            cmdSelect.Transaction = tran;
            try
            {
                // if (VehiclePhaseId == 0)
                //     cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId;
                // else
                //     cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId + " AND SRC.vehiclePhaseId=" + VehiclePhaseId;

                cmdSelect.CommandText = " SELECT COUNT(*) AS vehicleCount from TblPurchaseScheduleSummary SRC";

                if (rootScheduleId > 0 && statusId > 0 && VehiclePhaseId > 0)
                {
                    cmdSelect.CommandText += " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId + " AND SRC.vehiclePhaseId=" + VehiclePhaseId;
                }
                else if (rootScheduleId > 0 && statusId > 0 && VehiclePhaseId == 0)
                {
                    cmdSelect.CommandText += " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId;
                }
                else if (rootScheduleId > 0 && statusId == 0 && VehiclePhaseId > 0)
                {
                    cmdSelect.CommandText += " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.vehiclePhaseId=" + VehiclePhaseId;
                }
                else if (rootScheduleId > 0 && statusId == 0 && VehiclePhaseId == 0)
                {
                    cmdSelect.CommandText += " WHERE ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId;
                }

                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                Int32 count = 0;

                while (sqlReader.Read())
                {
                    if (sqlReader["vehicleCount"] != DBNull.Value)
                        count = Convert.ToInt32(sqlReader["vehicleCount"].ToString());
                }

                sqlReader.Dispose();
                return count;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }


        public List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleDBBackUp(Int32 statusId,Int32 isDBBackUp)
        {

            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,0) = " + 0 + " AND SRC.statusId=" + statusId + " AND SRC.isDBup = " + isDBBackUp + " AND SRC.modbusRefId > 0 ";
                cmdSelect.CommandText = SelectQuery() + " WHERE SRC.statusId=" + statusId + " AND SRC.isDBup = " + isDBBackUp + " AND SRC.modbusRefId > 0 AND SRC.isActive = 1 ";

                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }



        public List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId)
        {

            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                if (rootScheduleId > 0 && statusId > 0 && VehiclePhaseId > 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId + " AND SRC.vehiclePhaseId=" + VehiclePhaseId;
                }
                else if (rootScheduleId > 0 && statusId > 0 && VehiclePhaseId == 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.statusId=" + statusId;
                }
                else if (rootScheduleId > 0 && statusId == 0 && VehiclePhaseId > 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId + " AND SRC.vehiclePhaseId=" + VehiclePhaseId;
                }
                else if (rootScheduleId > 0 && statusId == 0 && VehiclePhaseId == 0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId;
                }
                //chetan[16-March-2020] added for migration of IOT to DB database
                else if(statusId>0 && rootScheduleId==0 && VehiclePhaseId==0)
                {
                    cmdSelect.CommandText = SelectQuery() + " WHERE  ISNULL(SRC.rootScheduleId,0) = " + 0 + " AND SRC.statusId=" + statusId;

                }
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        public List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, string statusIds, string vehiclePhaseIds)
        {

            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SelectQuery();

                string whrCondition = " WHERE  ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId;

                if (!String.IsNullOrEmpty(statusIds))
                {
                    whrCondition += " AND SRC.statusId in ( " + statusIds + ")";
                }

                if (!String.IsNullOrEmpty(vehiclePhaseIds))
                {
                    whrCondition += " AND SRC.vehiclePhaseId in (" + vehiclePhaseIds + ")";
                }

                cmdSelect.CommandText += whrCondition;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryDtlsByEnquiryId(Int32 purchaseEnquiryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() + " WHERE SRC.purchaseEnquiryId = " + purchaseEnquiryId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryDtlsByEnquiryId(Int32 purchaseEnquiryId, Int32 rootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //Prajakta[2019-03-08] Commented and added
                // cmdSelect.CommandText = SelectQuery() + " WHERE SRC.purchaseEnquiryId = " + purchaseEnquiryId + " AND SRC.isActive =1 AND ISNULL(rootScheduleId,0)= " + rootScheduleId;

                //Prajakta[2019-04-15] Added  and get value from configuration
                Boolean isGetCancledVehicles = true;
                string statusIds = "";

                if (isGetCancledVehicles)
                {
                    statusIds = Convert.ToInt32(Constants.TranStatusE.New).ToString() + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_CANCELED).ToString();
                    //+ "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_REJECTED_BEFORE_WEIGHING).ToString();
                }
                else
                {
                    statusIds = Constants.TranStatusE.New.ToString();
                }

                //Prajakta[2019-06-10] Commented rootScheduleId = 0 condition to get enquiry record for isBoth yes
                //cmdSelect.CommandText = SelectQuery() + " WHERE SRC.purchaseEnquiryId = " + purchaseEnquiryId + "  AND ISNULL(rootScheduleId,0)= " + rootScheduleId + "  AND statusId IN ( " + statusIds + " )";
                cmdSelect.CommandText = SelectQuery() + " WHERE SRC.purchaseEnquiryId = " + purchaseEnquiryId + "  AND statusId IN ( " + statusIds + " )";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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
        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryDtlsByEnquiryId(Int32 purchaseEnquiryId, Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {

                cmdSelect.CommandText = SelectQuery() + " WHERE SRC.purchaseEnquiryId = " + purchaseEnquiryId + "  AND ISNULL(rootScheduleId,0)= " + rootScheduleId + "  AND statusId = " + (Int32)Constants.TranStatusE.New + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);

                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummary(Int32 purchaseEnquiryId)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() + " WHERE  SRC.purchaseEnquiryId = " + purchaseEnquiryId + " and SRC.statusId=" + Convert.ToInt32(Constants.TranStatusE.New);
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;

                //cmdSelect.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseScheduleSummaryTO> SelectEnquiryScheduleSummary(Int32 purchaseEnquiryId)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                //Prajata[2021-05-13] Commented and added for showing split vehicle qty
                //cmdSelect.CommandText = SelectQuery() + " WHERE ISNULL(SRC.isActive,0)=1 and SRC.purchaseEnquiryId = " + purchaseEnquiryId;
                cmdSelect.CommandText = SelectQueryForEnqVehDisplay() + " WHERE ISNULL(SRC.isActive,0)=1 and isnull(SRC.purLinkVehEnquiryId,SRC.purchaseEnquiryId) = " + purchaseEnquiryId;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //Prajata[2021-05-13] Commented and added for showing split vehicle qty
                //List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToListForVehDisplay(sqlReader); 
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseScheduleSummaryTO> SelectSuperwiserFromTblPurchaseScheduleSummary(int statusId, string roleIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblPurchaseScheduleSummary  " +
                "where statusId = " + statusId + " and SupervisorId  IS not NULL and SupervisorId  " +
                "in (select iduser from tblUser tblUser  " +
                               "   INNER JOIN tblUserRole tblUserRole on tblUserRole.userId = tblUser.idUser  " +
                                 "  INNER JOIN tblRole tblRole on tblRole.idRole = tblUserRole.roleId  " +
                    "where tblRole.idRole in (" + roleIds.ToString() + ") )  ";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList2(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryDetails(Int32 idSchedulePurchaseSummary, Boolean isActive)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();

                if (isActive)
                {
                    cmdSelect.CommandText += " where ISNULL(SRC.isActive,0)=1  and SRC.idPurchaseScheduleSummary= " + idSchedulePurchaseSummary;
                }
                else
                {
                    cmdSelect.CommandText += " where SRC.idPurchaseScheduleSummary= " + idSchedulePurchaseSummary;
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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
        public List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();
                //cmdSelect.CommandText += " where SRC.idPurchaseScheduleSummary= " + rootScheduleId + " OR SRC.idPurchaseScheduleSummary=" + rootScheduleId;
                cmdSelect.CommandText += " where ISNULL(SRC.rootscheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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
        public List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId, string statusIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string orderByStr = "";
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();
                cmdSelect.CommandText += " where SRC.rootScheduleId= " + rootScheduleId + " and SRC.statusId in (" + statusIds + ")";

                orderByStr = " order by SRC.idPurchaseScheduleSummary desc ";
                cmdSelect.CommandText += orderByStr;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId, string statusIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            string orderByStr = "";
            try
            {
                cmdSelect.CommandText = SelectQuery();
                cmdSelect.CommandText += " where SRC.rootScheduleId= " + rootScheduleId + " and SRC.statusId in (" + statusIds + ")";

                orderByStr = " order by SRC.idPurchaseScheduleSummary desc ";
                cmdSelect.CommandText += orderByStr;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }


        
         public List<TblPurchaseScheduleSummaryTO> UnloadingRateScrapQuery(DateTime fromDate,DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string orderByStr = "";
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = UnloadingRateScrapQuery() +
                                       " WHERE tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 4 AND" +
                                         " CAST(corretionCompletedOn AS date) BETWEEN @fromDate AND @toDate " +
                                    " GROUP BY CAST(corretionCompletedOn AS DATE), tblPurchaseScheduleSummary.cOrNcId,tblpurchaseEnquiry.prodClassId, " +
                                       " tblProdClassification.prodClassDesc";
                                     


                //orderByStr = " order by SRC.idPurchaseScheduleSummary desc ";
                cmdSelect.CommandText += orderByStr;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToListForUnloadingScrap(sqlReader);
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseScheduleSummaryTO> SelectAllCorrectionCompleVehicles(DateTime toDate, Int32 cOrNcId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string orderByStr = "";
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();
                //SRC.statusId IN (" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED + ") AND
                int confiqId = _iTblConfigParamsDAO.IoTSetting();
                if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                {
                    cmdSelect.CommandText += " WHERE  ISNULL(SRC.isCorrectionCompleted,0)=1 AND ISNULL(SRC.cOrNCId,0)= " + cOrNcId + " AND SRC.vehiclePhaseId=" + (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS;
                }else
                    cmdSelect.CommandText += " WHERE SRC.statusId IN (" + (Int32)Constants.TranStatusE.UNLOADING_COMPLETED + ") AND ISNULL(SRC.isCorrectionCompleted,0)=1 AND ISNULL(SRC.cOrNCId,0)= " + cOrNcId + " AND SRC.vehiclePhaseId=" + (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS;


                if (toDate != new DateTime())
                {
                    cmdSelect.CommandText += " AND cast(ISNULL(corretionCompletedOn,createdOn) as date) <= @toDate ";
                }

                //orderByStr = " order by SRC.idPurchaseScheduleSummary desc ";
                cmdSelect.CommandText += orderByStr;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                if (list != null)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryDetailsList(Int32 idSchedulePurchaseSummary, Int32 rootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();
                if (rootScheduleId > 0)
                {
                    cmdSelect.CommandText += " where  SRC.rootScheduleId= " + rootScheduleId + " AND SRC.statusId = " + Convert.ToInt32(Constants.TranStatusE.SEND_IN);
                }
                else
                {
                    cmdSelect.CommandText += " where  SRC.idPurchaseScheduleSummary= " + idSchedulePurchaseSummary;
                }


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<VehicleStatusDateTO> SelectAllVehicleTrackingDtls(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectVehicleTrackingDtlsQry();

                cmdSelect.CommandText += " AND ISNULL(rootScheduleId,idPurchaseScheduleSummary)  " +
                                         " IN ( SELECT purchaseScheduleSummaryId FROM tblPurchaseSchStatusHistory " +
                                         " WHERE CAST(createdOn AS date) BETWEEN @fromDate AND @toDate) ";

                //cmdSelect.CommandText += "  AND CAST (scheduleDate AS date) BETWEEN @fromDate AND @toDate ";

                cmdSelect.CommandText += " ORDER BY tblpurchaseschedulesummary.idpurchaseschedulesummary DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.FromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.ToDate;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<VehicleStatusDateTO> list = VehicleTrackingConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsList(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();
                cmdSelect.CommandText += " WHERE ISNULL(SRC.isActive,0)=1  and  ISNULL(SRC.isConvertToSauda,0) =1 ";

                if (!tblPurSchSummaryFilterTO.SkipDateTime)
                {
                    cmdSelect.CommandText += " and cast(scheduleDate as date) between @fromDate and @toDate ";
                }

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.UserId))
                {
                    cmdSelect.CommandText += " AND SRC.userId IN (" + tblPurSchSummaryFilterTO.UserId + ")";
                }

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.COrNcId))
                {
                    cmdSelect.CommandText += " AND SRC.cOrNCId IN (" + tblPurSchSummaryFilterTO.COrNcId + ")";
                }

                if (tblPurSchSummaryFilterTO.SupplierId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.supplierId = " + tblPurSchSummaryFilterTO.SupplierId;
                }

                if (tblPurSchSummaryFilterTO.NotInStatusIds != null)
                {
                    cmdSelect.CommandText += " AND SRC.statusId not in (" + tblPurSchSummaryFilterTO.NotInStatusIds + ") ";
                }
                if (tblPurSchSummaryFilterTO.StatusId > 0)
                {
                    if (tblPurSchSummaryFilterTO.StatusId == Convert.ToInt32(Constants.TranStatusE.VEHICLE_SCHEDULE_REJECTED))
                    {
                        cmdSelect.CommandText += " AND SRC.statusId not in (" + tblPurSchSummaryFilterTO.StatusId + ")";
                    }
                    else
                    {
                        cmdSelect.CommandText += " AND SRC.statusId = " + tblPurSchSummaryFilterTO.StatusId;
                    }
                }
                else if (tblPurSchSummaryFilterTO.InStatusIds != null)
                {
                    if (tblPurSchSummaryFilterTO.ForScheduleActualOrUnloading == 2)
                    {
                        tblPurSchSummaryFilterTO.InStatusIds = "" + (int)Constants.TranStatusE.New;
                    }
                    // cmdSelect.CommandText += " AND SRC.statusId in (" + tblPurSchSummaryFilterTO.InStatusIds + ") and cast(SRC.scheduleDate as date) <= GETDATE() ";
                    string statusCondition = " AND SRC.statusId in (" + tblPurSchSummaryFilterTO.InStatusIds + ")";


                    if (tblPurSchSummaryFilterTO.ForScheduleActualOrUnloading == 3)
                    {
                        statusCondition = " AND (SRC.statusId in (" + tblPurSchSummaryFilterTO.InStatusIds + ") OR (SRC.statusId in (" + (int)Constants.TranStatusE.VEHICLE_OUT + ") " +
                        " AND cast(SRC.createdOn as date) between cast(GETDATE() as date) and cast(GETDATE() as date)))";
                    }

                    cmdSelect.CommandText += statusCondition;

                }


                if (tblPurSchSummaryFilterTO.PhaseId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.vehiclePhaseId = " + tblPurSchSummaryFilterTO.PhaseId;
                }

                if (tblPurSchSummaryFilterTO.VehicleTypeId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.vehicleTypeId = " + tblPurSchSummaryFilterTO.VehicleTypeId;
                }

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.LoginUserId) && tblPurSchSummaryFilterTO.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.GRADER))
                {
                    cmdSelect.CommandText += " AND (isnull(SRC.GraderId,0) = " + tblPurSchSummaryFilterTO.LoginUserId + " or isnull(SRC.GraderId,0) = 0)";
                }

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.LoginUserId) && tblPurSchSummaryFilterTO.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.PHOTOGRAPHER))
                {
                    cmdSelect.CommandText += " AND (isnull(SRC.photographerId,0) = " + tblPurSchSummaryFilterTO.LoginUserId + " or isnull(SRC.photographerId,0) = 0)";
                }
                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.LoginUserId) && tblPurSchSummaryFilterTO.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.UNLOADING_SUPERVISOR))
                {
                    cmdSelect.CommandText += " AND (isnull(SRC.supervisorId,0) = " + tblPurSchSummaryFilterTO.LoginUserId + " or isnull(SRC.supervisorId,0) = 0)";
                }
                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.LoginUserId) && tblPurSchSummaryFilterTO.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.RECOVERY_ENGINEER))
                {
                    cmdSelect.CommandText += " AND (isnull(SRC.engineerId,0) = " + tblPurSchSummaryFilterTO.LoginUserId + " or isnull(SRC.engineerId,0) = 0)";
                }

                if (tblPurSchSummaryFilterTO.ProdClassId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.prodClassId = " + tblPurSchSummaryFilterTO.ProdClassId;
                }
                // if (tblPurSchSummaryFilterTO.ForScheduleActualOrUnloading == 3)
                // {
                //     cmdSelect.CommandText += " or (SRC.statusId in (" + (int)Constants.TranStatusE.VEHICLE_OUT + ") and cast(SRC.createdOn as date) between cast(GETDATE() as date) and cast(GETDATE() as date))";
                // }

                cmdSelect.CommandText += " order by SRC.idPurchaseScheduleSummary desc";
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.FromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.ToDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        //Added by minal 26 May 2021 for Dropbox
        public List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForGradeNoteForDropbox(string vehicleIds,int cOrNcId) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();
                //Prajakta[2019-03-06]Commented and added
                //cmdSelect.CommandText += " WHERE cast(createdOn as date) between @fromDate and @toDate and  ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                //cmdSelect.CommandText += " WHERE cast(isnull(corretionCompletedOn,createdOn) as date) between @fromDate and @toDate and  ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                cmdSelect.CommandText += " WHERE ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                cmdSelect.CommandText += " AND SRC.vehiclePhaseId = " + Convert.ToInt32(Constants.PurchaseVehiclePhasesE.CORRECTIONS);
                cmdSelect.CommandText += " AND SRC.statusId = " + Convert.ToInt32(Constants.TranStatusE.UNLOADING_COMPLETED);
                if (cOrNcId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                    cmdSelect.CommandText += " AND SRC.cOrNCId=" + cOrNcId;

                if (!String.IsNullOrEmpty(vehicleIds))
                {
                    cmdSelect.CommandText += " AND SRC.rootScheduleId IN (" + vehicleIds + ")";
                }

                cmdSelect.CommandText += " order by SRC.idPurchaseScheduleSummary desc";
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                //cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.FromDate;
                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.ToDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
        public List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForMasterReportForDropbox(string vehicleIds, int cOrNcId) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQueryForMasterReport();
                //Prajakta[2019-03-06]Commented and added
                //cmdSelect.CommandText += " WHERE cast(createdOn as date) between @fromDate and @toDate and  ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                //cmdSelect.CommandText += " WHERE cast(isnull(corretionCompletedOn,createdOn) as date) between @fromDate and @toDate and  ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                cmdSelect.CommandText += " WHERE ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                cmdSelect.CommandText += " AND SRC.vehiclePhaseId = " + Convert.ToInt32(Constants.PurchaseVehiclePhasesE.CORRECTIONS);
                cmdSelect.CommandText += " AND SRC.statusId = " + Convert.ToInt32(Constants.TranStatusE.UNLOADING_COMPLETED);
                if (cOrNcId != (int)StaticStuff.Constants.ConfirmTypeE.BOTH)
                    cmdSelect.CommandText += " AND SRC.cOrNCId=" + cOrNcId;

                if (!String.IsNullOrEmpty(vehicleIds))
                {
                    cmdSelect.CommandText += " AND SRC.rootScheduleId IN (" + vehicleIds + ")";
                }

                cmdSelect.CommandText += " order by SRC.idPurchaseScheduleSummary desc";
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                //cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.FromDate;
                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.ToDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToListForMasterReport(reader);
                reader.Dispose();
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

        //Added by minal 26 May 2021
        public List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForGradeNote(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO,String purchaseManagerIds) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();
                //Prajakta[2019-03-06]Commented and added

                

                if (tblPurSchSummaryFilterTO.IsConsiderTm == 1)
                {
                    cmdSelect.CommandText += " WHERE isnull(corretionCompletedOn,createdOn) between @fromDate and @toDate and  ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                }
                else
                {
                    cmdSelect.CommandText += " WHERE cast(isnull(corretionCompletedOn,createdOn) as date) between @fromDate and @toDate and  ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                }

                if (tblPurSchSummaryFilterTO.PhaseId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.vehiclePhaseId = " + tblPurSchSummaryFilterTO.PhaseId;
                }


                if (tblPurSchSummaryFilterTO.StatusId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.statusId = " + tblPurSchSummaryFilterTO.StatusId;
                }

                if (!String.IsNullOrEmpty(purchaseManagerIds))
                {
                    cmdSelect.CommandText += " AND SRC.userId IN (" + purchaseManagerIds + ")";
                }

                cmdSelect.CommandText += " order by SRC.idPurchaseScheduleSummary desc";
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                if (tblPurSchSummaryFilterTO.IsConsiderTm == 1)
                {
                    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = tblPurSchSummaryFilterTO.FromDate;
                    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;
                }
                else
                {
                    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.FromDate;
                    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.ToDate;
                }
                
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForMasterReport(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO,String purchaseManagerIds) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQueryForMasterReport();
                //Prajakta[2019-03-06]Commented and added

                

                if (tblPurSchSummaryFilterTO.IsConsiderTm == 1)
                {
                    cmdSelect.CommandText += " WHERE isnull(corretionCompletedOn,createdOn) between @fromDate and @toDate and  ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                }
                else
                {
                    cmdSelect.CommandText += " WHERE cast(isnull(corretionCompletedOn,createdOn) as date) between @fromDate and @toDate and  ISNULL(SRC.isCorrectionCompleted,0) =1 ";
                }

                if (tblPurSchSummaryFilterTO.PhaseId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.vehiclePhaseId = " + tblPurSchSummaryFilterTO.PhaseId;
                }


                if (tblPurSchSummaryFilterTO.StatusId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.statusId = " + tblPurSchSummaryFilterTO.StatusId;
                }

                if (!String.IsNullOrEmpty(purchaseManagerIds))
                {
                    cmdSelect.CommandText += " AND SRC.userId IN (" + purchaseManagerIds + ")";
                }

                cmdSelect.CommandText += " order by SRC.idPurchaseScheduleSummary desc";
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = tblPurSchSummaryFilterTO.FromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToListForMasterReport(reader);
                reader.Dispose();
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


        public List<TblPurchaseScheduleSummaryTO> SelectVehicleListForAllCommonApprovals(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQueryForApprovals();
                cmdSelect.CommandText += " WHERE ISNULL(SRC.isActive,0)=1  and  ISNULL(SRC.isLatest,0)=1 and cast(scheduleDate as date) between @fromDate and @toDate and  ISNULL(SRC.historyIsActive,0)=1 ";

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.UserId))
                {
                    cmdSelect.CommandText += " AND SRC.userId IN (" + tblPurSchSummaryFilterTO.UserId + ") ";
                }

                if (tblPurSchSummaryFilterTO.SupplierId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.supplierId = " + tblPurSchSummaryFilterTO.SupplierId;
                }

                // if (tblPurSchSummaryFilterTO.StatusId > 0)
                // {
                //     cmdSelect.CommandText += " AND SRC.statusId = " + tblPurSchSummaryFilterTO.StatusId;
                // }

                if (tblPurSchSummaryFilterTO.PhaseId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.vehiclePhaseId = " + tblPurSchSummaryFilterTO.PhaseId;
                }
                cmdSelect.CommandText += " order by SRC.idPurchaseScheduleSummary desc";
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.FromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.ToDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToListForApprovals(reader);
                reader.Dispose();
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


        public List<TblPurchaseScheduleSummaryTO> GetVehicleListForPendingQualityFlags(string pmUserId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQueryForPendingQualityFlag();
                
                if(!String.IsNullOrEmpty(pmUserId))
                {
                    cmdSelect.CommandText += " and tblPurchaseEnquiry.userId In (" + pmUserId + ")";
                }

                cmdSelect.CommandText += " order by tblPurchaseScheduleSummary.rootScheduleId desc ";

               cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToListForPendingFlags(reader);
                reader.Dispose();
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailList(String statusId, Int32 loggedInUserId, DateTime date) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() +
                                " WHERE  SRC.statusId in (" + statusId + ")" +
                                " AND ISNULL(SRC.isActive,0)=1 AND cast(SRC.scheduleDate as date) = @date ";

                if (loggedInUserId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.supervisorId = " + loggedInUserId;
                }

                String orderBy = " order by SRC.createdOn desc";

                cmdSelect.CommandText += orderBy;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = date;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public Dictionary<Int32, Int32> GetAllocatedVehiclesAgainstRole(Int32 roleTypeId, Boolean getAllAlocatedVehicles)
        {
            Dictionary<Int32, Int32> userCountDCT = new Dictionary<int, int>();
            if (roleTypeId == 0)
            {
                return userCountDCT;
            }
            if (roleTypeId != (Int32)Constants.SystemRoleTypeE.UNLOADING_SUPERVISOR && roleTypeId != (Int32)Constants.SystemRoleTypeE.RECOVERY_ENGINEER && roleTypeId != (Int32)Constants.SystemRoleTypeE.GRADER)
            {
                return userCountDCT;
            }
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string dateConditionStr = "";
            try
            {
                conn.Open();

                String statIdsStr = String.Empty;

                if (roleTypeId == (Int32)Constants.SystemRoleTypeE.UNLOADING_SUPERVISOR)
                {
                    statIdsStr = _iTblConfigParamsBL.SelectTblConfigParamsValByNameString(Constants.CP_SCRAP_UNLOADING_SUPERVISOR_VEHICLE_ALLOCATED_STATUS);

                    string groupBy = " GROUP BY supervisorId ";
                    string condition = " ";

                    cmdSelect.CommandText = "SELECT supervisorId as userId ,COUNT(*) as count from tblPurchaseScheduleSummary " +
                                            " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                                            " WHERE tblPurchaseScheduleSummary.statusId IN ( " + statIdsStr + " ) " +
                                            " AND ISNULL(tblPurchaseScheduleSummary.isActive,0)= 1 and isnull(tblPurchaseScheduleSummary.isUnloadingCompleted,0) = 0 AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 " +
                                            "  GROUP BY supervisorId ";


                    // cmdSelect.CommandText = "SELECT supervisorId as userId ,COUNT(*) as count from tblPurchaseScheduleSummary " +
                    //                                             " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                    //                                             " WHERE tblPurchaseScheduleSummary.statusId IN ( " + statIdsStr + " ) " +
                    //                                             " AND ISNULL(tblPurchaseScheduleSummary.isActive,0)= 1  AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 ";


                    // if (!getAllAlocatedVehicles)
                    // {
                    //     condition = "and isnull(tblPurchaseScheduleSummary.isUnloadingCompleted,0) = 0";
                    // }

                    //cmdSelect.CommandText += condition + groupBy;
                }

                else if (roleTypeId == (Int32)Constants.SystemRoleTypeE.GRADER)
                {

                    string groupBy = " GROUP BY graderId ";
                    string condition = " ";

                    statIdsStr = _iTblConfigParamsBL.SelectTblConfigParamsValByNameString(Constants.CP_SCRAP_GRADER_VEHICLE_ALLOCATED_STATUS);

                    cmdSelect.CommandText = "SELECT graderId as userId ,COUNT(*)  as count from tblPurchaseScheduleSummary " +
                                            " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                                            " WHERE tblPurchaseScheduleSummary.statusId IN ( " + statIdsStr + " ) " +
                                            " AND ISNULL(tblPurchaseScheduleSummary.isActive,0)= 1 and isnull(tblPurchaseScheduleSummary.isGradingCompleted,0) = 0 AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 " +
                                            " GROUP BY graderId";

                    // cmdSelect.CommandText = "SELECT graderId as userId ,COUNT(*)  as count from tblPurchaseScheduleSummary " +
                    //                        " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                    //                        " WHERE tblPurchaseScheduleSummary.statusId IN ( " + statIdsStr + " ) " +
                    //                        " AND ISNULL(tblPurchaseScheduleSummary.isActive,0)= 1  AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 ";


                    // if (!getAllAlocatedVehicles)
                    // {
                    //     condition = "and isnull(tblPurchaseScheduleSummary.isGradingCompleted,0) = 0";
                    // }

                    //cmdSelect.CommandText += condition + groupBy;
                }
                else if (roleTypeId == (Int32)Constants.SystemRoleTypeE.RECOVERY_ENGINEER)
                {

                    string groupBy = " GROUP BY engineerId ";
                    string condition = " ";

                    statIdsStr = _iTblConfigParamsBL.SelectTblConfigParamsValByNameString(Constants.CP_SCRAP_RECOVERY_ENGINEER_VEHICLE_ALLOCATED_STATUS);


                    cmdSelect.CommandText = "SELECT engineerId as userId ,COUNT(*)  as count from tblPurchaseScheduleSummary " +
                                            " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                                            " WHERE tblPurchaseScheduleSummary.statusId IN ( " + statIdsStr + " ) " +
                                            " AND ISNULL(tblPurchaseScheduleSummary.isActive,0)= 1 and isnull(tblPurchaseScheduleSummary.isRecovery,0) = 0  AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 " +
                                            " GROUP BY engineerId ";

                    // cmdSelect.CommandText = "SELECT engineerId as userId ,COUNT(*)  as count from tblPurchaseScheduleSummary " +
                    //                       " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                    //                       " WHERE tblPurchaseScheduleSummary.statusId IN ( " + statIdsStr + " ) " +
                    //                       " AND ISNULL(tblPurchaseScheduleSummary.isActive,0)= 1  AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 ";


                    // if (!getAllAlocatedVehicles)
                    // {
                    //     condition = "and isnull(tblPurchaseScheduleSummary.isRecovery,0) = 0 ";
                    // }

                    //cmdSelect.CommandText += condition + groupBy;
                }


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 userId = 0, count = 0;
                        if (reader["userId"] != DBNull.Value)
                            userId = Convert.ToInt32(reader["userId"].ToString());
                        if (reader["count"] != DBNull.Value)
                            count = Convert.ToInt32(reader["count"].ToString());

                        if (!userCountDCT.ContainsKey(userId))
                        {
                            userCountDCT.Add(userId, count);
                        }

                    }
                }

                reader.Dispose();

                return userCountDCT;

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



        public Dictionary<Int32, Int32> GetTodaysAllocatedVehiclesCnt(Int32 roleTypeId, DateTime serverDate)
        {
            Dictionary<Int32, Int32> userCountDCT = new Dictionary<int, int>();

            if (roleTypeId == 0)
            {
                return userCountDCT;
            }
            if (roleTypeId != (Int32)Constants.SystemRoleTypeE.UNLOADING_SUPERVISOR && roleTypeId != (Int32)Constants.SystemRoleTypeE.RECOVERY_ENGINEER && roleTypeId != (Int32)Constants.SystemRoleTypeE.GRADER)
            {
                return userCountDCT;
            }
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string dateConditionStr = "";

            Int32 statusId = (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;

            try
            {
                conn.Open();

                String statIdsStr = String.Empty;


                if (roleTypeId == (Int32)Constants.SystemRoleTypeE.UNLOADING_SUPERVISOR)
                {
                    // statIdsStr = _iTblConfigParamsBL.SelectTblConfigParamsValByNameString(Constants.CP_SCRAP_UNLOADING_SUPERVISOR_VEHICLE_ALLOCATED_STATUS);

                    Int32 vehiclePhaseId = (Int32)Constants.PurchaseVehiclePhasesE.UNLOADING_COMPLETED;
                    cmdSelect.CommandText = " select supervisorId as userId ,COUNT(*)  as count from  tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                            " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                                            " WHERE cast(tblPurchaseScheduleSummary.createdOn as date) = @date" +
                                            " AND  tblPurchaseScheduleSummary.vehiclePhaseId  = " + vehiclePhaseId +
                                            " AND  tblPurchaseScheduleSummary.statusId  = " + statusId +
                                            " AND ISNULL(isUnloadingCompleted,0) =1 AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 " +
                                            " GROUP BY supervisorId ";


                }

                else if (roleTypeId == (Int32)Constants.SystemRoleTypeE.GRADER)
                {

                    // statIdsStr = _iTblConfigParamsBL.SelectTblConfigParamsValByNameString(Constants.CP_SCRAP_GRADER_VEHICLE_ALLOCATED_STATUS);
                    Int32 vehiclePhaseId = (Int32)Constants.PurchaseVehiclePhasesE.GRADING;

                    cmdSelect.CommandText = " select graderId as userId ,COUNT(*)  as count from  tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                          " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                                          " WHERE cast(tblPurchaseScheduleSummary.createdOn as date) = @date" +
                                          " AND  tblPurchaseScheduleSummary.vehiclePhaseId  = " + vehiclePhaseId +
                                          " AND  tblPurchaseScheduleSummary.statusId  = " + statusId +
                                          " AND ISNULL(isGradingCompleted,0) =1 AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 " +
                                          " GROUP BY graderId ";

                }
                else if (roleTypeId == (Int32)Constants.SystemRoleTypeE.RECOVERY_ENGINEER)
                {
                    //statIdsStr = _iTblConfigParamsBL.SelectTblConfigParamsValByNameString(Constants.CP_SCRAP_RECOVERY_ENGINEER_VEHICLE_ALLOCATED_STATUS);
                    Int32 vehiclePhaseId = (Int32)Constants.PurchaseVehiclePhasesE.RECOVERY;

                    cmdSelect.CommandText = " select engineerId as userId ,COUNT(*)  as count from  tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                           " LEFT JOIN tblPurchaseEnquiry ON tblPurchaseScheduleSummary.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                                           " WHERE cast(tblPurchaseScheduleSummary.createdOn as date) = @date" +
                                           " AND tblPurchaseScheduleSummary.vehiclePhaseId  = " + vehiclePhaseId +
                                           " AND tblPurchaseScheduleSummary.statusId  = " + statusId +
                                           " AND ISNULL(isRecovery,0) =1 AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0) = 1 " +
                                           " GROUP BY engineerId ";

                }



                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = serverDate;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 userId = 0, count = 0;
                        if (reader["userId"] != DBNull.Value)
                            userId = Convert.ToInt32(reader["userId"].ToString());
                        if (reader["count"] != DBNull.Value)
                            count = Convert.ToInt32(reader["count"].ToString());

                        if (!userCountDCT.ContainsKey(userId))
                        {
                            userCountDCT.Add(userId, count);
                        }

                    }
                }

                reader.Dispose();

                return userCountDCT;

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



        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailListForRecovery(DateTime fromDate, DateTime toDate, string statusId, Int32 loggedInUserId, Int32 rootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string dateConditionStr = "";
            try
            {
                conn.Open();


                cmdSelect.CommandText = SelectQuery() +
                                               " WHERE  SRC.statusId in (" + statusId + ")" +
                                               " AND ISNULL(SRC.isActive,0)=1  AND ISNULL(SRC.isRecovery,0) = 0";


                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    cmdSelect.CommandText += dateConditionStr + " AND cast(SRC.scheduleDate as date) BETWEEN @fromDate and @toDate ";
                }

                if (loggedInUserId > 0)
                {
                    //Prajakta[2019-05-07] Commented and added as recovery engineer can only view asigned vehicles
                    //cmdSelect.CommandText += " AND (isnull(SRC.engineerId,0) = " + loggedInUserId + "OR isnull(SRC.engineerId,0) =0)";
                    cmdSelect.CommandText += " AND isnull(SRC.engineerId,0) = " + loggedInUserId;
                }

                //Priyanka [22-03-2019] added for respective entry routing condition.
                if (rootScheduleId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.rootScheduleId=" + rootScheduleId;
                }
                String orderBy = " order by SRC.createdOn desc";

                cmdSelect.CommandText += orderBy;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailList(DateTime fromDate, DateTime toDate, string statusId, Int32 loggedInUserId, Int32 ShowList, Int32 idPurchaseScheduleSummary, Int32 rootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string dateConditionStr = " AND cast(SRC.scheduleDate as date) BETWEEN @fromDate and @toDate ";
            try
            {
                conn.Open();


                cmdSelect.CommandText = SelectQuery() +
                                               " WHERE  SRC.statusId in (" + statusId + ")" +
                                               " AND ISNULL(SRC.isActive,0)=1 and  ISNULL(SRC.isConvertToSauda,0) =1";
                if (idPurchaseScheduleSummary == 0 || rootScheduleId == 0)
                {
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        cmdSelect.CommandText += dateConditionStr;  //+ " AND cast(SRC.scheduleDate as date) BETWEEN @fromDate and @toDate ";
                    }
                }
                if (ShowList == Convert.ToInt32(Constants.ShowListE.UNLOADING))
                {
                    cmdSelect.CommandText += "  AND  isnull(SRC.isUnloadingCompleted,0) not in (1) ";

                    if (loggedInUserId > 0)
                    {
                        cmdSelect.CommandText += " AND Isnull(SRC.supervisorId,0) = " + loggedInUserId;
                    }

                }

                if (ShowList == Convert.ToInt32(Constants.ShowListE.ADD_FREIGHT))
                {
                    cmdSelect.CommandText += "  AND Isnull(SRC.isFreightAdded,0) = 0";

                }

                else if (ShowList == Convert.ToInt32(Constants.ShowListE.GRADING))
                {
                    if (loggedInUserId > 0)
                    {
                        //Prajakta[2019-05-07] Commented and added as grader can only view asigned vehicles
                        // cmdSelect.CommandText += "and (Isnull(SRC.graderId,0) = " + loggedInUserId + " OR Isnull(SRC.graderId,0) = 0)";
                        cmdSelect.CommandText += "and Isnull(SRC.graderId,0) = " + loggedInUserId;
                    }
                    cmdSelect.CommandText += " and Isnull(SRC.isGradingCompleted,0) = 0";

                }
                
                String orderBy = " order by SRC.createdOn desc";
                if (idPurchaseScheduleSummary > 0)
                {
                    cmdSelect.CommandText += " AND SRC.idPurchaseScheduleSummary =" + idPurchaseScheduleSummary;
                }
                if (rootScheduleId > 0)
                {
                    cmdSelect.CommandText += " AND SRC.rootScheduleId = " + rootScheduleId;
                }
               
                cmdSelect.CommandText += orderBy;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseScheduleSummaryTO> GetAllCorrectionCompletedVeh(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery();

                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    cmdSelect.CommandText += " WHERE  cast(SRC.corretionCompletedOn as date) BETWEEN @fromDate AND @toDate AND" +
                        " SRC.vehiclePhaseId = " + (int)Constants.PurchaseVehiclePhasesE.CORRECTIONS + " AND SRC.isCorrectionCompleted = 1 ";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsListPhasewise(DateTime fromDate, DateTime toDate, String userId, Int32 rootScheduleId, string showListE, string ignoreStatusIds) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string dateConditionStr = "";
            string rootScheduleIdStr = "";
            string orderByStr = "";
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() +
                                " WHERE   isnull(src.isActive,0)=1 ";
                // " AND SRC.statusId = " + Convert.ToInt32(StaticStuff.Constants.TranStatusE.New);


                //Prajakta[2019-05-27] Commented and added following to show vwhicles as per unloader,grader,engineer
                if (!String.IsNullOrEmpty(userId))
                {
                    if (showListE == Convert.ToInt32(Constants.ShowListE.UNLOADING).ToString())
                    {
                        cmdSelect.CommandText += " AND SRC.supervisorId IN (" + userId + ")";
                    }
                    else if (showListE == Convert.ToInt32(Constants.ShowListE.GRADING).ToString())
                    {
                        cmdSelect.CommandText += " AND SRC.graderId IN (" + userId + ")";
                    }
                    else if (showListE == Convert.ToInt32(Constants.ShowListE.RECOVERY).ToString())
                    {
                        cmdSelect.CommandText += " AND SRC.engineerId IN (" + userId + ")";
                    }
                    else
                    {
                        cmdSelect.CommandText += " AND SRC.userId IN (" + userId + ")";
                    }
                }

                // if (!String.IsNullOrEmpty(userId))
                // {
                //     cmdSelect.CommandText += " AND SRC.userId IN (" + userId + ")";
                // }

                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    dateConditionStr = " AND cast(SRC.createdOn as date) BETWEEN @fromDate AND @toDate ";
                }

                rootScheduleIdStr = " AND ISNULL(SRC.rootScheduleId,SRC.idPurchaseScheduleSummary) = " + rootScheduleId;

                if (ignoreStatusIds != null && ignoreStatusIds != "")
                {
                    cmdSelect.CommandText += " AND SRC.statusId not in (" + ignoreStatusIds + ") ";

                }
                orderByStr = " order by src.createdOn desc";

                if (rootScheduleId > 0)
                {
                    cmdSelect.CommandText += rootScheduleIdStr + orderByStr;
                }
                else
                    cmdSelect.CommandText += dateConditionStr + orderByStr;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsListPhasewiseForComp(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string dateConditionStr = "";
            string rootScheduleIdStr = "";
            string orderByStr = "";
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() + " WHERE   isnull(src.isActive,0)=1 ";

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.UserId))
                {
                    cmdSelect.CommandText += " AND src.userId IN (" + tblPurSchSummaryFilterTO.UserId + ")";
                }

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.LoginUserId) && tblPurSchSummaryFilterTO.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.GRADER))
                {
                    cmdSelect.CommandText += " AND (isnull(src.GraderId,0) = " + tblPurSchSummaryFilterTO.LoginUserId + ")";
                }

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.LoginUserId) && tblPurSchSummaryFilterTO.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.UNLOADING_SUPERVISOR))
                {
                    cmdSelect.CommandText += " AND (isnull(src.supervisorId,0) = " + tblPurSchSummaryFilterTO.LoginUserId + ")";
                }
                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.LoginUserId) && tblPurSchSummaryFilterTO.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.RECOVERY_ENGINEER))
                {
                    cmdSelect.CommandText += " AND (isnull(src.engineerId,0) = " + tblPurSchSummaryFilterTO.LoginUserId + ")";
                }

                if (tblPurSchSummaryFilterTO.FromDate != DateTime.MinValue && tblPurSchSummaryFilterTO.ToDate != DateTime.MinValue)
                {
                    dateConditionStr = " AND cast(src.createdOn as date) BETWEEN @fromDate AND @toDate ";
                }

                if (!String.IsNullOrEmpty(tblPurSchSummaryFilterTO.COrNcId))
                {
                    cmdSelect.CommandText += " AND src.cOrNCId IN (" + tblPurSchSummaryFilterTO.COrNcId + ")";
                }

                if (tblPurSchSummaryFilterTO.SupplierId > 0)
                {
                    cmdSelect.CommandText += " AND src.supplierId = " + tblPurSchSummaryFilterTO.SupplierId;
                }

                if (tblPurSchSummaryFilterTO.StatusId > 0)
                {
                    cmdSelect.CommandText += "AND src.statusId = " + tblPurSchSummaryFilterTO.StatusId;
                }

                if (tblPurSchSummaryFilterTO.ProdClassId > 0)
                {
                    cmdSelect.CommandText += "AND src.prodClassId = " + tblPurSchSummaryFilterTO.ProdClassId;

                }

                if (tblPurSchSummaryFilterTO.RootScheduleId > 0)
                {
                    rootScheduleIdStr = " AND ISNULL(src.rootScheduleId,src.idPurchaseScheduleSummary) = " + tblPurSchSummaryFilterTO.RootScheduleId;
                }

                if (tblPurSchSummaryFilterTO.PhaseId > 0)
                {
                    cmdSelect.CommandText += " AND src.vehiclePhaseId = " + tblPurSchSummaryFilterTO.PhaseId;
                }

                if (tblPurSchSummaryFilterTO.VehicleTypeId > 0)
                {
                    cmdSelect.CommandText += " AND src.vehicleTypeId = " + tblPurSchSummaryFilterTO.VehicleTypeId;
                }

                if (tblPurSchSummaryFilterTO.NotInStatusIds != null && tblPurSchSummaryFilterTO.NotInStatusIds != "")
                {
                    cmdSelect.CommandText += " AND src.statusId not in (" + tblPurSchSummaryFilterTO.NotInStatusIds + ") ";

                }
                orderByStr = " order by src.createdOn desc";

                if (tblPurSchSummaryFilterTO.RootScheduleId > 0)
                {
                    cmdSelect.CommandText += rootScheduleIdStr + orderByStr;
                }
                else
                    cmdSelect.CommandText += dateConditionStr + orderByStr;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.FromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = tblPurSchSummaryFilterTO.ToDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForPurchaseEnquiry(Int32 purchaseEnquiryId) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() +
                            //" WHERE  SRC.purchaseEnquiryId = (" + purchaseEnquiryId + ")" +
                            " WHERE  SRC.purchaseEnquiryId = (" + purchaseEnquiryId + ")" +
                            " AND ISNULL(SRC.isActive,0)=1 ";


                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
        public List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListByDate(DateTime scheduleDate) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() +
                            " WHERE  cast(SRC.scheduleDate as date) = @scheduleDate and ISNULL(SRC.isActive,0)=1 and ISNULL(vehiclePhaseId,0)=" + Convert.ToInt32(StaticStuff.Constants.PurchaseVehiclePhasesE.GRADING) +
                            " order by SRC.createdOn desc";
                // " AND SRC.statusId in (" + StaticStuff.Constants.TranStatusE.VEHICLE_REQUESTED;

                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@scheduleDate", System.Data.SqlDbType.Date).Value = scheduleDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsList(String vehicleNo, String statusId, Int32 idPurchaseScheduleSummary) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQueryNew();
                cmdSelect.CommandText = SelectQuery() +
               " WHERE tblPurchaseScheduleSummary.vehicleNo=" + "'" + vehicleNo + "'" + " and tblPurchaseScheduleSummary.statusId=" + statusId +
               " AND ISNULL(SRC.isActive,0)=1 ";

                // cmdSelect.CommandText += " WHERE tblPurchaseScheduleSummary.vehicleNo=" + "'" + vehicleNo + "'" + " and tblPurchaseScheduleSummary.statusId=" + statusId;

                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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


        public List<TblPurchaseScheduleSummaryTO> ConvertDTToList2(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                while (tblPurchaseScheduleSummaryTODT.Read())
               {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                    if (tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["createdBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["updatedBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["scheduleDate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.OrgScheduleQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["scheduleQty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Padta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padta"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["remark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Remark = Convert.ToString(tblPurchaseScheduleSummaryTODT["remark"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["firmName"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.SupplierName = Convert.ToString(tblPurchaseScheduleSummaryTODT["firmName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNc = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"].ToString());


                    //  if (tblPurchaseScheduleSummaryTODT["displayName"] != DBNull.Value)
                    //   tblPurchaseScheduleSummaryTONew.MaterailType = Convert.ToString(tblPurchaseScheduleSummaryTODT["displayName"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["rate_band_costing"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.RateBand = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rate_band_costing"].ToString());

                    //if (tblPurchaseScheduleSummaryTODT["stateName"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.StateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["stateName"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["areaName"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.AreaName = Convert.ToString(tblPurchaseScheduleSummaryTODT["areaName"].ToString());

                    //  if (tblPurchaseScheduleSummaryTODT["prodClassId"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.ProdClassId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prodClassId"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["stateId"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.StateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["stateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["engineerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["engineerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supervisorId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PhotographerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["photographerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["qualityFlag"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.QualityFlag = Convert.ToBoolean(tblPurchaseScheduleSummaryTODT["qualityFlag"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["driverName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverName = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverName"]);

                    if (tblPurchaseScheduleSummaryTODT["driverContactNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverContactNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverContactNo"]);

                    if (tblPurchaseScheduleSummaryTODT["transporterName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransporterName = Convert.ToString(tblPurchaseScheduleSummaryTODT["transporterName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleTypeId"].ToString());

                    //   if (tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.VehicleTypeName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Freight = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freight"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["containerNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ContainerNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["containerNo"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleStateId"].ToString());

                    //  if (tblPurchaseScheduleSummaryTODT["vehicleStateName"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.VehicleStateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleStateName"]);

                    if (tblPurchaseScheduleSummaryTODT["locationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LocationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["locationId"]);

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNcId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"]);

                    // if (tblPurchaseScheduleSummaryTODT["locationDesc"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["locationDesc"]);

                    // if (tblPurchaseScheduleSummaryTODT["userDisplayName"] != DBNull.Value)
                    //   tblPurchaseScheduleSummaryTONew.SupervisorName = Convert.ToString(tblPurchaseScheduleSummaryTODT["userDisplayName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleCatId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleCatId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleCatId"]);

                    // if (tblPurchaseScheduleSummaryTODT["prevStatusId"] != DBNull.Value)
                    //  tblPurchaseScheduleSummaryTONew.PreviousStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prevStatusId"]);

                    // if (tblPurchaseScheduleSummaryTODT["previousStatusName"] != DBNull.Value)
                    //  tblPurchaseScheduleSummaryTONew.PreviousStatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["previousStatusName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehiclePhaseId"]);
                    //  if (tblPurchaseScheduleSummaryTODT["phaseName"] != DBNull.Value)
                    //   tblPurchaseScheduleSummaryTONew.VehiclePhaseName = Convert.ToString(tblPurchaseScheduleSummaryTODT["phaseName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehRejectReasonDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["correNarration"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CorreNarration = Convert.ToString(tblPurchaseScheduleSummaryTODT["correNarration"]);

                    //if (String.IsNullOrEmpty(tblPurchaseScheduleSummaryTONew.CorreNarration))
                    {
                        if (tblPurchaseScheduleSummaryTODT["saudaRemark"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.SaudaNarration = Convert.ToString(tblPurchaseScheduleSummaryTODT["saudaRemark"]);

                        if (tblPurchaseScheduleSummaryTODT["saudaNarration"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.SaudaNarration = Convert.ToString(tblPurchaseScheduleSummaryTODT["saudaNarration"]);
                    }


                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);
                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }

        public List<VehicleStatusDateTO> VehicleTrackingConvertDTToList(SqlDataReader vehicleStatusTODT)
        {
            List<VehicleStatusDateTO> vehicleStatusDateTOList = new List<VehicleStatusDateTO>();
            if (vehicleStatusTODT != null)
            {
                while (vehicleStatusTODT.Read())
                {
                    VehicleStatusDateTO vehicleStatusTO = new VehicleStatusDateTO();

                    if (vehicleStatusTODT["vehicleId"] != DBNull.Value)
                        vehicleStatusTO.RootScheduleId = Convert.ToInt32(vehicleStatusTODT["vehicleId"].ToString());

                    if (vehicleStatusTODT["bookingType"] != DBNull.Value)
                        vehicleStatusTO.BookingType = Convert.ToString(vehicleStatusTODT["bookingType"].ToString());

                    if (vehicleStatusTODT["purchaseManager"] != DBNull.Value)
                        vehicleStatusTO.ManagerName = Convert.ToString(vehicleStatusTODT["purchaseManager"].ToString());

                    if (vehicleStatusTODT["partyName"] != DBNull.Value)
                        vehicleStatusTO.PartyName = Convert.ToString(vehicleStatusTODT["partyName"].ToString());

                    if (vehicleStatusTODT["vehicleNo"] != DBNull.Value)
                        vehicleStatusTO.VehicleNo = Convert.ToString(vehicleStatusTODT["vehicleNo"].ToString());

                    
                    if (vehicleStatusTODT["cOrNCId"] != DBNull.Value)
                        vehicleStatusTO.COrNcId = Convert.ToInt32(vehicleStatusTODT["cOrNCId"].ToString());

                    if (vehicleStatusTODT["scheduleOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.ScheduleOn = Convert.ToDateTime(vehicleStatusTODT["scheduleOn"].ToString());
                        vehicleStatusTO.ScheduleOnStr = Constants.GetDateWithFormate(vehicleStatusTO.ScheduleOn);
                        vehicleStatusTO.ScheduleOn = vehicleStatusTO.ScheduleOn.AddSeconds(-vehicleStatusTO.ScheduleOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["scheduleOn"] != DBNull.Value)
                    //{
                    //    vehicleStatusTO.ScheduleOnStr = Convert.ToString(vehicleStatusTODT["scheduleOn"].ToString());
                    //}
                        

                    if (vehicleStatusTODT["scheduleBy"] != DBNull.Value)
                        vehicleStatusTO.ScheduleBy = Convert.ToString(vehicleStatusTODT["scheduleBy"].ToString());

                    if (vehicleStatusTODT["commercialApprovalOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.SendForCommercialapproval = Convert.ToDateTime(vehicleStatusTODT["commercialApprovalOn"].ToString());
                        vehicleStatusTO.SendForCommercialapprovalOnStr = Constants.GetDateWithFormate(vehicleStatusTO.SendForCommercialapproval);
                        vehicleStatusTO.SendForCommercialapproval = vehicleStatusTO.SendForCommercialapproval.AddSeconds(-vehicleStatusTO.SendForCommercialapproval.Second);
                    }
                        

                    //if (vehicleStatusTODT["commercialApprovalOn"] != DBNull.Value)
                    //    vehicleStatusTO.SendForCommercialapprovalOnStr = (vehicleStatusTODT["commercialApprovalOn"].ToString());

                    if (vehicleStatusTODT["commercialApprovalBy"] != DBNull.Value)
                        vehicleStatusTO.SendForCommercialapprovalBy = Convert.ToString(vehicleStatusTODT["commercialApprovalBy"].ToString());

                    if (vehicleStatusTODT["outSideInspectionOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.SendForOutsideInspection = Convert.ToDateTime(vehicleStatusTODT["outSideInspectionOn"].ToString());
                        vehicleStatusTO.SendForOutsideInspectionOnStr = Constants.GetDateWithFormate(vehicleStatusTO.SendForOutsideInspection);
                        vehicleStatusTO.SendForOutsideInspection = vehicleStatusTO.SendForOutsideInspection.AddSeconds(-vehicleStatusTO.SendForOutsideInspection.Second);
                    }
                        

                    //if (vehicleStatusTODT["outSideInspectionOn"] != DBNull.Value)
                    //    vehicleStatusTO.SendForOutsideInspectionOnStr = (vehicleStatusTODT["outSideInspectionOn"].ToString());

                    if (vehicleStatusTODT["outsideInspeBy"] != DBNull.Value)
                        vehicleStatusTO.SendForOutsideInspectionBy = Convert.ToString(vehicleStatusTODT["outsideInspeBy"].ToString());

                    if (vehicleStatusTODT["vehicleRepotedOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.VehicleReportedOn = Convert.ToDateTime(vehicleStatusTODT["vehicleRepotedOn"].ToString());
                        vehicleStatusTO.VehicleReportedOnStr = Constants.GetDateWithFormate(vehicleStatusTO.VehicleReportedOn);
                        vehicleStatusTO.VehicleReportedOn = vehicleStatusTO.VehicleReportedOn.AddSeconds(-vehicleStatusTO.VehicleReportedOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["vehicleRepotedOn"] != DBNull.Value)
                    //    vehicleStatusTO.VehicleReportedOnStr = (vehicleStatusTODT["vehicleRepotedOn"].ToString());

                    if (vehicleStatusTODT["vehicleReportedBy"] != DBNull.Value)
                        vehicleStatusTO.VehicleReportedBy = Convert.ToString(vehicleStatusTODT["vehicleReportedBy"].ToString());

                    if (vehicleStatusTODT["vehRequestedOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.RequestedToSendIn = Convert.ToDateTime(vehicleStatusTODT["vehRequestedOn"].ToString());
                        vehicleStatusTO.RequestedToSendInOnStr = Constants.GetDateWithFormate(vehicleStatusTO.RequestedToSendIn);
                        vehicleStatusTO.RequestedToSendIn = vehicleStatusTO.RequestedToSendIn.AddSeconds(-vehicleStatusTO.RequestedToSendIn.Second);
                    }
                        

                    //if (vehicleStatusTODT["vehRequestedOn"] != DBNull.Value)
                    //    vehicleStatusTO.RequestedToSendInOnStr = (vehicleStatusTODT["vehRequestedOn"].ToString());

                    if (vehicleStatusTODT["vehRequestedBy"] != DBNull.Value)
                        vehicleStatusTO.RequestedToSendInBy = Convert.ToString(vehicleStatusTODT["vehRequestedBy"].ToString());

                    if (vehicleStatusTODT["sendInOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.SentIn = Convert.ToDateTime(vehicleStatusTODT["sendInOn"].ToString());
                        vehicleStatusTO.SentInOnStr = Constants.GetDateWithFormate(vehicleStatusTO.SentIn);
                        vehicleStatusTO.SentIn = vehicleStatusTO.SentIn.AddSeconds(-vehicleStatusTO.SentIn.Second);
                    }
                        

                    //if (vehicleStatusTODT["sendInOn"] != DBNull.Value)
                    //    vehicleStatusTO.SentInOnStr = Convert.ToString(vehicleStatusTODT["sendInOn"].ToString());

                    if (vehicleStatusTODT["sendInBy"] != DBNull.Value)
                        vehicleStatusTO.SentInBy = Convert.ToString(vehicleStatusTODT["sendInBy"].ToString());

                    if (vehicleStatusTODT["grossWtTakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.GrossWtTakenOn = Convert.ToDateTime(vehicleStatusTODT["grossWtTakenOn"].ToString());
                        vehicleStatusTO.GrossWtTakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.GrossWtTakenOn);
                        vehicleStatusTO.GrossWtTakenOn = vehicleStatusTO.GrossWtTakenOn.AddSeconds(-vehicleStatusTO.GrossWtTakenOn.Second);
                    }

                    //if (vehicleStatusTODT["grossWtTakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.GrossWtTakenOnStr = Convert.ToString(vehicleStatusTODT["grossWtTakenOn"].ToString());

                    if (vehicleStatusTODT["grossWtTakenBy"] != DBNull.Value)
                        vehicleStatusTO.GrossWtTakenBy = Convert.ToString(vehicleStatusTODT["grossWtTakenBy"].ToString());

                    if (vehicleStatusTODT["wtStage1TakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.WtStage1TakenOn = Convert.ToDateTime(vehicleStatusTODT["wtStage1TakenOn"].ToString());
                        vehicleStatusTO.WtStage1TakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.WtStage1TakenOn);
                        vehicleStatusTO.WtStage1TakenOn = vehicleStatusTO.WtStage1TakenOn.AddSeconds(-vehicleStatusTO.WtStage1TakenOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["wtStage1TakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.WtStage1TakenOnStr = Convert.ToString(vehicleStatusTODT["wtStage1TakenOn"].ToString());

                    if (vehicleStatusTODT["wtStage1TakenBy"] != DBNull.Value)
                        vehicleStatusTO.WtStage1TakenBy = Convert.ToString(vehicleStatusTODT["wtStage1TakenBy"].ToString());


                    if (vehicleStatusTODT["wtStage2TakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.WtStage2TakenOn = Convert.ToDateTime(vehicleStatusTODT["wtStage2TakenOn"].ToString());
                        vehicleStatusTO.WtStage2TakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.WtStage2TakenOn);
                        vehicleStatusTO.WtStage2TakenOn = vehicleStatusTO.WtStage2TakenOn.AddSeconds(-vehicleStatusTO.WtStage2TakenOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["wtStage2TakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.WtStage2TakenOnStr = Convert.ToString(vehicleStatusTODT["wtStage2TakenOn"].ToString());

                    if (vehicleStatusTODT["wtStage2TakenBy"] != DBNull.Value)
                        vehicleStatusTO.WtStage2TakenBy = Convert.ToString(vehicleStatusTODT["wtStage2TakenBy"].ToString());

                    if (vehicleStatusTODT["wtStage3TakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.WtStage3TakenOn = Convert.ToDateTime(vehicleStatusTODT["wtStage3TakenOn"].ToString());
                        vehicleStatusTO.WtStage3TakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.WtStage3TakenOn);
                        vehicleStatusTO.WtStage3TakenOn = vehicleStatusTO.WtStage3TakenOn.AddSeconds(-vehicleStatusTO.WtStage3TakenOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["wtStage3TakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.WtStage3TakenOnStr = Convert.ToString(vehicleStatusTODT["wtStage3TakenOn"].ToString());

                    if (vehicleStatusTODT["wtStage3TakenBy"] != DBNull.Value)
                        vehicleStatusTO.WtStage3TakenBy = Convert.ToString(vehicleStatusTODT["wtStage3TakenBy"].ToString());

                    if (vehicleStatusTODT["wtStage4TakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.WtStage4TakenOn = Convert.ToDateTime(vehicleStatusTODT["wtStage4TakenOn"].ToString());
                        vehicleStatusTO.WtStage4TakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.WtStage4TakenOn);
                        vehicleStatusTO.WtStage4TakenOn = vehicleStatusTO.WtStage4TakenOn.AddSeconds(-vehicleStatusTO.WtStage4TakenOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["wtStage4TakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.WtStage4TakenOnStr = Convert.ToString(vehicleStatusTODT["wtStage4TakenOn"].ToString());

                    if (vehicleStatusTODT["wtStage4TakenBy"] != DBNull.Value)
                        vehicleStatusTO.WtStage4TakenBy = Convert.ToString(vehicleStatusTODT["wtStage4TakenBy"].ToString());

                    if (vehicleStatusTODT["wtStage5TakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.WtStage5TakenOn = Convert.ToDateTime(vehicleStatusTODT["wtStage5TakenOn"].ToString());
                        vehicleStatusTO.WtStage5TakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.WtStage5TakenOn);
                        vehicleStatusTO.WtStage5TakenOn = vehicleStatusTO.WtStage5TakenOn.AddSeconds(-vehicleStatusTO.WtStage5TakenOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["wtStage5TakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.WtStage5TakenOnStr = Convert.ToString(vehicleStatusTODT["wtStage5TakenOn"].ToString());

                    if (vehicleStatusTODT["wtStage5TakenBy"] != DBNull.Value)
                        vehicleStatusTO.WtStage5TakenBy = Convert.ToString(vehicleStatusTODT["wtStage5TakenBy"].ToString());

                    if (vehicleStatusTODT["wtStage6TakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.WtStage6TakenOn = Convert.ToDateTime(vehicleStatusTODT["wtStage6TakenOn"].ToString());
                        vehicleStatusTO.WtStage6TakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.WtStage6TakenOn);
                        vehicleStatusTO.WtStage6TakenOn = vehicleStatusTO.WtStage6TakenOn.AddSeconds(-vehicleStatusTO.WtStage6TakenOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["wtStage6TakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.WtStage6TakenOnStr = Convert.ToString(vehicleStatusTODT["wtStage6TakenOn"].ToString());

                    if (vehicleStatusTODT["wtStage6TakenBy"] != DBNull.Value)
                        vehicleStatusTO.WtStage6TakenBy = Convert.ToString(vehicleStatusTODT["wtStage6TakenBy"].ToString());

                    if (vehicleStatusTODT["wtStage7TakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.WtStage7TakenOn = Convert.ToDateTime(vehicleStatusTODT["wtStage7TakenOn"].ToString());
                        vehicleStatusTO.WtStage7TakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.WtStage7TakenOn);
                        vehicleStatusTO.WtStage7TakenOn = vehicleStatusTO.WtStage7TakenOn.AddSeconds(-vehicleStatusTO.WtStage7TakenOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["wtStage7TakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.WtStage7TakenOnStr = Convert.ToString(vehicleStatusTODT["wtStage7TakenOn"].ToString());

                    if (vehicleStatusTODT["wtStage7TakenBy"] != DBNull.Value)
                        vehicleStatusTO.WtStage7TakenBy = Convert.ToString(vehicleStatusTODT["wtStage7TakenBy"].ToString());

                    if (vehicleStatusTODT["tareWtTakenOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.TareWtTakenOn = Convert.ToDateTime(vehicleStatusTODT["tareWtTakenOn"].ToString());
                        vehicleStatusTO.TareWtTakenOnStr = Constants.GetDateWithFormate(vehicleStatusTO.TareWtTakenOn);
                        vehicleStatusTO.TareWtTakenOn = vehicleStatusTO.TareWtTakenOn.AddSeconds(-vehicleStatusTO.TareWtTakenOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["tareWtTakenOn"] != DBNull.Value)
                    //    vehicleStatusTO.TareWtTakenOnStr = Convert.ToString(vehicleStatusTODT["tareWtTakenOn"].ToString());

                    if (vehicleStatusTODT["tareWtTakenBy"] != DBNull.Value)
                        vehicleStatusTO.TareWtTakenBy = Convert.ToString(vehicleStatusTODT["tareWtTakenBy"].ToString());

                    if (vehicleStatusTODT["weighingCompletedOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.WeighingCompletedOn = Convert.ToDateTime(vehicleStatusTODT["weighingCompletedOn"].ToString());
                        vehicleStatusTO.WeighingCompletedOnStr = Constants.GetDateWithFormate(vehicleStatusTO.WeighingCompletedOn);
                        vehicleStatusTO.WeighingCompletedOn = vehicleStatusTO.WeighingCompletedOn.AddSeconds(-vehicleStatusTO.WeighingCompletedOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["weighingCompletedOn"] != DBNull.Value)
                    //    vehicleStatusTO.WeighingCompletedOnStr = Convert.ToString(vehicleStatusTODT["weighingCompletedOn"].ToString());

                    if (vehicleStatusTODT["weighingCompletedBy"] != DBNull.Value)
                        vehicleStatusTO.WeighingCompletedBy = Convert.ToString(vehicleStatusTODT["weighingCompletedBy"].ToString());

                    if (vehicleStatusTODT["vehicleOutOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.VehicleOutOn = Convert.ToDateTime(vehicleStatusTODT["vehicleOutOn"].ToString());
                        vehicleStatusTO.VehicleOutOnStr = Constants.GetDateWithFormate(vehicleStatusTO.VehicleOutOn);
                        vehicleStatusTO.VehicleOutOn = vehicleStatusTO.VehicleOutOn.AddSeconds(-vehicleStatusTO.VehicleOutOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["vehicleOutOn"] != DBNull.Value)
                    //    vehicleStatusTO.VehicleOutOnStr = Convert.ToString(vehicleStatusTODT["vehicleOutOn"].ToString());

                    if (vehicleStatusTODT["vehicleOutBy"] != DBNull.Value)
                        vehicleStatusTO.VehicleOutBy = Convert.ToString(vehicleStatusTODT["vehicleOutBy"].ToString());

                    if (vehicleStatusTODT["unloadingCompleOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.UnloadingCompletedOn = Convert.ToDateTime(vehicleStatusTODT["unloadingCompleOn"].ToString());
                        vehicleStatusTO.UnloadingCompletedOnStr = Constants.GetDateWithFormate(vehicleStatusTO.UnloadingCompletedOn);
                        vehicleStatusTO.UnloadingCompletedOn = vehicleStatusTO.UnloadingCompletedOn.AddSeconds(-vehicleStatusTO.UnloadingCompletedOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["unloadingCompleOn"] != DBNull.Value)
                    //    vehicleStatusTO.UnloadingCompletedOnStr = Convert.ToString(vehicleStatusTODT["unloadingCompleOn"].ToString());

                    if (vehicleStatusTODT["unloadingCompleBy"] != DBNull.Value)
                        vehicleStatusTO.UnloadingCompletedBy = Convert.ToString(vehicleStatusTODT["unloadingCompleBy"].ToString());

                    if (vehicleStatusTODT["gradingCompleOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.GradingCompletedOn = Convert.ToDateTime(vehicleStatusTODT["gradingCompleOn"].ToString());
                        vehicleStatusTO.GradingCompletedOnStr = Constants.GetDateWithFormate(vehicleStatusTO.GradingCompletedOn);
                        vehicleStatusTO.GradingCompletedOn = vehicleStatusTO.GradingCompletedOn.AddSeconds(-vehicleStatusTO.GradingCompletedOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["gradingCompleOn"] != DBNull.Value)
                    //    vehicleStatusTO.GradingCompletedOnStr = Convert.ToString(vehicleStatusTODT["gradingCompleOn"].ToString());

                    if (vehicleStatusTODT["gradingCompleBy"] != DBNull.Value)
                        vehicleStatusTO.GradingCompletedBy = Convert.ToString(vehicleStatusTODT["gradingCompleBy"].ToString());

                    if (vehicleStatusTODT["recoveryCompleOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.RecoveryCompletedOn = Convert.ToDateTime(vehicleStatusTODT["recoveryCompleOn"].ToString());
                        vehicleStatusTO.RecoveryCompletedOnStr = Constants.GetDateWithFormate(vehicleStatusTO.RecoveryCompletedOn);
                        vehicleStatusTO.RecoveryCompletedOn = vehicleStatusTO.RecoveryCompletedOn.AddSeconds(-vehicleStatusTO.RecoveryCompletedOn.Second);
                    }
                        

                    //if (vehicleStatusTODT["recoveryCompleOn"] != DBNull.Value)
                    //    vehicleStatusTO.RecoveryCompletedOnStr = Convert.ToString(vehicleStatusTODT["recoveryCompleOn"].ToString());

                    if (vehicleStatusTODT["recoveryCompleBy"] != DBNull.Value)
                        vehicleStatusTO.RecoveryCompletedBy = Convert.ToString(vehicleStatusTODT["recoveryCompleBy"].ToString());

                    if (vehicleStatusTODT["correctionCompleOn"] != DBNull.Value)
                    {
                        vehicleStatusTO.CorrectionCompletedOn = Convert.ToDateTime(vehicleStatusTODT["correctionCompleOn"].ToString());
                        vehicleStatusTO.CorrectionCompletedOnStr = Constants.GetDateWithFormate(vehicleStatusTO.CorrectionCompletedOn);
                        vehicleStatusTO.CorrectionCompletedOn = vehicleStatusTO.CorrectionCompletedOn.AddSeconds(-vehicleStatusTO.CorrectionCompletedOn.Second);
                    }

                    //if (vehicleStatusTODT["correctionCompleOn"] != DBNull.Value)
                    //    vehicleStatusTO.CorrectionCompletedOnStr = Convert.ToString(vehicleStatusTODT["correctionCompleOn"].ToString());

                    if (vehicleStatusTODT["correctionCompleBy"] != DBNull.Value)
                        vehicleStatusTO.CorrectionCompletedBy = Convert.ToString(vehicleStatusTODT["correctionCompleBy"].ToString());

                    if (vehicleStatusTODT["supervisorName"] != DBNull.Value)
                        vehicleStatusTO.SupervisorName = Convert.ToString(vehicleStatusTODT["supervisorName"].ToString());

                    if (vehicleStatusTODT["graderName"] != DBNull.Value)
                        vehicleStatusTO.GraderName = Convert.ToString(vehicleStatusTODT["graderName"].ToString());

                    if (vehicleStatusTODT["engineerName"] != DBNull.Value)
                        vehicleStatusTO.EngineerName = Convert.ToString(vehicleStatusTODT["engineerName"].ToString());

                    if (vehicleStatusTODT["isCorrectionCompleted"] != DBNull.Value)
                        vehicleStatusTO.IsCorrectionCompleted = Convert.ToInt32(vehicleStatusTODT["isCorrectionCompleted"].ToString());

                    vehicleStatusDateTOList.Add(vehicleStatusTO);
                }
            }
            return vehicleStatusDateTOList;
        }

        public List<TblPurchaseScheduleSummaryTO> ConvertDTToListForUnloadingScrap(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                while (tblPurchaseScheduleSummaryTODT.Read())
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();

                    if (tblPurchaseScheduleSummaryTODT["cOrNcId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNcId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNcId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["unloadingdate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnloadingDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["unloadingdate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["total"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Total = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["total"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["prodClassDesc"].ToString());
                 
                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);
                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }


        public List<TblPurchaseScheduleSummaryTO> ConvertDTToList(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                List<DropDownTO> phasesList = new List<DropDownTO>();
                phasesList = getListofPhasesUsedForUnloadingQty();
                while (tblPurchaseScheduleSummaryTODT.Read())
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                    if (tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["isVehicleVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsVehicleVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isVehicleVerified"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["createdBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["createdByName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedByName = Convert.ToString(tblPurchaseScheduleSummaryTODT["createdByName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["updatedBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["updatedByName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedByName = Convert.ToString(tblPurchaseScheduleSummaryTODT["updatedByName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["scheduleDate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["updatedOn"].ToString());
                   
                        
                    if (tblPurchaseScheduleSummaryTODT["scheduleQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.OrgScheduleQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["scheduleQty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Padta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padta"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padtaForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PadtaForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padtaForEnquiry"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["remark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Remark = Convert.ToString(tblPurchaseScheduleSummaryTODT["remark"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["statusDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusDesc"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["colorCode"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ColorCode = Convert.ToString(tblPurchaseScheduleSummaryTODT["colorCode"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["firmName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierName = Convert.ToString(tblPurchaseScheduleSummaryTODT["firmName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNc = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["narrationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NarrationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["narrationId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["gradingCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GradingCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["gradingCompCnt"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["unloadingCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnloadingCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["unloadingCompCnt"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["recoveryCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryCompCnt"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["wtStageCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.WtStageCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["wtStageCompCnt"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["narration"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Narration = Convert.ToString(tblPurchaseScheduleSummaryTODT["narration"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Rate = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["bookingRate"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["displayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MaterailType = Convert.ToString(tblPurchaseScheduleSummaryTODT["displayName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rate_band_costing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateBand = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rate_band_costing"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["stateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["stateName"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["areaName"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.AreaName = Convert.ToString(tblPurchaseScheduleSummaryTODT["areaName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prodClassId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["prodClassDesc"].ToString());




                    if (tblPurchaseScheduleSummaryTODT["stateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["stateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["engineerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["engineerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supervisorId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PhotographerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["photographerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographer"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Photographer = Convert.ToString(tblPurchaseScheduleSummaryTODT["photographer"]);


                    if (tblPurchaseScheduleSummaryTODT["qualityFlag"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.QualityFlag = Convert.ToBoolean(tblPurchaseScheduleSummaryTODT["qualityFlag"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["driverName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverName = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverName"]);

                    if (tblPurchaseScheduleSummaryTODT["driverContactNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverContactNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverContactNo"]);

                    if (tblPurchaseScheduleSummaryTODT["transporterName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransporterName = Convert.ToString(tblPurchaseScheduleSummaryTODT["transporterName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleTypeId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["targetPadta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TargetPadta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["targetPadta"]);

                    if (tblPurchaseScheduleSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Freight = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freight"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["containerNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ContainerNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["containerNo"]);

                    if (tblPurchaseScheduleSummaryTODT["lotSize"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LotSize = Convert.ToString(tblPurchaseScheduleSummaryTODT["lotSize"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleStateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleStateName"]);

                    if (tblPurchaseScheduleSummaryTODT["locationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LocationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["locationId"]);

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNcId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"]);

                    // if (tblPurchaseScheduleSummaryTODT["locationDesc"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["locationDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["userDisplayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorName = Convert.ToString(tblPurchaseScheduleSummaryTODT["userDisplayName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleCatId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleCatId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleCatId"]);

                    if (tblPurchaseScheduleSummaryTODT["prevStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prevStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["previousStatusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["previousStatusName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehiclePhaseId"]);


                    double vehQty = 0;

                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                    {
                        vehQty = tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());
                    }
                        
                   
                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                    {
                        DropDownTO Temp = phasesList.Where(w => w.Value == tblPurchaseScheduleSummaryTONew.VehiclePhaseId).FirstOrDefault();
                        if (Temp != null)
                        {
                            tblPurchaseScheduleSummaryTONew.OrgUnloadedQty = vehQty;
                        }
                        else
                        {
                            tblPurchaseScheduleSummaryTONew.OrgUnloadedQty = 0;
                        }
                    }

                    if (tblPurchaseScheduleSummaryTODT["sequanceNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseSequanceNo = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["sequanceNo"]);

                    if (tblPurchaseScheduleSummaryTODT["phaseName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseName = Convert.ToString(tblPurchaseScheduleSummaryTODT["phaseName"]);

                    if (tblPurchaseScheduleSummaryTODT["isActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isActive"]);

                    if (tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ParentPurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"]);

                    if (tblPurchaseScheduleSummaryTODT["location"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["location"]);

                    if (tblPurchaseScheduleSummaryTODT["rootScheduleId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RootScheduleId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rootScheduleId"]);

                    if (tblPurchaseScheduleSummaryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["enqDisplayNo"]);

                    if (tblPurchaseScheduleSummaryTODT["isRecovery"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsRecovery = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isRecovery"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryBy"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["recoveryOn"]);

                    if (tblPurchaseScheduleSummaryTODT["isWeighing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsWeighing = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isWeighing"]);

                    // Deepali[07-02-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["graderId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GraderId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["graderId"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["isGradingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsCorrectionCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["corretionCompletedOn"] != DBNull.Value)
                    {
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["corretionCompletedOn"].ToString());
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOnStr = tblPurchaseScheduleSummaryTONew.CorretionCompletedOn.ToString(StaticStuff.Constants.DefaultDateFormat);
                    }


                    if (tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsUnloadingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"].ToString());

                    // Priyanka[28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["commercialApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialApproval"].ToString());

                    //Priyanka [28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["purchaseManager"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseManager = Convert.ToString(tblPurchaseScheduleSummaryTODT["purchaseManager"]);

                    //Priyanka [04-01-2019] Added

                    if (tblPurchaseScheduleSummaryTODT["UserId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UserId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["UserId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["GreaderName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GreaderName = Convert.ToString(tblPurchaseScheduleSummaryTODT["GreaderName"]);

                    if (tblPurchaseScheduleSummaryTODT["EngineerName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerName = Convert.ToString(tblPurchaseScheduleSummaryTODT["EngineerName"]);

                    //Priyanka [18-02-2019]
                    if (tblPurchaseScheduleSummaryTODT["commercialVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialVerified"].ToString());

                    //Prajakta[2019-02-27] Added
                    if (tblPurchaseScheduleSummaryTODT["isBoth"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsBoth = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isBoth"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusRemark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusRemark = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusRemark"]);

                    if (tblPurchaseScheduleSummaryTODT["navigationUrl"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NavigationUrl = Convert.ToString(tblPurchaseScheduleSummaryTODT["navigationUrl"]);

                    if (tblPurchaseScheduleSummaryTODT["scheduleHistoryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleHistoryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["scheduleHistoryId"]);

                    if (tblPurchaseScheduleSummaryTODT["historyPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.HistoryPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["historyPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["rejectStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["acceptStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.AcceptStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["acceptStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["isLatest"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsLatest = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isLatest"]);

                    if (tblPurchaseScheduleSummaryTODT["isApproved"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsApproved = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isApproved"]);

                    if (tblPurchaseScheduleSummaryTODT["isIgnoreApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsIgnoreApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isIgnoreApproval"]);


                    if (tblPurchaseScheduleSummaryTODT["rejectPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["acceptPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.AcceptPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["acceptPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["historyIsActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.HistoryIsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["historyIsActive"]);

                    if (tblPurchaseScheduleSummaryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsFixed = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isFixed"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["transportAmtPerMT"].ToString());

                    //Prajakta[2019-05-08] Added
                    if (tblPurchaseScheduleSummaryTODT["rejectedQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rejectedQty"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectedBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["rejectedOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedByUserName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedByUserName = Convert.ToString(tblPurchaseScheduleSummaryTODT["rejectedByUserName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonId = (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["corretionCompletedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["corretionCompletedOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rateForC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateForC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rateForC"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rateForNC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rateForNC"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["modbusRefId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ModbusRefId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["modbusRefId"]);
                    if (tblPurchaseScheduleSummaryTODT["gateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["gateId"]);
                    if (tblPurchaseScheduleSummaryTODT["portNumber"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PortNumber = Convert.ToString(tblPurchaseScheduleSummaryTODT["portNumber"]);
                    if (tblPurchaseScheduleSummaryTODT["ioTUrl"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IoTUrl = Convert.ToString(tblPurchaseScheduleSummaryTODT["ioTUrl"]);
                    if (tblPurchaseScheduleSummaryTODT["machineIP"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MachineIP = Convert.ToString(tblPurchaseScheduleSummaryTODT["machineIP"]);

                    if (tblPurchaseScheduleSummaryTODT["latestWtTakenOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LatestWtTakenOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["latestWtTakenOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isAutoSpotVehSauda"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsAutoSpotVehSauda = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isAutoSpotVehSauda"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isVehicleOut"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsVehicleOut = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isVehicleOut"]);


                    if (tblPurchaseScheduleSummaryTODT["isDBup"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsDBup = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isDBup"]);

                    if (tblPurchaseScheduleSummaryTODT["isGradingUnldCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingUnldCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingUnldCompleted"]);

                    if (tblPurchaseScheduleSummaryTODT["refRateofV48Var"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RefRateofV48Var = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateofV48Var"]);
                    if (tblPurchaseScheduleSummaryTODT["refRateC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RefRateC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateC"]);

                    if (tblPurchaseScheduleSummaryTONew.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
                    {
                        if (tblPurchaseScheduleSummaryTODT["refRateForSaudaC"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.RefRateForSauda = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateForSaudaC"]);
                    }
                    else
                    {
                        if (tblPurchaseScheduleSummaryTODT["refRateForSaudaNC"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.RefRateForSaudaNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateForSaudaNC"]);
                    }
                    if (tblPurchaseScheduleSummaryTODT["height"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Height = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["height"]);

                    if (tblPurchaseScheduleSummaryTODT["width"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Width = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["width"]);

                    if (tblPurchaseScheduleSummaryTODT["length"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Length = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["length"]);


                    //if (tblPurchaseScheduleSummaryTODT["refRateC"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.RefRateC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateC"]);

                    //if (tblPurchaseScheduleSummaryTODT["refRateofV48Var"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.RefRateofV48Var = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateofV48Var"]);


                    if (tblPurchaseScheduleSummaryTODT["reportedDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ReportedDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["reportedDate"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTon"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnldDatePadtaPerTon = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTon"]);

                    if (tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTonForNC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnldDatePadtaPerTonForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTonForNC"]);

                    if (tblPurchaseScheduleSummaryTODT["processChargePerVeh"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProcessChargePerVeh = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["processChargePerVeh"]);

                    if (tblPurchaseScheduleSummaryTODT["processChargePerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProcessChargePerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["processChargePerMT"]);

                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehRejectReasonDesc"]);
                    //Added by minal 25 March 2021
                    if (tblPurchaseScheduleSummaryTODT["freightAmount"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.FreightAmount = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freightAmount"]);
                    //if (tblPurchaseScheduleSummaryTODT["partyName"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.PartyName = Convert.ToString(tblPurchaseScheduleSummaryTODT["partyName"]);

                    //Reshma[01-3-21]
                    if (tblPurchaseScheduleSummaryTODT["GradingComplOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GradingComplOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["GradingComplOn"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["linkSaudaNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LinkSaudaNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["linkSaudaNo"]);

                    if(!String.IsNullOrEmpty(tblPurchaseScheduleSummaryTONew.LinkSaudaNo))
                    {
                        tblPurchaseScheduleSummaryTONew.EnqDisplayNo = tblPurchaseScheduleSummaryTONew.LinkSaudaNo;
                    }

                    if (tblPurchaseScheduleSummaryTODT["saudaTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SaudaTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["saudaTypeId"]);

                    if (tblPurchaseScheduleSummaryTODT["enqQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EnqQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["enqQty"]);

                    //Gokul [14-03-21]
                    if (tblPurchaseScheduleSummaryTONew.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
                    {
                        if (tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForOrder"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.CorrectionPadtaAmt = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForOrder"].ToString());
                    }
                    else
                    {
                        if (tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForEnquiry"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.CorrectionPadtaAmt = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForEnquiry"].ToString());

                    }
                    if (tblPurchaseScheduleSummaryTODT["isFreightAdded"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsFreightAdded = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isFreightAdded"]);

                    
                    if (tblPurchaseScheduleSummaryTODT["correNarration"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CorreNarration = Convert.ToString(tblPurchaseScheduleSummaryTODT["correNarration"]);

                    //if (String.IsNullOrEmpty(tblPurchaseScheduleSummaryTONew.CorreNarration))
                    {
                        if (tblPurchaseScheduleSummaryTODT["saudaRemark"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.SaudaNarration = Convert.ToString(tblPurchaseScheduleSummaryTODT["saudaRemark"]);

                        if (tblPurchaseScheduleSummaryTODT["saudaNarration"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.SaudaNarration = Convert.ToString(tblPurchaseScheduleSummaryTODT["saudaNarration"]);
                    }


                    if (tblPurchaseScheduleSummaryTODT["saudaSupplierName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SaudaSupplierName = Convert.ToString(tblPurchaseScheduleSummaryTODT["saudaSupplierName"]);

                    
                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);

                    


                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }
        public List<TblPurchaseScheduleSummaryTO> ConvertDTToListForVehDisplay(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                List<DropDownTO> phasesList = new List<DropDownTO>();
                phasesList = getListofPhasesUsedForUnloadingQty();
                while (tblPurchaseScheduleSummaryTODT.Read())
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                    if (tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["isVehicleVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsVehicleVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isVehicleVerified"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["createdBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["createdByName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedByName = Convert.ToString(tblPurchaseScheduleSummaryTODT["createdByName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["updatedBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["updatedByName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedByName = Convert.ToString(tblPurchaseScheduleSummaryTODT["updatedByName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["scheduleDate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["updatedOn"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["scheduleQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.OrgScheduleQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["scheduleQty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Padta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padta"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padtaForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PadtaForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padtaForEnquiry"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["remark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Remark = Convert.ToString(tblPurchaseScheduleSummaryTODT["remark"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["statusDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusDesc"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["colorCode"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ColorCode = Convert.ToString(tblPurchaseScheduleSummaryTODT["colorCode"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["firmName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierName = Convert.ToString(tblPurchaseScheduleSummaryTODT["firmName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNc = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["narrationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NarrationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["narrationId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["gradingCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GradingCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["gradingCompCnt"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["unloadingCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnloadingCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["unloadingCompCnt"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["recoveryCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryCompCnt"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["wtStageCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.WtStageCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["wtStageCompCnt"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["narration"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Narration = Convert.ToString(tblPurchaseScheduleSummaryTODT["narration"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Rate = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["bookingRate"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["displayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MaterailType = Convert.ToString(tblPurchaseScheduleSummaryTODT["displayName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rate_band_costing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateBand = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rate_band_costing"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["stateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["stateName"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["areaName"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.AreaName = Convert.ToString(tblPurchaseScheduleSummaryTODT["areaName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prodClassId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["prodClassDesc"].ToString());




                    if (tblPurchaseScheduleSummaryTODT["stateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["stateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["engineerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["engineerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supervisorId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PhotographerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["photographerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographer"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Photographer = Convert.ToString(tblPurchaseScheduleSummaryTODT["photographer"]);


                    if (tblPurchaseScheduleSummaryTODT["qualityFlag"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.QualityFlag = Convert.ToBoolean(tblPurchaseScheduleSummaryTODT["qualityFlag"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["driverName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverName = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverName"]);

                    if (tblPurchaseScheduleSummaryTODT["driverContactNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverContactNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverContactNo"]);

                    if (tblPurchaseScheduleSummaryTODT["transporterName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransporterName = Convert.ToString(tblPurchaseScheduleSummaryTODT["transporterName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleTypeId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["targetPadta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TargetPadta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["targetPadta"]);

                    if (tblPurchaseScheduleSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Freight = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freight"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["containerNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ContainerNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["containerNo"]);

                    if (tblPurchaseScheduleSummaryTODT["lotSize"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LotSize = Convert.ToString(tblPurchaseScheduleSummaryTODT["lotSize"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleStateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleStateName"]);

                    if (tblPurchaseScheduleSummaryTODT["locationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LocationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["locationId"]);

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNcId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"]);

                    // if (tblPurchaseScheduleSummaryTODT["locationDesc"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["locationDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["userDisplayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorName = Convert.ToString(tblPurchaseScheduleSummaryTODT["userDisplayName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleCatId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleCatId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleCatId"]);

                    if (tblPurchaseScheduleSummaryTODT["prevStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prevStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["previousStatusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["previousStatusName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehiclePhaseId"]);


                    double vehQty = 0;

                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                    {
                        vehQty = tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());
                    }

                    //Prajakta[2021-05-13] Added to show link veh qty for split vehicle
                    if (tblPurchaseScheduleSummaryTODT["linkedQty"] != DBNull.Value)
                    {
                        tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["linkedQty"].ToString());
                        vehQty = tblPurchaseScheduleSummaryTONew.Qty;
                    }


                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                    {
                        DropDownTO Temp = phasesList.Where(w => w.Value == tblPurchaseScheduleSummaryTONew.VehiclePhaseId).FirstOrDefault();
                        if (Temp != null)
                        {
                            tblPurchaseScheduleSummaryTONew.OrgUnloadedQty = vehQty;
                        }
                        else
                        {
                            tblPurchaseScheduleSummaryTONew.OrgUnloadedQty = 0;
                        }
                    }

                    if (tblPurchaseScheduleSummaryTODT["sequanceNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseSequanceNo = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["sequanceNo"]);

                    if (tblPurchaseScheduleSummaryTODT["phaseName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseName = Convert.ToString(tblPurchaseScheduleSummaryTODT["phaseName"]);

                    if (tblPurchaseScheduleSummaryTODT["isActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isActive"]);

                    if (tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ParentPurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"]);

                    if (tblPurchaseScheduleSummaryTODT["location"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["location"]);

                    if (tblPurchaseScheduleSummaryTODT["rootScheduleId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RootScheduleId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rootScheduleId"]);

                    if (tblPurchaseScheduleSummaryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["enqDisplayNo"]);

                    if (tblPurchaseScheduleSummaryTODT["isRecovery"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsRecovery = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isRecovery"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryBy"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["recoveryOn"]);

                    if (tblPurchaseScheduleSummaryTODT["isWeighing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsWeighing = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isWeighing"]);

                    // Deepali[07-02-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["graderId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GraderId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["graderId"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["isGradingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsCorrectionCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["corretionCompletedOn"] != DBNull.Value)
                    {
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["corretionCompletedOn"].ToString());
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOnStr = tblPurchaseScheduleSummaryTONew.CorretionCompletedOn.ToString(StaticStuff.Constants.DefaultDateFormat);
                    }


                    if (tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsUnloadingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"].ToString());

                    // Priyanka[28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["commercialApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialApproval"].ToString());

                    //Priyanka [28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["purchaseManager"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseManager = Convert.ToString(tblPurchaseScheduleSummaryTODT["purchaseManager"]);

                    //Priyanka [04-01-2019] Added

                    if (tblPurchaseScheduleSummaryTODT["UserId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UserId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["UserId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["GreaderName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GreaderName = Convert.ToString(tblPurchaseScheduleSummaryTODT["GreaderName"]);

                    if (tblPurchaseScheduleSummaryTODT["EngineerName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerName = Convert.ToString(tblPurchaseScheduleSummaryTODT["EngineerName"]);

                    //Priyanka [18-02-2019]
                    if (tblPurchaseScheduleSummaryTODT["commercialVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialVerified"].ToString());

                    //Prajakta[2019-02-27] Added
                    if (tblPurchaseScheduleSummaryTODT["isBoth"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsBoth = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isBoth"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusRemark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusRemark = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusRemark"]);

                    if (tblPurchaseScheduleSummaryTODT["navigationUrl"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NavigationUrl = Convert.ToString(tblPurchaseScheduleSummaryTODT["navigationUrl"]);

                    if (tblPurchaseScheduleSummaryTODT["scheduleHistoryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleHistoryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["scheduleHistoryId"]);

                    if (tblPurchaseScheduleSummaryTODT["historyPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.HistoryPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["historyPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["rejectStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["acceptStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.AcceptStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["acceptStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["isLatest"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsLatest = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isLatest"]);

                    if (tblPurchaseScheduleSummaryTODT["isApproved"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsApproved = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isApproved"]);

                    if (tblPurchaseScheduleSummaryTODT["isIgnoreApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsIgnoreApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isIgnoreApproval"]);


                    if (tblPurchaseScheduleSummaryTODT["rejectPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["acceptPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.AcceptPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["acceptPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["historyIsActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.HistoryIsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["historyIsActive"]);

                    if (tblPurchaseScheduleSummaryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsFixed = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isFixed"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["transportAmtPerMT"].ToString());

                    //Prajakta[2019-05-08] Added
                    if (tblPurchaseScheduleSummaryTODT["rejectedQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rejectedQty"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectedBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["rejectedOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedByUserName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedByUserName = Convert.ToString(tblPurchaseScheduleSummaryTODT["rejectedByUserName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonId = (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["corretionCompletedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["corretionCompletedOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rateForC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateForC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rateForC"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rateForNC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rateForNC"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["modbusRefId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ModbusRefId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["modbusRefId"]);
                    if (tblPurchaseScheduleSummaryTODT["gateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["gateId"]);
                    if (tblPurchaseScheduleSummaryTODT["portNumber"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PortNumber = Convert.ToString(tblPurchaseScheduleSummaryTODT["portNumber"]);
                    if (tblPurchaseScheduleSummaryTODT["ioTUrl"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IoTUrl = Convert.ToString(tblPurchaseScheduleSummaryTODT["ioTUrl"]);
                    if (tblPurchaseScheduleSummaryTODT["machineIP"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MachineIP = Convert.ToString(tblPurchaseScheduleSummaryTODT["machineIP"]);

                    if (tblPurchaseScheduleSummaryTODT["latestWtTakenOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LatestWtTakenOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["latestWtTakenOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isAutoSpotVehSauda"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsAutoSpotVehSauda = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isAutoSpotVehSauda"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isVehicleOut"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsVehicleOut = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isVehicleOut"]);


                    if (tblPurchaseScheduleSummaryTODT["isDBup"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsDBup = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isDBup"]);

                    if (tblPurchaseScheduleSummaryTODT["isGradingUnldCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingUnldCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingUnldCompleted"]);

                    if (tblPurchaseScheduleSummaryTODT["refRateofV48Var"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RefRateofV48Var = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateofV48Var"]);
                    if (tblPurchaseScheduleSummaryTODT["refRateC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RefRateC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateC"]);

                    if (tblPurchaseScheduleSummaryTONew.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
                    {
                        if (tblPurchaseScheduleSummaryTODT["refRateForSaudaC"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.RefRateForSauda = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateForSaudaC"]);
                    }
                    else
                    {
                        if (tblPurchaseScheduleSummaryTODT["refRateForSaudaNC"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.RefRateForSaudaNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateForSaudaNC"]);
                    }
                    if (tblPurchaseScheduleSummaryTODT["height"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Height = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["height"]);

                    if (tblPurchaseScheduleSummaryTODT["width"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Width = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["width"]);

                    if (tblPurchaseScheduleSummaryTODT["length"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Length = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["length"]);


                    //if (tblPurchaseScheduleSummaryTODT["refRateC"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.RefRateC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateC"]);

                    //if (tblPurchaseScheduleSummaryTODT["refRateofV48Var"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.RefRateofV48Var = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateofV48Var"]);


                    if (tblPurchaseScheduleSummaryTODT["reportedDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ReportedDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["reportedDate"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTon"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnldDatePadtaPerTon = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTon"]);

                    if (tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTonForNC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnldDatePadtaPerTonForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTonForNC"]);

                    if (tblPurchaseScheduleSummaryTODT["processChargePerVeh"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProcessChargePerVeh = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["processChargePerVeh"]);

                    if (tblPurchaseScheduleSummaryTODT["processChargePerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProcessChargePerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["processChargePerMT"]);

                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehRejectReasonDesc"]);
                    //Added by minal 25 March 2021
                    if (tblPurchaseScheduleSummaryTODT["freightAmount"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.FreightAmount = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freightAmount"]);
                    //if (tblPurchaseScheduleSummaryTODT["partyName"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.PartyName = Convert.ToString(tblPurchaseScheduleSummaryTODT["partyName"]);

                    //Reshma[01-3-21]
                    if (tblPurchaseScheduleSummaryTODT["GradingComplOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GradingComplOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["GradingComplOn"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["linkSaudaNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LinkSaudaNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["linkSaudaNo"]);

                    if (!String.IsNullOrEmpty(tblPurchaseScheduleSummaryTONew.LinkSaudaNo))
                    {
                        tblPurchaseScheduleSummaryTONew.EnqDisplayNo = tblPurchaseScheduleSummaryTONew.LinkSaudaNo;
                    }

                    if (tblPurchaseScheduleSummaryTODT["saudaTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SaudaTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["saudaTypeId"]);

                    if (tblPurchaseScheduleSummaryTODT["enqQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EnqQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["enqQty"]);

                    //Gokul [14-03-21]
                    if (tblPurchaseScheduleSummaryTONew.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
                    {
                        if (tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForOrder"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.CorrectionPadtaAmt = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForOrder"].ToString());
                    }
                    else
                    {
                        if (tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForEnquiry"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.CorrectionPadtaAmt = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForEnquiry"].ToString());

                    }
                    if (tblPurchaseScheduleSummaryTODT["isFreightAdded"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsFreightAdded = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isFreightAdded"]);


                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);




                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }

        public List<DropDownTO> getListofPhasesUsedForUnloadingQty()
        {
            List<DropDownTO> phasesListForUnloadingQty = new List<DropDownTO>();
            DropDownTO commonTO = new DropDownTO();

            commonTO.Text = "Unloading Completed";
            commonTO.Value = (int)Constants.PurchaseVehiclePhasesE.UNLOADING_COMPLETED;
            phasesListForUnloadingQty.Add(commonTO);


            commonTO = new DropDownTO();
            commonTO.Text = "Grading Completed";
            commonTO.Value = (int)Constants.PurchaseVehiclePhasesE.GRADING;
            phasesListForUnloadingQty.Add(commonTO);

            commonTO = new DropDownTO();
            commonTO.Text = "Recovery Completed";
            commonTO.Value = (int)Constants.PurchaseVehiclePhasesE.RECOVERY;
            phasesListForUnloadingQty.Add(commonTO);


            commonTO = new DropDownTO();
            commonTO.Text = "Correction Completed";
            commonTO.Value = (int)Constants.PurchaseVehiclePhasesE.CORRECTIONS;
            phasesListForUnloadingQty.Add(commonTO);

            return phasesListForUnloadingQty;
        }

        public List<TblPurchaseScheduleSummaryTO> ConvertDTToListLight(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                while (tblPurchaseScheduleSummaryTODT.Read())
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                    if (tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["createdBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["updatedBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["scheduleDate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.OrgScheduleQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["scheduleQty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Padta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padta"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padtaForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PadtaForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padtaForEnquiry"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["remark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Remark = Convert.ToString(tblPurchaseScheduleSummaryTODT["remark"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNc = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["narrationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NarrationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["narrationId"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["engineerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["engineerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supervisorId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PhotographerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["photographerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleTypeId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Freight = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freight"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["containerNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ContainerNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["containerNo"]);

                    if (tblPurchaseScheduleSummaryTODT["lotSize"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LotSize = Convert.ToString(tblPurchaseScheduleSummaryTODT["lotSize"]);

                    if (tblPurchaseScheduleSummaryTODT["locationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LocationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["locationId"]);


                    if (tblPurchaseScheduleSummaryTODT["vehicleCatId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleCatId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleCatId"]);


                    if (tblPurchaseScheduleSummaryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehiclePhaseId"]);


                    if (tblPurchaseScheduleSummaryTODT["isActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isActive"]);

                    if (tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ParentPurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"]);

                    if (tblPurchaseScheduleSummaryTODT["location"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["location"]);

                    if (tblPurchaseScheduleSummaryTODT["rootScheduleId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RootScheduleId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rootScheduleId"]);


                    if (tblPurchaseScheduleSummaryTODT["isRecovery"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsRecovery = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isRecovery"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryBy"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["recoveryOn"]);

                    if (tblPurchaseScheduleSummaryTODT["isWeighing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsWeighing = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isWeighing"]);

                    // Deepali[07-02-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["graderId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GraderId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["graderId"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["isGradingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsCorrectionCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsUnloadingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"].ToString());

                    // Priyanka[28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["commercialApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialApproval"].ToString());

                    //Priyanka [18-02-2019]
                    if (tblPurchaseScheduleSummaryTODT["commercialVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialVerified"].ToString());

                    //Prajakta[2019-02-27] Added
                    if (tblPurchaseScheduleSummaryTODT["isBoth"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsBoth = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isBoth"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsFixed = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isFixed"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["transportAmtPerMT"].ToString());

                    //Prajakta[2019-05-08] Added
                    if (tblPurchaseScheduleSummaryTODT["rejectedQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rejectedQty"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectedBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["rejectedOn"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonId = (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["corretionCompletedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["corretionCompletedOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["latestWtTakenOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LatestWtTakenOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["latestWtTakenOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["height"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Height = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["height"]);

                    if (tblPurchaseScheduleSummaryTODT["width"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Width = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["width"]);

                    if (tblPurchaseScheduleSummaryTODT["length"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Length = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["length"]);

                    if (tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTon"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnldDatePadtaPerTon = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTon"]);

                    if (tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTonForNC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnldDatePadtaPerTonForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTonForNC"]);

                    if (tblPurchaseScheduleSummaryTODT["processChargePerVeh"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProcessChargePerVeh = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["processChargePerVeh"]);

                    if (tblPurchaseScheduleSummaryTODT["processChargePerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProcessChargePerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["processChargePerMT"]);

                    if (tblPurchaseScheduleSummaryTODT["correNarration"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CorreNarration = Convert.ToString(tblPurchaseScheduleSummaryTODT["correNarration"]);


                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);
                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }


        public List<TblPurchaseScheduleSummaryTO> ConvertDTToListForPendingFlags(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                while (tblPurchaseScheduleSummaryTODT.Read())
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                    if (tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["isVehicleVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsVehicleVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isVehicleVerified"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["createdBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["updatedBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["scheduleDate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.OrgScheduleQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["scheduleQty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Padta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padta"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padtaForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PadtaForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padtaForEnquiry"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["remark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Remark = Convert.ToString(tblPurchaseScheduleSummaryTODT["remark"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["statusDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusDesc"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["colorCode"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ColorCode = Convert.ToString(tblPurchaseScheduleSummaryTODT["colorCode"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["firmName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierName = Convert.ToString(tblPurchaseScheduleSummaryTODT["firmName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNc = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Rate = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["bookingRate"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["displayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MaterailType = Convert.ToString(tblPurchaseScheduleSummaryTODT["displayName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rate_band_costing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateBand = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rate_band_costing"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["stateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["stateName"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["areaName"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.AreaName = Convert.ToString(tblPurchaseScheduleSummaryTODT["areaName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prodClassId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["prodClassDesc"].ToString());




                    if (tblPurchaseScheduleSummaryTODT["stateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["stateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["engineerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["engineerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supervisorId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PhotographerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["photographerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["qualityFlag"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.QualityFlag = Convert.ToBoolean(tblPurchaseScheduleSummaryTODT["qualityFlag"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["driverName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverName = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverName"]);

                    if (tblPurchaseScheduleSummaryTODT["driverContactNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverContactNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverContactNo"]);

                    if (tblPurchaseScheduleSummaryTODT["transporterName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransporterName = Convert.ToString(tblPurchaseScheduleSummaryTODT["transporterName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleTypeId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Freight = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freight"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["containerNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ContainerNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["containerNo"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleStateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleStateName"]);

                    if (tblPurchaseScheduleSummaryTODT["locationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LocationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["locationId"]);

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNcId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"]);

                    // if (tblPurchaseScheduleSummaryTODT["locationDesc"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["locationDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["userDisplayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorName = Convert.ToString(tblPurchaseScheduleSummaryTODT["userDisplayName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleCatId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleCatId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleCatId"]);

                    if (tblPurchaseScheduleSummaryTODT["prevStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prevStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["previousStatusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["previousStatusName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehiclePhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["sequanceNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseSequanceNo = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["sequanceNo"]);

                    if (tblPurchaseScheduleSummaryTODT["phaseName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseName = Convert.ToString(tblPurchaseScheduleSummaryTODT["phaseName"]);

                    if (tblPurchaseScheduleSummaryTODT["isActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isActive"]);

                    if (tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ParentPurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"]);

                    if (tblPurchaseScheduleSummaryTODT["location"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["location"]);

                    if (tblPurchaseScheduleSummaryTODT["rootScheduleId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RootScheduleId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rootScheduleId"]);

                    if (tblPurchaseScheduleSummaryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["enqDisplayNo"]);

                    if (tblPurchaseScheduleSummaryTODT["isRecovery"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsRecovery = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isRecovery"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryBy"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["recoveryOn"]);

                    if (tblPurchaseScheduleSummaryTODT["isWeighing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsWeighing = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isWeighing"]);

                    // Deepali[07-02-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["graderId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GraderId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["graderId"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["isGradingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingCompleted"].ToString());

                    // Priyanka[28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["commercialApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialApproval"].ToString());

                    //Priyanka [28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["purchaseManager"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseManager = Convert.ToString(tblPurchaseScheduleSummaryTODT["purchaseManager"]);

                    //Priyanka [04-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["UserId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UserId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["UserId"].ToString());


                    //Prajakta[2019-02-27] Added
                    if (tblPurchaseScheduleSummaryTODT["isBoth"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsBoth = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isBoth"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsFixed = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isFixed"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["transportAmtPerMT"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["modbusRefId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ModbusRefId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["modbusRefId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["gateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["gateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isDBup"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsDBup = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isDBup"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["portNumber"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PortNumber = Convert.ToString(tblPurchaseScheduleSummaryTODT["portNumber"]);
                    if (tblPurchaseScheduleSummaryTODT["ioTUrl"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IoTUrl = Convert.ToString(tblPurchaseScheduleSummaryTODT["ioTUrl"]);
                    if (tblPurchaseScheduleSummaryTODT["machineIP"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MachineIP = Convert.ToString(tblPurchaseScheduleSummaryTODT["machineIP"]);

                    //Prajakta[2019-05-08] Added
                    // if (tblPurchaseScheduleSummaryTODT["rejectedQty"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.RejectedQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rejectedQty"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["rejectedBy"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.RejectedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectedBy"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["rejectedOn"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.RejectedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["rejectedOn"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["rejectedByUserName"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.RejectedByUserName = Convert.ToString(tblPurchaseScheduleSummaryTODT["rejectedByUserName"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.VehRejectReasonId =(tblPurchaseScheduleSummaryTODT["vehRejectReasonId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["height"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Height = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["height"]);

                    if (tblPurchaseScheduleSummaryTODT["width"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Width = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["width"]);

                    if (tblPurchaseScheduleSummaryTODT["length"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Length = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["length"]);


                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);
                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }


        //Added by minal 04 May 2021 for Master Report for Vertical
        public List<TblPurchaseScheduleSummaryTO> ConvertDTToListForMasterReport(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                List<DropDownTO> phasesList = new List<DropDownTO>();
                phasesList = getListofPhasesUsedForUnloadingQty();
                while (tblPurchaseScheduleSummaryTODT.Read())
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                    if (tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["isVehicleVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsVehicleVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isVehicleVerified"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["createdBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["createdByName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedByName = Convert.ToString(tblPurchaseScheduleSummaryTODT["createdByName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["updatedBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["updatedByName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedByName = Convert.ToString(tblPurchaseScheduleSummaryTODT["updatedByName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["scheduleDate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());




                    if (tblPurchaseScheduleSummaryTODT["scheduleQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.OrgScheduleQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["scheduleQty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Padta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padta"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padtaForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PadtaForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padtaForEnquiry"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["remark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Remark = Convert.ToString(tblPurchaseScheduleSummaryTODT["remark"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["statusDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusDesc"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["colorCode"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ColorCode = Convert.ToString(tblPurchaseScheduleSummaryTODT["colorCode"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["firmName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierName = Convert.ToString(tblPurchaseScheduleSummaryTODT["firmName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNc = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["narrationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NarrationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["narrationId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["gradingCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GradingCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["gradingCompCnt"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["unloadingCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnloadingCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["unloadingCompCnt"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["recoveryCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryCompCnt"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["wtStageCompCnt"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.WtStageCompCnt = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["wtStageCompCnt"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["narration"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Narration = Convert.ToString(tblPurchaseScheduleSummaryTODT["narration"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Rate = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["bookingRate"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["displayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MaterailType = Convert.ToString(tblPurchaseScheduleSummaryTODT["displayName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rate_band_costing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateBand = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rate_band_costing"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["stateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["stateName"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["areaName"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.AreaName = Convert.ToString(tblPurchaseScheduleSummaryTODT["areaName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prodClassId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["prodClassDesc"].ToString());




                    if (tblPurchaseScheduleSummaryTODT["stateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["stateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["engineerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["engineerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supervisorId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PhotographerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["photographerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographer"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Photographer = Convert.ToString(tblPurchaseScheduleSummaryTODT["photographer"]);


                    if (tblPurchaseScheduleSummaryTODT["qualityFlag"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.QualityFlag = Convert.ToBoolean(tblPurchaseScheduleSummaryTODT["qualityFlag"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["driverName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverName = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverName"]);

                    if (tblPurchaseScheduleSummaryTODT["driverContactNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverContactNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverContactNo"]);

                    if (tblPurchaseScheduleSummaryTODT["transporterName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransporterName = Convert.ToString(tblPurchaseScheduleSummaryTODT["transporterName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleTypeId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["targetPadta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TargetPadta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["targetPadta"]);

                    if (tblPurchaseScheduleSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Freight = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freight"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["containerNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ContainerNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["containerNo"]);

                    if (tblPurchaseScheduleSummaryTODT["lotSize"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LotSize = Convert.ToString(tblPurchaseScheduleSummaryTODT["lotSize"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleStateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleStateName"]);

                    if (tblPurchaseScheduleSummaryTODT["locationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LocationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["locationId"]);

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNcId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"]);

                    // if (tblPurchaseScheduleSummaryTODT["locationDesc"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["locationDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["userDisplayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorName = Convert.ToString(tblPurchaseScheduleSummaryTODT["userDisplayName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleCatId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleCatId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleCatId"]);

                    if (tblPurchaseScheduleSummaryTODT["prevStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prevStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["previousStatusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["previousStatusName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehiclePhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                    {
                        DropDownTO Temp = phasesList.Where(w => w.Value == tblPurchaseScheduleSummaryTONew.VehiclePhaseId).FirstOrDefault();
                        if (Temp != null)
                        {
                            tblPurchaseScheduleSummaryTONew.OrgUnloadedQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());
                        }
                        else
                        {
                            tblPurchaseScheduleSummaryTONew.OrgUnloadedQty = 0;
                        }
                    }

                    if (tblPurchaseScheduleSummaryTODT["sequanceNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseSequanceNo = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["sequanceNo"]);

                    if (tblPurchaseScheduleSummaryTODT["phaseName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseName = Convert.ToString(tblPurchaseScheduleSummaryTODT["phaseName"]);

                    if (tblPurchaseScheduleSummaryTODT["isActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isActive"]);

                    if (tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ParentPurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"]);

                    if (tblPurchaseScheduleSummaryTODT["location"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["location"]);

                    if (tblPurchaseScheduleSummaryTODT["rootScheduleId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RootScheduleId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rootScheduleId"]);

                    if (tblPurchaseScheduleSummaryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["enqDisplayNo"]);

                    if (tblPurchaseScheduleSummaryTODT["isRecovery"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsRecovery = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isRecovery"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryBy"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["recoveryOn"]);

                    if (tblPurchaseScheduleSummaryTODT["isWeighing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsWeighing = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isWeighing"]);

                    // Deepali[07-02-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["graderId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GraderId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["graderId"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["isGradingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsCorrectionCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["corretionCompletedOn"] != DBNull.Value)
                    {
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["corretionCompletedOn"].ToString());
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOnStr = tblPurchaseScheduleSummaryTONew.CorretionCompletedOn.ToString(StaticStuff.Constants.DefaultDateFormat);
                    }


                    if (tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsUnloadingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"].ToString());

                    // Priyanka[28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["commercialApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialApproval"].ToString());

                    //Priyanka [28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["purchaseManager"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseManager = Convert.ToString(tblPurchaseScheduleSummaryTODT["purchaseManager"]);

                    //Priyanka [04-01-2019] Added

                    if (tblPurchaseScheduleSummaryTODT["UserId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UserId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["UserId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["GreaderName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GreaderName = Convert.ToString(tblPurchaseScheduleSummaryTODT["GreaderName"]);

                    if (tblPurchaseScheduleSummaryTODT["EngineerName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerName = Convert.ToString(tblPurchaseScheduleSummaryTODT["EngineerName"]);

                    //Priyanka [18-02-2019]
                    if (tblPurchaseScheduleSummaryTODT["commercialVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialVerified"].ToString());

                    //Prajakta[2019-02-27] Added
                    if (tblPurchaseScheduleSummaryTODT["isBoth"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsBoth = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isBoth"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusRemark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusRemark = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusRemark"]);

                    if (tblPurchaseScheduleSummaryTODT["navigationUrl"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NavigationUrl = Convert.ToString(tblPurchaseScheduleSummaryTODT["navigationUrl"]);

                    if (tblPurchaseScheduleSummaryTODT["scheduleHistoryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleHistoryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["scheduleHistoryId"]);

                    if (tblPurchaseScheduleSummaryTODT["historyPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.HistoryPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["historyPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["rejectStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["acceptStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.AcceptStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["acceptStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["isLatest"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsLatest = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isLatest"]);

                    if (tblPurchaseScheduleSummaryTODT["isApproved"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsApproved = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isApproved"]);

                    if (tblPurchaseScheduleSummaryTODT["isIgnoreApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsIgnoreApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isIgnoreApproval"]);


                    if (tblPurchaseScheduleSummaryTODT["rejectPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["acceptPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.AcceptPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["acceptPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["historyIsActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.HistoryIsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["historyIsActive"]);

                    if (tblPurchaseScheduleSummaryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsFixed = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isFixed"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["transportAmtPerMT"].ToString());

                    //Prajakta[2019-05-08] Added
                    if (tblPurchaseScheduleSummaryTODT["rejectedQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rejectedQty"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectedBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["rejectedOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedByUserName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedByUserName = Convert.ToString(tblPurchaseScheduleSummaryTODT["rejectedByUserName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonId = (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["corretionCompletedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["corretionCompletedOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rateForC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateForC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rateForC"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rateForNC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rateForNC"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["modbusRefId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ModbusRefId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["modbusRefId"]);
                    if (tblPurchaseScheduleSummaryTODT["gateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["gateId"]);
                    if (tblPurchaseScheduleSummaryTODT["portNumber"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PortNumber = Convert.ToString(tblPurchaseScheduleSummaryTODT["portNumber"]);
                    if (tblPurchaseScheduleSummaryTODT["ioTUrl"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IoTUrl = Convert.ToString(tblPurchaseScheduleSummaryTODT["ioTUrl"]);
                    if (tblPurchaseScheduleSummaryTODT["machineIP"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MachineIP = Convert.ToString(tblPurchaseScheduleSummaryTODT["machineIP"]);

                    if (tblPurchaseScheduleSummaryTODT["latestWtTakenOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LatestWtTakenOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["latestWtTakenOn"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isAutoSpotVehSauda"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsAutoSpotVehSauda = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isAutoSpotVehSauda"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isVehicleOut"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsVehicleOut = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isVehicleOut"]);


                    if (tblPurchaseScheduleSummaryTODT["isDBup"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsDBup = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isDBup"]);

                    if (tblPurchaseScheduleSummaryTODT["isGradingUnldCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingUnldCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingUnldCompleted"]);

                    if (tblPurchaseScheduleSummaryTODT["refRateofV48Var"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RefRateofV48Var = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateofV48Var"]);
                    if (tblPurchaseScheduleSummaryTODT["refRateC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RefRateC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateC"]);

                    if (tblPurchaseScheduleSummaryTONew.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
                    {
                        if (tblPurchaseScheduleSummaryTODT["refRateForSaudaC"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.RefRateForSauda = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateForSaudaC"]);
                    }
                    else
                    {
                        if (tblPurchaseScheduleSummaryTODT["refRateForSaudaNC"] != DBNull.Value)
                            tblPurchaseScheduleSummaryTONew.RefRateForSaudaNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateForSaudaNC"]);
                    }
                    if (tblPurchaseScheduleSummaryTODT["height"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Height = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["height"]);

                    if (tblPurchaseScheduleSummaryTODT["width"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Width = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["width"]);

                    if (tblPurchaseScheduleSummaryTODT["length"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Length = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["length"]);


                    //if (tblPurchaseScheduleSummaryTODT["refRateC"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.RefRateC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateC"]);

                    //if (tblPurchaseScheduleSummaryTODT["refRateofV48Var"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.RefRateofV48Var = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["refRateofV48Var"]);


                    if (tblPurchaseScheduleSummaryTODT["reportedDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ReportedDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["reportedDate"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTon"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnldDatePadtaPerTon = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTon"]);

                    if (tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTonForNC"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UnldDatePadtaPerTonForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["unldDatePadtaPerTonForNC"]);

                    if (tblPurchaseScheduleSummaryTODT["processChargePerVeh"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProcessChargePerVeh = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["processChargePerVeh"]);

                    if (tblPurchaseScheduleSummaryTODT["processChargePerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProcessChargePerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["processChargePerMT"]);

                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehRejectReasonDesc"]);
                    //Added by minal 25 March 2021
                    if (tblPurchaseScheduleSummaryTODT["freightAmount"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.FreightAmount = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freightAmount"]);
                    //if (tblPurchaseScheduleSummaryTODT["partyName"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.PartyName = Convert.ToString(tblPurchaseScheduleSummaryTODT["partyName"]);

                    ////Reshma[01-3-21]
                    //if (tblPurchaseScheduleSummaryTODT["GradingComplOn"] != DBNull.Value)
                    //    tblPurchaseScheduleSummaryTONew.GradingComplOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["GradingComplOn"].ToString());

                    ////Gokul [14-03-21]
                    //if (tblPurchaseScheduleSummaryTONew.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
                    //{
                    //    if (tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForOrder"] != DBNull.Value)
                    //        tblPurchaseScheduleSummaryTONew.CorrectionPadtaAmt = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForOrder"].ToString());
                    //}
                    //else
                    //{
                    //    if (tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForEnquiry"] != DBNull.Value)
                    //        tblPurchaseScheduleSummaryTONew.CorrectionPadtaAmt = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["correcUnldDatePadtaForEnquiry"].ToString());

                    //}

                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);

                   

                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }


        public List<TblPurchaseScheduleSummaryTO> ConvertDTToListForApprovals(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                while (tblPurchaseScheduleSummaryTODT.Read())
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                    if (tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsUnloadingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["isVehicleVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsVehicleVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isVehicleVerified"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["createdBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["updatedBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["scheduleDate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.OrgScheduleQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["scheduleQty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Padta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padta"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padtaForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PadtaForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padtaForEnquiry"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["remark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Remark = Convert.ToString(tblPurchaseScheduleSummaryTODT["remark"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["statusDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusDesc"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["colorCode"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ColorCode = Convert.ToString(tblPurchaseScheduleSummaryTODT["colorCode"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["GreaderName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GreaderName = Convert.ToString(tblPurchaseScheduleSummaryTODT["GreaderName"]);

                    if (tblPurchaseScheduleSummaryTODT["EngineerName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerName = Convert.ToString(tblPurchaseScheduleSummaryTODT["EngineerName"]);


                    if (tblPurchaseScheduleSummaryTODT["firmName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierName = Convert.ToString(tblPurchaseScheduleSummaryTODT["firmName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNc = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Rate = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["bookingRate"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["displayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.MaterailType = Convert.ToString(tblPurchaseScheduleSummaryTODT["displayName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rate_band_costing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RateBand = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rate_band_costing"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["stateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["stateName"].ToString());

                    // if (tblPurchaseScheduleSummaryTODT["areaName"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.AreaName = Convert.ToString(tblPurchaseScheduleSummaryTODT["areaName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prodClassId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ProdClassDesc = Convert.ToString(tblPurchaseScheduleSummaryTODT["prodClassDesc"].ToString());




                    if (tblPurchaseScheduleSummaryTODT["stateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["stateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["engineerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["engineerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supervisorId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PhotographerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["photographerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["qualityFlag"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.QualityFlag = Convert.ToBoolean(tblPurchaseScheduleSummaryTODT["qualityFlag"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["driverName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverName = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverName"]);

                    if (tblPurchaseScheduleSummaryTODT["driverContactNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.DriverContactNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["driverContactNo"]);

                    if (tblPurchaseScheduleSummaryTODT["transporterName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransporterName = Convert.ToString(tblPurchaseScheduleSummaryTODT["transporterName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleTypeId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleTypeDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Freight = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freight"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["containerNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ContainerNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["containerNo"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleStateId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleStateName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleStateName = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleStateName"]);

                    if (tblPurchaseScheduleSummaryTODT["locationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LocationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["locationId"]);

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNcId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"]);

                    // if (tblPurchaseScheduleSummaryTODT["locationDesc"] != DBNull.Value)
                    //     tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["locationDesc"]);

                    if (tblPurchaseScheduleSummaryTODT["userDisplayName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorName = Convert.ToString(tblPurchaseScheduleSummaryTODT["userDisplayName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehicleCatId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleCatId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleCatId"]);

                    if (tblPurchaseScheduleSummaryTODT["prevStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["prevStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["previousStatusName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PreviousStatusName = Convert.ToString(tblPurchaseScheduleSummaryTODT["previousStatusName"]);

                    if (tblPurchaseScheduleSummaryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehiclePhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["sequanceNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseSequanceNo = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["sequanceNo"]);

                    if (tblPurchaseScheduleSummaryTODT["phaseName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseName = Convert.ToString(tblPurchaseScheduleSummaryTODT["phaseName"]);

                    if (tblPurchaseScheduleSummaryTODT["isActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isActive"]);

                    if (tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ParentPurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"]);

                    if (tblPurchaseScheduleSummaryTODT["location"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["location"]);

                    if (tblPurchaseScheduleSummaryTODT["rootScheduleId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RootScheduleId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rootScheduleId"]);

                    if (tblPurchaseScheduleSummaryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["enqDisplayNo"]);

                    if (tblPurchaseScheduleSummaryTODT["statusRemark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusRemark = Convert.ToString(tblPurchaseScheduleSummaryTODT["statusRemark"]);

                    if (tblPurchaseScheduleSummaryTODT["navigationUrl"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NavigationUrl = Convert.ToString(tblPurchaseScheduleSummaryTODT["navigationUrl"]);

                    if (tblPurchaseScheduleSummaryTODT["scheduleHistoryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleHistoryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["scheduleHistoryId"]);

                    if (tblPurchaseScheduleSummaryTODT["historyPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.HistoryPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["historyPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["rejectStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["acceptStatusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.AcceptStatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["acceptStatusId"]);

                    if (tblPurchaseScheduleSummaryTODT["isLatest"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsLatest = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isLatest"]);

                    if (tblPurchaseScheduleSummaryTODT["isApproved"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsApproved = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isApproved"]);

                    if (tblPurchaseScheduleSummaryTODT["isIgnoreApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsIgnoreApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isIgnoreApproval"]);

                    if (tblPurchaseScheduleSummaryTODT["isRecovery"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsRecovery = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isRecovery"]);

                    if (tblPurchaseScheduleSummaryTODT["isGradingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingCompleted"]);


                    if (tblPurchaseScheduleSummaryTODT["rejectPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["acceptPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.AcceptPhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["acceptPhaseId"]);

                    if (tblPurchaseScheduleSummaryTODT["narrationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NarrationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["narrationId"]);

                    if (tblPurchaseScheduleSummaryTODT["narration"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Narration = (tblPurchaseScheduleSummaryTODT["narration"]).ToString();

                    if (tblPurchaseScheduleSummaryTODT["historyIsActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.HistoryIsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["historyIsActive"]);

                    //Prajakta[2019-02-27] Added
                    if (tblPurchaseScheduleSummaryTODT["isBoth"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsBoth = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isBoth"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["purchaseManagerName"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseManager = Convert.ToString(tblPurchaseScheduleSummaryTODT["purchaseManagerName"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["globalRatePurchaseId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["wtRateApprovalDiff"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.WtRateApprovalDiff = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["wtRateApprovalDiff"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["approvalType"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ApprovalType = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["approvalType"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["height"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Height = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["height"]);

                    if (tblPurchaseScheduleSummaryTODT["width"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Width = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["width"]);

                    if (tblPurchaseScheduleSummaryTODT["length"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Length = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["length"]);


                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);
                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }




        #endregion

        #region Insertion
        public int InsertTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseScheduleSummaryTO, cmdInsert);
            }
            catch (Exception ex)
            {

                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseScheduleSummaryTO, cmdInsert);
            }
            catch (Exception ex)
            {

                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseScheduleSummary]( " +
            //"  [idPurchaseScheduleSummary]" +
            "  [purchaseEnquiryId]" +
            " ,[supplierId]" +
            " ,[statusId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[scheduleDate]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[qty]" +
            " ,[scheduleQty]" +
            " ,[calculatedMetalCost]" +
            " ,[baseMetalCost]" +
            " ,[padta]" +
            " ,[vehicleNo]" +
            " ,[remark]" +
            " ,[engineerId]" +
            " ,[supervisorId]" +
            " ,[photographerId]" +
            " ,[qualityFlag]" +
            " ,[parentPurchaseScheduleSummaryId]" +
            " ,[driverName] " +
            " ,[driverContactNo] " +
            " ,[transporterName] " +
            " ,[vehicleTypeId] " +
            " ,[freight] " +
            " ,[containerNo] " +
            " ,[vehicleStateId] " +
            " ,[locationId] " +
            " ,[spotEntryVehicleId] " +
            " ,[oldScheduleDate] " +
            " ,[cOrNCId] " +
            " ,[vehicleCatId] " +
            " ,[vehiclePhaseId] " +
            " ,[calculatedMetalCostForEnquiry] " +
            " ,[baseMetalCostForEnquiry] " +
            " ,[padtaForEnquiry] " +
            " ,[isActive] " +
            " ,[location] " +
            " ,[rootScheduleId] " +
            " ,[isVehicleVerified] " +
            " ,[isRecovery] " +
            " ,[recoveryBy] " +
            " ,[recoveryOn] " +
            " ,[isWeighing] " +
            " ,[commercialApproval] " +
            " ,[graderId] " +
            " ,[isGradingCompleted] " +
            " ,[isCorrectionCompleted] " +
            " ,[commercialVerified] " +               //Priyanka [19-02-2019]
            " ,[isUnloadingCompleted] " +
            " ,[narrationId] " +
            " ,[isBoth] " +   //Prajakta[2019-02-27] Added
            " ,[isFixed] " +   //Prajakta[2019-04-24] Added
            " ,[transportAmtPerMT] " +   //Prajakta[2019-04-24] Added
            " ,[rejectedQty] " +
            " ,[rejectedBy] " +
            " ,[rejectedOn] " +
            " ,[vehRejectReasonId] " +
            " ,[lotSize] " +
            " ,[corretionCompletedOn] " + //Prajakta[2019-03-06] Added
             ",[modbusRefId]" +
             ",[gateId]" +
             ",[isVehicleOut]" +
            " ,[latestWtTakenOn] " +
            " ,[isGradingUnldCompleted] " +
            " ,[refRateofV48Var] " +
            " ,[refRateC] " +
            " ,[height] " +
            " ,[width] " +
            " ,[length] " +
            " ,[reportedDate] " +
            " ,[gradingDate] " +
            " ,[unldDatePadtaPerTon] " +
            " ,[unldDatePadtaPerTonForNC] " +
            " ,[vehRejectReasonDesc] " +
            " ,[processChargePerVeh] " +
            " ,[processChargePerMT] " +
            " ,[enqQty] " +
            " ,[correNarration] " +
            " ) " +
" VALUES (" +
            //"  @IdPurchaseScheduleSummary " +
            "  @PurchaseEnquiryId " +
            " ,@SupplierId " +
            " ,@StatusId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@ScheduleDate " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Qty " +
            " ,@OrgScheduleQty " +
            " ,@CalculatedMetalCost " +
            " ,@BaseMetalCost " +
            " ,@Padta " +
            " ,@VehicleNo " +
            " ,@Remark " +
            " ,@EngineerId " +
            " ,@SupervisorId " +
            " ,@PhotographerId " +
            " ,@QualityFlag " +
            " ,@ParentPurchaseScheduleSummaryId " +
            " ,@driverName " +
            " ,@driverContactNo " +
            " ,@transporterName " +
            " ,@vehicleTypeId " +
            " ,@freight " +
            " ,@containerNo " +
            " ,@vehicleStateId " +
            " ,@locationId " +
            " ,@spotEntryVehicleId " +
            " ,@oldScheduleDate " +
            " ,@cOrNCId " +
            " ,@vehicleCatId " +
            " ,@vehiclePhaseId " +
            " ,@calculatedMetalCostForEnquiry " +
            " ,@baseMetalCostForEnquiry " +
            " ,@padtaForEnquiry " +
            " ,@isActive " +
            " ,@location " +
            " ,@rootScheduleId " +
            " ,@IsVehicleVerified " +
            " ,@IsRecovery " +
            " ,@RecoveryBy " +
            " ,@RecoveryOn " +
            " ,@IsWeighing " +
            " ,@CommercialApproval" +
            " ,@GraderId" +
            " ,@IsGradingCompleted" +
            " ,@IsCorrectionCompleted" +
            " ,@CommercialVerified" +
            " ,@IsUnloadingCompleted" +
            " ,@NarrationId" +
            " ,@IsBoth" +  //Prajakta[2019-02-27] Added
            " ,@IsFixed" +  //Prajakta[2019-04-24] Added
            " ,@TransportAmtPerMT" +  //Prajakta[2019-04-24] Added
            " ,@RejectedQty" +
            " ,@RejectedBy" +
            " ,@RejectedOn" +
            " ,@VehRejectReasonId" +
            " ,@LotSize" +
            " ,@CorretionCompletedOn" +
             " ,@ModbusRefId" +
              " ,@GateId" +
              " ,@IsVehicleOut" +
            " ,@LatestWtTakenOn" +
            " ,@IsGradingUnldCompleted" +
            " ,@RefRateofV48Var" +
            " ,@RefRateC" +
            " ,@Height" +
            " ,@Width" +
            " ,@Length" +
            " ,@reportedDate " +
            " ,@GradingDate " +
            " ,@UnldDatePadtaPerTon" +
            " ,@UnldDatePadtaPerTonForNC" +
            " ,@VehRejectReasonDesc" +
            " ,@ProcessChargePerVeh" +
            " ,@ProcessChargePerMT" +
            " ,@enqQty" +
            " ,@CorreNarration" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            cmdInsert.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.PurchaseEnquiryId;
            cmdInsert.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.SupplierId;
            cmdInsert.Parameters.Add("@IsVehicleVerified", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IsVehicleVerified;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UpdatedBy);
            cmdInsert.Parameters.Add("@ScheduleDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseScheduleSummaryTO.ScheduleDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseScheduleSummaryTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UpdatedOn);
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.Qty;
            cmdInsert.Parameters.Add("@OrgScheduleQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.OrgScheduleQty;
            cmdInsert.Parameters.Add("@CalculatedMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.CalculatedMetalCost;
            cmdInsert.Parameters.Add("@BaseMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.BaseMetalCost;
            cmdInsert.Parameters.Add("@Padta", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.Padta;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.VehicleNo;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Remark);
            cmdInsert.Parameters.Add("@EngineerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.EngineerId);
            cmdInsert.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SupervisorId);
            cmdInsert.Parameters.Add("@PhotographerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.PhotographerId);
            cmdInsert.Parameters.Add("@QualityFlag", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.QualityFlag);
            cmdInsert.Parameters.Add("@ParentPurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ParentPurchaseScheduleSummaryId);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.DriverName);
            cmdInsert.Parameters.Add("@driverContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.DriverContactNo);
            cmdInsert.Parameters.Add("@transporterName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.TransporterName);
            cmdInsert.Parameters.Add("@vehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleTypeId);
            cmdInsert.Parameters.Add("@freight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Freight);
            cmdInsert.Parameters.Add("@containerNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ContainerNo);
            cmdInsert.Parameters.Add("@vehicleStateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleStateId);
            cmdInsert.Parameters.Add("@locationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LocationId);
            cmdInsert.Parameters.Add("@spotEntryVehicleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SpotEntryVehicleId);
            cmdInsert.Parameters.Add("@oldScheduleDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.OldScheduleDate);
            cmdInsert.Parameters.Add("@cOrNCId", System.Data.SqlDbType.Int).Value = (tblPurchaseScheduleSummaryTO.COrNcId);
            cmdInsert.Parameters.Add("@vehicleCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleCatId);
            cmdInsert.Parameters.Add("@vehiclePhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehiclePhaseId);
            cmdInsert.Parameters.Add("@calculatedMetalCostForEnquiry", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CalculatedMetalCostForNC);
            cmdInsert.Parameters.Add("@baseMetalCostForEnquiry", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.BaseMetalCostForNC;
            cmdInsert.Parameters.Add("@padtaForEnquiry", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.PadtaForNC;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.IsActive;
            cmdInsert.Parameters.Add("@location", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Location);
            cmdInsert.Parameters.Add("@rootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RootScheduleId);
            cmdInsert.Parameters.Add("@IsRecovery", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsRecovery);
            cmdInsert.Parameters.Add("@RecoveryBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RecoveryBy);
            cmdInsert.Parameters.Add("@RecoveryOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RecoveryOn);
            cmdInsert.Parameters.Add("@IsWeighing", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsWeighing);
            cmdInsert.Parameters.Add("@CommercialApproval", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CommercialApproval);
            cmdInsert.Parameters.Add("@GraderId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.GraderId);
            cmdInsert.Parameters.Add("@IsGradingCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsGradingCompleted);
            cmdInsert.Parameters.Add("@IsUnloadingCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsUnloadingCompleted);
            cmdInsert.Parameters.Add("@IsCorrectionCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsCorrectionCompleted);
            cmdInsert.Parameters.Add("@NarrationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.NarrationId);
            cmdInsert.Parameters.Add("@CommercialVerified", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CommercialVerified);
            cmdInsert.Parameters.Add("@IsBoth", System.Data.SqlDbType.Int).Value = (tblPurchaseScheduleSummaryTO.IsBoth);
            cmdInsert.Parameters.Add("@IsFixed", System.Data.SqlDbType.Int).Value = (tblPurchaseScheduleSummaryTO.IsFixed);
            cmdInsert.Parameters.Add("@TransportAmtPerMT", System.Data.SqlDbType.Decimal).Value = (tblPurchaseScheduleSummaryTO.TransportAmtPerMT);
            //Prajakta[2019-05-08] Added
            cmdInsert.Parameters.Add("@RejectedQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedQty);
            cmdInsert.Parameters.Add("@RejectedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedBy);
            cmdInsert.Parameters.Add("@RejectedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedOn);
            cmdInsert.Parameters.Add("@VehRejectReasonId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehRejectReasonId);
            cmdInsert.Parameters.Add("@LotSize", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LotSize);
            cmdInsert.Parameters.Add("@CorretionCompletedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CorretionCompletedOn);
            cmdInsert.Parameters.Add("@ModbusRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ModbusRefId);
            cmdInsert.Parameters.Add("@GateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.GateId);
            cmdInsert.Parameters.Add("@IsVehicleOut", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsVehicleOut);
            cmdInsert.Parameters.Add("@LatestWtTakenOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LatestWtTakenOn);
            cmdInsert.Parameters.Add("@IsGradingUnldCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsGradingUnldCompleted);
            cmdInsert.Parameters.Add("@RefRateofV48Var", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RefRateofV48Var);
            cmdInsert.Parameters.Add("@RefRateC", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RefRateC);
            cmdInsert.Parameters.Add("@Height", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Height);
            cmdInsert.Parameters.Add("@Width", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Width);
            cmdInsert.Parameters.Add("@Length", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Length);
            cmdInsert.Parameters.Add("@ReportedDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ReportedDate);
            cmdInsert.Parameters.Add("@GradingDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.GradingComplOn);
            cmdInsert.Parameters.Add("@UnldDatePadtaPerTon", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UnldDatePadtaPerTon);
            cmdInsert.Parameters.Add("@UnldDatePadtaPerTonForNC", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UnldDatePadtaPerTonForNC);
            cmdInsert.Parameters.Add("@VehRejectReasonDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehRejectReasonDesc);
            cmdInsert.Parameters.Add("@ProcessChargePerVeh", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ProcessChargePerVeh);
            cmdInsert.Parameters.Add("@ProcessChargePerMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ProcessChargePerMT);
            cmdInsert.Parameters.Add("@enqQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.EnqQty);
            cmdInsert.Parameters.Add("@CorreNarration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CorreNarration);


            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary = Convert.ToInt32(cmdInsert.ExecuteScalar());

                if (tblPurchaseScheduleSummaryTO.RootScheduleId == 0)
                {
                    tblPurchaseScheduleSummaryTO.RootScheduleId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                    return UpdateRootAgainstById(tblPurchaseScheduleSummaryTO, cmdInsert);
                }

                return 1;
            }
            else return 0;
        }



        #endregion

        #region Updation

        public int UpdateRootAgainstById(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate)
        {

            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
            " [rootScheduleId]= " + tblPurchaseScheduleSummaryTO.RootScheduleId +
            " WHERE idPurchaseScheduleSummary = " + tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            //cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            //cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RootScheduleId);

            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseScheduleSummaryTO, cmdUpdate);

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateVehicleTypeOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
           " [vehicleTypeId]= @VehicleTypeId" +
           " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                //cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.VehicleTypeId;
                cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleTypeId);

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateWeighingCompletedAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                // conn.Open();

                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
           " [isWeighing]= @IsWeighing" +
           //"  ,[qty] =@qty "+
           " WHERE idPurchaseScheduleSummary = @RootScheduleId OR  rootScheduleId = @RootScheduleId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.RootScheduleId;
                cmdUpdate.Parameters.Add("@IsWeighing", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.IsWeighing;
                //cmdUpdate.Parameters.Add("@qty", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.Qty;

                return cmdUpdate.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }


        public int UpdateStatusWeighingCompletedAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                // conn.Open();

                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [statusId]= @StatusId" +
                " ,[createdBy]= @CreatedBy" +
                " ,[updatedBy]= @UpdatedBy" +
                " ,[createdOn]= @CreatedOn" +
                " ,[updatedOn]= @UpdatedOn" +
                "  ,[qty] =@qty " +//Reshma[03-02-21] For Process Charges
                " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.StatusId;
                cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.CreatedBy;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@createdOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CreatedOn);
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UpdatedOn);
                cmdUpdate.Parameters.Add("@qty", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.Qty;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }


        public int UpdateRejectedQtyDtlsAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                                 "  [rejectedQty] = @RejectedQty" +
                                 " ,[rejectedBy] = @RejectedBy" +
                                 " ,[rejectedOn] = @RejectedOn" +
                                 " ,[vehRejectReasonId] = @VehRejectReasonId" +
                                 " ,[isWeighing] = @IsWeighing" +
                                 " WHERE idPurchaseScheduleSummary = @RootScheduleId OR  rootScheduleId = @RootScheduleId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.RootScheduleId;
                cmdUpdate.Parameters.Add("@RejectedQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedQty);
                cmdUpdate.Parameters.Add("@RejectedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedBy);
                cmdUpdate.Parameters.Add("@RejectedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedOn);
                cmdUpdate.Parameters.Add("@VehRejectReasonId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehRejectReasonId);
                cmdUpdate.Parameters.Add("@IsWeighing", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsWeighing);

                return cmdUpdate.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateScheduleVehicleNoOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return UpdateScheduleVehicleNoOnly(tblPurchaseScheduleSummaryTO, cmdUpdate);

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }



        public int UpdateSpotEntryVehicleSupplier(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return UpdateSpotEntryVehicleSupplier(tblPurchaseVehicleSpotEntryTO, cmdUpdate);

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }


        public int UpdateSpotEntryVehicleSupplier(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseVehicleSpotEntry] SET " +
            " [supplierId]= @SupplierId " +
            " WHERE idVehicleSpotEntry = @IdVehicleSpotEntry ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.NVarChar).Value = tblPurchaseVehicleSpotEntryTO.SupplierId;
            return cmdUpdate.ExecuteNonQuery();


        }



        public int UpdateScheduleVehicleNoOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
            " [vehicleNo]= @VehicleNo ," +
            " [containerNo]= @ContainerNo ," +
            " [lotSize]= @LotSize" +
            " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.VehicleNo;
            cmdUpdate.Parameters.Add("@ContainerNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ContainerNo);
            cmdUpdate.Parameters.Add("@LotSize", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LotSize);

            return cmdUpdate.ExecuteNonQuery();


        }

        public int UpdateScheduleEnquiryIdOnly(Int32 rootScheduleId,Int32 purchaseEnqId, SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
            " [purchaseEnquiryId]= @PurchaseEnquiryId " +
            " WHERE idPurchaseScheduleSummary = @RootScheduleId OR  rootScheduleId = @RootScheduleId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.Connection = conn;
            cmdUpdate.Transaction = tran;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = rootScheduleId;
            cmdUpdate.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = purchaseEnqId;
          
            return cmdUpdate.ExecuteNonQuery();


        }

        public int UpdateVehicleType(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
            " [vehicleNo]= @VehicleNo ," +
            " [containerNo]= @ContainerNo ," +
            " [lotSize]= @LotSize" +
            " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.VehicleNo;
            cmdUpdate.Parameters.Add("@ContainerNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ContainerNo);
            cmdUpdate.Parameters.Add("@LotSize", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LotSize);

            return cmdUpdate.ExecuteNonQuery();


        }

        public int UpdateScheduleVehicleQtyOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [qty]= @Qty ," +
                " [ScheduleQty]= @ScheduleQty " +
                " WHERE idPurchaseScheduleSummary = @RootScheduleId OR  rootScheduleId = @RootScheduleId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;
                cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.Qty;
                cmdUpdate.Parameters.Add("@ScheduleQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.OrgScheduleQty);

                return cmdUpdate.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }

        }

        public int UpdateScheduleEnqQtyOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [enqQty]= @EnqQty " +
                " WHERE idPurchaseScheduleSummary = @RootScheduleId OR  rootScheduleId = @RootScheduleId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;
                cmdUpdate.Parameters.Add("@EnqQty", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.EnqQty;

                return cmdUpdate.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }

        }

        public int UpdateTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseScheduleSummaryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblPurchaseScheduleSummaryStatusOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandStatusOnly(tblPurchaseScheduleSummaryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateCorrectionCompletedFlag(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return UpdateCorrectionCompletedFlag(tblPurchaseScheduleSummaryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }



        public int UpdateTblPurchaseScheduleSummaryCommercialApproval(Int32 rootScheduleId, Int32 CommercialApproval, Int32 CommercialVerified, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [commercialApproval]= @CommercialApproval" +
                " ,[commercialVerified]= @CommercialVerified" +
                " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary OR  rootScheduleId = @IdPurchaseScheduleSummary";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = rootScheduleId;
                cmdUpdate.Parameters.Add("@CommercialApproval", System.Data.SqlDbType.Int).Value = CommercialApproval;
                cmdUpdate.Parameters.Add("@CommercialVerified", System.Data.SqlDbType.Int).Value = CommercialVerified;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateGradingCompletedOn(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [gradingDate]= @GradingComplOn" +
                " ,[isGradingCompleted]= @IsGradingCompleted" +
                " WHERE ISNULL(rootScheduleId,idPurchaseScheduleSummary) = @RootScheduleId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ActualRootScheduleId);
                cmdUpdate.Parameters.Add("@IsGradingCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsGradingCompleted);
                cmdUpdate.Parameters.Add("@GradingComplOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.GradingComplOn);

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateIsVehicleOut(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [isVehicleOut]= @IsVehicleOut" +
                " WHERE ISNULL(rootScheduleId,idPurchaseScheduleSummary) = @RootScheduleId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ActualRootScheduleId);
                cmdUpdate.Parameters.Add("@IsVehicleOut", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsVehicleOut);

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateModbusRefPurchaseSchedule(Int32 rootScheduleId,  SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [modbusRefId]= NULL" +
                " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary OR  rootScheduleId = @IdPurchaseScheduleSummary";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = rootScheduleId;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }


        public int UpdateCorNcIdOfVehicle(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [cOrNCId]= @COrNCId" +
                " ,[updatedBy]= @UpdatedBy" +
                " ,[updatedOn]= @UpdatedOn" +
                " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary OR  rootScheduleId = @IdPurchaseScheduleSummary";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = scheduleSummaryTO.ActualRootScheduleId;
                cmdUpdate.Parameters.Add("@COrNCId", System.Data.SqlDbType.Int).Value = scheduleSummaryTO.COrNcId;
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(scheduleSummaryTO.UpdatedBy);
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(scheduleSummaryTO.UpdatedOn);
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdatePurchaseScheduleCalculationDtls(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                                    "  [qty]= @Qty" +
                                    " ,[calculatedMetalCost]= @CalculatedMetalCost" +
                                    " ,[baseMetalCost]= @BaseMetalCost" +
                                    " ,[padta]= @Padta" +
                                    " ,[calculatedMetalCostForEnquiry]= @CalculatedMetalCostForEnquiry" +
                                    " ,[baseMetalCostForEnquiry]= @BaseMetalCostForEnquiry" +
                                    " ,[padtaForEnquiry]= @PadtaForEnquiry" +
                                    " ,[unldDatePadtaPerTon]= @UnldDatePadtaPerTon" +
                                    " ,[unldDatePadtaPerTonForNC]= @UnldDatePadtaPerTonForNC" +
                                    " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.Qty;
                cmdUpdate.Parameters.Add("@CalculatedMetalCost", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.CalculatedMetalCost;
                cmdUpdate.Parameters.Add("@BaseMetalCost", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.BaseMetalCost;
                cmdUpdate.Parameters.Add("@Padta", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.Padta;
                cmdUpdate.Parameters.Add("@CalculatedMetalCostForEnquiry", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.CalculatedMetalCostForNC;
                cmdUpdate.Parameters.Add("@BaseMetalCostForEnquiry", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.BaseMetalCostForNC;
                cmdUpdate.Parameters.Add("@PadtaForEnquiry", System.Data.SqlDbType.Decimal).Value = tblPurchaseScheduleSummaryTO.PadtaForNC;
                cmdUpdate.Parameters.Add("@UnldDatePadtaPerTon", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UnldDatePadtaPerTon);
                cmdUpdate.Parameters.Add("@UnldDatePadtaPerTonForNC", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UnldDatePadtaPerTonForNC);

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateIsActiveOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                                    "  [isActive]= @IsActive" +
                                    " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IsActive;

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
            //"  [idPurchaseScheduleSummary] = @IdPurchaseScheduleSummary" +
            "  [purchaseEnquiryId]= @PurchaseEnquiryId" +
            " ,[supplierId]= @SupplierId" +
            " ,[statusId]= @StatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[scheduleDate]= @ScheduleDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[qty]= @Qty" +
            " ,[scheduleQty]= @OrgScheduleQty" +
            " ,[calculatedMetalCost]= @CalculatedMetalCost" +
            " ,[baseMetalCost]= @BaseMetalCost" +
            " ,[padta]= @Padta" +
            " ,[vehicleNo]= @VehicleNo" +
            " ,[remark] = @Remark" +
            " ,[engineerId] = @EngineerId" +
            " ,[supervisorId] = @SupervisorId" +
            " ,[locationId] = @LocationId" +
            " ,[photographerId] = @PhotographerId" +
            " ,[qualityFlag] = @QualityFlag" +
            " ,[parentPurchaseScheduleSummaryId] = @ParentPurchaseScheduleSummaryId" +
            " ,[spotEntryVehicleId] = @SpotEntryVehicleId" +
            " ,[cOrNCId] = @COrNCId" +
            " ,[vehicleCatId] = @VehicleCatId" +
            " ,[vehiclePhaseId] = @VehiclePhaseId" +
             " ,[driverName] = @DriverName" +
            " ,[vehicleTypeId] = @VehicleTypeId" +
            " ,[transPorterName] = @TransPorterName" +
            " ,[freight] = @Freight" +
            " ,[driverContactNo] = @DriverContactNo" +
            " ,[containerNo] = @ContainerNo" +
            " ,[lotSize] = @LotSize" +
            " ,[vehicleStateId] = @VehicleStateId" +
            " ,[calculatedMetalCostForEnquiry]= @CalculatedMetalCostForEnquiry" +
            " ,[baseMetalCostForEnquiry]= @BaseMetalCostForEnquiry" +
            " ,[padtaForEnquiry]= @PadtaForEnquiry" +
            " ,[isActive]= @IsActive" +
            " ,[location]= @Location" +
            " ,[rootScheduleId]= @RootScheduleId" +
            " ,[isVehicleVerified]= @isVehicleVerified" +
            " ,[isRecovery]= @IsRecovery" +
            " ,[recoveryBy]= @RecoveryBy" +
            " ,[recoveryOn]= @RecoveryOn" +
            " ,[isWeighing]= @IsWeighing" +
            " ,[commercialApproval] = @CommercialApproval" +
            " ,[graderId] = @GraderId" +
            " ,[isGradingCompleted] = @IsGradingCompleted" +
            " ,[isCorrectionCompleted] = @IsCorrectionCompleted" +
            " ,[isUnloadingCompleted] = @IsUnloadingCompleted" +
            " ,[narrationId] = @NarrationId" +
            " ,[commercialVerified] = @CommercialVerified" +
            " ,[isBoth] = @IsBoth" + //Prajakta[2019-02-27] Added
            " ,[isFixed] = @IsFixed" + //Prajakta[2019-04-24] Added
            " ,[transportAmtPerMT] = @TransportAmtPerMT" + //Prajakta[2019-04-24] Added
            " ,[rejectedQty] = @RejectedQty" +
            " ,[rejectedBy] = @RejectedBy" +
            " ,[rejectedOn] = @RejectedOn" +
            " ,[vehRejectReasonId] = @VehRejectReasonId" +
            " ,[corretionCompletedOn] = @CorretionCompletedOn" +
            " ,[isVehicleOut] = @IsVehicleOut" +
            " ,[modbusRefId] = @ModbusRefId" +
            " ,[latestWtTakenOn] = @LatestWtTakenOn" +
            " ,[isGradingUnldCompleted] = @IsGradingUnldCompleted" +
            ", [gateId]=@GateId" +
            ", [height]=@Height" +
            ", [width]=@Width" +
            ", [length]=@Length" +
            ", [reportedDate]=@ReportedDate" +
            ", [gradingDate]=@GradingDate" +
            ", [unldDatePadtaPerTon]=@UnldDatePadtaPerTon" +
            ", [unldDatePadtaPerTonForNC]=@UnldDatePadtaPerTonForNC" +
            ", [vehRejectReasonDesc]=@VehRejectReasonDesc" +
            ", [processChargePerVeh]=@ProcessChargePerVeh" +
            ", [processChargePerMT]=@ProcessChargePerMT" +
            ", [enqQty]=@enqQty" +
             ", [correNarration]=@CorreNarration" +
            " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            cmdUpdate.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.PurchaseEnquiryId;
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SupplierId);
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@ScheduleDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseScheduleSummaryTO.ScheduleDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.Qty;
            cmdUpdate.Parameters.Add("@OrgScheduleQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.OrgScheduleQty;
            cmdUpdate.Parameters.Add("@CalculatedMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.CalculatedMetalCost;
            cmdUpdate.Parameters.Add("@BaseMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.BaseMetalCost;
            cmdUpdate.Parameters.Add("@Padta", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.Padta;
            cmdUpdate.Parameters.Add("@LotSize", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LotSize);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.VehicleNo;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(string.Empty);
            cmdUpdate.Parameters.Add("@EngineerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.EngineerId);
            cmdUpdate.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SupervisorId);
            cmdUpdate.Parameters.Add("@PhotographerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.PhotographerId);
            cmdUpdate.Parameters.Add("@QualityFlag", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.QualityFlag);
            cmdUpdate.Parameters.Add("@ParentPurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ParentPurchaseScheduleSummaryId);
            cmdUpdate.Parameters.Add("@SpotEntryVehicleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SpotEntryVehicleId);
            cmdUpdate.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LocationId);
            cmdUpdate.Parameters.Add("@OldScheduleDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.OldScheduleDate);
            cmdUpdate.Parameters.Add("@COrNCId", System.Data.SqlDbType.Int).Value = (tblPurchaseScheduleSummaryTO.COrNcId);
            cmdUpdate.Parameters.Add("@VehicleCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleCatId);
            cmdUpdate.Parameters.Add("@VehiclePhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehiclePhaseId);
            cmdUpdate.Parameters.Add("@CalculatedMetalCostForEnquiry", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.CalculatedMetalCostForNC;
            cmdUpdate.Parameters.Add("@BaseMetalCostForEnquiry", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.BaseMetalCostForNC;
            cmdUpdate.Parameters.Add("@PadtaForEnquiry", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.PadtaForNC;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = (tblPurchaseScheduleSummaryTO.IsActive);
            cmdUpdate.Parameters.Add("@Location", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Location);
            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RootScheduleId);

            cmdUpdate.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.DriverName);
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleTypeId);
            cmdUpdate.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Freight);
            cmdUpdate.Parameters.Add("@DriverContactNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.DriverContactNo);
            cmdUpdate.Parameters.Add("@ContainerNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ContainerNo);
            cmdUpdate.Parameters.Add("@VehicleStateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleStateId);
            cmdUpdate.Parameters.Add("@TransporterName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.TransporterName);
            cmdUpdate.Parameters.Add("@isVehicleVerified", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsVehicleVerified);
            cmdUpdate.Parameters.Add("@IsRecovery", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsRecovery);
            cmdUpdate.Parameters.Add("@RecoveryBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RecoveryBy);
            cmdUpdate.Parameters.Add("@RecoveryOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RecoveryOn);
            cmdUpdate.Parameters.Add("@IsWeighing", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsWeighing);
            cmdUpdate.Parameters.Add("@CommercialApproval", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CommercialApproval);
            cmdUpdate.Parameters.Add("@GraderId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.GraderId);
            cmdUpdate.Parameters.Add("@IsGradingCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsGradingCompleted);
            cmdUpdate.Parameters.Add("@IsCorrectionCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsCorrectionCompleted);
            cmdUpdate.Parameters.Add("@IsUnloadingCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsUnloadingCompleted);
            cmdUpdate.Parameters.Add("@CommercialVerified", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CommercialVerified);
            cmdUpdate.Parameters.Add("@IsBoth", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsBoth);
            cmdUpdate.Parameters.Add("@IsFixed", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsFixed);
            cmdUpdate.Parameters.Add("@NarrationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.NarrationId);
            cmdUpdate.Parameters.Add("@TransportAmtPerMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.TransportAmtPerMT);
            cmdUpdate.Parameters.Add("@RejectedQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedQty);
            cmdUpdate.Parameters.Add("@RejectedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedBy);
            cmdUpdate.Parameters.Add("@RejectedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedOn);
            cmdUpdate.Parameters.Add("@VehRejectReasonId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehRejectReasonId);
            cmdUpdate.Parameters.Add("@CorretionCompletedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CorretionCompletedOn);
            cmdUpdate.Parameters.Add("@IsVehicleOut", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsVehicleOut);
            cmdUpdate.Parameters.Add("@ModbusRefId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ModbusRefId);
            cmdUpdate.Parameters.Add("@LatestWtTakenOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LatestWtTakenOn);
            cmdUpdate.Parameters.Add("@IsGradingUnldCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsGradingUnldCompleted);
            cmdUpdate.Parameters.Add("@GateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.GateId);
            cmdUpdate.Parameters.Add("@Height", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Height);
            cmdUpdate.Parameters.Add("@Width", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Width);
            cmdUpdate.Parameters.Add("@Length", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Length);
            cmdUpdate.Parameters.Add("@ReportedDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ReportedDate);
            cmdUpdate.Parameters.Add("@GradingDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.GradingComplOn);
            cmdUpdate.Parameters.Add("@UnldDatePadtaPerTon", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UnldDatePadtaPerTon);
            cmdUpdate.Parameters.Add("@UnldDatePadtaPerTonForNC", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UnldDatePadtaPerTonForNC);
            cmdUpdate.Parameters.Add("@VehRejectReasonDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehRejectReasonDesc);
            cmdUpdate.Parameters.Add("@ProcessChargePerVeh", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ProcessChargePerVeh);
            cmdUpdate.Parameters.Add("@ProcessChargePerMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ProcessChargePerMT);
            cmdUpdate.Parameters.Add("@enqQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.EnqQty);
            cmdUpdate.Parameters.Add("@CorreNarration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CorreNarration);

            return cmdUpdate.ExecuteNonQuery();


        }


        public int ExecuteUpdationCommandStatusOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
            //"  [idPurchaseScheduleSummary] = @IdPurchaseScheduleSummary" +
            "  [purchaseEnquiryId]= @PurchaseEnquiryId" +
            " ,[supplierId]= @SupplierId" +
            " ,[statusId]= @StatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[scheduleDate]= @ScheduleDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[qty]= @Qty" +
            " ,[calculatedMetalCost]= @CalculatedMetalCost" +
            " ,[baseMetalCost]= @BaseMetalCost" +
            " ,[padta]= @Padta" +
            " ,[vehicleNo]= @VehicleNo" +
            " ,[remark] = @Remark" +
            " ,[engineerId] = @EngineerId" +
            " ,[supervisorId] = @SupervisorId" +
            " ,[locationId] = @LocationId" +
            " ,[photographerId] = @PhotographerId" +
            " ,[qualityFlag] = @QualityFlag" +
            " ,[spotEntryVehicleId] = @SpotEntryVehicleId" +
            " ,[cOrNCId] = @COrNCId" +
            " ,[vehicleCatId] = @VehicleCatId" +
            " ,[vehiclePhaseId] = @VehiclePhaseId" +
             " ,[driverName] = @DriverName" +
            " ,[vehicleTypeId] = @VehicleTypeId" +
            " ,[transPorterName] = @TransPorterName" +
            " ,[freight] = @Freight" +
            " ,[driverContactNo] = @DriverContactNo" +
            " ,[containerNo] = @ContainerNo" +
            " ,[vehicleStateId] = @VehicleStateId" +
            " ,[calculatedMetalCostForEnquiry]= @CalculatedMetalCostForEnquiry" +
            " ,[baseMetalCostForEnquiry]= @BaseMetalCostForEnquiry" +
            " ,[padtaForEnquiry]= @PadtaForEnquiry" +
            " ,[isActive]= @IsActive" +
            " ,[location]= @Location" +
            " ,[rootScheduleId]= @RootScheduleId" +
            " ,[isVehicleVerified]= @isVehicleVerified" +
            " ,[isRecovery]= @IsRecovery" +
            " ,[recoveryBy]= @RecoveryBy" +
            " ,[recoveryOn]= @RecoveryOn" +
            " ,[isWeighing]= @IsWeighing" +
            " ,[isCorrectionCompleted]= @IsCorrectionCompleted" +
            " ,[narrationId]= @NarrationId" +
            " ,[isBoth]= @IsBoth" +
            " ,[isFixed]= @IsFixed" +
            " ,[transportAmtPerMT]= @TransportAmtPerMT" +
            " ,[rejectedQty] = @RejectedQty" +
            " ,[rejectedBy] = @RejectedBy" +
            " ,[rejectedOn] = @RejectedOn" +
            " ,[vehRejectReasonId] = @VehRejectReasonId" +
            " ,[corretionCompletedOn] = @CorretionCompletedOn" +
            " ,[isVehicleOut] = @IsVehicleOut" +
            " ,[latestWtTakenOn] = @LatestWtTakenOn" +
            " ,[scheduleQty] = @OrgScheduleQty" +
            " ,[isGradingUnldCompleted] = @IsGradingUnldCompleted" +
            " ,[unldDatePadtaPerTon] = @UnldDatePadtaPerTon" +
            " ,[unldDatePadtaPerTonForNC] = @UnldDatePadtaPerTonForNC" +
            " ,[vehRejectReasonDesc] = @VehRejectReasonDesc" +
            " ,[processChargePerVeh] = @ProcessChargePerVeh" +
            " ,[processChargePerMT] = @ProcessChargePerMT" +
            " ,[enqQty] = @enqQty" +
            " ,[correNarration] = @CorreNarration" +
            " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            cmdUpdate.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.PurchaseEnquiryId;
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SupplierId);
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@ScheduleDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseScheduleSummaryTO.ScheduleDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.Qty;
            cmdUpdate.Parameters.Add("@CalculatedMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.CalculatedMetalCost;
            cmdUpdate.Parameters.Add("@BaseMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.BaseMetalCost;
            cmdUpdate.Parameters.Add("@Padta", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.Padta;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.VehicleNo;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(string.Empty);
            cmdUpdate.Parameters.Add("@EngineerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.EngineerId);
            cmdUpdate.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SupervisorId);
            cmdUpdate.Parameters.Add("@PhotographerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.PhotographerId);
            cmdUpdate.Parameters.Add("@QualityFlag", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.QualityFlag);
            cmdUpdate.Parameters.Add("@SpotEntryVehicleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SpotEntryVehicleId);
            cmdUpdate.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LocationId);
            cmdUpdate.Parameters.Add("@OldScheduleDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.OldScheduleDate);
            cmdUpdate.Parameters.Add("@COrNCId", System.Data.SqlDbType.Int).Value = (tblPurchaseScheduleSummaryTO.COrNcId);
            cmdUpdate.Parameters.Add("@VehicleCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleCatId);
            cmdUpdate.Parameters.Add("@VehiclePhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehiclePhaseId);
            cmdUpdate.Parameters.Add("@CalculatedMetalCostForEnquiry", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.CalculatedMetalCostForNC;
            cmdUpdate.Parameters.Add("@BaseMetalCostForEnquiry", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.BaseMetalCostForNC;
            cmdUpdate.Parameters.Add("@PadtaForEnquiry", System.Data.SqlDbType.NVarChar).Value = tblPurchaseScheduleSummaryTO.PadtaForNC;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = (tblPurchaseScheduleSummaryTO.IsActive);
            cmdUpdate.Parameters.Add("@Location", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Location);
            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RootScheduleId);

            cmdUpdate.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.DriverName);
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleTypeId);
            cmdUpdate.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Freight);
            cmdUpdate.Parameters.Add("@DriverContactNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.DriverContactNo);
            cmdUpdate.Parameters.Add("@ContainerNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ContainerNo);
            cmdUpdate.Parameters.Add("@VehicleStateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleStateId);
            cmdUpdate.Parameters.Add("@TransporterName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.TransporterName);
            cmdUpdate.Parameters.Add("@isVehicleVerified", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsVehicleVerified);
            cmdUpdate.Parameters.Add("@IsRecovery", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsRecovery);
            cmdUpdate.Parameters.Add("@RecoveryBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RecoveryBy);
            cmdUpdate.Parameters.Add("@RecoveryOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RecoveryOn);
            cmdUpdate.Parameters.Add("@IsWeighing", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsWeighing);
            cmdUpdate.Parameters.Add("@IsCorrectionCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsCorrectionCompleted);
            cmdUpdate.Parameters.Add("@IsBoth", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsBoth);
            cmdUpdate.Parameters.Add("@NarrationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.NarrationId);
            cmdUpdate.Parameters.Add("@IsFixed", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsFixed);
            cmdUpdate.Parameters.Add("@TransportAmtPerMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.TransportAmtPerMT);
            cmdUpdate.Parameters.Add("@RejectedQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedQty);
            cmdUpdate.Parameters.Add("@RejectedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedBy);
            cmdUpdate.Parameters.Add("@RejectedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RejectedOn);
            cmdUpdate.Parameters.Add("@VehRejectReasonId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehRejectReasonId);
            cmdUpdate.Parameters.Add("@CorretionCompletedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CorretionCompletedOn);
            cmdUpdate.Parameters.Add("@IsVehicleOut", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsVehicleOut);
            cmdUpdate.Parameters.Add("@LatestWtTakenOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.LatestWtTakenOn);
            cmdUpdate.Parameters.Add("@OrgScheduleQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.OrgScheduleQty);
            cmdUpdate.Parameters.Add("@IsGradingUnldCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsGradingUnldCompleted);
            cmdUpdate.Parameters.Add("@UnldDatePadtaPerTon", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UnldDatePadtaPerTon);
            cmdUpdate.Parameters.Add("@UnldDatePadtaPerTonForNC", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UnldDatePadtaPerTonForNC);
            cmdUpdate.Parameters.Add("@VehRejectReasonDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehRejectReasonDesc);
            cmdUpdate.Parameters.Add("@ProcessChargePerVeh", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ProcessChargePerVeh);
            cmdUpdate.Parameters.Add("@ProcessChargePerMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ProcessChargePerMT);
            cmdUpdate.Parameters.Add("@enqQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.EnqQty);
            cmdUpdate.Parameters.Add("@CorreNarration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CorreNarration);


            return cmdUpdate.ExecuteNonQuery();


        }

        public int UpdateCorrectionCompletedFlag(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
            " [isRecovery]= @IsRecovery" +
            " ,[isGradingCompleted]= @IsGradingCompleted" +
            " ,[isCorrectionCompleted]= @IsCorrectionCompleted" +
            " ,[isUnloadingCompleted]= @IsUnloadingCompleted" +
            // " ,[updatedOn]= @UpdatedOn" +
             " ,[narrationId]= @NarrationId" +
             " ,[corretionCompletedOn]= @CorretionCompletedOn" + //Prajakta[2019-03-06] Added
             " ,[isBoth]= @IsBoth" +
              " ,[correNarration]= @CorreNarration" +
            " WHERE isnull(rootScheduleId,idPurchaseScheduleSummary) = @RootScheduleId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.RootScheduleId);
            cmdUpdate.Parameters.Add("@IsRecovery", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsRecovery);
            cmdUpdate.Parameters.Add("@IsGradingCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsGradingCompleted);
            cmdUpdate.Parameters.Add("@IsCorrectionCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsCorrectionCompleted);
            cmdUpdate.Parameters.Add("@IsUnloadingCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsUnloadingCompleted);
            // cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@NarrationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.NarrationId);
            cmdUpdate.Parameters.Add("@CorretionCompletedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CorretionCompletedOn);
            cmdUpdate.Parameters.Add("@IsBoth", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsBoth);
            cmdUpdate.Parameters.Add("@CorreNarration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.CorreNarration);

            return cmdUpdate.ExecuteNonQuery();


        }

        public int UpdateScheduleSupplier(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
             "[supplierId]= @SupplierId" +
             ",[purchaseEnquiryId]= @PurchaseEnquiryId" +
            " WHERE isnull(rootScheduleId,idPurchaseScheduleSummary) = @RootScheduleId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.Connection = conn;
            cmdUpdate.Transaction = tran;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ActualRootScheduleId);
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.SupplierId);
            cmdUpdate.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.PurchaseEnquiryId);

            return cmdUpdate.ExecuteNonQuery();


        }

        public int UpdateVehProcessCharge(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
             "[processChargePerVeh]= @ProcessChargePerVeh" +
             ",[processChargePerMT]= @ProcessChargePerMT" +
            " WHERE isnull(rootScheduleId,idPurchaseScheduleSummary) = @RootScheduleId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.Connection = conn;
            cmdUpdate.Transaction = tran;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ActualRootScheduleId);
            cmdUpdate.Parameters.Add("@ProcessChargePerVeh", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ProcessChargePerVeh);
            cmdUpdate.Parameters.Add("@ProcessChargePerMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ProcessChargePerMT);

            return cmdUpdate.ExecuteNonQuery();


        }

        public int UpdateIsGradingWhileUnloadingFlag(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
             "[isGradingUnldCompleted]= @IsGradingUnldCompleted" +
            " WHERE isnull(rootScheduleId,idPurchaseScheduleSummary) = @RootScheduleId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.Connection = conn;
            cmdUpdate.Transaction = tran;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.ActualRootScheduleId);
            cmdUpdate.Parameters.Add("@IsGradingUnldCompleted", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IsGradingUnldCompleted);

            return cmdUpdate.ExecuteNonQuery();


        }


        public int updateDensityAndVehicleTypeForCurrentPhase(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
             "[height]= @Height" +
             ",[width]= @Width" +
             ",[length]= @Length" +
             ",[vehicleTypeId]= @VehicleTypeId" +
            " WHERE idPurchaseScheduleSummary = @IdPurchaseScheduleSummary ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.Connection = conn;
            cmdUpdate.Transaction = tran;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary);
            cmdUpdate.Parameters.Add("@Height", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Height);
            cmdUpdate.Parameters.Add("@Width", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Width);
            cmdUpdate.Parameters.Add("@Length", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.Length);
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleSummaryTO.VehicleTypeId);

            return cmdUpdate.ExecuteNonQuery();


        }


       public int UpdateIsFreightDetailsAdded(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran)
        {

            try
            {
                SqlCommand cmdUpdate = new SqlCommand();

                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                 "[isFreightAdded]= @IsFreightAdded" +
                " WHERE rootScheduleId = @RootScheduleId ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.PurchaseScheduleSummaryId);
                cmdUpdate.Parameters.Add("@IsFreightAdded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.IsFreightAdded);

                return cmdUpdate.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }
        public int SelectScheduledVehiclesAgainstEnquiry(int purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            Int32 count = 0;
            try
            {
                cmdSelect.CommandText = SelectQuery() + "WHERE [purchaseEnquiryId]= " + purchaseEnquiryId + " AND IsActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null && list.Count > 0)
                {
                    count = list.Count;
                }
                return count;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {

                cmdSelect.Dispose();
            }
        }



        #endregion

        #region Deletion
        public int DeleteTblPurchaseScheduleSummary(Int32 idPurchaseScheduleSummary)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseScheduleSummary, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblPurchaseScheduleSummary(Int32 idPurchaseScheduleSummary, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseScheduleSummary, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idPurchaseScheduleSummary, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseScheduleSummary] " +
            " WHERE idPurchaseScheduleSummary = " + idPurchaseScheduleSummary + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            return cmdDelete.ExecuteNonQuery();
        }

        public List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryTOByModBusRefId(Int32 modbusRefId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectQuery() + " WHERE SRC.modbusRefId = " + modbusRefId + "";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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
        //chetan[20-jan-2020] added for data write IOT to DB
        public List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummaryTOListFromStatusIds(String statusId) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQueryNew();
                cmdSelect.CommandText = SelectQuery() +
               " WHERE  SRC.statusId in(" + statusId + ") AND ISNULL(SRC.isActive,0)=1";

                // cmdSelect.CommandText += " WHERE tblPurchaseScheduleSummary.vehicleNo=" + "'" + vehicleNo + "'" + " and tblPurchaseScheduleSummary.statusId=" + statusId;

                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public int UpdateParentScheduleIdToNUll(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [parentPurchaseScheduleSummaryId]= NULL" +
                " WHERE parentPurchaseScheduleSummaryId = @IdPurchaseScheduleSummary";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = scheduleSummaryTO.IdPurchaseScheduleSummary;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateCorNcIdForBothVehicle(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                " [corncid]= @Corncid" +
                " WHERE rootScheduleId = @RootScheduleId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = scheduleSummaryTO.ActualRootScheduleId;
                cmdUpdate.Parameters.Add("@Corncid", System.Data.SqlDbType.Int).Value = scheduleSummaryTO.COrNcId;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }


        #endregion

    }
}
