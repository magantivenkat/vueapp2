using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.MRFAPI.ViewModels;

namespace GoRegister.MRFAPI.Services
{
    public interface IMRFClientRequestService
    {
        /// <summary>
        /// get list of all mrfclient
        /// </summary>
        /// <returns></returns>
        List<MRFClientRequest> GetMRFClientRequestList();

        /// <summary>
        /// get mrfclient details by mrfclient id
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        MRFClientRequest GetMRFClientRequestDetailsById(int clientuniqueid);

        /// <summary>
        ///  add edit mrfclient
        /// </summary>
        /// <param name="mrfclientModel"></param>
        /// <returns></returns>
        ResponseModel SaveMRFClientRequest(MRFClientRequest mrfclientModel);


        /// <summary>
        /// delete mrfclient
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        ResponseModel DeleteMRFClientRequest(int clientuniqueid);
    }
}
