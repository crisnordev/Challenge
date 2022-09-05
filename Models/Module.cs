using System.ComponentModel.DataAnnotations;
using Challenge.Shared;

namespace Challenge.Models;

public class Module : Entity
{
    public Module()
    {
        Lectures = new List<Lecture>();
    }

    public Module(string title ,int order)
    {
        ModuleId = Guid.NewGuid();
        ModuleTitle = title;
        Order = order;
        Lectures = new List<Lecture>();
    }

    [Display(Name = "Module Id")] public Guid ModuleId { get; set; }

    [Required(ErrorMessage = "Module title is required.")]
    [Display(Name = "Module title")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Module title must have between 2 and 80 characters.")]
    public string ModuleTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Module order is required.")]
    [Display(Name = "Order")]
    [Range(1, 1000, ErrorMessage = "Module order must be between 1 and 1000.")]
    public int Order { get; set; }

    [Display(Name = "CourseId")] public Course Course { get; set; }

    public IEnumerable<Lecture> Lectures { get; set; }
}