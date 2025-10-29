using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.MRFAPI.Services;
using GoRegister.MRFAPI.ViewModels;

namespace GoRegister.MRFAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MRFClientRequestController : ControllerBase
    {
        IMRFClientRequestService _mrfclientrequestService;
        public MRFClientRequestController(IMRFClientRequestService service)
        {
            _mrfclientrequestService = service;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllMRFClientRequest()
        {
            try
            {
                var mrfclient = _mrfclientrequestService.GetMRFClientRequestList();
                if (mrfclient == null)
                    return NotFound();
                return Ok(mrfclient);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// get mrfclient request details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/id")]
        public IActionResult GetMRFClientRequestById(int id)
        {
            try
            {
                var mrfclient = _mrfclientrequestService.GetMRFClientRequestDetailsById(id);
                if (mrfclient == null)
                    return NotFound();
                return Ok(mrfclient);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// save mrfclient request
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveMRFClientRequest(MRFClientRequest mrfclientModel)
        {
            try
            {
                var model = _mrfclientrequestService.SaveMRFClientRequest(mrfclientModel);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// delete mrfclient request  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteMRFClientRequest(int id)
        {
            try
            {
                var model = _mrfclientrequestService.DeleteMRFClientRequest(id);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
