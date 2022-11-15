using System.ComponentModel.DataAnnotations;
using courseappchallenge.Shared;

namespace courseappchallenge.Models;

public class Course : Entity
{
    public Guid CourseId { get; set; } = Guid.NewGuid();

    public string CourseTitle { get; set; } = string.Empty;

    public string Tag { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public int DurationInMinutes { get; set; }

    public IEnumerable<CourseItem> CourseItems { get; set; } = new List<CourseItem>();
}