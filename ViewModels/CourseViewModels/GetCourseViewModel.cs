using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Models;

namespace CourseAppChallenge.ViewModels.CourseViewModels;

public class GetCourseViewModel
{
    public Guid CourseId { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Tag")] public string Tag { get; set; } = string.Empty;

    [Display(Name = "DurationInMinutes")] public int Duration { get; set; }

    public static implicit operator GetCourseViewModel(Course course) => new()
    {
        CourseId = course.CourseId,
        CourseTitle = course.CourseTitle,
        Tag = course.Tag,
        Duration = course.DurationInMinutes
    };
}