using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;

namespace courseappchallenge.ViewModels.LectureViewModels;

public class GetLecturesViewModel
{
    public GetLecturesViewModel() { }

    public Guid LectureId { get; set; }

    [Display(Name = "Lecture")] public string LectureTitle { get; set; }
    
    [Display(Name = "Module")] public string CourseItemTitle { get; set; }

    public static implicit operator GetLecturesViewModel(Lecture lecture) => new()
    {
        LectureId = lecture.LectureId,
        LectureTitle = lecture.LectureTitle,
        CourseItemTitle = lecture.CourseItem.CourseItemTitle
    };
}