using courseappchallenge.Models;
using courseappchallenge.ViewModels.CourseItemViewModels;

namespace CourseAppChallenge.ViewModels.CourseViewModels;

public class GetCoursesViewModel
{
    public IList<GetCourseViewModel> GetCoursesViewModelList { get; set; } = new List<GetCourseViewModel>();

    [Display(Name = "Course")] public string CourseTitle { get; set; }

    [Display(Name = "Tag")] public string Tag { get; set; }

    public static implicit operator GetCoursesViewModel(Course course) => new()
    {
        var coursesViewModel = new GetCoursesViewModel();

        foreach (var item in courses)
        {
            coursesViewModel.GetCoursesViewModelList?.Add(item);
        }

        return coursesViewModel;
    }
}