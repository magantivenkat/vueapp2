using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class DelegateSessionBooking
    {
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public int DelegateUserId { get; set; }
        public DelegateUser DelegateUser { get; set; }

        public int ActionedByUserId { get; set; }
        public DateTime DateActionedUtc { get; set; }
        public DateTime? DateReleasedUtc { get; set; }
    }

    public class DelegateSessionBookingMap : IEntityTypeConfiguration<DelegateSessionBooking>
    {
        public void Configure(EntityTypeBuilder<DelegateSessionBooking> builder)
        {
            builder.HasKey(dsb => new { dsb.SessionId, dsb.DelegateUserId });
            builder.HasOne(dsb => dsb.DelegateUser)
                    .WithMany(du => du.DelegateSessionBookings)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(e => e.DelegateUserId);
        }
    }
}