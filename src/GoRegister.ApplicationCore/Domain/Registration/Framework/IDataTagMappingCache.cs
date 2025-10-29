using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public interface IDataTagMappingCache
    {
        Dictionary<string, DataTagMapping> Get();
    }

    public class DataTagMappingCache : IDataTagMappingCache
    {
        private readonly ApplicationDbContext _context;

        public DataTagMappingCache(ApplicationDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, DataTagMapping> Get()
        {
            //TODO: cache these values
            var dic = new Dictionary<string, DataTagMapping>();
            var fields = _context.Fields.Where(e => !string.IsNullOrWhiteSpace(e.DataTag)).ToList();

            foreach (var field in fields)
            {
                dic[field.DataTag.ToUpperInvariant()] = new DataTagMapping(field);
            }
            
            return dic;
        }
    }

    public class DataTagMapping
    {
        public DataTagMapping() { }

        public DataTagMapping(Field field)
        {
            FieldId = field.Id;
            FieldType = field.FieldTypeId;
        }

        public int FieldId { get; set; }
        public FieldTypeEnum FieldType { get; set; }
    }
}
