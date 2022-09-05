using Challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challenge.Data.Mappings;

public class LectureMap : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.ToTable("Lecture");
        
        builder.HasKey(x => x.LectureId);
        
        builder.Property(x => x.LectureTitle).
            IsRequired().
            HasColumnName("LectureTitle").
            HasColumnType("NVARCHAR").
            HasMaxLength(80);
        
        builder.Property(x => x.Description).
            IsRequired().
            HasColumnName("Description")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.VideoUrl)
            .IsRequired()
            .HasColumnName("VideoUrl")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(2046);

        builder.HasOne(x => x.Module)
            .WithMany(x => x.Lectures)
            .HasForeignKey("LectureId")
            .HasConstraintName("FK_Module_LectureId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}