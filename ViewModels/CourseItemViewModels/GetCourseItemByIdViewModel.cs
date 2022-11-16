using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Models;

namespace CourseAppChallenge.ViewModels.CourseItemViewModels;

public class GetCourseItemByIdViewModel
{
    public Guid CourseItemId { get; set; }

    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    [Display(Name = "Order")] public int Order { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Lectures")] public IList<string> Lectures { get; set; } = new List<string>();

    public static implicit operator GetCourseItemByIdViewModel(CourseItem courseItem)
    {
        var item = new GetCourseItemByIdViewModel
        {
            CourseItemId = courseItem.CourseItemId,
            CourseItemTitle = courseItem.CourseItemTitle,
            Order = courseItem.Order,
            CourseTitle = courseItem.Course.CourseTitle,
            Lectures = new List<string>()
        };

        foreach (var lecture in courseItem.Lectures)
        {
            item.Lectures.Add(lecture.LectureTitle);
        }

        return item;
    }
}