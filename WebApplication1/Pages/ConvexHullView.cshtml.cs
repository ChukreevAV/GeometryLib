using GeometryLib.Geometry;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class ConvexHullViewModel : PageModel
    {

        public List<Point2d> Points { get; set; } = new();

        public List<Point2d> SelectPoints { get; } = new();

        public List<Line2d> UnselectLines { get; } = new();

        public List<Line2d> Lines { get; set; } = new();

        public Line2d? CurrentLine { get; set; }

        public int Index1 { get; set; } = 0;

        public int Index2 { get; set; } = 0;

        private readonly ISlowConvexHullService _convexHull;

        public ConvexHullViewModel(ISlowConvexHullService dataService)
        {
            _convexHull = dataService;
            Points.AddRange(_convexHull.Points);
            Index1 = _convexHull.Index1;
            Index2 = _convexHull.Index2;
            CurrentLine = _convexHull.CurrentLine;
            SelectPoints.Clear();
            SelectPoints.AddRange(_convexHull.SelectPoints);
            UnselectLines.Clear();
            UnselectLines.AddRange(_convexHull.UnselectLines);
            Lines.Clear();
            Lines.AddRange(_convexHull.ConvexHull);
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _convexHull.Next();

            return RedirectToAction("Get");
        }
    }
}