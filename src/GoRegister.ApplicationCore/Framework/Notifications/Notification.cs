using Microsoft.AspNetCore.Html;

namespace GoRegister.ApplicationCore.Framework.Notifications
{
    public class Notification {
        public NotificationType Type { get; set; }
        public HtmlString Message { get; set; }
    }

    public enum NotificationType {
        Success,
        Information,
        Warning,
        Error
    }
}
