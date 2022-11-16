using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Shared;

namespace CourseAppChallenge.ViewModels;

public class CourseItem : Entity
{
    public Guid CourseItemId { get; set; } = Guid.NewGuid();

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

    public int Order { get; set; }

    public Guid CourseId { get; set; } = Guid.NewGuid();

    public Course Course { get; set; } = default!;

    public IEnumerable<Lecture> Lectures { get; set; } = new List<Lecture>();
}