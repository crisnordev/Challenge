using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace courseappchallenge.Pages.CourseItems;

public class CourseNamePageModel : PageModel
{
    public SelectList CourseNameSelectList { get; set; }

    public void PopulateCoursesDropDownList(ApplicationDbContext context, object selectedDepartment = null)
    {
        var coursesQuery = from item in context.Courses
            orderby item.CourseTitle
            select item;

        CourseNameSelectList = new SelectList(coursesQuery.AsNoTracking(),
            "CourseId", "CourseTitle", selectedDepartment);
    }
}