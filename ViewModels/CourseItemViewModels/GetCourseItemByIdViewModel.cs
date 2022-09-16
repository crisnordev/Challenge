using System.ComponentModel.DataAnnotations;

namespace Challenge.ViewModels.CourseItemViewModels;

public class GetCourseItemByIdViewModel
{
    public GetCourseItemByIdViewModel()
    {
        Lectures = new Dictionary<Guid, string>();
    }

    public Guid CourseItemId { get; set; }

    [Display(Name = "Module")]
    public string CourseItemTitle { get; set; } = string.Empty;

    [Display(Name = "Order")]
    public int Order { get; set; }

    public Guid CourseId { get; set; }
    
    [Display(Name = "Course")] public string CourseTitle { get; set; }

    [Display(Name = "Lectures")] public Dictionary<Guid, string> Lectures { get; set; }
    
    public static implicit operator GetCourseItemByIdViewModel(CourseItem courseItem)
    {
        var item = new GetCourseItemByIdViewModel
        {
            CourseItemId = courseItem.CourseItemId,
            CourseItemTitle = courseItem.CourseItemTitle,
            Order = courseItem.Order,
            CourseId = courseItem.Course.CourseId,
            CourseTitle = courseItem.Course.CourseTitle
        };
        
        foreach (var lecture in courseItem.Lectures)
            item.Lectures.Add(lecture.LectureId, lecture.LectureTitle);
            
        return item;
    }
}