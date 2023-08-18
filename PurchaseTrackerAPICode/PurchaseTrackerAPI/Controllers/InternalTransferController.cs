using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DashboardModels;
using System.Globalization;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class InternalTransferController : Controller
    {
        private readonly ITblVehicleBL _iTblVehicleBL;
        private readonly ITblTransferRequestBL _iTblTransferRequestBL;
        private readonly Idimensionbl _idimensionbl;

        #region Vehical
        public InternalTransferController(ITblVehicleBL iTblVehicleBL, ITblTransferRequestBL iTblTransferRequestBL, Idimensionbl idimensionbl)
        {
            _iTblVehicleBL = iTblVehicleBL;
            _iTblTransferRequestBL = iTblTransferRequestBL;
            _idimensionbl = idimensionbl;
        }
        [Route("PostTransferVehical")]
        [HttpPost]
        public ResultMessage PostInternalTransferVehical([FromBody]TblVehicleTO tblVehicleTO)
        {
             return  _iTblVehicleBL.InsertTblVehicle(tblVehicleTO); 
        }
        [Route("UpdateTransferVehical")]
        [HttpPut]
        public ResultMessage UpdateInternalTransferVehical([FromBody]TblVehicleTO tblVehicleTO)
        {
            return _iTblVehicleBL.UpdateTblVehicleDtl (tblVehicleTO);
        }

        [Route("GetTransferVehical")]
        [HttpGet]
        public TblVehicleTO GetInternalTransferVehicalDetails(int VehicalId)
        {
            return _iTblVehicleBL.SelectTblVehicleTO(VehicalId);
        }
        [Route("GetAllTranferVehical")]
        [HttpPost]
        public List<TblVehicleTO> GetAllInternalTranferVehical([FromBody] InternalTransferFilterTO InternalTransferFilterTO)
        {
            return _iTblVehicleBL .GetAllInternalTranferVehical(InternalTransferFilterTO);
        }
        [Route("UpdateVehicalApprovalStatus")]
        [HttpPut]
        public ResultMessage UpdateVehicalApprovalStatus([FromBody]TblVehicleTO tblVehicleTO)
        {
            return _iTblVehicleBL.UpdateVehicalApprovalStatus(tblVehicleTO);
        }
        [Route("UpdateVehicalStatus")]
        [HttpPost]
        public ResultMessage UpdateVehicalStatus([FromBody] TblVehicleTO tblVehicleTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iTblVehicleBL.UpdateVehicalStatus(tblVehicleTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to AddSLA");
                return resultMessage;
            }
        }
        #endregion

        #region Internal Tranfer request
        [Route("PostInternalTransferRequest")]
        [HttpPost]
        public ResultMessage PostInternalTransferRequest([FromBody]  TblTransferRequestTO tblTransferRequestTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                //TblTransferRequestTO tblTransferRequestTO = JsonConvert.DeserializeObject<TblTransferRequestTO>(data[].ToString());
                tblTransferRequestTO.StatusId = (int)Constants.InternalTransferRequestStatusE.NEW;
                resultMessage = _iTblTransferRequestBL.InsertTblTransferRequestData(tblTransferRequestTO);
            }
            catch (Exception ex)
            {
                return null;
            }
            return resultMessage;
        }
        [Route("UpdateInternalTransferRequest")]
        [HttpPut]
        public ResultMessage UpdateInternalTransferRequest([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblTransferRequestTO tblTransferRequestTO = JsonConvert.DeserializeObject<TblTransferRequestTO>(data["TblTransferRequestTO"].ToString());

                int result = _iTblTransferRequestBL.UpdateTblTransferRequest(tblTransferRequestTO);

            }
            catch (Exception ex)
            {
                return null;
            }
            return resultMessage;
        }

        [Route("GetAllInternalTransferRequest")]
        [HttpPost ]
        public List<TblTransferRequestTO> GetAllInternalTranferRequest([FromBody] InternalTransferFilterTO InternalTransferFilterTO)
        {
            return _iTblTransferRequestBL.GetTransferRequestDtlList(InternalTransferFilterTO);
        }
        [Route("GetInternalTransferRequest")]
        [HttpGet]
        public TblTransferRequestTO GetInternalTranferRequest( int idTranferRequest)
        {
            return _iTblTransferRequestBL.SelectTblTransferRequestTO(idTranferRequest);
        }
        #endregion
        //Master Data
        #region Master data
        [Route("PostGenericMasterData")]
        [HttpPost]
        public ResultMessage PostGenericMasterData([FromBody] DimGenericMasterTO DimGenericMasterTO)
        {
            return _idimensionbl.PostGenericMasterData(DimGenericMasterTO);
        }
        [Route("UpdateGenericMasterData")]
        [HttpPut]
        public ResultMessage UpdateGenericMasterData([FromBody] DimGenericMasterTO DimGenericMasterTO)
        {
            return _idimensionbl.UpdateGenericMasterData(DimGenericMasterTO);
        }

        [Route("GetGenericMasterData")]
        [HttpGet]
        public List<DimGenericMasterTO> GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0)
        {
            return _idimensionbl.GetGenericMasterData(IdDimension, SkipIsActiveFilter, ParentIdGenericMaster);
        }

        [Route("GetApprovalUserList")]
        [HttpGet]
        public ResultMessage GetApprovalUserList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                return _idimensionbl.GetApprovalUserList();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error in GetApprovalUserList");
                return resultMessage;
            }
        }

        #endregion
    }
}
