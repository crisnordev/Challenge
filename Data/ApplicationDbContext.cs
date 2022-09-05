using Challenge.Models;
using Challenge.Data.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<Course> Courses { get; set; }

    public DbSet<Module> Modules { get; set; }

    public DbSet<Lecture> Lectures { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ModuleMap());
        modelBuilder.ApplyConfiguration(new LectureMap());
        modelBuilder.ApplyConfiguration(new CourseMap());
        base.OnModelCreating(modelBuilder);
    }
}
