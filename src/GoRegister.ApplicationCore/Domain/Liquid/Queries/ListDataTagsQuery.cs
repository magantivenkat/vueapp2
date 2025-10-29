using AutoMapper;
using GoRegister.ApplicationCore.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Liquid.Queries
{
    public static class ListDataTagsQuery
    {
        public class Query : IRequest<List<DataTagCategory>>
        {
        }     

        public class DataTagCategory
        {
            public string Name { get; set; }
            public List<BaseDataTag> Tags { get; set; } = new List<BaseDataTag>();
        }

        public abstract class BaseDataTag
        {
            public BaseDataTag(string name, string key)
            {
                Name = name;
                Key = key;
            }
            public string Name { get; set; }
            public string Key { get; set; }
            public abstract string DataTag { get; }
        }

        public class ObjectDataTag : BaseDataTag
        {
            public ObjectDataTag(string name, string key) : base(name, key)
            {
            }

            public override string DataTag => $"{{{{ {Key} }}}}";
        }

        public class TagDataTag : BaseDataTag
        {
            public TagDataTag(string name, string key) : base(name, key)
            {
            }

            public override string DataTag => $"{{% {Key} %}}";
        }

        public class QueryHandler : IRequestHandler<Query, List<DataTagCategory>>
        {
            private readonly ApplicationDbContext _db;           

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;                
            }

            public async Task<List<DataTagCategory>> Handle(Query request, CancellationToken cancellationToken)
            {
                // query the db for all fields in the forms reg, cancel and decline
                // add all the fields that have a non null or whitespace datatag
                var fieldsRegistration = await _db.Fields.Where(f => f.RegistrationPage.Form.FormTypeId == Data.Enums.FormType.Registration && !string.IsNullOrWhiteSpace(f.DataTag)).ToListAsync();
                //List<DataTagListModel> model = new List<DataTagListModel>();
                var categories = new List<DataTagCategory>();

                var regCategory = new DataTagCategory { Name = "Registration" };
                //Fetch registration data tags
                foreach (var f in fieldsRegistration.OrderBy(e => e.SortOrder))
                {
                    var dataTag = $"user.{f.DataTag}";

                    regCategory.Tags.Add(new ObjectDataTag(f.Name, dataTag));
                }

                categories.Add(regCategory);

                //Fetch sessions data tags
                var sessionsCategory = new DataTagCategory { Name = "Sessions" };

                var fieldsSessions = await _db.SessionCategories.ToListAsync();

                foreach (var f in fieldsSessions)
                {
                    var dataTag = $"sessions.{f.Name}";

                    sessionsCategory.Tags.Add(new ObjectDataTag(f.Name, dataTag));
                }

                categories.Add(sessionsCategory);

                //Fetch project data tags
                var projectTags = new List<BaseDataTag>()
                {
                    new ObjectDataTag("Name", "project.name"),
                    new ObjectDataTag("Start Date", "project.startdate"),
                    new ObjectDataTag("End Date", "project.enddate"),
                    new ObjectDataTag("Email Reply To", "project.emailreplyto"),
                    new ObjectDataTag("Telephone Number", "project.tel"),
                    new ObjectDataTag("Timezone", "project.timezone"),
                };

                categories.Add(new DataTagCategory
                {
                    Name = "Project",
                    Tags = projectTags
                });

                return categories;
            }
        }
    }
}