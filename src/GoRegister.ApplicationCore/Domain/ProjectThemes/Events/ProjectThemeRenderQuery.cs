//using GoRegister.ApplicationCore.Data;
//using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
//using GoRegister.ApplicationCore.Domain.Settings.Services;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Queries
//{
//    public class ProjectThemeRenderQuery : IRequest<ProjectThemeRenderQuery.Response>
//    {
//        public class Response
//        {

//        }

//        public class Handler : IRequestHandler<ProjectThemeRenderQuery, Response>
//        {
//            private readonly IProjectThemeService _projectThemeService;
//            private readonly ApplicationDbContext _db;
//            private readonly IProjectSettingsAccessor _projectSettingsAccessor;

//            public Handler(IProjectThemeService projectThemeService, ApplicationDbContext db, IProjectSettingsAccessor projectSettingsAccessor)
//            {
//                _projectThemeService = projectThemeService;
//                _db = db;
//                _projectSettingsAccessor = projectSettingsAccessor;
//            }

//            public async Task<Response> Handle(ProjectThemeRenderQuery request, CancellationToken cancellationToken)
//            {

//            }
//        }
//    }
//}
