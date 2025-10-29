namespace GoRegister.ApplicationCore.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public class RegistrationTypeField : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public int RegistrationTypeId { get; set; }
        public int FieldId { get; set; }
        public bool IsInternalOnly { get; set; }
        public bool IsHidden { get; set; }
    
    	/// <summary>
    	/// RelationshipName: FK_RegistrationTypeField_Field
    	/// </summary>
        public virtual Field Field { get; set; }
    	/// <summary>
    	/// RelationshipName: FK_RegistrationTypeField_RegistrationType
    	/// </summary>
        public virtual RegistrationType RegistrationType { get; set; }
    }
}
