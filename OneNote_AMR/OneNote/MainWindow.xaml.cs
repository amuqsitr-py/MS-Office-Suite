using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.ComponentModel;

namespace LikePaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        // stvaranje novog adornerlayera koji ce sadrzavat tocke za resize i ide preko elementa
        AdornerLayer aLayer;

        bool _isDown;
        bool _isDragging;
        bool selected = false;
        //element koji ce sadrzavat trenutno selektirani element
        UIElement selectedElement = null;
        //element koji sadrzi trenutno selektirani menuitem (rectangle, ellipse,...)
        MenuItem selectedItem = null;
        //koja je boja selektirana 
        Button selectedButton;

        // za bindanje selektirane boje s rectanglom koji pokazuje koja je boja trenutno selektirana
        private string _colorName;
        // za bindanje selektirane boje outlinea s rectanglom koji pokazuje koja je boja trenutno selektirana
        private Brush _outlineColors;
        bool colorPickerClicked;

        //je li selektirani element rectangle, circle,...
        Shape selectedShape;
        private bool handle = true;

        //tocka od koje krece dragg
        Point _startPoint;
        private double _originalLeft;
        private double _originalTop;

        public MainWindow()
        {
            InitializeComponent();

            _holder = new makeCanvas(canvas);

        }

        public readonly makeCanvas _holder;
        private Shapes shape;

        public event PropertyChangedEventHandler PropertyChanged;

        //click na pointer da mozemo selektirati elemente
        private void pointerButton_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = null;
            drawLineButton.IsChecked = false;

        }
        //odabir oblika kojeg zelimo nacrtati
        private void shapesButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        //click na oblik rectangle
        private void rectangle_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = e.Source as MenuItem;
            pointerButton.IsChecked = false;
        }

        // klikom na canvas, ukoliko nije prethodno kliknut izbor za crtanje, deselektira se element
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }
        }

        // tokom resize, kad dignemo prst s misa, resize je gotov
        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }
        // za bind rectangle fill koji poprima odabranu boju ovisno o pritisnutom botunu
        public string colorName
        {
            get
            {
                return _colorName;
            }
            set
            {
                _colorName = value;
                RaisePropertyChanged("colorName");

            }
        }

        // za bind rectangle fill koji poprima odabranu boju ovisno o pritisnutom outline botunu
        public Brush outlineColors
        {
            get
            {
                return _outlineColors;
            }
            set
            {
                _outlineColors = value;
                RaisePropertyChanged("outlineColors");
            }
        }

        // event koji se spaja na sve botune za odabir boja
        public void color_picker(object sender, RoutedEventArgs e)
        {
            selectedButton = sender as Button;
            colorName = selectedButton.Name;
            this.DataContext = this;
            colorPickerClicked = true;

            if (selectedElement!=null && selectedShape!=null)
            { 
                if(selectedButton== Transparent)
                    selectedShape.Fill = Brushes.Transparent;

                else
                    selectedShape.Fill = selectedButton.Background;
            }

        }
        // da se promini vrijednost bindanog elementa kad se vrijednost property promini
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //nakon ucitanja prozora, napravi handlere i vezi ih za cavnas
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseMove += new MouseEventHandler(MetroWindow_MouseMove);
            this.MouseLeave += new MouseEventHandler(Window1_MouseLeave);

            canvas.PreviewMouseDown += new MouseButtonEventHandler(canvas_PreviewMouseDown);
            canvas.PreviewMouseUp += new MouseButtonEventHandler(DragFinishedMouseHandler);

        }

        // zavrseno draganje elementa tokom resize, zavrsi 
        void DragFinishedMouseHandler(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // odabir elementa, clickom na canvas, kojeg zelimo resize tako da dobijemo thumbse za resize
        private void canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // stvaranje novih elemenata ukoliko pointer botun nije pritisnut, a selecteditem postoji (menu item je pritisnut)
            if (selectedItem == rectangle)
            {
                shape = new MyRectangle();
                _holder.Draw(shape, Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
                _holder.AddShape(shape);
            }

            else if(selectedItem == circle)
            {
                shape = new MyCircle();
                _holder.Draw(shape, Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
                _holder.AddShape(shape);
            }
            
            else if(selectedItem == ellipse)
            {
                shape = new MyElipse();
                _holder.Draw(shape, Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
                _holder.AddShape(shape);
                
            }

            else if(selectedItem == triangle)
            {
                shape = new MyTriangle(Mouse.GetPosition(canvas));
                _holder.Draw(shape, Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
                _holder.AddShape(shape);
            }

            if(drawLineButton.IsChecked == true)
            {
                shape = new MyLine(Mouse.GetPosition(canvas));
                _holder.Draw(shape, Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
                _holder.AddShape(shape);
            }

            // ako smo vec kreirali neke elemente i pointer botun je pritisnut, stavaramo na kliknutom elementu thumbse za resize
            if(pointerButton.IsChecked == true)
            {
                // Micanje selekcije ukoliko kliknemo bilo gdje 
                if (selected && colorPickerClicked == true)
                {
                    selected = false;
                    if (selectedElement != null)
                    {
                        // makni adorner layer od tog elementa
                        aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                        selectedElement = null;
                    }
                }

                // ako je pritisnut bilo koji element osim canvasa, 
                //postavi ga ka selected i stavi mu adorners za resize
                if (e.Source != canvas)
                {
                    _isDown = true;
                    _startPoint = e.GetPosition(canvas);

                    selectedShape = e.Source as Shape;

                    selectedElement = e.Source as UIElement;

                    _originalLeft = Canvas.GetLeft(selectedElement);
                    _originalTop = Canvas.GetTop(selectedElement);

                    aLayer = AdornerLayer.GetAdornerLayer(selectedElement);
                    aLayer.Add(new ResizingAdorner(selectedElement));
                    selected = true;
                    e.Handled = true;
                }

            }

        }

        // kad se makne s windowa, zavrsi drag
        void Window1_MouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // handler za resize funkcionalnost
        private void MetroWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(canvas).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(canvas).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    _isDragging = true;

                if (_isDragging)
                {
                    Point position = Mouse.GetPosition(canvas);
                    Canvas.SetTop(selectedElement, position.Y - (_startPoint.Y - _originalTop));
                    Canvas.SetLeft(selectedElement, position.X - (_startPoint.X - _originalLeft));
                }
            }
        }

        // metoda za prekid drag-a
        private void StopDragging()
        {
            if (_isDown)
            {
                _isDown = false;
                _isDragging = false;
            }
        }
        // click na circle shape
        private void circle_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = e.Source as MenuItem;
            pointerButton.IsChecked = false;
        }
        // click na ellipse shape
        private void elipse_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = e.Source as MenuItem;
            pointerButton.IsChecked = false;
            drawLineButton.IsChecked = false;
        }
        // click na trokut shape
        private void triangle_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = e.Source as MenuItem;
            pointerButton.IsChecked = false;
            drawLineButton.IsChecked = false;
        }
        //click na crtanje ravne linije
        private void drawLineButton_Click(object sender, RoutedEventArgs e)
        {
            pointerButton.IsChecked = false;
            selectedItem = null;
        }
        // combobox di se nalaze debljine linije za odabir
        private void lineWidthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            if (selectedElement != null && selectedShape != null)
                Handle();
        }
        // kada zatvorimo taj isti combobox
        private void lineWidthComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (handle) Handle();
            handle = true;
        }
        // switchanje ovisno koju smo debljinu izabrali, tj koji izbor
        private void Handle()
        {
            switch (lineWidthComboBox.SelectedIndex)
            {
                case 0:
                    //Handle za prvi combobox
                    selectedShape.StrokeThickness = 3;
                    break;
                case 1:
                    //Handle za drugi combobox
                    selectedShape.StrokeThickness = 4;
                    break;
                case 2:
                    //Handle za treci combobox
                    selectedShape.StrokeThickness = 5;
                    break;
                case 3:
                    //Handle za cetvrti combobox
                    selectedShape.StrokeThickness = 6;
                    break;
                case 4:
                    //Handle za peti combobox
                    selectedShape.StrokeThickness = 7;
                    break;
                case 5:
                    //Handle za sesti combobox
                    selectedShape.Stroke = Brushes.Transparent;
                    break;
            }
        }
        // event koji se spaja na sve botune za odabir boja za outline
        public void outline_color_picker(object sender, RoutedEventArgs e)
        {
            selectedButton = sender as Button;
            outlineColors = selectedButton.Background;
            this.DataContext = this;
            colorPickerClicked = true;

            if (selectedElement != null && selectedShape != null)
                selectedShape.Stroke = selectedButton.Background;
        }
    }
}
