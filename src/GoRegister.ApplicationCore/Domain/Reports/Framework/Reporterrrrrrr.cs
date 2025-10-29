using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Reports.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Reports.Framework
{
    public class ReportFilterContainer
    {
        public string Key => ViewModel.Key;

        public IReportFilterModel BlankModel { get; set; }
        public IReportFilterViewModel ViewModel { get; set; }
    }

    public class ReportRegistrationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ReportField
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ReportSelectField
    {
        public ReportSelectField(string key, Action<Query> action)
        {
            Key = key;
            SelectAction = action;
        }

        public ReportSelectField(string key, string statement)
        {
            Key = key;
            SelectStatement = statement;
        }

        public string Key { get; set; }
        public string Name { get; set; }
        public string Group { get; set; } = "Other";
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public string SelectStatement { get; set; }
        public Action<Query> SelectAction { get; set; }
        public List<string> JoinsOLD { get; } = new List<string>();
        public List<IJoin> Joins { get; } = new List<IJoin>();

        public ReportSelectField Requires(params string[] joins)
        {
            JoinsOLD.AddRange(joins);
            return this;
        }
        
        public ReportSelectField Requires(IJoin join)
        {
            Joins.Add(join);
            return this;
        }

        public void Execute(Query query)
        {
            if (SelectAction != null)
            {
                SelectAction(query);
            }
            else if (!string.IsNullOrWhiteSpace(SelectStatement))
            {
                query.Select(SelectStatement);
            }
        }
    }

    public interface IReportFilter
    {
        string Key { get; }
        string Type { get; }
        List<string> JoinsOld { get; }
        List<IJoin> Joins { get; }

        IReportFilterModel GetModel();
        IReportFilterViewModel GetViewModel();
        Type GetFilterType();
        void Apply(Query query, IReportFilterModel model);
    }

    public interface IReportFilterModel
    {
        string Key { get; set; }
        string Type { get; set; }
        string Name { get; set; }
    }

    public interface IReportFilterViewModel
    {
        string Key { get; set; }
        string Type { get; set; }
        string Name { get; set; }
    }

    public class ReportFilterModel : IReportFilterModel
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class ReportFilterViewModel : IReportFilterViewModel
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public abstract class ReportFilter<TModel, TViewModel> : IReportFilter
        where TModel : IReportFilterModel, new()
        where TViewModel : IReportFilterViewModel, new()
    {
        public ReportFilter(string key, string name)
        {
            Key = key;
            Name = name;
        }

        public string Key { get; private set; }
        public string Name { get; }

        public abstract string Type { get; }
        public List<string> JoinsOld { get; } = new List<string>();
        public List<IJoin> Joins { get; } = new List<IJoin>();

        public abstract IReportFilterModel GetModel();
        public abstract IReportFilterViewModel GetViewModel();

        public Type GetFilterType()
        {
            return typeof(TModel);
        }

        public void Apply(Query query, IReportFilterModel model)
        {
            Apply(query, (TModel)model);
        }

        public void Requires(params string[] joins)
        {
            JoinsOld.AddRange(joins);
        }

        public void Requires(IJoin join)
        {
            Joins.Add(join);
        }

        public abstract void Apply(Query query, TModel model);
    }

    public abstract class AliasReportFilter<TModel, TViewModel> : ReportFilter<TModel, TViewModel>
        where TModel : IReportFilterModel, new()
        where TViewModel : IReportFilterViewModel, new()
    {
        public AliasReportFilter(string key, string name, string alias) : base(key, name)
        {
            Alias = alias;
        }
        protected string Alias { get; }
    }

    public class StringReportFilter : AliasReportFilter<StringReportFilter.StringReportFilterModel, StringReportFilter.ViewModel>
    {
        public StringReportFilter(string key, string name, string alias) : base(key, name, alias)
        {
        }

        public override string Type => "String";

        public override void Apply(Query query, StringReportFilterModel model)
        {
            query.Where(Alias, model.Value);
        }

        public override IReportFilterModel GetModel()
        {
            var model = new StringReportFilterModel();
            model.Key = this.Key;
            model.Name = Name;
            model.Type = Type;
            return model;
        }

        public override IReportFilterViewModel GetViewModel()
        {
            var model = new ViewModel();
            model.Key = this.Key;
            model.Name = Name;
            model.Type = Type;
            return model;
        }

        public class StringReportFilterModel : ReportFilterModel
        {
            public string Operation { get; set; }
            public string Value { get; set; }

            public List<string> Operations = new List<string>
            {

            };
        }

        public class ViewModel : ReportFilterViewModel
        {

        }
    }

    public class DateReportFilter : AliasReportFilter<DateReportFilter.DateReportFilterModel, DateReportFilter.ViewModel>
    {
        public DateReportFilter(string key, string name, string alias) : base(key, name, alias)
        {
        }

        public override string Type => "Date";

        public override void Apply(Query query, DateReportFilterModel model)
        {
            query.WhereDate(Alias, model.Operation, model.Value);
        }

        public override IReportFilterModel GetModel()
        {
            var model = new DateReportFilterModel();
            model.Key = Key;
            model.Type = Type;
            model.Name = Name;
            return model;
        }

        public override IReportFilterViewModel GetViewModel()
        {
            var model = new ViewModel();
            model.Key = this.Key;
            model.Name = Name;
            model.Type = Type;
            return model;
        }

        public class DateReportFilterModel : ReportFilterModel
        {
            public string Operation { get; set; } = "=";
            public DateTime Value { get; set; }
        }

        public class ViewModel : ReportFilterViewModel
        {

        }
    }

    public class ForeignKeyReportFilter : AliasReportFilter<ForeignKeyReportFilter.ForeignKeyReportFilterModel, ForeignKeyReportFilter.ViewModel>
    {
        private IEnumerable<NameValueModel> options;

        public ForeignKeyReportFilter(string key, string name, string alias) : base(key, name, alias)
        {
        }

        public override string Type => "ForeignKey";

        public override void Apply(Query query, ForeignKeyReportFilterModel model)
        {
            if (model.Values.Any())
            {
                if (model.Values.Count() == 1)
                {
                    query.Where(Alias, model.Values.FirstOrDefault());
                }
                else
                {
                    query.WhereIn(Alias, model.Values);
                }
            }
        }

        public override IReportFilterModel GetModel()
        {
            var model = new ForeignKeyReportFilterModel();
            model.Key = Key;
            model.Type = Type;
            model.Name = Name;
            return model;
        }

        public override IReportFilterViewModel GetViewModel()
        {
            var model = new ViewModel();
            if (options != null) model.Options = options;
            model.Key = Key;
            model.Type = Type;
            model.Name = Name;
            return model;
        }

        public void SetOptions(IEnumerable<NameValueModel> options)
        {
            this.options = options;
        }

        public class ForeignKeyReportFilterModel : ReportFilterModel
        {
            public int[] Values { get; set; } = Array.Empty<int>();
        }

        public class ViewModel : ReportFilterViewModel
        {
            public IEnumerable<NameValueModel> Options { get; set; }
        }
    }

    public class UserReportProvider : IReportProvider
    {
        private readonly ApplicationDbContext _context;

        public UserReportProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Build(ReportContext context)
        {
            // joins

            // filters
            context.Filter(new DateReportFilter("ConfirmationDate", "Confirmation Date", "DelegateUser.ConfirmedUtc"));
            context.Filter(new DateReportFilter("InvitedDate", "Invited Date", "DelegateUser.InvitedUtc"));
            context.Filter(new DateReportFilter("DeclinedDate", "Declined Date", "DelegateUser.DeclinedUtc"));
            context.Filter(new DateReportFilter("CancelledDate", "Cancelled Date", "DelegateUser.CancelledUtc"));
            context.Filter(new DateReportFilter("ModifiedDate", "Modified Date", "DelegateUser.ModifiedUtc"));

            var regTypes = _context.RegistrationTypes.ToList();
            var regTypeFilter = new ForeignKeyReportFilter("RegistrationType", "Registration Type", "DelegateUser.RegistrationTypeId");
            regTypeFilter.SetOptions(regTypes.Select(e => new NameValueModel(e.Id, e.Name)));
            context.Filter(regTypeFilter);

            var regStatusFilter = new ForeignKeyReportFilter("RegistrationStatus", "Registration Status", "DelegateUser.RegistrationStatusId");
            regStatusFilter.SetOptions(_context.RegistrationStatuses.Select(e => new NameValueModel(e.Id, e.Description)).ToList());
            context.Filter(regStatusFilter);

            // selects
            context.Select("Id", "DelegateUser.Id");
            context.Select("ConfirmationDate", "Confirmation Date", "DelegateUser.ConfirmedUtc");
            context.Select("InvitedDate", "Invited Date", "DelegateUser.InvitedUtc");
            context.Select("CancelledDate", "Cancelled Date", "DelegateUser.CancelledUtc");
            context.Select("DeclinedDate", "Declined Date", "DelegateUser.DeclinedUtc");
            context.Select("ModifiedDate", "Modified Date", "DelegateUser.ModifiedUtc");
            context.Select("RegistrationStatus", "Registration Status", "RegistrationStatus.Description");
            context.Select("RegistrationType", "Registration Type", "RegistrationType.Name");
            context.Select("IsGuest", "Is Guest", "case when DelegateUser.ParentDelegateUserId is null then 0 else 1 end");

            // group bys
            context.AddGroupBy(new GroupByViewModel(new RegistrationStatusGroupBy())
            {
                DisplayName = "Registration Status"
            });
        }
    }

    public class FieldReportProvider : IReportProvider
    {
        private readonly ApplicationDbContext _context;

        public FieldReportProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Build(ReportContext context)
        {
            var fields = _context.Fields
                .Include("FieldOptions")
                .Include(e => e.RegistrationPage)
                .ThenInclude(p => p.Form)
                .ToList();
            foreach (var field in fields)
            {
                var fieldJoin = new FieldJoin(field.Id, field.RegistrationPage.FormId);

                switch (field.FieldTypeId)
                {
                    case FieldTypeEnum.FirstName:
                        context.Select("FirstName", field.Name, "User.FirstName");
                        context.Filter(new StringReportFilter("FirstName", field.Name, "User.FirstName"));
                        break;
                    case FieldTypeEnum.LastName:
                        context.Select("LastName", field.Name, "User.LastName");
                        context.Filter(new StringReportFilter("LastName", field.Name, "User.LastName"));
                        break;
                    case FieldTypeEnum.Email:
                        context.Select("Email", field.Name, "User.Email");
                        break;
                    case FieldTypeEnum.Textbox:
                        var textFilter = new StringReportFilter($"{field.Id}", field.Name, $"{fieldJoin.Alias}.StringValue");
                        textFilter.Requires(fieldJoin);
                        context.Filter(textFilter);

                        context.Select($"{fieldJoin}", field.Name, $"{fieldJoin}.StringValue").Requires(fieldJoin);
                        break;
                    case FieldTypeEnum.RadioButton:
                        var fieldOptionJoin = new FieldOptionJoin(field.Id, field.RegistrationPage.FormId);
                        var radioButtonFilter = new ForeignKeyReportFilter($"field{field.Id}", field.Name, $"{fieldOptionJoin}.FieldOptionId");
                        radioButtonFilter.SetOptions(field.FieldOptions.Select(e => new NameValueModel(e.Id, e.Description)));
                        radioButtonFilter.Requires(fieldOptionJoin);
                        context.Filter(radioButtonFilter);

                        context.Select($"{fieldOptionJoin}", field.Name, $"{fieldOptionJoin}.Description").Requires(fieldOptionJoin);

                        context.AddGroupBy(new GroupByViewModel(new SingleSelectGroupBy()
                        {
                            FieldId = field.Id,
                            FormId = field.RegistrationPage.FormId,
                            DisplayName = field.Name
                        })
                        {
                            DisplayName = field.Name,
                        });

                        break;
                    case FieldTypeEnum.Date:
                        context.Select($"{fieldJoin}", field.Name, $"{fieldJoin}.DateTimeValue").Requires(fieldJoin);

                        var dateFilter = new DateReportFilter($"{field.Id}", field.Name, $"{fieldJoin}.FieldOptionId");
                        dateFilter.Requires(fieldJoin);
                        context.Filter(dateFilter);

                        break;
                }

            }
        }
    }

    public class FieldOptionJoin : Join
    {
        public readonly IJoin FieldJoin;

        public FieldOptionJoin(int fieldId, int formId)
        {
            FieldJoin = new FieldJoin(fieldId, formId);

            _joins.Add(FieldJoin);
        }

        public override string Alias => $"{FieldJoin}option";

        public override Query Apply(Query query)
        {
            return query.LeftJoin($"FieldOption as {Alias}", $"{Alias}.Id", $"{FieldJoin.Alias}.FieldOptionId");
        }
    }

    public interface IReportProvider
    {
        void Build(ReportContext context);
    }

    public class ReportContext
    {
        public List<IReportFilter> Filters { get; } = new List<IReportFilter>();
        public Dictionary<string, Action<Query>> Joins { get; } = new Dictionary<string, Action<Query>>();
        public List<ReportSelectField> Selects { get; } = new List<ReportSelectField>();
        public List<IGroupBy> GroupByOLD { get; } = new List<IGroupBy>();
        public List<GroupByViewModel> GroupBys { get; } = new List<GroupByViewModel>();

        public void AddGroupBy(IGroupBy groupBy)
        {
            GroupByOLD.Add(groupBy);
        }

        public void AddGroupBy(GroupByViewModel groupBy)
        {
            GroupBys.Add(groupBy);
        }

        public List<ReportSelectField> GetSelects(IEnumerable<string> fieldsToSelect)
        {
            var list = new List<ReportSelectField>();
            foreach (var field in fieldsToSelect)
            {
                list.Add(Selects.FirstOrDefault(e => e.Key == field));
            }
            return list;
        }

        public void Filter(IReportFilter filter)
        {
            Filters.Add(filter);
        }

        public void Join(string alias, Action<Query> query)
        {
            Joins.TryAdd(alias, query);
        }

        public ReportSelectField Select(string key, Action<Query> query)
        {
            var select = new ReportSelectField(key, query);
            Selects.Add(select);
            return select;
        }

        public ReportSelectField Select(string key, string statement)
        {
            return Select(key, key, statement);
        }

        public ReportSelectField Select(string key, string name, string statement)
        {
            return Select(key, name, "Other", statement);
        }

        public ReportSelectField Select(string key, string name, string group, string statement)
        {
            var select = new ReportSelectField(key, statement);
            select.Name = name;
            select.Group = group;
            Selects.Add(select);
            return select;
        }

        public List<string> GetHeaders(ReportViewModel model)
        {
            var headers = new List<string>();
            if (model.Type == ReportViewModel.ReportType.Delegates)
            {
                headers = GetSelects(model.SelectedFields).Select(e => e.Name).ToList();
            }

            if (model.Type == ReportViewModel.ReportType.Summary)
            {
                //var first = GroupByOLD.FirstOrDefault(e => e.Key == model.GroupByStartFrom);
                //headers.Add(first.DisplayName);


                //headers.Add("Total");
            }

            return headers;
        }
    }

    public interface IJoin
    {
        string Alias { get; }
        Query Apply(Query query);
        void Requires(params IJoin[] joins);
        IEnumerable<IJoin> GetJoins();
    }

    public abstract class Join : IJoin
    {
        protected List<IJoin> _joins = new List<IJoin>();

        public abstract string Alias { get; }

        public abstract Query Apply(Query query);

        public IEnumerable<IJoin> GetJoins()
        {
            return _joins.SelectMany(e => e.GetJoins()).Append(this);
        }

        void IJoin.Requires(params IJoin[] joins)
        {
            _joins.AddRange(joins);
        }

        public override string ToString()
        {
            return Alias;
        }
    }

    public class FormJoin : Join
    {
        private readonly int _formId;

        public FormJoin(int formId)
        {
            _formId = formId;
        }

        public override string Alias => $"formResponse{_formId}";

        public override Query Apply(Query query)
        {
            return query.LeftJoin($"UserFormResponse as {Alias}", q =>
                    q.On($"{Alias}.UserId", "User.Id").Where($"{Alias}.FormId", _formId));
        }
    }

    public class FieldJoin : Join
    {
        private readonly int _fieldId;
        private readonly int _formId;
        public readonly IJoin FormJoin;

        public FieldJoin(int fieldId, int formId)
        {
            _fieldId = fieldId;
            _formId = formId;
            FormJoin = new FormJoin(formId);

            _joins.Add(FormJoin);
        }

        public override string Alias => $"form{_formId}field{_fieldId}";

        public override Query Apply(Query query)
        {
            return query.LeftJoin($"UserFieldResponse as {Alias}", q =>
                    q.On($"{Alias}.UserFormResponseId", $"{FormJoin.Alias}.Id").Where($"{Alias}.FieldId", _fieldId));
        }
    }

    public interface IGroupBy
    {
        string Key { get; set; }
        string Type { get; }
        string DisplayName { get; set; }
        string GroupByTable { get; }
        (string, string) Join { get; }
        HashSet<string> Selects { get; set; }
        string ModelType { get; set; }

        void ApplyGroupBy(QueryContext context);
        void ApplyResultsQuery(Query query);
        void ApplyRowExpandSelect(Query query);
        List<GroupBySelect> GetSelects();
        string GetKey();
    }

    public interface IGroupByViewModel
    {
        string Type { get; }
        string DisplayName { get; }
        IGroupBy GetModel();

    }

    public class GroupByViewModel
    {
        public GroupByViewModel(IGroupBy model)
        {
            Model = model;
            model.Key = Model.GetKey();
            model.ModelType = model.Type;
            if(model.Selects == null)
            {
                // this hack is really for vuejs, so let's move this code into vuejs unless we can find a cleaner way
                // required because vuejs will map checkboxes to a boolean unless it detects an array
                model.Selects = new HashSet<string>();
            }
        }

        public string Type => Model.Type;
        public string Key => Model.GetKey();
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public IGroupBy Model { get; }
        public IEnumerable<GroupBySelectViewModel> Selects => Model.GetSelects().Where(e => !e.IsHidden).Select(e => new GroupBySelectViewModel
        {
            DisplayName = e.DisplayName,
            Key = e.Key
        });
    }

    public class RegistrationStatusGroupBy : IGroupBy
    {
        public string ModelType { get; set; } = "RegistrationStatus";
        public string Type => "RegistrationStatus";
        public string Key { get; set; }
        public string DisplayName { get; set; } = "Registration Status";

        public string GroupByTable => "RegistrationStatus";

        public (string, string) Join => ("RegistrationStatusId", "RegistrationStatus.Id");

        public HashSet<string> Selects { get; set; } = new HashSet<string>() { "Name" };

        public void ApplyGroupBy(QueryContext context)
        {
            context.Query.Select("RegistrationStatus.Id as RegistrationStatusId");

            context.Query.GroupBy("RegistrationStatus.Id");
        }

        public void ApplyResultsQuery(Query query)
        {
        }

        public void ApplyRowExpandSelect(Query query)
        {
            query.Select("RegistrationStatus.Id as RegistrationStatusId");
        }

        public string GetKey() => "RegistrationStatus";

        public List<GroupBySelect> GetSelects()
        {
            return new List<GroupBySelect>
            {
                new GroupBySelect
                {
                    Alias = $"RegistrationStatus_Id",
                    Key = "Id",
                    QueryValue = $"RegistrationStatus.Id",
                    IsHidden = true
                },
                new GroupBySelect
                {
                    Alias = $"RegistrationStatus_Name",
                    Key = "Name",
                    DisplayName = "Status",
                    HeaderName = "",
                    QueryValue = $"RegistrationStatus.Description"
                }
            };
        }
    }

    public class SingleSelectGroupBy : IGroupBy
    {
        public string ModelType { get; set; }
        public string Type => "SingleSelectField";
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public int FieldId { get; set; }
        public int FormId { get; set; }
        public HashSet<string> Selects { get; set; } = new HashSet<string> { "Name" };

        public string GroupByTable => $"FieldOption as {groupByJoinAlias}";

        public (string, string) Join => (subqueryJoinAlias, $"{groupByJoinAlias}.Id");

        private string groupByJoinAlias => $"groupByField{FieldId}";
        private FieldJoin _fieldResponseJoin => new FieldJoin(FieldId, FormId);
        private string subqueryJoinAlias => $"userData{FormId}_{FieldId}OptionId";

        public string GetKey() => $"SingleSelectField_{FormId}_{FieldId}";

        public void ApplyGroupBy(QueryContext context)
        {
            context.Joins.Add(_fieldResponseJoin);
            context.Query.Select($"{_fieldResponseJoin.Alias}.FieldOptionId as {subqueryJoinAlias}");

            context.Query.GroupBy($"{_fieldResponseJoin.Alias}.FieldOptionId");
        }

        public void ApplyResultsQuery(Query query)
        {
            query.Where($"{groupByJoinAlias}.FieldId", FieldId);
        }

        public void ApplyRowExpandSelect(Query query)
        {
            query.Select($"{groupByJoinAlias}.Id as SingleSelectField_{FormId}_{FieldId}_Id");
        }

        public List<GroupBySelect> GetSelects()
        {
            return new List<GroupBySelect>
            {
                new GroupBySelect
                {
                    Alias = $"SingleSelectField_{FormId}_{FieldId}_Id",
                    Key = "Id",
                    DisplayName = "",
                    HeaderName = "",
                    QueryValue = $"{groupByJoinAlias}.Id",
                    IsHidden = true
                },
                new GroupBySelect
                {
                    Alias = $"SingleSelectField_{FormId}_{FieldId}_Name",
                    Key = "Name",
                    DisplayName = "Name",
                    HeaderName = "",
                    QueryValue = $"{groupByJoinAlias}.Description"
                },
                new GroupBySelect
                {
                    Alias = $"SingleSelectField_{FormId}_{FieldId}_AdditionalInformation",
                    Key = "AdditionalInformation",
                    DisplayName = "Additional Information",
                    HeaderName = "Additional Information",
                    QueryValue = $"{groupByJoinAlias}.AdditionalInformation"
                },
                new GroupBySelect
                {
                    Alias = $"SingleSelectField_{FormId}_{FieldId}_InternalInformation",
                    Key = "InternalInformation",
                    DisplayName = "Internal Information",
                    HeaderName = "Internal Information",
                    QueryValue = $"{groupByJoinAlias}.InternalInformation"
                }
            };
        }
    }

    public class GroupBySelect
    {
        public string Key { get; set; }
        public decimal Order { get; set; }
        public string DisplayName { get; set; }
        public string HeaderName { get; set; }
        public string Alias { get; set; }
        public string QueryValue { get; set; }
        public bool IsHidden { get; set; }
    }

    public class GroupBySelectViewModel
    {
        public string DisplayName { get; set; }
        public string Key { get; set; }
    }

    public class QueryContext
    {
        public QueryContext(Query query, int projectId)
        {
            Query = query;
            ProjectId = projectId;
        }

        public List<IJoin> Joins { get; } = new List<IJoin>();

        public Query Query { get; }
        public int ProjectId { get; }
    }
}
