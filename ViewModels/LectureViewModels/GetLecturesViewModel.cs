using CourseAppChallenge.Models;

namespace CourseAppChallenge.ViewModels.LectureViewModels;

public class GetLecturesViewModel
{
    public IList<GetLectureViewModel> GetLecturesViewModelList { get; set; } = new List<GetLectureViewModel>();

    public static implicit operator GetLecturesViewModel(List<Lecture> lectures)
    {
        var getLecturesViewModel = new GetLecturesViewModel();

        foreach (var lecture in lectures)
        {
            getLecturesViewModel.GetLecturesViewModelList.Add(lecture);
        }

        return getLecturesViewModel;
    }
}