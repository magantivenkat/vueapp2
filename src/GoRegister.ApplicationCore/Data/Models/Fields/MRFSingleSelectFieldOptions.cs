/*MRF Changes: Add new class to get country data from meeting source
Modified Date: 31st October 2022
Modified By: Mandar.Khade @amexgbt.com
Team member: Harish.Rane @amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models.Fields
{
    [NotMapped]
    public class MRFSingleSelectFieldOptions
    {
        public string UUId { get; set; }
        public string Name { get; set; }
    }
}
