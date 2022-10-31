using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;

namespace courseappchallenge.ViewModels.CourseViewModels;

public class DeleteCourseByIdViewModel
{
    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Tag")] public string Tag { get; set; } = string.Empty;

    [Display(Name = "Duration")] public int Duration { get; set; }

    public static implicit operator DeleteCourseByIdViewModel(Course course) => new()  
    {
        CourseTitle = course.CourseTitle,
        Tag = course.Tag,
        Duration = course.Duration
    };
}