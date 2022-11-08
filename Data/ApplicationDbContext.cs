using CourseAppChallenge.Data.Mappings;
using CourseAppChallenge.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;

namespace CourseAppChallenge.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }

    public DbSet<CourseItem> CourseItems { get; set; }

    public DbSet<Lecture> Lectures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseItemMap());
        modelBuilder.ApplyConfiguration(new LectureMap());
        modelBuilder.ApplyConfiguration(new CourseMap());
        base.OnModelCreating(modelBuilder);
    }
}