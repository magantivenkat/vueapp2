using GoRegister.ApplicationCore.Data.Specification;

namespace GoRegister.ApplicationCore.Domain.BulkUpload.Specs
{
    public class DuplicateEmailsSpecification : BaseSqlSpecification<string>
    {
        public DuplicateEmailsSpecification(object parameters) : base(parameters) 
        {
            Sql = @"
                    select [Email]
                    from [User]
                    join [DelegateUser] on [DelegateUser].Id = [User].Id
                    where [user].[ProjectId] = @projectId and [DelegateUser].IsTest = 0
                    and [user].email in @emails";
        }
    }
}
