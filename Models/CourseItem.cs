using CourseAppChallenge.Shared;

namespace CourseAppChallenge.Models;

public class CourseItem : Entity
{
    public Guid CourseItemId { get; set; } = Guid.NewGuid();

    public string CourseItemTitle { get; set; } = string.Empty;

    public int Order { get; set; }

    public Guid CourseId { get; set; } = Guid.NewGuid();

    public Course Course { get; set; } = default!;

    public IEnumerable<Lecture> Lectures { get; set; } = new List<Lecture>();
}