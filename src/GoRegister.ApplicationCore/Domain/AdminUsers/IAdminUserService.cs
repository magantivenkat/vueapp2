using AutoMapper;
using AutoMapper.QueryableExtensions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Settings.Models;
using GoRegister.ApplicationCore.Extensions;
using GoRegister.ApplicationCore.Framework.Dapper;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.AdminUsers
{
    public interface IAdminUserService
    {
        Task<Result<int>> CreateAsync(AdminUserCreateEditModel model);
        Task<AdminUserCreateEditModel> GetCreateModelAsync();
        Task<IEnumerable<AdminUserListItemModel>> GetList();
        Task<AccountSettingsModel> GetUserSettingsAsync(int id);
        Task<IdentityResult> UpdateUserSettingsAsync(AccountSettingsModel model);
        Task<AdminUserCreateEditModel> FindAsync(int id);
        Task<AdminUserCreateEditModel> FindAsyncChangePassword(int id);
        Task<Result<int>> EditAsync(AdminUserCreateEditModel model);
        Task<IdentityResult> ChangePasswordAsync(int id, string password);
        Task<ShareUserSettingsViewModel> GetAllMeetingPlannerUsers(int projectId);
        Task<ShareUserSettingsViewModel> SaveUserProjectMapping(int userId, int projectId);
        Task<ShareUserSettingsViewModel> DeleteUserProjectMapping(int userId, int projectId);

    }

    public class AdminUserService : IAdminUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IUrlHelper _url;
        private readonly ISlickConnection _slickConnection;
        private readonly IMapper _mapper;

        public AdminUserService(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<UserRole> roleManager,
            IEmailSender emailSender,
            IUrlHelper url,
            ISlickConnection slickConnection)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _url = url;
            _slickConnection = slickConnection;
        }

        public async Task<IEnumerable<AdminUserListItemModel>> GetList()
        {
            var items = await _context.Users.AsNoTracking().ToListAsync();
            var models = _mapper.Map<IEnumerable<AdminUserListItemModel>>(items);

            return models;
        }

        public async Task<AccountSettingsModel> GetUserSettingsAsync(int id)
        {
            var model = await _context.Users.Where(u => u.Id == id)
                .ProjectTo<AccountSettingsModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<IdentityResult> UpdateUserSettingsAsync(AccountSettingsModel model)
        {
            var user = await _context.Users.FindAsync(model.Id);
            _mapper.Map(model, user);

            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<AdminUserCreateEditModel> FindAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var model = _mapper.Map<AdminUserCreateEditModel>(user);
            var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);


            model.Roles = roles.Select(r => new AdminUserRoleModel
            {
                Role = r,
                Checked = userRoles.Contains(r)
            }).ToArray();

            model.Roles1 = userRoles.ToArray();

            await PopulateModelDropdownsAsync(model);

            return model;
        }

        public async Task<AdminUserCreateEditModel> FindAsyncChangePassword(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var model = _mapper.Map<AdminUserCreateEditModel>(user);
            return model;
        }

        public async Task<AdminUserCreateEditModel> GetCreateModelAsync()
        {
            var model = await PopulateModelDropdownsAsync(new AdminUserCreateEditModel());
            return model;
        }

        public async Task<AdminUserCreateEditModel> PopulateModelDropdownsAsync(AdminUserCreateEditModel model)
        {
            model.TimeZones.GetTimeZoneList();
            model.DateFormats.GetDateTimeFormatList();

            var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();

            model.SelectRoles = roles.Select(x => new SelectListItem() { Text = x.ToString(), Value = x.ToString() });

            model.Roles = roles.Select(r => new AdminUserRoleModel
            {
                Role = r,
                Checked = false
            }).ToArray();

            return model;
        }

        public async Task<Result<int>> CreateAsync(AdminUserCreateEditModel model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            user.UserName = user.Email;
            user.ProjectId = null;

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                if (model.Roles1 != null)
                    await ProcessRoles(user, model);

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = _url.Page("/Account/Setup",
                    pageHandler: null,
                    protocol: "https",
                    values: new { code, area = "Identity" });

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Activate your account - GoRegister",
                    $"<p>To finish setting up this GoRegister account, we just need to make sure this email address is yours.</p>" +
                    $"<a href='{callbackUrl}'>Verify</a>" +
                    $"<p>Upon verification you will be invited to reset your password.</p>" +
                    $"<p>If you didn’t make this request, please email <a href='mailto:goregister@amexgbt.com'>goregister@amexgbt.com</a></p>" +
                    $"<p>Thanks,<br />GoRegister Team</p>");

                return Result.Ok(user.Id);
            }

            return Result.Fail<int>();
        }

        public async Task<Result<int>> EditAsync(AdminUserCreateEditModel model)
        {
            var user = await _context.Users.FindAsync(model.Id);
            _mapper.Map(model, user);

            // user name is always the same as email
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (model.Roles1 != null)
                    await ProcessRoles(user, model);

                return Result.Ok(user.Id);
            }

            return Result.Fail<int>();
        }

        public async Task<IdentityResult> ChangePasswordAsync(int id, string password)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, password);

            return result;
        }

        private async Task<IdentityResult> ProcessRoles(ApplicationUser user, AdminUserCreateEditModel model)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            //var sentRoles = model.Roles.Where(e => e.Checked).Select(e => e.Role);
            var sentRoles = model.Roles1;

            foreach (string role in sentRoles.Except(userRoles))
                await _userManager.AddToRoleAsync(user, role);

            foreach (string role in userRoles.Except(sentRoles))
                await _userManager.RemoveFromRoleAsync(user, role);

            return IdentityResult.Success;
        }

        public async Task<ShareUserSettingsViewModel> GetAllMeetingPlannerUsers(int projectId)
        {
            using (var connection = _slickConnection.Get())
            {
                var results = (await connection.QueryAsync<ShareUserSettingsModel>(
                    @"select usr.Id as UserId,usr.FirstName,usr.LastName,usr.Email,rol.Id as RoleId,rol.Name as RoleName,(usr.FirstName + ' ' + usr.LastName + ' ( ' + usr.Email + ' )') as UserName from [dbo].[User] usr
                        inner join [dbo].[UserRole] usrl on usr.Id=usrl.UserId
                        inner join [dbo].[Role] rol on rol.Id=usrl.RoleId
                        where rol.Name='MeetingPlanner'")
                    ).ToList();

                var mappedUsers = (await connection.QueryAsync<ShareUserSettingsModel>(
                    @"select usr.Id as UserId,upm.ProjectId,usr.FirstName,usr.LastName,usr.Email,(usr.FirstName + ' ' + usr.LastName + ' ( ' + usr.Email + ' )') as UserName
                        from [dbo].[User] usr
                        inner join [dbo].[UserProjectMapping] upm on usr.Id=upm.UserId
                        where upm.ProjectId=" + projectId + "")
                    ).ToList();

                var model = new ShareUserSettingsViewModel
                {
                    Users = results,
                    MappedUsers = mappedUsers,
                    ProjectId=projectId
                };

                return model;
            }
        }

        public async Task<ShareUserSettingsViewModel> SaveUserProjectMapping(int userId, int projectId)
        {
            var users = await _context.UserProjectMappings.Where(u => u.ProjectId == projectId).ToListAsync();

            var exists = users.Any(u => u.UserId == userId && u.ProjectId == projectId);

            if (!exists)
            {
                var userProjMap = new UserProjectMapping
                {
                    UserId = userId,
                    ProjectId = projectId
                };
                _context.Add(userProjMap);
                var result = await _context.SaveChangesAsync();
            }
            return (await GetAllMeetingPlannerUsers(projectId));

        }

        public async Task<ShareUserSettingsViewModel> DeleteUserProjectMapping(int userId, int projectId)
        {
            var users = await _context.UserProjectMappings.FindAsync(projectId,userId);
            
            if(users!=null)
            {
                _context.UserProjectMappings.Remove(users);
                await _context.SaveChangesAsync();
            }
            return (await GetAllMeetingPlannerUsers(projectId));
        }

    }
}
