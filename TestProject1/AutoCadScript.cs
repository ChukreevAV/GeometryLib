using GeometryLib;

using System.Globalization;
using System.Text;

namespace TestProject1
{
    /// <summary>Создаём скрипт (.scr) для AutoCad</summary>
    public class AutoCadScript
    {
        /// <summary>Здесь храним скрипт</summary>
        private StringBuilder _stringBuilder = new();

        /// <summary>Добавляем точки</summary>
        /// <param name="points"></param>
        public void AddPoints(List<Point2d> points)
        {
            foreach (var p in points)
            {
                _stringBuilder.AppendLine($"(command \"_point\" '({p}))");
            }
        }

        /// <summary>Добавляем полилинию</summary>
        /// <param name="hull">Список точки полилинии</param>
        public void AddPolyline(List<Point2d> hull)
        {
            _stringBuilder.AppendLine("_pline");
            foreach (var p in hull)
            {
                _stringBuilder.AppendLine(
                    p.X.ToString(CultureInfo.InvariantCulture) + "," + 
                    p.Y.ToString(CultureInfo.InvariantCulture));
            }

            _stringBuilder.AppendLine(" ");
            _stringBuilder.AppendLine(" ");
        }

        public void AddLines(List<Line2d> lines)
        {
            foreach (var l in lines)
            {
                _stringBuilder.AppendLine($"(command \"_line\" '({l.Start}) '({l.End}) \"\")");
            }
        }

        public void WriteFile(string path)
        {
            File.WriteAllText(path, _stringBuilder.ToString());
        }
    }
}