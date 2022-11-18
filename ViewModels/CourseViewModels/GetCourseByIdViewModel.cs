using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Models;
using NuGet.Packaging;

namespace CourseAppChallenge.ViewModels.CourseViewModels;

public class GetCourseByIdViewModel
{
    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Tag")] public string Tag { get; set; } = string.Empty;

    [Display(Name = "Summary")] public string Summary { get; set; } = string.Empty;

    [Display(Name = "Duration in minutes")] public int DurationInMinutes { get; set; }

    [Display(Name = "Modules")] public IList<CourseItem> CourseItems { get; set; } = new List<CourseItem>();

    public static implicit operator GetCourseByIdViewModel(Course course)
    {
        var getCourseByIdViewModel = new GetCourseByIdViewModel()
        {
            CourseTitle = course.CourseTitle,
            Tag = course.Tag,
            Summary = course.Summary,
            DurationInMinutes = course.DurationInMinutes,
            CourseItems = new List<CourseItem>()
        };

        getCourseByIdViewModel.CourseItems.AddRange(course.CourseItems);
        return getCourseByIdViewModel;
    }
}