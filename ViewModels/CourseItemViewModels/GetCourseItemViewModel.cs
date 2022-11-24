using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Models;

namespace CourseAppChallenge.ViewModels.CourseItemViewModels;

public class GetCourseItemViewModel
{
    public Guid CourseItemId { get; set; }

    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    [Display(Name = "Order")] public int Order { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    public static implicit operator GetCourseItemViewModel(CourseItem courseItem) => new()
    {
        CourseItemId = courseItem.CourseItemId,
        CourseItemTitle = courseItem.CourseItemTitle,
        Order = courseItem.Order,
        CourseTitle = courseItem.Course.CourseTitle
    };
}