using GoRegister.ApplicationCore.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class sp_GetUserSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE usp_GetUserSessions
	-- Add the parameters for the stored procedure here
	@UserId INT,
	@ProjectId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select 
		sess.Id as Id,
		sess.Name as Name,
		sess.Description as Description,
		sess.DateStartUtc as DateStartUtc,
		sess.DateEndUtc as DateEndUtc,
		sess.DateCloseRegistrationUtc as DateCloseRegistrationUtc
	from [Session] sess WITH(NOLOCK) 
	LEFT JOIN [SessionRegistrationType] srt WITH(NOLOCK) ON sess.Id=srt.SessionId
	LEFT JOIN [DelegateUser] du WITH (NOLOCK) ON du.RegistrationTypeId=srt.RegistrationTypeId
	where (du.Id=@UserId OR sess.ProjectId=@ProjectId) AND sess.IsOptional=0 
	
	UNION

	select 
		sess.Id as Id,
		sess.Name as Name,
		sess.Description as Description,
		sess.DateStartUtc as DateStartUtc,
		sess.DateEndUtc as DateEndUtc,
		sess.DateCloseRegistrationUtc as DateCloseRegistrationUtc
	from [DelegateSessionBooking] dsp
	INNER JOIN [Session] sess WITH(NOLOCK) on sess.Id=dsp.SessionId
	INNER JOIN [DelegateUser] du WITH(NOLOCK) on du.Id=dsp.DelegateUserId
	where dsp.DelegateUserId=@UserId

END";

            migrationBuilder.SqlExec(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
