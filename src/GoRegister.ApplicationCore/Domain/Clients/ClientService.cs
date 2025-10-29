/*  MRF Changes : Added function to get client details for given Client ID 
    Modified Date : 16th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-213 

    MRF Changes : Added function to get client data
    Modified Date : 17th October 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-238-New
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoRegister.ApplicationCore.Framework.DataTables;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GoRegister.ApplicationCore.Domain.Clients
{
    public interface IClientService
    {
        Task<IEnumerable<ClientModel>> GetList();
        Task<ClientModel> GetById(int id);
        Task<ClientModel> Save(ClientModel model);
        IEnumerable<ClientModel> FilterClientTable(DataTables.DtParameters dtParameters, IEnumerable<ClientModel> clients);
        IEnumerable<ClientModel> OrderClientTable(DataTables.DtParameters dtParameters, IEnumerable<ClientModel> clients);
        Task<List<SelectListItem>> GetClientDropdownList();

        Task<ClientEmailModel> SaveEmailAsync(CreateEditClientEmailModel model);
        Task<bool> DeleteEmailAsync(int emailId);


        Task<List<SelectListItem>> GetClientDropdownListMRF(int clientId);

        Task<Client> GetClientById(int id);

        Task<TPNCountryClientEmailModel> SaveTPNEmailAsync(TPNCountryClientEmailModel model);

        Task<List<TPNCountryClientEmail>> GetTPNEmailsById(int id);
        Task<bool> DeleteTPNEmailAsync(int emailId,int userid);

        Task<TPNCountryClientEmailModel> GetTPNEmailDetails(int id);
        Task<TPNClientGAMappingModel> SaveTPNClientGAMappingAsync(TPNClientGAMappingModel model);

        Task<List<TPNClientGAMappingModel>> GetTPNClientGAMappingsById(int id);

        Task<TPNClientGAMappingModel> GetTPNClientGAMappingDetails(int mappingId);

        Task<bool> DeleteTPNClientGAMappingAsync(int mappingId, int userid);

    }

    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
      

        public ClientService(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
           
        }

        public async Task<IEnumerable<ClientModel>> GetList()
        {
            var clients = await _dbContext.Clients.AsNoTracking().ToListAsync();
            return _mapper.Map<List<ClientModel>>(clients);
        }

        public async Task<ClientModel> GetById(int id)
        {
            var client = await _dbContext.Clients.Include(c => c.ClientEmails).SingleOrDefaultAsync(c => c.Id == id);
           
            return _mapper.Map<ClientModel>(client);
           
        }

        //public async Task<ClientModel> GetTPNById(int id)
        //{
        //    var client = await _dbContext.Clients.Include(c => c.TPNCountryClientEmails).SingleOrDefaultAsync(c => c.Id == id); ;

        //    return _mapper.Map<ClientModel>(client);


        //}

        public async Task<List<TPNCountryClientEmail>> GetTPNEmailsById(int id)
        {
            var tpnclient = await _dbContext.TPNCountryClientEmails.Where(e => e.ClientId == id && e.IsDeleted == false).ToListAsync();

            return (tpnclient);


        }

        public async Task<TPNCountryClientEmailModel> GetTPNEmailDetails(int emailId)
        {
            var tpnClient = await _dbContext.TPNCountryClientEmails.SingleOrDefaultAsync(c => c.Id == emailId);

            return _mapper.Map<TPNCountryClientEmailModel>(tpnClient);


        }


        public async Task<ClientModel> Save(ClientModel model)
        {
            var client = _mapper.Map<Client>(model);

            if (client.Id == 0)
            {
                var ext = _dbContext.Clients.Any(c => c.Name == model.Name);

                if (!ext)
                {
                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        client.ClientEmails = new List<ClientEmail> { new ClientEmail { Email = model.Email } };
                    };
                    // Create a new client
                    client.DateCreated = DateTime.Now;
                    _dbContext.Clients.Add(client);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var ext = _dbContext.Clients.Any(c => c.Name == model.Name);
                if (!ext)
                {
                    // update exisitng client
                    _dbContext.Clients.Update(client);
                }
                else
                {
                    return null;
                }
            }

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ClientModel>(client);
        }

        public IEnumerable<ClientModel> FilterClientTable(DataTables.DtParameters dtParameters, IEnumerable<ClientModel> clients)
        {
            var searchBy = dtParameters.Search?.Value;
            if (!string.IsNullOrEmpty(searchBy))
            {
                clients = clients
                    .Where(c => c.Name?.ToUpper().Contains(searchBy.ToUpper()) == true
                                || c.Name?.ToUpper().Contains(searchBy.ToUpper()) == true
                    );
            }

            return clients;
        }

        public IEnumerable<ClientModel> OrderClientTable(DataTables.DtParameters dtParameters, IEnumerable<ClientModel> clients)
        {
            string orderCriteria;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
            }

            clients = orderAscendingDirection
                ? clients.AsQueryable().OrderByDynamic(orderCriteria, DataTablesOrderExtensions.Order.Asc)
                : clients.AsQueryable().OrderByDynamic(orderCriteria, DataTablesOrderExtensions.Order.Desc);
            return clients;
        }

        public async Task<ClientEmailModel> SaveEmailAsync(CreateEditClientEmailModel model)
        {
            var client = await _dbContext.Clients.Include(c => c.ClientEmails).SingleOrDefaultAsync(c => c.Id == model.ClientId);

            var ext = client.ClientEmails.Any(ce => ce.Email == model.Email);
            if (!ext)
            {
                var email = client.ClientEmails.SingleOrDefault(ce => ce.Id == model.Id);


                if (email != null) // update email
                {
                    email.Email = model.Email;
                    _dbContext.Update(email);
                    var result = await _dbContext.SaveChangesAsync();
                    return _mapper.Map<ClientEmailModel>(email);
                }
                else // new email
                {
                    var newEmail = new ClientEmail
                    {
                        Email = model.Email,
                        Client = client
                    };

                    _dbContext.Add(newEmail);
                    var result = await _dbContext.SaveChangesAsync();
                    return _mapper.Map<ClientEmailModel>(newEmail);
                }
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> DeleteEmailAsync(int emailId)
        {
            var email = await _dbContext.ClientEmails.FindAsync(emailId);
            if (email == null) return false;

            _dbContext.ClientEmails.Remove(email);
            var deleted = await _dbContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> DeleteTPNEmailAsync(int emailId, int userid)
        {
            var tpnemail = await _dbContext.TPNCountryClientEmails.FindAsync(emailId);
            if (tpnemail == null) return false;

            tpnemail.IsDeleted = true;
            tpnemail.DateModified = DateTime.Now;
            tpnemail.ModifiedBy = userid;

            var deleted = await _dbContext.SaveChangesAsync(); 

            return deleted > 0;
        }

        public async Task<List<SelectListItem>> GetClientDropdownList()
        {
            var clients = await GetList();
            var clientList = new List<SelectListItem>();

            foreach (var client in clients)
            {
                clientList.Add(new SelectListItem() { Text = client.Name, Value = client.Id.ToString() });
            }

            return clientList;
        }

        public async Task<List<SelectListItem>> GetClientDropdownListMRF(int clientId)
        {
            var client = await GetById(clientId);
            var clientList = new List<SelectListItem>();
            clientList.Add(new SelectListItem() { Text = client.Name, Value = client.Id.ToString() });
            return clientList;
        }


        public async Task<Client> GetClientById(int id)
        {
            var client = await _dbContext.Clients.SingleOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<Client>(client);
        }


        public async Task<TPNCountryClientEmailModel> SaveTPNEmailAsync(TPNCountryClientEmailModel model)
        {
            try
            {

                var client = await _dbContext.Clients.Include(c => c.TPNCountryClientEmails).SingleOrDefaultAsync(c => c.Id == model.ClientId);

                var ext = client.TPNCountryClientEmails.Any(ce => ce.TPNEmail == model.TPNEmail && ce.TPNCountry == model.TPNCountry && ce.IsDeleted==false);
                if (!ext)
                {
                    var tccemail = client.TPNCountryClientEmails.SingleOrDefault(ce => ce.Id == model.Id);


                    if (tccemail != null) // update tcc email
                    {
                        tccemail.TPNEmail = model.TPNEmail;
                        tccemail.DateModified = DateTime.Now;
                        tccemail.ModifiedBy = model.ModifiedBy;
                        _dbContext.Update(tccemail);
                        var result = await _dbContext.SaveChangesAsync();
                        return _mapper.Map<TPNCountryClientEmailModel>(tccemail);
                    }
                    else // new tcc email
                    {
                        var newtccEmail = new TPNCountryClientEmail
                        {
                            TPNEmail = model.TPNEmail,
                            ClientId = model.ClientId,
                            TPNCountry = model.TPNCountry,
                            ClientUuid = model.ClientUuid,
                            DateModified = DateTime.Now,
                            ModifiedBy = model.ModifiedBy
                           

                        };

                        _dbContext.Add(newtccEmail);
                        var result = await _dbContext.SaveChangesAsync();
                        return _mapper.Map<TPNCountryClientEmailModel>(newtccEmail);
                    }
                }
                else
                {
                    var tccemail = client.TPNCountryClientEmails.SingleOrDefault(ce => ce.Id == model.Id);


                    if (tccemail != null) // update tcc email
                    {
                        tccemail.TPNEmail = model.TPNEmail;
                        tccemail.DateModified = DateTime.Now;
                        tccemail.ModifiedBy = model.ModifiedBy;
                        _dbContext.Update(tccemail);
                        var result = await _dbContext.SaveChangesAsync();
                        return _mapper.Map<TPNCountryClientEmailModel>(tccemail);
                    }
                    else {
                        return null;
                    }
                    //return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<TPNClientGAMappingModel>> GetTPNClientGAMappingsById(int id)
        {

            var tpnclientMapping = await (from t in _dbContext.TPNCountryAdminMapping
                                          join c in _dbContext.Clients on t.ClientId equals c.Id
                                          where t.ClientId == id && t.IsDeleted == false
                                          select new TPNClientGAMappingModel
                                          {
                                              Id = t.Id,
                                              ClientId = t.ClientId,
                                              ClientName = c.Name,
                                              ClientUuid = c.ClientUuid,
                                              TPNCountry = t.TPNCountry,
                                              AdminUserId = t.AdminUserId,
                                              GAMEmail = t.GAMEmail,
                                              ReportFrequency = t.ReportFrequency,
                                              DateModified = t.DateModified,
                                              ModifiedBy = t.ModifiedBy
                                          }).ToListAsync();

            return tpnclientMapping;
        }

        public async Task<TPNClientGAMappingModel> GetTPNClientGAMappingDetails(int mappingId)
        {
            var tpnCountryAdminMapping = await _dbContext.TPNCountryAdminMapping.SingleOrDefaultAsync(t => t.Id == mappingId);

            return _mapper.Map<TPNClientGAMappingModel>(tpnCountryAdminMapping);
        }

        public async Task<TPNClientGAMappingModel> SaveTPNClientGAMappingAsync(TPNClientGAMappingModel model)
        {
            try
            {
                var client = await _dbContext.Clients.Include(c => c.TPNCountryAdminMappings).SingleOrDefaultAsync(c => c.Id == model.ClientId);

                var TPNAdminMapping = client.TPNCountryAdminMappings.Any(t => t.ClientId == model.ClientId && t.GAMEmail == model.GAMEmail && t.TPNCountry == model.TPNCountry && t.IsDeleted == false);

                if (!TPNAdminMapping)
                {
                    var adminMapping = client.TPNCountryAdminMappings.SingleOrDefault(t => t.Id == model.Id);

                    if (adminMapping != null) // update tcc email
                    {

                        adminMapping.ClientId = model.ClientId;
                        adminMapping.ClientUuid = model.ClientUuid;
                        adminMapping.TPNCountry = model.TPNCountry;
                        adminMapping.AdminUserId = model.AdminUserId;
                        adminMapping.GAMEmail = model.GAMEmail;
                        //adminMapping.ReportFrequency = model.ReportFrequency;
                        adminMapping.ReportFrequency = Data.Enums.ReportFrequency.Weekly;
                        adminMapping.DateModified = DateTime.Now;
                        adminMapping.ModifiedBy = model.ModifiedBy;

                        _dbContext.Update(adminMapping);
                        var result = await _dbContext.SaveChangesAsync();
                        return _mapper.Map<TPNClientGAMappingModel>(adminMapping);
                    }
                    else // new tcc email
                    {
                        var tpnCountryAdminMapping = new TPNCountryAdminMapping
                        {
                            ClientId = model.ClientId,
                            ClientUuid = model.ClientUuid,
                            TPNCountry = model.TPNCountry,
                            AdminUserId = model.AdminUserId,
                            GAMEmail = model.GAMEmail,
                            //ReportFrequency = model.ReportFrequency,
                            ReportFrequency = Data.Enums.ReportFrequency.Weekly,
                            DateModified = DateTime.Now,
                            ModifiedBy = model.ModifiedBy

                        };

                        _dbContext.Add(tpnCountryAdminMapping);
                        var result = await _dbContext.SaveChangesAsync();
                        return _mapper.Map<TPNClientGAMappingModel>(tpnCountryAdminMapping);
                    }
                }
                else
                {
                    var adminMapping = client.TPNCountryAdminMappings.SingleOrDefault(t => t.Id == model.Id);

                    if (adminMapping != null) // update tcc email
                    {
                        adminMapping.ClientId = model.ClientId;
                        adminMapping.ClientUuid = model.ClientUuid;
                        adminMapping.TPNCountry = model.TPNCountry;
                        adminMapping.AdminUserId = model.AdminUserId;
                        adminMapping.GAMEmail = model.GAMEmail;
                        //adminMapping.ReportFrequency = model.ReportFrequency;
                        adminMapping.ReportFrequency = Data.Enums.ReportFrequency.Weekly;
                        adminMapping.DateModified = DateTime.Now;
                        adminMapping.ModifiedBy = model.ModifiedBy;
                        adminMapping.IsDeleted = false;

                        _dbContext.Update(adminMapping);
                        var result = await _dbContext.SaveChangesAsync();
                        return _mapper.Map<TPNClientGAMappingModel>(adminMapping);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTPNClientGAMappingAsync(int mappingId, int userid)
        {
            var tpnClientGAMapping = await _dbContext.TPNCountryAdminMapping.FindAsync(mappingId);
            if (tpnClientGAMapping == null) return false;

            tpnClientGAMapping.IsDeleted = true;
            tpnClientGAMapping.DateModified = DateTime.Now;
            tpnClientGAMapping.ModifiedBy = userid;

            var deleted = await _dbContext.SaveChangesAsync();

            return deleted > 0;
        }

    }
}
