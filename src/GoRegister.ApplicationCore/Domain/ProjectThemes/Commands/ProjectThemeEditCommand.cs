using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Variables;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Commands
{
    public class ProjectThemeEditCommand : IRequest<Result<Unit>>
    {
        public class Handler : IRequestHandler<ProjectThemeEditCommand, Result<Unit>>
        {
            private readonly IProjectThemeService _projectThemeService;
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _db;
            private readonly IProjectSettingsAccessor _projectSettingsAccessor;

            public Handler(IProjectThemeService projectThemeService, IMapper mapper, ApplicationDbContext db, IProjectSettingsAccessor projectSettingsAccessor)
            {
                _projectThemeService = projectThemeService;
                _mapper = mapper;
                _db = db;
                _projectSettingsAccessor = projectSettingsAccessor;
            }

            public async Task<Result<Unit>> Handle(ProjectThemeEditCommand request, CancellationToken cancellationToken)
            {
                var settings = await _projectSettingsAccessor.GetAsync();
                var projectTheme = await _db.ProjectThemes.Include(p => p.Fonts).OrderByDescending(e => e.DateUpdated).FirstAsync(pt => pt.ProjectId == settings.Id);

                var isTemplate = settings.ProjectType == Data.Enums.ProjectTypeEnum.Template;

                if (isTemplate)
                {
                    projectTheme.LayoutName = request.LayoutName;
                }

                projectTheme.FooterScripts = request.FooterScripts;
                projectTheme.HeadScripts = request.HeadScripts;
                projectTheme.ThemeCss = request.ThemeCss;

                projectTheme.HeaderHtml = request.HeaderHtml;
                projectTheme.FooterHtml = request.FooterHtml;
                projectTheme.LogoUrl = request.LogoUrl;

                projectTheme.ThemeVariableObject = JsonConvert.SerializeObject(request.Variables);

                var context = new BuildCssVariableContext(request.Variables);
                foreach (var variable in VariableFactory.BuildVariableList())
                {
                    variable.Build(context);
                }

                projectTheme.ThemeVariables = context.CreateVariablesCSS();              

                var themeFonts = request.Fonts;                             

                List<ThemeFont> fontCollection = new List<ThemeFont>();
                
                foreach (var font in themeFonts)
                {
                    ThemeFont themeFont = new ThemeFont
                    {
                        Link = font.Link,
                        FontType = (FontType)Enum.Parse(typeof(FontType), Convert.ToString(font.FontType)),
                        Name = font.Name,
                        ProjectId = settings.Id,
                        ProjectThemeId = projectTheme.Id,
                        Variants = string.Join(",", font.Variants),
                    };

                    fontCollection.Add(themeFont);
                }

                projectTheme.Fonts = fontCollection;

                await _db.SaveChangesAsync();

                return Result.Ok(Unit.Value);
            }
        }

        public string LayoutName { get; set; }
        public string ThemeCss { get; set; }
        public string OverrideCss { get; set; }
        public string HeaderHtml { get; set; }
        public string FooterHtml { get; set; }
        public string HeadScripts { get; set; }
        public string FooterScripts { get; set; }
        public string LogoUrl { get; set; }
        public Dictionary<string, string> Variables { get; set; }
        public List<FontViewModel> Fonts { get; set; } = new List<FontViewModel>();

        // vm
        public List<SelectListItem> LayoutOptions { get; set; }
        public bool ProjectIsTemplate { get; set; }
    }

    public class FontViewModel
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public FontType FontType { get; set; }
        public string Name { get; set; }
        public string[] Variants { get; set; }
        //public string VariantsMap { get; set; }
    }
}
