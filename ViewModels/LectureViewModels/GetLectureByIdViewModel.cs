using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;

namespace courseappchallenge.ViewModels.LectureViewModels;

public class GetLectureByIdViewModel
{
    public Guid LectureId { get; set; } = Guid.Empty;

    [Display(Name = "Lecture")] public string LectureTitle { get; set; } = string.Empty;

    [Display(Name = "Description")] public string Description { get; set; } = string.Empty;

    [Display(Name = "Url")] public string VideoUrl { get; set; } = string.Empty;

    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = default!;

    public static implicit operator GetLectureByIdViewModel(Lecture lecture) => new()
    {
        LectureId = lecture.LectureId,
        LectureTitle = lecture.LectureTitle,
        Description = lecture.Description,
        VideoUrl = lecture.VideoUrl,
        CourseItemTitle = lecture.CourseItem.CourseItemTitle
    };
}