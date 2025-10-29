using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace GoRegister.ApplicationCore.Framework.Notifications {
    public interface INotifier {
        void Add(NotificationType type, HtmlString message);
        void Add(Notification notification);
        List<Notification> List();
        string GetJson();
        bool Rendered { get; set; }
    }

    public class Notifier : INotifier {
        private readonly List<Notification> _entries;

        public Notifier() {
            _entries = new List<Notification>();
        }

        public void Add(NotificationType type, HtmlString message) {
            _entries.Add(new Notification { Type = type, Message = message });
        }

        public void Add(Notification notification) {
            _entries.Add(notification);
        }

        public List<Notification> List() {
            return _entries;
        }

        public string GetJson() {
            return JsonConvert.SerializeObject(_entries);
        }

        public bool Rendered { get; set; }
    }

    public static class NotifierExtensions {
        public static void Information(this INotifier notifier, HtmlString message) {
            notifier.Add(NotificationType.Information, message);
        }

        public static void Warning(this INotifier notifier, HtmlString message) {
            notifier.Add(NotificationType.Warning, message);
        }

        public static void Error(this INotifier notifier, string message) {
            notifier.Add(NotificationType.Error, new HtmlString(message));
        }

        public static void Success(this INotifier notifier, string message) {
            notifier.Add(NotificationType.Success, new HtmlString(message));
        }

        public static void AddJson(this INotifier notifier, string json) {
            var notifications = JsonConvert.DeserializeObject<Notification[]>(json);
            if(notifications == null || notifications.Length == 0) {
                return;
            } 

            foreach(var notification in notifications) {
                notifier.Add(notification);
            }
        }
    }
}