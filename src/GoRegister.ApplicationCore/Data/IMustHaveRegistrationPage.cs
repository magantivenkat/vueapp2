using GoRegister.ApplicationCore.Data.Models;

namespace GoRegister.ApplicationCore.Data
{
    public interface IMustHaveRegistrationPage
    {
        int RegistrationPageId { get; set; }
        RegistrationPage RegistrationPage { get; set; }
    }
}
