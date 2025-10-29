using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MRFClientRequest
    {
        [Key]
        public int ClientuniqueID
        {
            get;
            set;
        }
        public string ClientUuid
        {
            get;
            set;
        }
        public string ClientID
        {
            get;
            set;
        }
        public string ClientName
        {
            get;
            set;
        }
        public string MRFClientStatus
        {
            get;
            set;
        }

        public DateTime? deletedAt
        {
            get;
            set;
        }
        public List<MRFClientRequestCountry> MRFClientRequestCountry { get; set; }
    }
}
