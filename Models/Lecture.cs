using System.ComponentModel.DataAnnotations;
using courseappchallenge.Shared;

namespace courseappchallenge.Models;

public class Lecture : Entity
{
    public Guid LectureId { get; set; } = Guid.NewGuid();

    public string LectureTitle { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = "https://www.";

    public Guid CourseItemId { get; set; } = Guid.NewGuid();

    public CourseItem CourseItem { get; set; } = default!;
}