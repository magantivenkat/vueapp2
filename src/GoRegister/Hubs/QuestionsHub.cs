using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GoRegister.Hubs
{
    public class QuestionsHub : Hub
    {
        private readonly ITenantResolver _resolver;
        private readonly ApplicationDbContext _context;

        public QuestionsHub(ITenantResolver resolver, ApplicationDbContext context)
        {
            _resolver = resolver;
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var fakeContext = new DefaultHttpContext();
            fakeContext.Request.Host = new HostString("localhost", 5021);
            fakeContext.Request.Path = "/admin";

            var tenant = await _resolver.ResolveAsync(fakeContext);
            Context.Items.Add("tenant", tenant);
        }

        public async Task Send(string name, string message)
        {
            var tenant = (ProjectTenant)Context.Items["tenant"];
            Context.GetHttpContext()?.SetTenantContext(tenant);

            var rndDelegate = await _context.Users.FirstOrDefaultAsync();

            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }
}
