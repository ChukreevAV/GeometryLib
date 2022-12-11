using GeometryLib;

using Microsoft.AspNetCore.Mvc;

using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController, Route("[controller]")]
    public class LineIntersectionsController
    {

        [HttpGet]
        public async Task<ActionResult<LineIntersectionsState>> GetLineIntersections()
        {
            var convexHull = new LineIntersectionsState(GetSampleData.GetRandomLine2ds(7));
            return await new ValueTask<ActionResult<LineIntersectionsState>>(convexHull);
        }
    }
}