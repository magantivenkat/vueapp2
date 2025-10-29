/* MRF Changes: Reduce field size on User MRF Form page
Modified Date: 18th October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New */


using GoRegister.ApplicationCore.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public abstract class Field : MustHaveProjectEntity
    {
        public Field()
        {
            this.FieldOptions = new HashSet<FieldOption>();
            this.FieldOptionRules = new HashSet<FieldOptionRule>();
            this.RegistrationTypeFields = new HashSet<RegistrationTypeField>();
        }

        public int Id { get; set; }
        public int? RegistrationPageId { get; set; }
        public string Name { get; set; }
        public FieldTypeEnum FieldTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public int SortOrder { get; set; }
        public string DataTag { get; set; }
        public bool CanModify { get; set; }
        public bool IsReadOnly { get; set; }

        public string DefaultValue { get; set; }
        public string HelpTextToolTip { get; set; }
        public string HelpTextBefore { get; set; }
        public string HelpTextAfter { get; set; }

        public int Cols { get; set; } = 12;

        //public int Cols { get; set; } = 7;
        public System.Guid UniqueIdentifier { get; set; }
        public string Class { get; set; }
        public string ReportingHeader { get; set; }
        public string ValidationName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsHidden { get; set; }
        public bool? IsStandardField { get; set; }
        public bool? AllowTPNCountries { get; set; }
        public bool? EnableApproval { get; set; }


        /// <summary>
        /// RelationshipName: FK_Field_RegistrationPage
        /// </summary>
        public virtual RegistrationPage RegistrationPage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        /// <summary>
        /// RelationshipName: FK_FieldOption_Field
        /// </summary>
        public virtual ICollection<FieldOption> FieldOptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        /// <summary>
        /// RelationshipName: FK_FieldOptionRule_Field
        /// </summary>
        public virtual ICollection<FieldOptionRule> FieldOptionRules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        /// <summary>
        /// RelationshipName: FK_RegistrationTypeField_Field
        /// </summary>
        public virtual ICollection<RegistrationTypeField> RegistrationTypeFields { get; set; } = new HashSet<RegistrationTypeField>();
        public ICollection<UserFieldResponseAudit> UserFieldResponseAudits { get; set; } = new HashSet<UserFieldResponseAudit>();
        public ICollection<UserFieldResponse> UserFieldResponses { get; set; } = new HashSet<UserFieldResponse>();

        public virtual string Key => $"field-{Id}";
        public string NameForValidation => 
            string.IsNullOrWhiteSpace(ValidationName) ? Name : ValidationName;


        [NotMapped]
        public string TempId { get; private set; }
        public void SetTempId(string id)
        {
            TempId = id;
        }

        public string GetRenderId()
        {
            return Id == 0 ? TempId : Id.ToString();
        }

        public string GetPageRenderId()
        {
            if (RegistrationPageId.HasValue && RegistrationPageId.Value != 0) return RegistrationPageId.ToString();

            return RegistrationPage?.GetRenderId();
        }
    }
}
