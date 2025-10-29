using Fluid;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.CustomPages.Models;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Liquid;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.CustomPages.Queries
{
    public class RenderCustomPageQuery : IRequest<Result<CustomPageDisplayModel>>
    {
        public PageType PageType { get; set; } = PageType.CustomPage;
        public string Slug { get; set; }

        public class Handler : IRequestHandler<RenderCustomPageQuery, Result<CustomPageDisplayModel>>
        {
            private readonly ApplicationDbContext _db;
            private readonly ICurrentAttendeeAccessor _currentAttendee;
            private readonly IAttendeeAuthorizationService _attendeeAuthorizationService;
            private readonly ILiquidTemplateManager _liquidTemplateManager;

            public Handler(ApplicationDbContext db, ILiquidTemplateManager liquidTemplateManager, IAttendeeAuthorizationService attendeeAuthorizationService, ICurrentAttendeeAccessor currentAttendee)
            {
                _db = db;
                _liquidTemplateManager = liquidTemplateManager;
                _attendeeAuthorizationService = attendeeAuthorizationService;
                _currentAttendee = currentAttendee;
            }

            public async Task<Result<CustomPageDisplayModel>> Handle(RenderCustomPageQuery request, CancellationToken cancellationToken)
            {
                var customPageQuery = _db.CustomPages
                    .Where(cp => cp.PageType == request.PageType);

                if(request.PageType != PageType.HomePage)
                {
                    customPageQuery = customPageQuery
                        .Include(cp => cp.CustomPageRegistrationStatuses)
                        .Include(cp => cp.CustomPageRegistrationTypes);
                }

                if(request.PageType == PageType.CustomPage)
                {
                    customPageQuery = customPageQuery.Where(cp => cp.Slug == request.Slug);
                }

                var customPage = await customPageQuery.FirstOrDefaultAsync();
                if(customPage == null)
                {
                    return Result.NotFound<CustomPageDisplayModel>("Not found");
                }


                var attendeeWrapper = await _currentAttendee.Get();

                if (request.PageType == PageType.CustomPage)
                {
                    var authModel = new ContentAuthorizationModel
                    {
                        AllowAnonymous = false,
                        RegistrationStatuses = customPage.CustomPageRegistrationStatuses.Select(e => (RegistrationStatus)e.RegistrationStatusId),
                        RegistrationTypes = customPage.CustomPageRegistrationTypes.Select(e => e.RegistrationTypeId)
                    };

                    if(!_attendeeAuthorizationService.Authorize(attendeeWrapper.GetAuthModel(), authModel))
                    {
                        return Result.NotAllowed<CustomPageDisplayModel>();
                    }
                }


                var vm = new CustomPageDisplayModel();

                var context = new TemplateContext();
                if(!attendeeWrapper.IsAnonymous)
                {
                    context.SetValue("user", attendeeWrapper.Value);
                }
                vm.Html = await _liquidTemplateManager.RenderAsync(customPage.Content, context);
                vm.Title = customPage.Title;

                return Result.Ok(vm);
            }
        }
    }
}
