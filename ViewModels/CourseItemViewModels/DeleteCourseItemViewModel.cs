using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Models;

namespace CourseAppChallenge.ViewModels.CourseItemViewModels;

public class DeleteCourseItemViewModel
{
    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    [Display(Name = "Order")] public int Order { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    public static implicit operator DeleteCourseItemViewModel(CourseItem courseItem) => new()
    {
        CourseItemTitle = courseItem.CourseItemTitle,
        Order = courseItem.Order,
        CourseTitle = courseItem.Course.CourseTitle
    };
}