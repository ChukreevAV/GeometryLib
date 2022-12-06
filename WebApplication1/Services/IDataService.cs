using GeometryLib.Geometry;

namespace WebApplication1.Services
{
    public interface IDataService
    {
        List<Point2d> GetPoints();

        List<Point2d> GetNewPoints();
    }
}