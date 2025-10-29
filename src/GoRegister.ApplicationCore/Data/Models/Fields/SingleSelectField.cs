namespace GoRegister.ApplicationCore.Data.Models
{
    public class SingleSelectField : Field
    {
        public SingleSelectTypeEnum SingleSelectType { get; set; }

        public enum SingleSelectTypeEnum
        {
            Radio = 0,
            Select = 1
        }
    }
}
