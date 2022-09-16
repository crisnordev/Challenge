using System.ComponentModel.DataAnnotations;

namespace Challenge.ViewModels.CourseViewModels;

public class GetCoursesViewModel
{
    public GetCoursesViewModel() { }

    public Guid CourseId { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; }

    [Display(Name = "Tag")] public string Tag { get; set; }

    public static implicit operator GetCoursesViewModel(Course course) => new() 
    {
        CourseId = course.CourseId,
        CourseTitle = course.CourseTitle,
        Tag = course.Tag
    };
}