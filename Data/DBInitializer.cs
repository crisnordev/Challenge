using CourseAppChallenge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context, RoleManager<AppRole> roleManager)
    {
        if (context.Courses.AsNoTracking().Any())
        {
            return;
        }

        var courses = new List<Course>
        {
            new()
            {
                CourseTitle = "C# course.",
                Tag = "1234",
                Summary = "In this course you will learn C#.",
                DurationInMinutes = 120
            },

            new()
            {
                CourseTitle = ".NET course.",
                Tag = "1212",
                Summary = "In this course you will learn .NET.",
                DurationInMinutes = 160
            },

            new()
            {
                CourseTitle = "ASP.NET course.",
                Tag = "1112",
                Summary = "In this course you will learn ASP.NET.",
                DurationInMinutes = 160
            }
        };

        context.Courses.AddRange(courses);

        var cSharpCourse = courses[0];
        var dotNetCourse = courses[1];
        var aspNetCourse = courses[2];

        var courseItems = new List<CourseItem>
        {
            new()
            {
                CourseItemTitle = "C# fundamentals.",
                Order = 1,
                CourseId = cSharpCourse.CourseId,
                Course = cSharpCourse
            },

            new()
            {
                CourseItemTitle = "Classes.",
                Order = 2,
                CourseId = cSharpCourse.CourseId,
                Course = cSharpCourse
            },

            new()
            {
                CourseItemTitle = "Methods.",
                Order = 3,
                CourseId = cSharpCourse.CourseId,
                Course = cSharpCourse
            },

            new()
            {
                CourseItemTitle = ".NET fundamentals.",
                Order = 1,
                CourseId = dotNetCourse.CourseId,
                Course = dotNetCourse
            },

            new()
            {
                CourseItemTitle = "Data access.",
                Order = 2,
                CourseId = dotNetCourse.CourseId,
                Course = dotNetCourse
            },

            new()
            {
                CourseItemTitle = "ASP.NET ModelBinder.",
                Order = 1,
                CourseId = aspNetCourse.CourseId,
                Course = aspNetCourse
            }
        };

        context.CourseItems.AddRange(courseItems);

        var cSharpModule = courseItems[0];
        var classesModule = courseItems[1];
        var methodsModule = courseItems[2];
        var dotNetModule = courseItems[3];

        var lectures = new List<Lecture>
        {
            new()
            {
                LectureTitle = "C# runtime",
                Description = "In this lecture you will learn about C# runtime.",
                VideoUrl = "https://learn.microsoft.com/",
                CourseItemId = cSharpModule.CourseItemId,
                CourseItem = cSharpModule
            },

            new()
            {
                LectureTitle = "C# SDK",
                Description = "In this lecture you will learn about C# SDK.",
                VideoUrl = "https://learn.microsoft.com/",
                CourseItemId = cSharpModule.CourseItemId,
                CourseItem = cSharpModule
            },

            new()
            {
                LectureTitle = "Strings",
                Description = "In this lecture you will learn about strings.",
                VideoUrl = "https://learn.microsoft.com/",
                CourseItemId = classesModule.CourseItemId,
                CourseItem = classesModule
            },

            new()
            {
                LectureTitle = "ToString",
                Description = "In this lecture you will learn about ToString method.",
                VideoUrl = "https://learn.microsoft.com/",
                CourseItemId = methodsModule.CourseItemId,
                CourseItem = methodsModule
            },

            new()
            {
                LectureTitle = ".NET CLI",
                Description = "In this lecture you will learn about .NET CLI.",
                VideoUrl = "https://learn.microsoft.com/",
                CourseItemId = dotNetModule.CourseItemId,
                CourseItem = dotNetModule
            },

            new()
            {
                LectureTitle = "Entity Framework Core",
                Description = "In this lecture you will learn about Entity Framework Core.",
                VideoUrl = "https://learn.microsoft.com/",
                CourseItemId = dotNetModule.CourseItemId,
                CourseItem = dotNetModule
            }
        };

        context.Lectures.AddRange(lectures);
        
        var roleAdmin = new AppRole { Name = "Administrator" };
        var roleStudent = new AppRole { Name = "Student" };

        roleManager.CreateAsync(roleAdmin);
        roleManager.CreateAsync(roleStudent);

        context.SaveChanges();
    }
}