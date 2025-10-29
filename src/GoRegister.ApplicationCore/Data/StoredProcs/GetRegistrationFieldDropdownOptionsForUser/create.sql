CREATE PROCEDURE [dbo].[GetRegistrationFieldDropdownOptionsForUser]
	@InvitationListId INT,
	@RegistrationPageTypeId INT
AS
BEGIN

	SET NOCOUNT ON;

 SELECT 
	FO.*
 FROM RegistrationTypeField RTF
 	JOIN Field F ON F.Id = RTF.FieldId 
	JOIN RegistrationType RT ON RT.Id = RTF.RegistrationTypeId
	JOIN RegistrationPage RP ON F.RegistrationPageId = RP.Id
	JOIN DelegateUser ILU ON ILU.RegistrationTypeId = RT.Id
	JOIN FieldOption FO ON FO.FieldId = F.Id
 WHERE 
	ILU.Id = @InvitationListId
	--AND RP.RegistrationPageTypeId = @RegistrationPageTypeId
	AND F.IsDeleted = 0
	AND FO.IsDeleted = 0

END