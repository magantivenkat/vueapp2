using GoRegister.ApplicationCore.Data.Enums;
using System;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class UserAction
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public UserActionType ActionType { get; set; }
        public string Data { get; set; }
        public DateTime DateCreatedUtc { get; set; }
    }
}
