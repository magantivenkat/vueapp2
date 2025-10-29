using AutoMapper;
using FluentAssertions;
using FluentAssertions.Equivalency;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Domain.Projects.Models;
using GoRegister.ApplicationCore.Domain.Projects.Services;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.ApplicationCore.Framework;
using GoRegister.TestingCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Projects.Services
{
    public class ProjectServiceCloneShould : DatabaseContextTest
    {
        private Project Project;
        private ProjectTheme ProjectTheme;
        private const int OLD_PROJECT_ID = 5;
        private readonly Mock<IConfiguration> _mockConfiguration;

        private const string CustomPageContent = "Content";

        public ProjectServiceCloneShould()
        {
            _mockConfiguration = new Mock<IConfiguration>();

            Project = new Project
            {
                Id = OLD_PROJECT_ID,
                Host = "host",
                Name = "name",
                Prefix = "prefix",

                Client = new Client
                {

                }
            };

            ProjectTheme = new ProjectTheme
            {
                ThemeCss = "ThemeCss",
                OverrideCss = "OverrideCss",
                FooterScripts = "FooterScripts",
                HeadScripts = "HeadScripts",
                LogoUrl = "LogoUrl",
                HeaderHtml = "HeaderHtml",
                FooterHtml = "FooterHtml",
                DateUpdated = new DateTime(2021, 07, 07, 10, 10, 10),
                Project = Project
            };

            Project.ProjectThemes = new List<ProjectTheme>
            {
                new ProjectTheme { Project = Project, DateUpdated = new DateTime(2021, 07, 07, 10, 10, 0) },
                new ProjectTheme { Project = Project, DateUpdated = new DateTime(2019, 07, 07, 10, 10, 0) },
                ProjectTheme
            };

            Project.CustomPages = new List<CustomPage>
                {
                    new CustomPage
                    {
                        Content = CustomPageContent,
                        Project = Project
                    },
                    new CustomPage
                    {
                        Content = "Content2",
                        Project = Project
                    }
                };

            Project.RegistrationPaths = new List<RegistrationPath> {
                    new RegistrationPath
                    {
                        Name = "Default",
                        CanCancel = false,
                        CanModify = false,
                        DateCreated = SystemTime.UtcNow,
                        UseCreatedById = 0,
                        RegistrationTypes = new List<RegistrationType>
                        {
                            new RegistrationType
                            {
                                Name = "Default",
                                DateCreated = SystemTime.UtcNow,
                                UserCreatedById = 0,
                                Capacity = 0,
                                Project = Project
                            }
                        }
                    }
                };

            Project.Forms = new List<Form>
            {
                new Form
                {
                    FormTypeId = FormType.Registration,
                    RegistrationPages = new List<RegistrationPage> {
                        new RegistrationPage
                        {
                            Title = "Personal Details",
                            Fields = new List<Field>
                            {
                                new FirstNameField() {
                                    Name = "First Name",
                                    DataTag = "FirstName",
                                    SortOrder = 1,
                                    IsMandatory = true,
                                    Project = Project
                                },
                                new LastNameField {
                                    Name = "Last Name",
                                    DataTag = "LastName",
                                    SortOrder = 2,
                                    IsMandatory = true,
                                    Project = Project
                                },
                                new EmailField {
                                    Name = "Email",
                                    DataTag = "Email",
                                    SortOrder = 3,
                                    IsMandatory = true,
                                    Project = Project
                                }
                            }
                        }
                    },
                    Project = Project
                },
                new Form
                {
                    FormTypeId = FormType.Decline,
                    SubmitButtonText = "Decline",
                    IsReviewPageHidden = true,
                    RegistrationPages = new List<RegistrationPage> {
                        new RegistrationPage
                        {
                            Title = "Decline",
                            Fields = new List<Field>
                            {
                                new FirstNameField() {
                                    Name = "First Name",
                                    DataTag = "FirstName",
                                    SortOrder = 1,
                                    IsMandatory = true,
                                    Project = Project
                                },
                                new LastNameField {
                                    Name = "Last Name",
                                    DataTag = "LastName",
                                    SortOrder = 2,
                                    IsMandatory = true,
                                    Project = Project
                                },
                                new EmailField {
                                    Name = "Email",
                                    DataTag = "Email",
                                    SortOrder = 3,
                                    IsMandatory = true,
                                    Project = Project
                                }
                            }
                        }
                    },
                    Project = Project
                }
            };

            using (var db = GetAdminDatabase())
            {
                db.Projects.Add(Project);
                db.SaveChanges();
            }

            ProjectTenantAccessor.Setup(e => e.Get).Returns(new ProjectTenant
            {
                Id = OLD_PROJECT_ID
            });
        }

        private IProjectService GetService(ApplicationDbContext db)
        {
            return new ProjectService(db, _mockConfiguration.Object);
        }

        [Fact]
        public async Task When_CloningProject_AllEntitiesAreDetachedWithId0()
        {
            var cloneModel = new CloneProjectModel()
            {
                CloneRegistrationTypes = true,
                ClonePages = true,
                CloneRegistration = true,
                CloneReports = true,
                CloneSessions = true,
                CloneTheme = true
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                // project
                clonedProject.Id.Should().Be(0);
                db.Entry(clonedProject).State.Should().Be(EntityState.Detached);

                // custom pages
                clonedProject.CustomPages.All(cp => IsDetachedAndId0(db, cp));
                clonedProject.CustomPages.SelectMany(cp => cp.CustomPageRegistrationStatuses).All(cprs => IsDetachedAndId0(db, cprs));

                // forms
                clonedProject.Forms.All(e => IsDetachedAndId0(db, e));
                clonedProject.Forms.SelectMany(f => f.RegistrationPages).All(e => IsDetachedAndId0(db, e));
                clonedProject.Forms.SelectMany(f => f.RegistrationPages).SelectMany(e => e.Fields).All(e => IsDetachedAndId0(db, e));
            }
        }

        [Fact]
        public async Task When_CloneRegistrationTypes_HasRegistrationTypes()
        {
            var cloneModel = new CloneProjectModel()
            {
                CloneRegistrationTypes = true
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                clonedProject.RegistrationTypes.Should().HaveCount(1);
                clonedProject.RegistrationPaths.Should().HaveCount(1);
            }
        }

        [Fact]
        public async Task When_DontCloneRegistrationTypes_NoRegistrationTypes()
        {
            var cloneModel = new CloneProjectModel()
            {
                CloneRegistrationTypes = false
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                var clonedEntities = clonedProject.RegistrationPaths.ToArray();
                var originalEntities = Project.RegistrationPaths.ToArray();

                for (int i = 0; i < clonedProject.RegistrationTypes.Count; i++)
                {
                    var clonedEntity = clonedEntities[i];
                    var originalEntity = originalEntities[i];

                    clonedEntity.Should().BeEquivalentTo(originalEntity, EquivalencyConfig);
                }
            }
        }

        [Fact]
        public async Task When_CloneRegistration_HasForms()
        {
            var cloneModel = new CloneProjectModel()
            {
                CloneRegistration = true
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                clonedProject.Forms.Should().HaveCount(2);

                var clonedForms = clonedProject.Forms.ToArray();
                var originalForms = Project.Forms.ToArray();

                for (int i = 0; i < clonedProject.Forms.Count; i++)
                {
                    var form = clonedForms[i];
                    var originalForm = originalForms[i];

                    form.Should().BeEquivalentTo(originalForm, config =>
                        EquivalencyConfig(config));
                }
            }
        }

        [Fact]
        public async Task When_DontCloneRegistration_HasNoForms()
        {
            var cloneModel = new CloneProjectModel()
            {
                CloneRegistration = false
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                clonedProject.Forms.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task When_ClonePages_HasPages()
        {
            var cloneModel = new CloneProjectModel()
            {
                ClonePages = true
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                clonedProject.CustomPages.Should().HaveCount(2);

                var clonedEntities = clonedProject.CustomPages.ToArray();
                var originalEntities = Project.CustomPages.ToArray();

                for (int i = 0; i < clonedProject.CustomPages.Count; i++)
                {
                    var clonedEntity = clonedEntities[i];
                    var originalEntity = originalEntities[i];

                    clonedEntity.Should().BeEquivalentTo(originalEntity, EquivalencyConfig);
                }
            }
        }

        [Fact]
        public async Task When_DontClonePages_HasNoPages()
        {
            var cloneModel = new CloneProjectModel()
            {
                ClonePages = false
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                clonedProject.CustomPages.Should().HaveCount(0);
            }
        }



        [Fact]
        public async Task When_CloneTheme_HasTheme()
        {
            var cloneModel = new CloneProjectModel()
            {
                CloneTheme = true
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                clonedProject.ProjectThemes.Should().HaveCount(1);
                var clonedTheme = clonedProject.ProjectThemes.First();
                var originalTheme = ProjectTheme;


                clonedTheme.ThemeCss.Should().Be(originalTheme.ThemeCss);
                clonedTheme.FooterScripts.Should().Be(originalTheme.FooterScripts);
                clonedTheme.HeadScripts.Should().Be(originalTheme.HeadScripts);
                clonedTheme.HeaderHtml.Should().Be(originalTheme.HeaderHtml);
                clonedTheme.FooterHtml.Should().Be(originalTheme.FooterHtml);
                clonedTheme.OverrideCss.Should().Be(originalTheme.OverrideCss);
            }
        }

        [Fact]
        public async Task When_DontCloneTheme_HasBlankTheme()
        {
            var cloneModel = new CloneProjectModel()
            {
                CloneTheme = false
            };

            using (var db = GetAdminDatabase())
            {
                var sut = GetService(db);
                var result = await sut.GetClonedProject(OLD_PROJECT_ID, cloneModel);
                var clonedProject = result.Value;

                clonedProject.ProjectThemes.Should().HaveCount(0);
            }
        }

        private bool IsDetachedAndId0<T>(ApplicationDbContext db, T entity)
        {
            var id = (int)entity.GetType().GetProperty("Id").GetValue(entity);
            id.Should().Be(0);
            db.Entry(entity).State.Should().Be(EntityState.Detached, $"{typeof(T).Name} was not detached");
            return true;
        }

        private EquivalencyAssertionOptions<T> EquivalencyConfig<T>(EquivalencyAssertionOptions<T> config)
        {
            return config
                .IgnoringCyclicReferences()
                .Excluding(e => e.SelectedMemberPath.EndsWith("UniqueIdentifier"))
                .Excluding(e => e.SelectedMemberPath == "Id")
                .Excluding(e => e.SelectedMemberPath == "ProjectId")
                .Excluding(e => e.SelectedMemberPath == "Project")
                .Excluding(e => e.SelectedMemberPath.EndsWith("Id"))
                .Excluding(e => e.SelectedMemberPath.EndsWith(".ProjectId"))
                .Excluding(e => e.SelectedMemberPath.EndsWith(".Project"));
        }
    }
}
