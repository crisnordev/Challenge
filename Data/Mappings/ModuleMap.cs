using Challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challenge.Data.Mappings;

public class ModuleMap : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Module");
        
        builder.HasKey(x => x.ModuleId);
        
        builder.Property(x => x.ModuleTitle)
            .IsRequired()
            .HasColumnName("ModuleTitle")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        
        builder.Property(x => x.Order)
            .IsRequired()
            .HasColumnName("Order")
            .HasColumnType("INT");

        builder.HasMany(x => x.Lectures)
            .WithOne()
            .HasForeignKey("LectureId")
            .HasConstraintName("FK_Module_LectureId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}