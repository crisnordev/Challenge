using System.ComponentModel.DataAnnotations;

namespace Challenge.ViewModels.CourseViewModels;

public class EditCourseViewModel
{
    public EditCourseViewModel()
    {
        CourseItems = new Dictionary<Guid, string>();
    }
    
    [Required(ErrorMessage = "Course title is required.")]
    [Display(Name = "Course title")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Course title must have between 2 and 80 characters.")]
    public string CourseTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Course tag is required.")]
    [Display(Name = "Tag")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "Course tag must have 4 characters.")]
    public string Tag { get; set; }
    
    [Required(ErrorMessage = "Course summary is required.")]
    [Display(Name = "Summary")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Course summary must have between 2 and 160 characters.")]
    public string Summary { get; set; } = string.Empty;

    [Required(ErrorMessage = "Module duration is required.")]
    [Display(Name = "Duration")]
    [Range(1, 1000, ErrorMessage = "Module duration must be between 1 and 1000.")]
    public int Duration { get; set; }

    public Dictionary<Guid, string> CourseItems { get; set; } 

    public static implicit operator EditCourseViewModel(Course course)
    {
        var editCourse = new EditCourseViewModel
        {
            CourseTitle = course.CourseTitle,
            Tag = course.Tag,
            Summary = course.Summary,
            Duration = course.Duration,
            CourseItems = new Dictionary<Guid, string>()
        };
        
        foreach (var item in course.CourseItems)
            editCourse.CourseItems.Add(item.CourseItemId, item.CourseItemTitle);
        
        return editCourse;
    }

    public static implicit operator Course(EditCourseViewModel editCourseView) => new()
    {
        CourseTitle = editCourseView.CourseTitle,
        Tag = editCourseView.Tag,
        Summary = editCourseView.Summary,
        Duration = editCourseView.Duration
    };
}