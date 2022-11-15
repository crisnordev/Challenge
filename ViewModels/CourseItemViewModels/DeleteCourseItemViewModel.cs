using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;

namespace courseappchallenge.ViewModels.CourseItemViewModels;

public class DeleteCourseItemViewModel
{
    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Lectures")] public IList<string> Lectures { get; set; } = new List<string>();

    public static implicit operator DeleteCourseItemViewModel(CourseItem courseItem)
    {
        var item = new DeleteCourseItemViewModel
        {
            CourseItemTitle = courseItem.CourseItemTitle,
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