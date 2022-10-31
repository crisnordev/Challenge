using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;

namespace courseappchallenge.ViewModels.CourseViewModels;

public class GetCourseByIdViewModel
{
    public Guid CourseId { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Tag")] public string Tag { get; set; } = string.Empty;

    [Display(Name = "Summary")] public string Summary { get; set; } = string.Empty;

    [Display(Name = "Duration")] public int Duration { get; set; }

    public static implicit operator GetCourseByIdViewModel(Course course) => new()
    {
        CourseId = course.CourseId,
        CourseTitle = course.CourseTitle,
        Tag = course.Tag,
        Summary = course.Summary,
        Duration = course.Duration
    };
}