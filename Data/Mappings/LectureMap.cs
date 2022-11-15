using courseappchallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace courseappchallenge.Data.Mappings;

public class LectureMap : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.ToTable("Lecture");

        builder.HasKey(x => x.LectureId);

        builder.Property(x => x.LectureTitle).IsRequired().HasColumnName("LectureTitle").HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Description).IsRequired().HasColumnName("Description")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.VideoUrl)
            .IsRequired()
            .HasColumnName("VideoUrl")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(2046);

        builder.HasOne(x => x.CourseItem)
            .WithMany(x => x.Lectures)
            .HasConstraintName("FK_Lecture_CourseItemId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}