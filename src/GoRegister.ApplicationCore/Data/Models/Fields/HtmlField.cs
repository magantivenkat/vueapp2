namespace GoRegister.ApplicationCore.Data.Models
{
    public class HtmlField : Field
    {
        // public string Body { get; set; }

        // Todo: Coel / Harry - Not sure Body is needed 
        // Perhaps rework of the database and change "Name" to "DisplayText" to make more generic
        // Reason: Body will be NULL in many cases where the field is not of type HTML
    }
}
