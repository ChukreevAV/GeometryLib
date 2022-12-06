using GeometryLib.Geometry;

using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Globalization;

namespace WebApplication1.Pages
{
    public class test1Model : PageModel
    {
        public static List<Point2d> GetRandomPoint2ds(int count)
        {
            var random = new Random();
            var list1 = new List<Point2d>();
            for (var i = 0; i < count; i++)
            {
                list1.Add(new Point2d(
                    random.NextDouble(),
                    random.NextDouble()
                ));
            }

            return list1;
        }

        public List<Point2d> Points { get; set; }

        public string Scale(double d)
        {
            return (d).ToString(CultureInfo.InvariantCulture);
        }

        public void OnCircleClick(ChangeEventArgs e)
        {
            //return null;
        }

       public void OnGet()
        {
            Points = GetRandomPoint2ds(20);
        }
    }
}