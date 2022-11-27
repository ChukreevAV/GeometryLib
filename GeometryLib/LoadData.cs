using GeometryLib.Geometry;

using System.Globalization;
using System.Text.RegularExpressions;

namespace GeometryLib
{
    public static class LoadData
    {
        private const string Pattern = "(?<x1>\\d+\\.\\d+) (?<y1>\\d+\\.\\d+);(?<x2>\\d+\\.\\d+) (?<y2>\\d+\\.\\d+)";
        private static readonly Regex RegexLine = new(Pattern);

        public static Line2d? ParseLine(string text)
        {
            if (!RegexLine.IsMatch(text)) return null;
            var m = RegexLine.Match(text);
            var x1 = double.Parse(m.Groups["x1"].Value, CultureInfo.InvariantCulture);
            var y1 = double.Parse(m.Groups["y1"].Value, CultureInfo.InvariantCulture);
            var x2 = double.Parse(m.Groups["x2"].Value, CultureInfo.InvariantCulture);
            var y2 = double.Parse(m.Groups["y2"].Value, CultureInfo.InvariantCulture);
            return new Line2d(x1, y1, x2, y2);
        }

        public static List<Line2d> ReadLines(string path) 
            => File.ReadAllLines(path)
                .Select(ParseLine)
                .Where(line => line != null)
                .ToList()!;
    }
}