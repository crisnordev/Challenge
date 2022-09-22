using System.ComponentModel.DataAnnotations;
using NuGet.Packaging;

namespace courseappchallenge.ViewModels.CourseItemViewModels;

public class GetCourseItemByIdViewModel
{
    public Guid CourseItemId { get; set; }

    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    [Display(Name = "Order")] public int Order { get; set; }

    public Guid CourseId { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Lectures")] public List<string> Lectures { get; set; } = new();
    
    public static implicit operator GetCourseItemByIdViewModel(CourseItem courseItem)
    {
        var item = new GetCourseItemByIdViewModel
        {
            CourseItemId = courseItem.CourseItemId,
            CourseItemTitle = courseItem.CourseItemTitle,
            Order = courseItem.Order,
            CourseId = courseItem.Course.CourseId,
            CourseTitle = courseItem.Course.CourseTitle,
            Lectures = new List<string>()
        };

        item.Lectures.AddRange(courseItem.Lectures.Select(x => x.LectureTitle));
            
        return item;
    }
}