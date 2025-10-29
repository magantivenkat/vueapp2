using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public partial class FieldOptionRule : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public int FieldOptionId { get; set; }
        public Nullable<int> NextFieldId { get; set; }
        public Nullable<int> NextFieldOptionId { get; set; }
    
    	/// <summary>
    	/// RelationshipName: FK_FieldOptionRule_Field
    	/// </summary>
        [ForeignKey("NextFieldId")]
        public virtual Field NextField { get; set; }
    	/// <summary>
    	/// RelationshipName: FK_FieldOptionRule_FieldOption
    	/// </summary>
        public virtual FieldOption FieldOption { get; set; }
    	/// <summary>
    	/// RelationshipName: FK_FieldOptionRule_FieldOption1
    	/// </summary>
        public virtual FieldOption NextFieldOption { get; set; }
    }
}
