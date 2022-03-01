using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LikePaint
{
    class MyElipse : Shapes
    {
        public Ellipse Ellipse { get; private set; }

        public MyElipse()
        {
            SolidColorBrush myEllipseSolidColor = new SolidColorBrush();
            SolidColorBrush myEllipseOutlineColor = new SolidColorBrush();

            myEllipseSolidColor.Color = Color.FromRgb(235, 245, 248);
            myEllipseOutlineColor.Color = Color.FromRgb(102, 121, 143);

            Ellipse = new Ellipse
            {
                Stroke = myEllipseOutlineColor,
                StrokeThickness = 2,
                Fill = myEllipseSolidColor,
                Width = 100,
                Height = 50
            };
        }

        public void Draw(double locationX, double locationY)
        {
            if (Ellipse != null)
            {
                Canvas.SetLeft(Ellipse, locationX - Ellipse.Margin.Left);
                Canvas.SetTop(Ellipse, locationY - Ellipse.Margin.Top);
            }
        }
    }
}
