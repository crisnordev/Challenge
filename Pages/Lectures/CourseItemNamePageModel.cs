using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Pages.Lectures;

public class CourseItemNamePageModel : PageModel
{
    public SelectList CourseItemNameSelectList { get; set; } = default!;

    public void PopulateCourseItemsDropDownList(ApplicationDbContext context, object? selectedCourseItem = null)
    {
        var coursesQuery = from item in context.CourseItems
            orderby item.CourseItemTitle
            select item;

        CourseItemNameSelectList = new SelectList(coursesQuery.AsNoTracking(),
            "CourseItemId", "CourseItemTitle", selectedCourseItem);
    }
}