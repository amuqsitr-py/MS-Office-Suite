using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LikePaint
{
    public class makeCanvas
    {
        public makeCanvas(Canvas holder)
        {
            _holder = holder;
        }

        public Canvas _holder;

        public void AddShape(Shapes shape)
        {
            if (shape is MyRectangle)
            {
                _holder.Children.Add((shape as MyRectangle).Rectangle);
            }
            else if (shape is MyCircle)
                _holder.Children.Add((shape as MyCircle).Circle);
            else if (shape is MyElipse)
                _holder.Children.Add((shape as MyElipse).Ellipse);
            else if (shape is MyTriangle)
                _holder.Children.Add((shape as MyTriangle).Triangle);
            else if (shape is MyLine)
                _holder.Children.Add((shape as MyLine).Line);
        }

        public void Draw(Shapes shape, double locationX, double locationY)
        {
            shape.Draw(locationX, locationY);
        }


    }
}
