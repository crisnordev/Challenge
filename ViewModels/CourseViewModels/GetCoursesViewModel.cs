using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels.CourseItemViewModels;

namespace CourseAppChallenge.ViewModels.CourseViewModels;

public class GetCoursesViewModel
{
    public IList<GetCourseViewModel> GetCoursesViewModelList { get; set; } = new List<GetCourseViewModel>();

    public static implicit operator GetCoursesViewModel(List<Course> courses)
    {
        var coursesViewModel = new GetCoursesViewModel();

        foreach (var item in courses)
        {
            coursesViewModel.GetCoursesViewModelList?.Add(item);
        }

        return coursesViewModel;
    }
}