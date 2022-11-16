using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;

namespace courseappchallenge.ViewModels.LectureViewModels;

public class DeleteLectureViewModel
{
    [Display(Name = "Lecture")] public string LectureTitle { get; set; } = string.Empty;

    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = default!;

    public static implicit operator DeleteLectureViewModel(Lecture lecture) => new()
    {
        LectureTitle = lecture.LectureTitle,
        CourseItemTitle = lecture.CourseItem.CourseItemTitle
    };
}