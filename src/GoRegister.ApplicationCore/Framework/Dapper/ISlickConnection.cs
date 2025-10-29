namespace GoRegister.ApplicationCore.Framework.Dapper
{
    public interface ISlickConnection
    {
        Slick Get();
        Slick Get(string connectionString);
    }
}
