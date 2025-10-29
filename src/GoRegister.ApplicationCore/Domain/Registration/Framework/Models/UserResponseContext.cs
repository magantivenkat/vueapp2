using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Models
{
    public class UserResponseContext
    {
        //public UserResponseContext(UserFormResponse response, Field field)
        //{
        //    Response = response;
        //    FieldId = field.Id;
        //}

        public UserResponseContext(UserFormResponse userFormResponseMRF, Field field)
        {
            UserFormResponseMRF = userFormResponseMRF;
            FieldId = field.Id;
        }

        //public UserResponseContext(UserFormResponse response, int fieldId)
        //{
        //    Response = response;
        //    FieldId = fieldId;
        //}

        public UserResponseContext(UserFormResponse userFormResponseMRF, int fieldId)
        {
            UserFormResponseMRF = userFormResponseMRF;
            FieldId = fieldId;
        }

        // public UserFormResponse Response { get; set; }
        public UserFormResponse UserFormResponseMRF { get; set; }
        public int FieldId { get; set; }
    }
}
