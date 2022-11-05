using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Models;
using NuGet.Packaging;

namespace CourseAppChallenge.ViewModels.CourseViewModels;

public class GetCourseByIdViewModel
{
    public Guid CourseId { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Tag")] public string Tag { get; set; } = string.Empty;

    [Display(Name = "Summary")] public string Summary { get; set; } = string.Empty;

    [Display(Name = "DurationInMinutes")] public int Duration { get; set; }

    [Display(Name = "Modules")] public IList<string> CourseItems { get; set; } = new List<string>();

    public static implicit operator GetCourseByIdViewModel(Course course)
    {
        var getCourseByIdViewModel = new GetCourseByIdViewModel()
        {
            CourseId = course.CourseId,
            CourseTitle = course.CourseTitle,
            Tag = course.Tag,
            Summary = course.Summary,
            Duration = course.DurationInMinutes,
            CourseItems = new List<string>()
        };

        getCourseByIdViewModel.CourseItems.AddRange(course.CourseItems.Select(x => x.CourseItemTitle));
        return getCourseByIdViewModel;
    }
}