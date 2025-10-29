using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsForPresentation = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Prefix = table.Column<string>(nullable: true),
                    PageTitleTag = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    BlockSearchEngineIndexing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomPage_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvitationList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvitationList_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    UniqueIdentifier = table.Column<Guid>(nullable: false),
                    IsInternalOnly = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationPage_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationPath",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateRegistrationFrom = table.Column<DateTime>(nullable: true),
                    DateRegistrationTo = table.Column<DateTime>(nullable: true),
                    DateModifyTo = table.Column<DateTime>(nullable: true),
                    DateCancelTo = table.Column<DateTime>(nullable: true),
                    CanCancel = table.Column<bool>(nullable: false),
                    CanModify = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    UseCreatedById = table.Column<int>(nullable: false),
                    NotInvitedText = table.Column<string>(nullable: true),
                    InvitedText = table.Column<string>(nullable: true),
                    ConfirmedText = table.Column<string>(nullable: true),
                    DeclinedText = table.Column<string>(nullable: true),
                    CancelledText = table.Column<string>(nullable: true),
                    WaitingText = table.Column<string>(nullable: true),
                    IsTesting = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationPath", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationPath_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    RegistrationPageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FieldTypeId = table.Column<int>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    Options = table.Column<string>(nullable: true),
                    DataTag = table.Column<string>(nullable: true),
                    MinLength = table.Column<int>(nullable: true),
                    MaxLength = table.Column<int>(nullable: true),
                    CanModify = table.Column<bool>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    DefaultValue = table.Column<string>(nullable: true),
                    HelpTextToolTip = table.Column<string>(nullable: true),
                    Placeholder = table.Column<string>(nullable: true),
                    UniqueIdentifier = table.Column<Guid>(nullable: false),
                    Class = table.Column<string>(nullable: true),
                    CustomAttributes = table.Column<string>(nullable: true),
                    SubText = table.Column<string>(nullable: true),
                    ApiFieldName = table.Column<string>(nullable: true),
                    ReportingHeader = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PreText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Field_FieldType_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Field_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_RegistrationPage_RegistrationPageId",
                        column: x => x.RegistrationPageId,
                        principalTable: "RegistrationPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    RegistrationPathId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    UserCreatedById = table.Column<int>(nullable: false),
                    Capacity = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationType_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationType_RegistrationPath_RegistrationPathId",
                        column: x => x.RegistrationPathId,
                        principalTable: "RegistrationPath",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    FieldId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Capacity = table.Column<int>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    DataTag = table.Column<string>(nullable: true),
                    ReportingTitle = table.Column<string>(nullable: true),
                    AgendaId = table.Column<int>(nullable: true),
                    Attributes = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    InternalInformation = table.Column<string>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true),
                    VisaAllowance = table.Column<decimal>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AdditionalInformation1 = table.Column<string>(nullable: true),
                    AdditionalInformation2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldOption_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldOption_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DelegateUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RegistrationTypeId = table.Column<int>(nullable: false),
                    InvitationListId = table.Column<int>(nullable: false),
                    ParentDelegateUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DelegateUser_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegateUser_InvitationList_InvitationListId",
                        column: x => x.InvitationListId,
                        principalTable: "InvitationList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegateUser_DelegateUser_ParentDelegateUserId",
                        column: x => x.ParentDelegateUserId,
                        principalTable: "DelegateUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DelegateUser_RegistrationType_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationPageRegistrationType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    RegistrationPageId = table.Column<int>(nullable: false),
                    RegistrationTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationPageRegistrationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationPageRegistrationType_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationPageRegistrationType_RegistrationPage_RegistrationPageId",
                        column: x => x.RegistrationPageId,
                        principalTable: "RegistrationPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationPageRegistrationType_RegistrationType_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationTypeField",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    RegistrationTypeId = table.Column<int>(nullable: false),
                    FieldId = table.Column<int>(nullable: false),
                    IsInternalOnly = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationTypeField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationTypeField_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationTypeField_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationTypeField_RegistrationType_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldOptionRule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    FieldOptionId = table.Column<int>(nullable: false),
                    NextFieldId = table.Column<int>(nullable: true),
                    NextFieldOptionId = table.Column<int>(nullable: true),
                    FieldId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOptionRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldOptionRule_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldOptionRule_FieldOption_FieldOptionId",
                        column: x => x.FieldOptionId,
                        principalTable: "FieldOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldOptionRule_FieldOption_NextFieldOptionId",
                        column: x => x.NextFieldOptionId,
                        principalTable: "FieldOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldOptionRule_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFieldResponse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    FieldId = table.Column<int>(nullable: false),
                    FieldOptionId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    ApplicationUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFieldResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFieldResponse_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFieldResponse_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFieldResponse_FieldOption_FieldOptionId",
                        column: x => x.FieldOptionId,
                        principalTable: "FieldOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFieldResponse_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFieldResponse_DelegateUser_UserId",
                        column: x => x.UserId,
                        principalTable: "DelegateUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "Host", "Name", "Prefix", "BlockSearchEngineIndexing", "MetaDescription", "PageTitleTag" },
                values: new object[] { 1, "localhost:5021", "Admin", "admin", false, null, null });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "a403dcb4-4121-4595-af1f-1690ae2f52b1", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProjectId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "f5142003-cc7a-4535-ba2d-3cc7c959606a", "WEBMASTER@BANKS-SADLER.COM", true, null, null, false, null, "WEBMASTER@BANKS-SADLER.COM", "admin", "AQAAAAEAACcQAAAAEGgOjySpZGo7N8+ibhY2qBAk4F46mRTJbrigU9PuNxwVmmLRUZ4QqxUdnPPR8wA9MQ==", null, false, 1, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_CustomPage_ProjectId",
                table: "CustomPage",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUser_InvitationListId",
                table: "DelegateUser",
                column: "InvitationListId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUser_ParentDelegateUserId",
                table: "DelegateUser",
                column: "ParentDelegateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUser_RegistrationTypeId",
                table: "DelegateUser",
                column: "RegistrationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FieldTypeId",
                table: "Field",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_ProjectId",
                table: "Field",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_RegistrationPageId",
                table: "Field",
                column: "RegistrationPageId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOption_FieldId",
                table: "FieldOption",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOption_ProjectId",
                table: "FieldOption",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptionRule_FieldId",
                table: "FieldOptionRule",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptionRule_FieldOptionId",
                table: "FieldOptionRule",
                column: "FieldOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptionRule_NextFieldOptionId",
                table: "FieldOptionRule",
                column: "NextFieldOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptionRule_ProjectId",
                table: "FieldOptionRule",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_InvitationList_ProjectId",
                table: "InvitationList",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationPage_ProjectId",
                table: "RegistrationPage",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationPageRegistrationType_ProjectId",
                table: "RegistrationPageRegistrationType",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationPageRegistrationType_RegistrationPageId",
                table: "RegistrationPageRegistrationType",
                column: "RegistrationPageId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationPageRegistrationType_RegistrationTypeId",
                table: "RegistrationPageRegistrationType",
                column: "RegistrationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationPath_ProjectId",
                table: "RegistrationPath",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationType_ProjectId",
                table: "RegistrationType",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationType_RegistrationPathId",
                table: "RegistrationType",
                column: "RegistrationPathId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTypeField_FieldId",
                table: "RegistrationTypeField",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTypeField_ProjectId",
                table: "RegistrationTypeField",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTypeField_RegistrationTypeId",
                table: "RegistrationTypeField",
                column: "RegistrationTypeId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_User_ProjectId",
                table: "User",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                columns: new[] { "NormalizedUserName", "ProjectId" },
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponse_ApplicationUserId",
                table: "UserFieldResponse",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponse_FieldId",
                table: "UserFieldResponse",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponse_FieldOptionId",
                table: "UserFieldResponse",
                column: "FieldOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponse_ProjectId",
                table: "UserFieldResponse",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponse_UserId",
                table: "UserFieldResponse",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomPage");

            migrationBuilder.DropTable(
                name: "FieldOptionRule");

            migrationBuilder.DropTable(
                name: "RegistrationPageRegistrationType");

            migrationBuilder.DropTable(
                name: "RegistrationTypeField");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserFieldResponse");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "FieldOption");

            migrationBuilder.DropTable(
                name: "DelegateUser");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "InvitationList");

            migrationBuilder.DropTable(
                name: "RegistrationType");

            migrationBuilder.DropTable(
                name: "FieldType");

            migrationBuilder.DropTable(
                name: "RegistrationPage");

            migrationBuilder.DropTable(
                name: "RegistrationPath");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
