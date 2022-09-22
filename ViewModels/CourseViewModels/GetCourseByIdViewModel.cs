using System.ComponentModel.DataAnnotations;

namespace courseappchallenge.ViewModels.CourseViewModels;

public class GetCourseByIdViewModel
{
    public Guid CourseId { get; set; }

    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    [Display(Name = "Tag")] public string Tag { get; set; } = string.Empty;

    [Display(Name = "Summary")] public string Summary { get; set; } = string.Empty;

    [Display(Name = "Duration")] public int Duration { get; set; }

    [Display(Name = "Modules")] public List<string> CourseItems { get; set; } = new(); 

    public static implicit operator GetCourseByIdViewModel(Course course)
    {
        var courseView = new GetCourseByIdViewModel
        {
            CourseId = course.CourseId,
            CourseTitle = course.CourseTitle,
            Tag = course.Tag,
            Summary = course.Summary,
            Duration = course.Duration,
            CourseItems = new List<string>()
        };

        courseView.CourseItems.AddRange(course.CourseItems.Select(x => x.CourseItemTitle));
        
        return courseView;
    }
}