using CourseAppChallenge.Models;

namespace CourseAppChallenge.ViewModels.CourseItemViewModels;

public class GetCourseItemsViewModel
{
    public IList<GetCourseItemViewModel>? CourseItemsViewModelList { get; set; } = new List<GetCourseItemViewModel>();

    public static implicit operator GetCourseItemsViewModel(List<CourseItem> courseItems)
    {
        var courseItemsViewModel = new GetCourseItemsViewModel();

        foreach (var item in courseItems)
        {
            courseItemsViewModel.CourseItemsViewModelList?.Add(item);
        }

        return courseItemsViewModel;
    }
}