using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.MRFAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GoRegister.MRFAPI.Services
{
    public class MRFClientRequestService : IMRFClientRequestService
    {
        private ApplicationDbContext _dbContext;

        public MRFClientRequestService(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public ResponseModel DeleteMRFClientRequest(int clientuniqueid)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                MRFClientRequest _temp = GetMRFClientRequestDetailsById(clientuniqueid);
                if (_temp != null)
                {
                    _dbContext.Remove<MRFClientRequest>(_temp);
                    _dbContext.SaveChanges();
                    model.IsSuccess = true;
                    model.Messsage = "MRFClientRequest Deleted Successfully";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "MRFClientRequest Not Found";
                }

            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }

        public MRFClientRequest GetMRFClientRequestDetailsById(int clientuniqueid)
        {
            MRFClientRequest mrf;
            try
            {
                mrf = _dbContext.Find<MRFClientRequest>(clientuniqueid);
            }
            catch (Exception)
            {
                throw;
            }
            return mrf;
        }

        public MRFClientRequest GetMRFClientRequestDetailsByuId(string clientuid)
        {
            MRFClientRequest mrf = new MRFClientRequest();
            try
            {
                var _temp = _dbContext.MRFClientRequest.Where(c => c.ClientUuid == clientuid).ToList();
                if (_temp.GetEnumerator().MoveNext() == true)
                {
                    foreach (var newmrf in _temp)
                    {
                        mrf.ClientID = newmrf.ClientID;
                        mrf.ClientName = newmrf.ClientName;
                        mrf.ClientUuid = newmrf.ClientUuid;
                        mrf.ClientuniqueID = newmrf.ClientuniqueID;
                    }

                    //var _tempdetails = _dbContext.MRFClientRequestCountry.Where(x => x.ClientuniqueID == mrf.ClientuniqueID).ToList();

                    //if (_tempdetails.GetEnumerator().MoveNext() == true)
                    //{
                    //    foreach (var newmrfdet in _tempdetails)
                    //    {
                    //        mrf.MRFClientRequestCountry.Add(newmrfdet);

                    //    }
                    //}
                }
                else
                {
                    mrf = null;
                }
                //mrf = _dbContext.Find<MRFClientRequest>(clientuid);
            }
            catch (Exception)
            {
                throw;
            }
            return mrf;
        }

        public List<MRFClientRequest> GetMRFClientRequestList()
        {
            List<MRFClientRequest> mrfclientList;
            try
            {
                mrfclientList = _dbContext.Set<MRFClientRequest>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return mrfclientList;
        }



        public ResponseModel SaveMRFClientRequest(MRFClientRequest mrfclientModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                MRFClientRequest _existUuid = GetMRFClientRequestDetailsByuId(mrfclientModel.ClientUuid);

                if (_existUuid != null)
                {

                    var _temp = _dbContext.MRFClientRequest
                                   .Where(p => p.ClientuniqueID == _existUuid.ClientuniqueID)
                                    .Include(p => p.MRFClientRequestCountry)
                                    .SingleOrDefault();

                    _temp.ClientID = mrfclientModel.ClientID;
                    _temp.ClientName = mrfclientModel.ClientName;
                    _temp.MRFClientStatus = mrfclientModel.MRFClientStatus;
                    _temp.deletedAt = mrfclientModel.deletedAt;

                    foreach(var existmrfclcountry in _temp.MRFClientRequestCountry.ToList())
                    {
                        _dbContext.MRFClientRequestCountry.Remove(existmrfclcountry);
                    }
                    
                    // Update and Insert children
                    foreach (var childModel in mrfclientModel.MRFClientRequestCountry)
                    {
                        var existingChild = _temp.MRFClientRequestCountry
                            .Where(c => c.ClientuniqueID == childModel.ClientuniqueID && c.ClientuniqueID != default(int))
                            .SingleOrDefault();

                        if (existingChild != null)
                            // Update child
                            _dbContext.Entry(existingChild).CurrentValues.SetValues(childModel);
                        else
                        {
                            // Insert child
                            var newChild = new MRFClientRequestCountry
                            {
                                ClientuniqueID = _temp.ClientuniqueID,
                                Countryguid = childModel.Countryguid,
                                CountryName = childModel.CountryName
                            };
                            _temp.MRFClientRequestCountry.Add(newChild);
                        }
                    }
                    _dbContext.Update<MRFClientRequest>(_temp);
                    model.Messsage = "MRFClientRequest Update Successfully";

                }
                else
                {
                    _dbContext.Add<MRFClientRequest>(mrfclientModel);
                   
                    Client _tempclient = new Client();

                    _tempclient.Name = mrfclientModel.ClientName;
                    _tempclient.DateCreated = DateTime.Now;
                    _tempclient.ClientUuid = mrfclientModel.ClientUuid;
                    _dbContext.Clients.Add(_tempclient);
                    model.Messsage = "MRFClientRequest Inserted Successfully";
                }
                _dbContext.SaveChanges(true);
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }
    }
}
