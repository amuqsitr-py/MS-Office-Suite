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
    public class MyRectangle : Shapes
    {
        public Rectangle Rectangle { get; private set; }

        public MyRectangle()
        {
            SolidColorBrush myRectangleSolidColor = new SolidColorBrush();
            SolidColorBrush myRectangleOutlineColor = new SolidColorBrush();

            myRectangleOutlineColor.Color = Color.FromRgb(102, 121, 143);
            myRectangleSolidColor.Color = Color.FromRgb(235, 245, 248);

            Rectangle = new Rectangle
            {
                Stroke = myRectangleOutlineColor,
                StrokeThickness = 2,
                Fill = myRectangleSolidColor,
                Width = 100,
                Height = 100
            };

        }

        public void Draw(double locationX, double locationY)
        { 
            Canvas.SetLeft(Rectangle, locationX);
            Canvas.SetTop(Rectangle, locationY);
        }
    }
}
