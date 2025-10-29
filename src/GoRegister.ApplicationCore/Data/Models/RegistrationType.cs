using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class RegistrationType : MustHaveProjectEntity //, ISoftDelete
    {
        public RegistrationType()
        {
            this.RegistrationTypeFields = new HashSet<RegistrationTypeField>();
        }

        public int Id { get; set; }

        public int RegistrationPathId { get; set; }

        public DateTime DateCreated { get; set; }

        public int UserCreatedById { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Capacity { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual ICollection<DelegateUser> DelegateUsers { get; set; }

        /// <summary>
        /// RelationshipName: FK_RegistrationPageRegistrationType_RegistrationType
        /// </summary>
        public virtual ICollection<RegistrationPageRegistrationType> RegistrationPageRegistrationTypes { get; set; }

        /// <summary>
        /// RelationshipName: FK_RegistrationType_RegistrationPath
        /// </summary>
        public virtual RegistrationPath RegistrationPath { get; set; }

        /// <summary>
        /// RelationshipName: FK_RegistrationTypeField_RegistrationType
        /// </summary>
        public virtual ICollection<RegistrationTypeField> RegistrationTypeFields { get; set; }

        //public List<CustomPage> CustomPages { get; set; } = new List<CustomPage>();

        public List<CustomPageRegistrationType> CustomPageRegistrationTypes { get; set; } = new List<CustomPageRegistrationType>();
        public List<SessionRegistrationType> SessionRegistrationTypes { get; set; }


    }
}
