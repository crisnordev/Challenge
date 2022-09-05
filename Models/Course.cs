using System.ComponentModel.DataAnnotations;
using Challenge.Shared;

namespace Challenge.Models;

public class Course : Entity
{
    protected Course()
    {
        Modules = new List<Module>();
    }

    public Course(string title, string tag, string summary, int duration)
    {
        CourseId = Guid.NewGuid();
        CourseTitle = title;
        Tag = tag;
        Summary = summary;
        Duration = duration;
        Modules = new List<Module>();
    }

    [Display(Name = "Course Id")] public Guid CourseId { get; set; }
    
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

    public IEnumerable<Module> Modules { get; set; }
}