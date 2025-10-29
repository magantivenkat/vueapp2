using GoRegister.ApplicationCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields
{
    public interface IFieldOptionCache
    {
        FieldOptionModel Get(int id);
        Dictionary<int, FieldOptionModel> Get();
        IEnumerable<FieldOptionModel> GetForField(int fieldId);
    }

    public class FieldOptionCache : IFieldOptionCache
    {
        private readonly ApplicationDbContext _context;
        private readonly Lazy<Dictionary<int, FieldOptionModel>> _fieldOptions;

        public FieldOptionCache(ApplicationDbContext context)
        {
            _context = context;

            _fieldOptions = new Lazy<Dictionary<int, FieldOptionModel>>(FieldOptionFactory);
        }

        public Dictionary<int, FieldOptionModel> Get()
        {
            return _fieldOptions.Value;
        }


        public FieldOptionModel Get(int id)
        {
            if (!_fieldOptions.Value.ContainsKey(id)) return null;
            return _fieldOptions.Value[id];
        }

        public IEnumerable<FieldOptionModel> GetForField(int fieldId)
        {
            return _fieldOptions.Value.Where(e => e.Value.FieldId == fieldId).Select(e => e.Value);
        }


        private Dictionary<int, FieldOptionModel> FieldOptionFactory()
        {
            var items = _context.FieldOptions.Select(e => new FieldOptionModel
            {
                Id = e.Id,
                Description = e.Description,
                FieldId = e.FieldId
            }).ToList();

            return items.ToDictionary(e => e.Id, e => e);
        }
    }

    public class FieldOptionModel
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
