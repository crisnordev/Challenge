using System.ComponentModel.DataAnnotations;

namespace Challenge.ViewModels.CourseItemViewModels;

public class CreateCourseItemViewModel
{
    public CreateCourseItemViewModel() { }

    [Required(ErrorMessage = "Module title is required.")]
    [Display(Name = "Module title")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Module title must have between 2 and 80 characters.")]
    public string CourseItemTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Module order is required.")]
    [Display(Name = "Order")]
    [Range(1, 1000, ErrorMessage = "Module order must be between 1 and 1000.")]
    public int Order { get; set; }

    public bool ExistingOrder { get; set; }

    public Guid CourseId { get; set; }
    
    public static implicit operator CreateCourseItemViewModel(CourseItem courseItem) => new()
    {
        CourseItemTitle = courseItem.CourseItemTitle,
        Order = courseItem.Order
    };
    
    public static implicit operator CourseItem(CreateCourseItemViewModel createCourseItem) => new()
    {
        CourseItemTitle = createCourseItem.CourseItemTitle,
        Order = createCourseItem.Order
    };
}