using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class DataQuery : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Document { get; set; }
        public DateTime CreatedUtc { get; set; }
        public int CreatedById { get; set; }
        public string AccessLinkId { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }

    public class DataQueryMap : IEntityTypeConfiguration<DataQuery>
    {
        public void Configure(EntityTypeBuilder<DataQuery> builder)
        {
            builder
                .HasOne(e => e.CreatedBy)
                .WithMany(e => e.DataQueries)
                .HasForeignKey(e => e.CreatedById)
                .IsRequired();
        }
    }
}