using System.ComponentModel.DataAnnotations;

namespace Challenge.ViewModels.CourseItemViewModels;

public class GetCourseItemsViewModel
{
    public GetCourseItemsViewModel() { }

    public Guid CourseItemId { get; set; }

    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    public int Order { get; set; }
    
    [Display(Name = "Course")] public string CourseTitle { get; set; }

    public static implicit operator GetCourseItemsViewModel(CourseItem courseItem) => new()
    {
        CourseItemId = courseItem.CourseItemId,
        CourseItemTitle = courseItem.CourseItemTitle,
        Order = courseItem.Order,
        CourseTitle = courseItem.Course.CourseTitle
    };
}