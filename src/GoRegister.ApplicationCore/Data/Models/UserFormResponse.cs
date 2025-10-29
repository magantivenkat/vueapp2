using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class UserFormResponse : IMustHaveProject
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int FormId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedUtc { get; set; }

        public Project Project { get; set; }
        public DelegateUser DelegateUser { get; set; }
        public Form Form { get; set; }
        public ICollection<UserFieldResponse> UserFieldResponses { get; set; } = new HashSet<UserFieldResponse>();


        private DelegateUserAudit _audit = new DelegateUserAudit();

        private DelegateUserAudit GetAudit()
        {
            return _audit ?? new DelegateUserAudit();
        }



        //public void ClearResponse(int fieldId)
        //{
        //    var response = GetResponse(fieldId);
        //    if (response != null)
        //    {
        //        UserFieldResponses.Remove(response);
        //        GetAudit().UserFieldResponseAudits.Add(
        //            new UserFieldResponseAudit
        //            {
        //                DelegateUser = DelegateUser,
        //                FieldId = response.FieldId
        //            }
        //        );
        //    }
        //}

        public void ClearResponse(int fieldId)
        {
            var responseMRF = GetResponse(fieldId);
            if (responseMRF != null)
            {
                UserFieldResponses.Remove(responseMRF);
                GetAudit().UserFieldResponseAudits.Add(
                    new UserFieldResponseAudit
                    {
                        DelegateUser = DelegateUser,
                        FieldId = responseMRF.FieldId
                    }
                );
            }
        }

        //public void SetStringValue(int fieldId, string value)
        //{
        //    var response = GetResponse(fieldId);
        //    if (response == null)
        //    {
        //        if (string.IsNullOrWhiteSpace(value)) return;

        //        response = new UserFieldResponse(fieldId);
        //        response.FieldId = fieldId;
        //        UserFieldResponses.Add(response);
        //    }
        //    else
        //    {
        //        if (response.StringValue == value) return;
        //    }

        //    response.StringValue = value;
        //    AddAudit(response);
        //}

        public void SetStringValue(int fieldId, string value)
        {
            var responseMRF = GetResponse(fieldId);
            if (responseMRF == null)
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                responseMRF = new UserFieldResponse(fieldId);
                responseMRF.FieldId = fieldId;
                UserFieldResponses.Add(responseMRF);
            }
            else
            {
                if (responseMRF.StringValue == value) return;
            }

            responseMRF.StringValue = value;
            AddAudit(responseMRF);
        }

        public void ClearStringValue(int fieldId)
        {
            SetStringValue(fieldId, null);
        }

        //public Result<string> GetStringValue(int fieldId)
        //{
        //    var response = GetResponse(fieldId);
        //    if (response == null)
        //    {
        //        return Result.Fail<string>();
        //    }

        //    return Result.Ok(response.StringValue);
        //}

        public Result<string> GetStringValue(int fieldId)
        {
            var responseMRF = GetResponse(fieldId);
            if (responseMRF == null)
            {
                return Result.Fail<string>();
            }

            return Result.Ok(responseMRF.StringValue);
        }

        public void ClearFieldOptionId(int fieldId)
        {
            ClearResponse(fieldId);
        }

        //public Result<int> GetFieldOptionId(int fieldId)
        //{
        //    var response = GetResponse(fieldId);
        //    if (response == null || response.FieldOptionId == null)
        //    {
        //        return Result.Fail<int>();
        //    }

        //    return Result.Ok(response.FieldOptionId.Value);
        //}

        public Result<int> GetFieldOptionId(int fieldId)
        {
            var responseMRF = GetResponse(fieldId);
            if (responseMRF == null || responseMRF.FieldOptionId == null)
            {
                return Result.Fail<int>();
            }

            return Result.Ok(responseMRF.FieldOptionId.Value);
        }

        //public Result<DateTime> GetDateTimeValue(int fieldId)
        //{
        //    var response = GetResponse(fieldId);
        //    if (response == null || response.DateTimeValue == null)
        //    {
        //        return Result.Fail<DateTime>();
        //    }

        //    return Result.Ok(response.DateTimeValue.Value);
        //}

        public Result<DateTime> GetDateTimeValue(int fieldId)
        {
            var responseMRF = GetResponse(fieldId);
            if (responseMRF == null || responseMRF.DateTimeValue == null)
            {
                return Result.Fail<DateTime>();
            }

            return Result.Ok(responseMRF.DateTimeValue.Value);
        }

        //public void SetDateTimeValue(int fieldId, DateTime? value)
        //{
        //    var response = GetResponse(fieldId);
        //    if (response == null)
        //    {
        //        if (value == null) return;

        //        response = new UserFieldResponse(fieldId);
        //        response.FieldId = fieldId;
        //        UserFieldResponses.Add(response);
        //    }
        //    else
        //    {
        //        if (response.DateTimeValue == value) return;
        //    }

        //    response.DateTimeValue = value;
        //    AddAudit(response);
        //}

        public void SetDateTimeValue(int fieldId, DateTime? value)
        {
            var responseMRF = GetResponse(fieldId);
            if (responseMRF == null)
            {
                if (value == null) return;

                responseMRF = new UserFieldResponse(fieldId);
                responseMRF.FieldId = fieldId;
                UserFieldResponses.Add(responseMRF);
            }
            else
            {
                if (responseMRF.DateTimeValue == value) return;
            }

            responseMRF.DateTimeValue = value;
            AddAudit(responseMRF);
        }

        public void ClearDateTimeValue(int fieldId)
        {
            SetDateTimeValue(fieldId, null);
        }

        //public void SetFieldOptionId(int fieldId, int value)
        //{
        //    var response = GetResponse(fieldId);
        //    if (response == null)
        //    {
        //        response = new UserFieldResponse(fieldId);
        //        response.FieldId = fieldId;
        //        UserFieldResponses.Add(response);
        //    }
        //    else if (response.FieldOptionId == value)
        //    {
        //        return;
        //    }

        //    response.FieldOptionId = value;
        //    AddAudit(response);
        //}

        public void SetFieldOptionId(int fieldId, int value)
        {
            var responseMRF = GetResponse(fieldId);
            if (responseMRF == null)
            {
                responseMRF = new UserFieldResponse(fieldId);
                responseMRF.FieldId = fieldId;
                UserFieldResponses.Add(responseMRF);
            }
            else if (responseMRF.FieldOptionId == value)
            {
                return;
            }

            responseMRF.FieldOptionId = value;
            AddAudit(responseMRF);
        }

        public UserFieldResponse GetResponse(int fieldId)
        {
            return UserFieldResponses.FirstOrDefault(e => e.FieldId == fieldId);
        }

        //public void AddAudit(UserFieldResponse response)
        //{
        //    GetAudit().UserFieldResponseAudits.Add(
        //        new UserFieldResponseAudit
        //        {
        //            DelegateUser = DelegateUser,
        //            FieldId = response.FieldId,
        //            FieldOptionId = response.FieldOptionId,
        //            StringValue = response.StringValue,
        //            BooleanValue = response.BooleanValue,
        //            NumberValue = response.NumberValue,
        //            DateTimeValue = response.DateTimeValue
        //        }
        //    );
        //}

        public void AddAudit(UserFieldResponse responseMRF)
        {
            GetAudit().UserFieldResponseAudits.Add(
                new UserFieldResponseAudit
                {
                    DelegateUser = DelegateUser,
                    FieldId = responseMRF.FieldId,
                    FieldOptionId = responseMRF.FieldOptionId,
                    StringValue = responseMRF.StringValue,
                    BooleanValue = responseMRF.BooleanValue,
                    NumberValue = responseMRF.NumberValue,
                    DateTimeValue = responseMRF.DateTimeValue
                }
            );
        }

        public void SetupAudit()
        {
            DelegateUser?.SetAudit(_audit);
        }

        public DelegateUserAudit GetAudit(ActionedFrom from)
        {
            return GetAudit(from, DelegateUser?.ApplicationUser);
        }

        public DelegateUserAudit GetAudit(ActionedFrom from, ApplicationUser actionedBy)
        {
            if (actionedBy == null) throw new ArgumentNullException("Actioned By ApplicationUser cannot be null when creating an audit");

            _audit.DelegateUser = DelegateUser;
            _audit.UserFormResponse = this;
            _audit.ActionedFrom = from;
            _audit.ActionedBy = actionedBy;
            return _audit;
        }
    }

    public class UserFormResponseMap : IEntityTypeConfiguration<UserFormResponse>
    {
        public void Configure(EntityTypeBuilder<UserFormResponse> builder)
        {
            builder
                .HasOne(e => e.DelegateUser)
                .WithMany(e => e.UserFormResponses)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            builder.HasMany(ufr => ufr.UserFieldResponses)
                .WithOne(ufr => ufr.UserFormResponse)
                .HasForeignKey(ufr => ufr.UserFormResponseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
