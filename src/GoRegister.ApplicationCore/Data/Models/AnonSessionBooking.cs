using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class AnonSessionBooking
    {
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public string AnonUserId { get; set; }
        //public DelegateUser DelegateUser { get; set; }

        public int ActionedByUserId { get; set; }
        public DateTime DateActionedUtc { get; set; }
        public DateTime? DateReleasedUtc { get; set; }
    }

    public class AnonSessionBookingMap : IEntityTypeConfiguration<AnonSessionBooking>
    {
        public void Configure(EntityTypeBuilder<AnonSessionBooking> builder)
        {
            builder.HasKey(dsb => new { dsb.SessionId, dsb.AnonUserId });
        }
    }
}