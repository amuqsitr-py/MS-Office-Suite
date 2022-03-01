using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace LikePaint
{
    class MyTriangle : Shapes
    {
        public Polygon Triangle { get; private set; }

        public MyTriangle(Point location)
        {
            SolidColorBrush myTriangleSolidColor = new SolidColorBrush();
            SolidColorBrush myTriangleOutlineColor = new SolidColorBrush();

            Point point1 = new Point(location.X, location.Y);
            Point point2 = new Point(location.X - 50, location.Y + 100);
            Point point3 = new Point(location.X + 50, location.Y + 100);

            PointCollection polygonPoints = new PointCollection();
            polygonPoints.Add(point1);
            polygonPoints.Add(point2);
            polygonPoints.Add(point3);

            myTriangleSolidColor.Color = Color.FromRgb(235, 245, 248);
            myTriangleOutlineColor.Color = Color.FromRgb(102, 121, 143);

            Triangle = new Polygon
            {
                Stroke = myTriangleOutlineColor,
                StrokeThickness = 2,
                Fill = myTriangleSolidColor,
                Points = polygonPoints,
                Stretch = Stretch.Uniform
            };

        }

        public void Draw(double locationX, double locationY)
        {
            Canvas.SetLeft(Triangle, locationX);
            Canvas.SetTop(Triangle, locationY);
        }
    }
}
