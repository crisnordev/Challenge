using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Pages.CourseItems;

public class CourseNamePageModel : PageModel
{
    public SelectList CourseNameSelectList { get; set; } = default!;

    public void PopulateCoursesDropDownList(ApplicationDbContext context, object? selectedCourse = null)
    {
        var coursesQuery = from item in context.Courses
            orderby item.CourseTitle
            select item;

        CourseNameSelectList = new SelectList(coursesQuery.AsNoTracking(),
            "CourseId", "CourseTitle", selectedCourse);
    }
}