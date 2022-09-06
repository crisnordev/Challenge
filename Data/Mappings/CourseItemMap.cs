using Challenge.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challenge.Data.Mappings;

public class CourseItemMap : IEntityTypeConfiguration<CourseItem>
{
    public void Configure(EntityTypeBuilder<CourseItem> builder)
    {
        builder.ToTable("Module");
        
        builder.HasKey(x => x.CourseItemId);
        
        builder.Property(x => x.CourseItemTitle)
            .IsRequired()
            .HasColumnName("CourseItemTitle")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        
        builder.Property(x => x.Order)
            .IsRequired()
            .HasColumnName("Order")
            .HasColumnType("INTEGER");

        builder.HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey("CourseId")
            .HasConstraintName("FK_CourseItem_CourseId");

        builder.HasMany(x => x.Lectures);
    }
}