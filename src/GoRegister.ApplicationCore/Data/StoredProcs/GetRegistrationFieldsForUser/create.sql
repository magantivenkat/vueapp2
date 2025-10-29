CREATE PROCEDURE [dbo].[GetRegistrationFieldsForUser]
	@InvitationListId INT,
	@RegistrationPageTypeId INT
AS
BEGIN

	SET NOCOUNT ON;

   SELECT 
	F.*,
	RTF.IsInternalOnly,
	RTF.IsHidden,
	UFR.[Value] AS UserFieldValue,
	FO.[Description] AS UserFieldValueDescription,
	FO.Id AS UserFieldOptionId,
	FT.IsForPresentation
 FROM RegistrationTypeField RTF
	JOIN RegistrationType RT ON RT.Id = RTF.RegistrationTypeId
	JOIN Field F ON F.Id = RTF.FieldId 
	JOIN FieldType FT ON FT.Id = F.FieldTypeId
	JOIN dbo.DelegateUser ILU ON ILU.RegistrationTypeId = RT.Id
	JOIN [User] U ON U.Id = ILU.Id
	JOIN RegistrationPage RP ON F.RegistrationPageId = RP.Id
	LEFT JOIN [UserFieldResponse] UFR ON UFR.UserId = U.Id AND UFR.FieldId = F.Id
	LEFT JOIN FieldOption FO ON FO.Id = UFR.FieldOptionId
	
 WHERE 
	ILU.Id = @InvitationListId
	--AND RP.RegistrationPageTypeId = @RegistrationPageTypeId
	AND F.IsDeleted = 0
	
 ORDER BY 
	RP.SortOrder,
	F.SortOrder

END