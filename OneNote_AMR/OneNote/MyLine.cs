using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LikePaint
{
    class MyLine : Shapes
    {
        public Line Line { get; private set; }

        public MyLine(Point location)
        {
            Line = new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = location.X,
                X2 = location.X + 100,
                Y1 = location.Y,
                Y2 = location.Y + 100,
                Stretch = Stretch.Uniform
            };
        }

        public void Draw(double locationX, double locaitonY)
        {
            Canvas.SetLeft(Line, locationX);
            Canvas.SetTop(Line, locaitonY);
        }
    }
}
