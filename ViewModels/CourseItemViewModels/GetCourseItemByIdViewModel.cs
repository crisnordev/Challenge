using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Models;
using NuGet.Packaging;

namespace CourseAppChallenge.ViewModels.CourseItemViewModels;

public class GetCourseItemByIdViewModel
{
    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    [Display(Name = "Order")] public int Order { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Lectures")] public IList<Lecture> Lectures { get; set; } = new List<Lecture>();

    public Guid CourseId { get; set; }

    public static implicit operator GetCourseItemByIdViewModel(CourseItem courseItem)
    {
        var item = new GetCourseItemByIdViewModel
        {
            CourseItemTitle = courseItem.CourseItemTitle,
            Order = courseItem.Order,
            CourseTitle = courseItem.Course.CourseTitle,
            Lectures = new List<Lecture>(),
            CourseId = courseItem.CourseId
        };

        item.Lectures.AddRange(courseItem.Lectures);
        return item;
    }
}