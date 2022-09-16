using System.ComponentModel.DataAnnotations;

namespace Challenge.ViewModels.CourseViewModels;

public class GetCourseByIdViewModel
{
    public GetCourseByIdViewModel()
    {
        CourseItems = new Dictionary<Guid, string>();
    }

    public Guid CourseId { get; set; }
    
    [Display(Name = "Course")] public string CourseTitle { get; set; }

    [Display(Name = "Tag")] public string Tag { get; set; }

    [Display(Name = "Summary")] public string Summary { get; set; } = string.Empty;

    [Display(Name = "Duration")] public int Duration { get; set; }
    
    [Display(Name = "Modules")] public Dictionary<Guid, string> CourseItems { get; set; } = null!;

    public static implicit operator GetCourseByIdViewModel(Course course)
    {
        var courseView = new GetCourseByIdViewModel
        {
            CourseId = course.CourseId,
            CourseTitle = course.CourseTitle,
            Tag = course.Tag,
            Summary = course.Summary,
            Duration = course.Duration
        };
        
        foreach (var courseItem in course.CourseItems)
            courseView.CourseItems.Add(courseItem.CourseItemId, courseItem.CourseItemTitle);

        return courseView;
    }
}