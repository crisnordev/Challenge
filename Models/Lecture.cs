using System.ComponentModel.DataAnnotations;
using Challenge.Shared;

namespace Challenge.Models;

public class Lecture : Entity
{
    public Lecture() { }

    public Lecture(string title, string description, string videoUrl, CourseItem courseItem)
    {
        LectureId = Guid.NewGuid();
        LectureTitle = title;
        Description = description;
        VideoUrl = videoUrl;
        CourseItem = courseItem;
    }

    [Display(Name = "Lecture Id")] public Guid LectureId { get; set; }
    
    [Required(ErrorMessage = "Lecture title is required.")]
    [Display(Name = "Lecture")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Lecture title must have between 2 and 80 characters.")]
    public string LectureTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lecture description is required.")]
    [Display(Name = "Description")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Lecture description must have between 2 and 160 characters.")]
    public string Description { get; set; } = string.Empty;

    [Url]
    [Required]
    [Display(Name = "Url")]
    [StringLength(2046, MinimumLength = 10, ErrorMessage = "Lecture video Url must have between 10 and 2046 characters.")]
    public string VideoUrl { get; set; } = "https://www.";

    [Display(Name = "Module")] public CourseItem CourseItem { get; set; }
}