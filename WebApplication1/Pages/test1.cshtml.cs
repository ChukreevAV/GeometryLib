using GeometryLib.Geometry;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Globalization;

using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class Test1Model : PageModel
    {
        private IDataService _dataService;

        public List<Point2d> Points { get; set; } = new();

        public Test1Model(IDataService dataService)
        {
            _dataService = dataService;

            var ps = _dataService.GetPoints();
            ps = ps
                .OrderBy(p => p.Y)
                .ThenBy(p => p.X)
                .ToList();

            Points.AddRange(ps);
        }

        public string Scale(double d) => d.ToString(CultureInfo.InvariantCulture);

        public void OnGet()
        {
        }

        public void CreateNewPoints()
        {
            var ps = _dataService.GetNewPoints();
            ps = ps
                .OrderBy(p => p.Y)
                .ThenBy(p => p.X)
                .ToList();

            Points.Clear();
            Points.AddRange(ps);
        }


        public IActionResult OnPost()
        {
            CreateNewPoints();
            return RedirectToAction("Get");
        }
    }
}