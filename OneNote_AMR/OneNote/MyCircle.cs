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
    public class MyCircle : Shapes
    {
        public Ellipse Circle { get; private set; }

        public MyCircle()
        {
            SolidColorBrush myCircleSolidColor = new SolidColorBrush();
            SolidColorBrush myCircleOutlineColor = new SolidColorBrush();

            myCircleSolidColor.Color = Color.FromRgb(235, 245, 248);
            myCircleOutlineColor.Color = Color.FromRgb(102, 121, 143);

            Circle = new Ellipse
            {
                Stroke = myCircleOutlineColor,
                StrokeThickness = 2,
                Fill = myCircleSolidColor,
                Width = 100,
                Height = 100
            };
        }

        public void Draw(double locationX, double locationY)
        {
            if (Circle != null)
            {
                Canvas.SetLeft(Circle, locationX - Circle.Margin.Left);
                Canvas.SetTop(Circle, locationY - Circle.Margin.Top);
            }
        }
    }
}
