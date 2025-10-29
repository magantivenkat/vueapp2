/* MRF Changes: Add new class to save MRD User Response
Modified Date: 18th October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New 

 MRF Changes: Add new property to save MRF User Response with ID
Modified Date: 31st October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 228 
 
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MRFClientResponseDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserGUID { get; set; }
        public string ClientGUID { get; set; }
        public int ProjectId { get; set; }
        public int FormId { get; set; }
        public string ClientUserResponse { get; set; }
        public string ClientUserResponseWithID { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public int? APIErrorCode { get; set; }
        public bool? AllowTPNCountries { get; set; }
        public DateTime? SendTPNEmailDateTime { get; set; }
        public bool? CopyToReport { get; set; }
    }
}
