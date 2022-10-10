using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Shared;

namespace CourseAppChallenge.ViewModels;

public class CourseItem : Entity
{
    public CourseItem()
    {
        Lectures = new List<Lecture>();
    }

    public CourseItem(string title, int order, Course course)
    {
        CourseItemId = Guid.NewGuid();
        CourseItemTitle = title;
        Order = order;
        Course = course;
        Lectures = new List<Lecture>();
    }

    [Display(Name = "Module Id")] public Guid CourseItemId { get; set; }

    [Required(ErrorMessage = "Module title is required.")]
    [Display(Name = "Module")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Module title must have between 2 and 80 characters.")]
    public string CourseItemTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Module order is required.")]
    [Display(Name = "Order")]
    [Range(1, 1000, ErrorMessage = "Module order must be between 1 and 1000.")]
    public int Order { get; set; }

    [Display(Name = "Course")] public Course Course { get; set; }

    [Display(Name = "Lectures")] public IEnumerable<Lecture> Lectures { get; set; }
}