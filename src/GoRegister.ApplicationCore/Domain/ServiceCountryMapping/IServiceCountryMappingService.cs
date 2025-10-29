using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GoRegister.ApplicationCore.Data.Models.Fields;
using GoRegister.ApplicationCore.Domain.Registration.Fields;
using Newtonsoft.Json;
using GoRegister.ApplicationCore.Domain.Countries;
using Microsoft.AspNetCore.Mvc.Rendering;
using EFCore.BulkExtensions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace GoRegister.ApplicationCore.Domain.ServiceCountryMapping
{
    public interface IServiceCountryMappingService
    {
        Task<List<MRFServiceCountryMapping>> GetList(int projectId, string clientuuid);

        Task<int> Create(List<MRFServiceCountryMapping> dtos);
        Task<List<SelectListItem>> LoadRequestCountry();
        Task<IEnumerable<FieldOptionDisplayModel>> LoadServiceCountry(int projectid, string clientid);
        Task<List<MRFServiceCountryMapping>> GetMappingbyServicecountry(int projectId, string clientuuid, string servicecounrty);
        Task Delete(int projectId, string clientuuid, string servicecounrty);
        Task<string> GetCountryISO(string country);
    }
    public class ServiceCountryMappingService : IServiceCountryMappingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFormService _formService;
        private readonly IConfiguration _configuration;
        private readonly ICountryCacheService _countryCacheService;
        public ServiceCountryMappingService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, ICountryCacheService countryCacheService, IFormService formService, IConfiguration configuration )
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _formService = formService;
            _configuration = configuration;
            _countryCacheService = countryCacheService;
        }
        public async Task<int> Create(List<MRFServiceCountryMapping> dtos)
        {
            try
            {
                int result = 0;
                foreach (MRFServiceCountryMapping model in dtos)
                {
                    _context.MRFServiceCountryMapping.Add(model);
                }
                result = await _context.SaveChangesAsync();                
                return result;
            }
            catch (Exception ex) {
                var message = ex.Message;   
                return 0;
            }        
            
        }

        public async Task<List<SelectListItem>> LoadRequestCountry()
        {           
            var scm=await _countryCacheService.GetCountryDropdownListMRF();
            foreach (var item in scm)
            {
                item.Text=item.Text.Replace(",", "");
                item.Value=item.Value.Replace(",", "");
            }
            return scm;
        }
        public async Task<string>GetCountryISO(string country)
        {
            var countries = await _countryCacheService.GetCountries();
            foreach (var item in countries)
            {
                item.Name=item.Name.Replace(",", "");
            }
            var iso = countries.FirstOrDefault(i => i.Name == country).ISO.ToString();
            return iso;
        }
        public async Task <IEnumerable<FieldOptionDisplayModel>> LoadServiceCountry(int projectId,string clientid)
        {
            var mrfSubsidiariesDetailsAPI = this._configuration.GetSection("APIDetails")["MRFSubsidiariesDetailsAPI"];
            var accessToken = this._configuration.GetSection("APIDetails")["AccessToken"];
            var mrfMeetingTypeAPI = this._configuration.GetSection("APIDetails")["MRFMeetingTypeAPI"];

            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(mrfSubsidiariesDetailsAPI + "/" + project.Prefix);
            client.DefaultRequestHeaders.Add("Authorization", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Select* from TPNCountryClientEmail where clientid = 113
            //select* from Field where projectid = 88 and AllowTPNCountries = 1

            var TPNcountries = await _context.Countries.FromSqlRaw(@"  select c.ISO ,c.Name, c.ISO3,c.Numeric from Country c 
                                                                   inner join TPNCountryClientEmail t on c.[Name]= t.TPNCountry
                                                                   inner join Project p on t.ClientId=p.ClientId  
                                                                  inner join Field f on f.projectid=p.id
                                                                  where p.id={0} and f.DataTag='ServicingCountry' and f.AllowTPNCountries=1
                                                                  and t.ClientUuid={1}", projectId, clientid).Distinct().ToListAsync();
            using (var response = client.GetAsync(client.BaseAddress))
            {
                IEnumerable<FieldOptionDisplayModel> options = null ;
                if (response.Result.IsSuccessStatusCode)
                {
                    var fileJsonString = response.Result.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<MRFSingleSelectFieldOptions>>(fileJsonString.Result);
                    options = data.Select(e => new FieldOptionDisplayModel(e.UUId, e.Name));
                    if (TPNcountries != null && TPNcountries.Count > 0)
                    {
                        foreach (var country in TPNcountries)
                        {
                            options = options.Append(new FieldOptionDisplayModel(country.ISO, country.Name));
                        }
                    }
                    //return options;
                }else
                if (TPNcountries != null && TPNcountries.Count > 0)
                {
                   options = TPNcountries.Select(e=> new FieldOptionDisplayModel(e.ISO, e.Name));
                }
                return options;
            }
        }

        public async Task<List<MRFServiceCountryMapping>> GetList(int projectId, string clientuuid)
        {
            var dtos=await _context.MRFServiceCountryMapping.Where(i=>i.ClientUuid==clientuuid &&  i.ProjectId==projectId && i.IsActive==true).OrderByDescending(e=>e.MappingCountryId).ToListAsync();
            return dtos;
        }
        public async Task<List<MRFServiceCountryMapping>> GetMappingbyServicecountry(int projectId, string clientuuid,string servicecountry)
        {
            var dtos = await _context.MRFServiceCountryMapping.Where(i => i.ClientUuid == clientuuid && i.ProjectId == projectId && i.ServiceCountryUuid== servicecountry && i.IsActive == true).ToListAsync();
            return dtos;
        }
        public async Task Delete(int projectId, string clientuuid, string servicecountry)
        {           
            try
            {
                
                var dtos= await _context.MRFServiceCountryMapping.Where(i => i.ClientUuid == clientuuid && i.ProjectId == projectId && i.ServiceCountryUuid == servicecountry && i.IsActive == true).ToListAsync();
                
                foreach(var dto in dtos)
                {
                    dto.IsActive = false;
                    _context.Update(dto);
                }                
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}
