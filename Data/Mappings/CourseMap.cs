using courseappchallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace courseappchallenge.Data.Mappings;

public class CourseMap : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Course");

        builder.HasKey(x => x.CourseId);

        builder.Property(x => x.CourseTitle).IsRequired().HasColumnName("CourseTitle").HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Tag).IsRequired().HasColumnName("Tag")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(4);

        builder.Property(x => x.Summary).IsRequired().HasColumnName("Summary").HasColumnType("NVARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.DurationInMinutes)
            .IsRequired()
            .HasColumnName("Duration")
            .HasColumnType("INTEGER");

        builder.HasMany(x => x.CourseItems);
    }
}