using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GoRegister.ApplicationCore.Domain.Countries.CountryCacheService;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoRegister.ApplicationCore.Domain.Countries
{
    public interface ICountryCacheService
    {
        Task<List<CountryModel>> GetCountries();
        Task<CountryModel> GetCountry(string iso);

        Task<CountryModelMRF> GetCountryMRF(string Id);
        Task<List<SelectListItem>> GetCountryDropdownListMRF();
    }

    public class CountryCacheService : ICountryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _db;
        public static string CountryCacheKey = "Countries";

        public CountryCacheService(IMemoryCache memoryCache, ApplicationDbContext db)
        {
            _memoryCache = memoryCache;
            _db = db;
        }

        public async Task<List<CountryModel>> GetCountries()
        {
            var cacheEntry = await _memoryCache.GetOrCreateAsync(CountryCacheKey, async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromHours(12));
                return await _db.Countries.Select(e => new CountryModel
                {
                    ISO = e.ISO,
                    ISO3 = e.ISO3,
                    Name = e.Name,
                    Numeric = e.Numeric
                }).ToListAsync();
            });

            return cacheEntry;
        }

        public class CountryModelMRF
        {
            public string ClientUuid { get; set; }
            public int ClientuniqueID { get; set; }
            public int CountryID { get; set; }            
            public string Countryguid { get; set; }
            public string CountryName { get; set; }
        }

        public async Task<CountryModel> GetCountry(string iso)
        {
            var countries = await GetCountries();
            return countries.FirstOrDefault(e => e.ISO == iso);
        }
        public async Task<CountryModelMRF> GetCountryMRF(string Id)
        {

           var mrfSubsidiaries   = await _db.MRFClientRequest
                                       .Join(_db.MRFClientRequestCountry,
                                       c => c.ClientuniqueID,
                                       m => m.ClientuniqueID,
                                       (c, m) => new CountryModelMRF
                                       {
                                           ClientUuid = c.ClientUuid,
                                           ClientuniqueID = c.ClientuniqueID,
                                           CountryID = m.CountryID,
                                           Countryguid = m.Countryguid,
                                           CountryName = m.CountryName                                         
                                       }).Where(m => m.Countryguid == Id)                                       
                                       .ToListAsync();

            return mrfSubsidiaries.FirstOrDefault(e => e.Countryguid == Id);
        }

        public async Task<List<SelectListItem>> GetCountryDropdownListMRF()
        {
            var countries = await GetCountries();
            var countryList = new List<SelectListItem>();

            foreach (var country in countries)
            {
                countryList.Add(new SelectListItem() { Text = country.Name, Value = country.Name.ToString() });
            }

            return countryList;
        }
    }
}
