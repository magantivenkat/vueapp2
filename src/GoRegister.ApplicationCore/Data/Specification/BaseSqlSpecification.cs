namespace GoRegister.ApplicationCore.Data.Specification
{
    public class BaseSqlSpecification<T> : ISqlSpecification<T>
    {
        public BaseSqlSpecification() { }

        public BaseSqlSpecification(object parameters)
        {
            Parameters = parameters;
        }

        public string Sql { get; protected set; }
        public object Parameters { get; protected set; } = null;
    }
}
