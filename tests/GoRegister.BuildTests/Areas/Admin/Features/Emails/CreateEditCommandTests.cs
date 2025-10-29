using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using GoRegister.Areas.Admin.Features.Emails;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GoRegister.Tests.Areas.Admin.Features.Emails
{
    public class CreateEditCommandTests
    {
        protected int ExistingProjectId = 2;
        private int regTypeOneId = 2;
        private int regTypeTwoId = 3;

        private readonly IMapper _mapper;

        public CreateEditCommandTests()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())));
        }

        public ApplicationDbContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var tenant = new ProjectTenant { Id = ExistingProjectId, IsAdmin = true };
            var tenantAccessorMock = new Mock<ITenantAccessor>();
            tenantAccessorMock.Setup(e => e.Get).Returns(tenant);

            var context = new ApplicationDbContext(tenantAccessorMock.Object, options);

            var project = new Project { Id = ExistingProjectId, Name = "Project 1" };
            context.Projects.Add(project);
            context.RegistrationTypes.Add(new RegistrationType { Id = regTypeOneId, Name = "Test" });
            context.RegistrationTypes.Add(new RegistrationType { Id = regTypeTwoId, Name = "Test" });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async void Handle_NoTemplates_Invalid()
        {
            var context = GetContextWithData();
            var mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())));

            var sut = new CreateEditEmail.CommandHandler(context, mapper);
            var command = new CreateEditEmail.Command();

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Failed.Should().BeTrue();
            result.Error.Should().BeOfType<Error.Invalid>();
        }

        [Fact]
        public async void Handle_AddCustomEmailWithExistingCustomEmail_SuccessResult()
        {
            // add email with an emailtype of custom to db
            var context = GetContextWithData();
            var email = new Email { EmailType = EmailType.Custom };
            context.Emails.Add(email);
            context.SaveChanges();

            // make a valid command with an id of 0 and emailtype of custom

            var command = new CreateEditEmail.Command
            {
                Id = 0,
                EmailType = EmailType.Custom,
                Templates = new List<CreateEditEmail.EmailTemplateModel>
                {
                    new CreateEditEmail.EmailTemplateModel { }
                }
            };

            // it should return success and add to db, check count is now 2        
            var mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())));
            var sut = new CreateEditEmail.CommandHandler(context, mapper);
            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Failed.Should().BeFalse();
            context.Emails.ToList().Should().HaveCount(2);
        }

        [Theory]
        [InlineData(EmailType.Cancellation)]
        [InlineData(EmailType.Confirmation)]
        [InlineData(EmailType.Invitation)]
        [InlineData(EmailType.Decline)]
        [InlineData(EmailType.Waiting)]
        [InlineData(EmailType.InvitationReminder)]
        public async void Handle_AddEmailWithExistingType_Invalid(EmailType emailType)
        {
            var context = GetContextWithData();
            // add email with specified emailType to db
            //using (var db = GetContextWithData())
            //{
            var email = new Email { EmailType = emailType };
            context.Emails.Add(email);
            context.SaveChanges();
            //}
            // make a valid command with an id of 0 and emailtype of specified emailType
            var command = new CreateEditEmail.Command
            {
                Id = 0,
                EmailType = emailType
            };
            // it should return badrequest

            var mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())));
            var sut = new CreateEditEmail.CommandHandler(context, mapper);
            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Error.Should().BeOfType<Error.Invalid>();
        }

        [Fact]
        public async void Handle_EditRequestWhereIdNotFound_ReturnNotFound()
        {
            // make a command with an id, it should return an error result of NotFound
            var context = GetContextWithData();

            var sut = new CreateEditEmail.CommandHandler(context, _mapper);

            var command = new CreateEditEmail.Command
            {
                Id = 1,
                EmailType = EmailType.Custom,
                Templates = new List<CreateEditEmail.EmailTemplateModel> {
                    new CreateEditEmail.EmailTemplateModel { Id = 1 }
                }
            };

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Error.Should().BeOfType<Error.NotFound>();
        }


        [Fact]
        public async void Handle_ValidEditRequest_EditsEmail()
        {
            // create a valid email (email model with id and one default template attached to it) and add to db context
            var context = GetContextWithData();
            List<EmailTemplate> emailTemplates = new List<EmailTemplate>();
            EmailTemplate template = new EmailTemplate { Id = 1 };
            emailTemplates.Add(template);

            context.Emails.Add(new Email { Id = 1, EmailTemplates = emailTemplates });

            context.SaveChanges();

            var sut = new CreateEditEmail.CommandHandler(context, _mapper);

            var updatedSubject = "Updated Subject for test";
            // create a command with the email id, an updated subject property and an email template model
            var command = new CreateEditEmail.Command
            {
                Id = 1,
                EmailType = EmailType.Custom,
                Templates = new List<CreateEditEmail.EmailTemplateModel> {
                    new CreateEditEmail.EmailTemplateModel { Id = 1 }
                },
                Subject = updatedSubject
            };
            // retrieve the email from the db context and check the subject has been updated            

            // check for successresult
            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Failed.Should().BeFalse();
            context.Emails.First(e => e.Id == 1).Subject.Should().Be(updatedSubject);
        }

        [Fact]
        public async void Handle_MissingTemplateEditRequest_NotFound()
        {
            // create a valid email (email model with id and one default template attached to it) and add to db context
            var context = GetContextWithData();
            context.Emails.Add(new Email { Id = 1 });

            context.SaveChanges();

            var sut = new CreateEditEmail.CommandHandler(context, _mapper);

            var updatedSubject = "Updated Subject for test";
            // create a command with the email id, an updated subject property and an email template model
            var command = new CreateEditEmail.Command
            {
                Id = 1,
                EmailType = EmailType.Custom,
                Templates = new List<CreateEditEmail.EmailTemplateModel> {
                    new CreateEditEmail.EmailTemplateModel { Id = 1 }
                },
                Subject = updatedSubject
            };
            // retrieve the email from the db context and check the subject has been updated            

            // check for successresult
            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Failed.Should().BeTrue();
            result.Error.Should().BeOfType<Error.NotFound>();
        }

        [Fact]
        public async void Handle_ValidEditRequestWithUpdatedTemplate_EditsEmailTemplate()
        {
            // create a valid email (email model with id and one default template attached to it) and add to db context
            var context = GetContextWithData();
            List<EmailTemplate> emailTemplates = new List<EmailTemplate>();
            EmailTemplate template = new EmailTemplate { Id = 1 };
            emailTemplates.Add(template);

            context.Emails.Add(new Email { Id = 1, EmailTemplates = emailTemplates });

            context.SaveChanges();

            var sut = new CreateEditEmail.CommandHandler(context, _mapper);

            var updatedBody = "Updated body for test";
            // create a command with the email id, and an email template model with an updated body property
            var command = new CreateEditEmail.Command
            {
                Id = 1,
                EmailType = EmailType.Custom,
                Templates = new List<CreateEditEmail.EmailTemplateModel> {
                    new CreateEditEmail.EmailTemplateModel { Id = 1 }
                },
                Subject = updatedBody
            };
            // retrieve the email from the db context and check the template has been updated
            // check for successresult
            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Failed.Should().BeFalse();
            context.Emails.First(e => e.Id == 1).Subject.Should().Be(updatedBody);
        }

        [Fact]
        public async void Handle_MultipleTemplates_AddsTemplates()
        {
            var context = GetContextWithData();
            var mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())));

            var sut = new CreateEditEmail.CommandHandler(context, mapper);
            var command = new CreateEditEmail.Command()
            {
                Templates = new List<CreateEditEmail.EmailTemplateModel>
                {
                    new CreateEditEmail.EmailTemplateModel { },
                    new CreateEditEmail.EmailTemplateModel { }
                }
            };

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            var email = context.Emails
                .Include(e => e.EmailTemplates)
                .ThenInclude(e => e.RegistrationTypes)
                .FirstOrDefault();

            result.Failed.Should().BeFalse();
            email.EmailTemplates.Should().HaveCount(2);
        }

        [Fact]
        public async void Handle_MultipleRegTypesOnTemplate_AddsLinkTableRows()
        {
            var context = GetContextWithData();
            var mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())));

            var sut = new CreateEditEmail.CommandHandler(context, mapper);
            var command = new CreateEditEmail.Command()
            {
                Templates = new List<CreateEditEmail.EmailTemplateModel>
                {
                    new CreateEditEmail.EmailTemplateModel
                    {
                        RegistrationTypes = new List<int> { regTypeOneId, regTypeTwoId }
                    }
                }
            };

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            var email = context.Emails
                .Include(e => e.EmailTemplates)
                .ThenInclude(e => e.RegistrationTypes)
                .FirstOrDefault();

            result.Failed.Should().BeFalse();
            email.EmailTemplates.First().RegistrationTypes.Should().HaveCount(2);
        }
    }
}
