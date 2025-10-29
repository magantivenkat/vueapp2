/*  MRF Changes : Added method to get domain list
    Modified Date : 16th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-213   */

using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Domains
{

    public interface IDomainService
    {
        Task<Result<DomainModel>> SaveAsync(DomainModel model);
        Task<DomainModel> GetById(int id);
        Task<List<DomainModel>> GetDomainsAsync();
        Task<List<DomainModel>> GetDomainsAsync(int clientId);
        Task DeleteDomainsAsync(int domainId);

        Task<List<SelectListItem>> GetDomainDropdownListMRF();
    }

    public class DomainService : IDomainService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DomainService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteDomainsAsync(int domainId)
        {
            var domain = await _context.TenantUrls.FindAsync(domainId);
            _context.TenantUrls.Remove(domain);
            _context.SaveChanges();
        }

        public async Task<DomainModel> GetById(int id)
        {
            var domain = await _context.TenantUrls.FindAsync(id);
            return _mapper.Map<DomainModel>(domain);
        }

        public async Task<List<DomainModel>> GetDomainsAsync()
        {
            var domains = await _context.TenantUrls.ToListAsync();
            return _mapper.Map<List<DomainModel>>(domains);
        }

        public async Task<List<DomainModel>> GetDomainsAsync(int clientId)
        {
            var domains = await _context.TenantUrls
                .Where(u => u.ClientId == clientId || u.ClientId == null)
                .ToListAsync();
            return _mapper.Map<List<DomainModel>>(domains);
        }

        public async Task<Result<DomainModel>> SaveAsync(DomainModel model)
        {

            // see if already exists
            var tenantUrl = await _context.TenantUrls.FindAsync(model.Id);
            var newUrl = _mapper.Map<TenantUrl>(model);
            newUrl.ClientId = model.ClientId == 0 ? null : model.ClientId;

            if (tenantUrl == null)
            {
                var hostnameExists = await _context.TenantUrls.SingleOrDefaultAsync(u => u.Host == model.Host);
                if (hostnameExists != null) return Result.Fail<DomainModel>(new Error.Invalid("Domain name already exists"));

                // create new
                _context.TenantUrls.Add(newUrl);
            }
            else
            {
                // update
                _context.Entry(tenantUrl).CurrentValues.SetValues(newUrl);
            }

            await _context.SaveChangesAsync();

            return Result.Ok(_mapper.Map<DomainModel>(newUrl));
        }


        public async Task<List<SelectListItem>> GetDomainDropdownListMRF()
        {
            var domains = await GetDomainsAsync();
            var domainList = new List<SelectListItem>();

            foreach (var domain in domains)
            {
                domainList.Add(new SelectListItem() { Text = domain.Host, Value = domain.Id.ToString() });
                break;
            }

            return domainList;
        }

    }
}
