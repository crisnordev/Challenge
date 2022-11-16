using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;
using NuGet.Packaging;

namespace courseappchallenge.ViewModels.CourseViewModels;

public class DeleteCourseViewModel
{
    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Tag")] public string Tag { get; set; } = string.Empty;

    [Display(Name = "Modules")] public IList<string> CourseItems { get; set; } = new List<string>();

    public static implicit operator DeleteCourseViewModel(Course course)
    {
        var deleteCourseViewModel = new DeleteCourseViewModel
        {
            CourseTitle = course.CourseTitle,
            Tag = course.Tag,
            CourseItems = new List<string>()
        };
        
        deleteCourseViewModel.CourseItems.AddRange(course.CourseItems.Select(x => x.CourseItemTitle));
        return deleteCourseViewModel;
    }
}