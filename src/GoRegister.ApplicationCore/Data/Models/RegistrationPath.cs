namespace GoRegister.ApplicationCore.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public class RegistrationPath : MustHaveProjectEntity
    {
        public RegistrationPath()
        {
            this.RegistrationTypes = new HashSet<RegistrationType>();
        }
    
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateRegistrationFrom { get; set; }
        public Nullable<System.DateTime> DateRegistrationTo { get; set; }
        public Nullable<System.DateTime> DateModifyTo { get; set; }
        public Nullable<System.DateTime> DateDeclineTo { get; set; }
        public Nullable<System.DateTime> DateCancelTo { get; set; }
        public bool CanDecline { get; set; }
        public bool CanCancel { get; set; }
        public bool CanModify { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int UseCreatedById { get; set; }
        public string NotInvitedText { get; set; }
        public string InvitedText { get; set; }
        public string ConfirmedText { get; set; }
        public string DeclinedText { get; set; }
        public string CancelledText { get; set; }
        public string WaitingText { get; set; }
        

    	/// <summary>
    	/// RelationshipName: FK_RegistrationType_RegistrationPath
    	/// </summary>
        public virtual ICollection<RegistrationType> RegistrationTypes { get; set; }
    }
}
