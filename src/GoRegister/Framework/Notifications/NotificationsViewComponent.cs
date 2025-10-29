using GoRegister.ApplicationCore.Framework.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace GoRegister.Framework.Notifications
{
    public class NotificationsViewComponent : ViewComponent {
        private readonly INotifier _notifier;

        public NotificationsViewComponent(INotifier notifier) {
            _notifier = notifier;
        }

        public IViewComponentResult Invoke() {
            var vm = new NotificationsViewModel() {
                Notifications = _notifier.List()
            };

            _notifier.Rendered = true;

            return View("Default", vm);
        }
    }
}