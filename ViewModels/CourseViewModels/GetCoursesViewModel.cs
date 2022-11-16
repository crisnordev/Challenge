using courseappchallenge.Models;
using courseappchallenge.ViewModels.CourseItemViewModels;

namespace courseappchallenge.ViewModels.CourseViewModels;

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