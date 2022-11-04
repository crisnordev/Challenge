using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Models;

namespace CourseAppChallenge.ViewModels.LectureViewModels;

public class GetLectureByIdViewModel
{
    public GetLectureByIdViewModel() { }

    public Guid LectureId { get; set; }

    [Display(Name = "Lecture")] public string LectureTitle { get; set; }

    [Display(Name = "Description")] public string Description { get; set; }

    [Display(Name = "Url")] public string VideoUrl { get; set; }

    public Guid CourseItemId { get; set; }

    [Display(Name = "Module")] public string CourseItemTitle { get; set; }

    public static implicit operator GetLectureByIdViewModel(Lecture lecture) => new()
    {
        LectureId = lecture.LectureId,
        LectureTitle = lecture.LectureTitle,
        Description = lecture.Description,
        VideoUrl = lecture.VideoUrl,
        CourseItemId = lecture.CourseItem.CourseItemId,
        CourseItemTitle = lecture.CourseItem.CourseItemTitle
    };
}

