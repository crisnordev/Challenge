using courseappchallenge.Models;

namespace CourseAppChallenge.ViewModels.LectureViewModels;

public class GetLecturesViewModel
{
    public GetLecturesViewModel()
    {
    }

    public Guid LectureId { get; set; }

    [Display(Name = "Lecture")] public string LectureTitle { get; set; }

    [Display(Name = "Module")] public string CourseItemTitle { get; set; }

    public static implicit operator GetLecturesViewModel(Lecture lecture) => new()
    {
        var getLecturesViewModel = new GetLecturesViewModel();

        foreach (var lecture in lectures)
        {
            getLecturesViewModel.GetLecturesViewModelList.Add(lecture);
        }

        return getLecturesViewModel;
    }
}