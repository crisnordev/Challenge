using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;

namespace courseappchallenge.ViewModels.LectureViewModels;

public class EditLectureViewModel
{
    public EditLectureViewModel() { }
    
    [Required(ErrorMessage = "Lecture title is required.")]
    [Display(Name = "Lecture title")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Lecture title must have between 2 and 80 characters.")]
    public string LectureTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lecture description is required.")]
    [Display(Name = "Description")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Lecture description must have between 2 and 160 characters.")]
    public string Description { get; set; } = string.Empty;

    [Url]
    [Required(ErrorMessage = "Lecture video Url is required.")]
    [Display(Name = "Url")]
    [StringLength(2046, MinimumLength = 10, ErrorMessage = "Lecture video Url must have between 10 and 2046 characters.")]
    public string VideoUrl { get; set; } = "https://www.";
    
    public static implicit operator EditLectureViewModel(Lecture lecture) => new()
    {
        LectureTitle = lecture.LectureTitle,
        Description = lecture.Description,
        VideoUrl = lecture.VideoUrl,
    };
    
    public static implicit operator Lecture(EditLectureViewModel editLectureViewModel) => new()
    {
        LectureTitle = editLectureViewModel.LectureTitle,
        Description = editLectureViewModel.Description,
        VideoUrl = editLectureViewModel.VideoUrl,
    };
}