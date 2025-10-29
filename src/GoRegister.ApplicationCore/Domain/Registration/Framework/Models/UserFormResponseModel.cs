using GoRegister.ApplicationCore.Data.Models;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Models
{
    public class UserFormResponseModel
    {
        public UserFormResponseModel(UserFormResponse response)
        {
            Response = response;
        }

        public UserFormResponse Response { get; set; }

        public HashSet<int> GetRegistrationTypeIds()
        {
            var ids = new HashSet<int>()
            {
                Response.DelegateUser.RegistrationTypeId
            };

            return ids;
        }

        public List<DelegateUser> GetAllUsers()
        {
            return new List<DelegateUser> { Response.DelegateUser };
        } 
    }
}
