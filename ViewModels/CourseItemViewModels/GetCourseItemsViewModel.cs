using System.ComponentModel.DataAnnotations;

namespace courseappchallenge.ViewModels.CourseItemViewModels;

public class GetCourseItemsViewModel
{
    public GetCourseItemsViewModel() { }

    public Guid CourseItemId { get; set; }

    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    [Display(Name = "Order")] public int Order { get; set; }
    
    [Display(Name = "Course")] public string CourseTitle { get; set; }

    public static implicit operator GetCourseItemsViewModel(CourseItem courseItem) => new()
    {
        CourseItemId = courseItem.CourseItemId,
        CourseItemTitle = courseItem.CourseItemTitle,
        Order = courseItem.Order,
        CourseTitle = courseItem.Course.CourseTitle
    };
}