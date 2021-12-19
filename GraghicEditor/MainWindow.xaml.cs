using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraghicEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point p1, p2, centre ;
        protected bool isDragging;
        protected bool isRotating;
        private Point clickPosition;
        private TranslateTransform translate;
        private RotateTransform rotate;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(canvas.Children.OfType<Line>().FirstOrDefault(x => x.IsMouseOver == true) == null)
                p1 = Mouse.GetPosition(canvas);
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (canvas.Children.OfType<Line>().FirstOrDefault(x => x.IsMouseOver == true) == null)
            {
                p2 = Mouse.GetPosition(canvas);
                DrawLine(p1, p2, false);
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    Point p = Mouse.GetPosition(canvas);
            //    DrawLine(p1, p, true);
            //}
        }

        private void DrawLine(Point start, Point stop, bool move)
        {
            Line line = new Line();
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 3;
            line.X1 = start.X;
            line.X2 = stop.X;
            line.Y1 = start.Y;
            line.Y2 = stop.Y;
            line.MouseEnter += Line_MouseEnter;
            line.MouseLeave += Line_MouseLeave;
            line.MouseMove += Line_MouseMove;
            line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
            line.MouseLeftButtonUp += Line_MouseLeftButtonUp;
            line.MouseRightButtonDown += Line_MouseRightButtonDown;
            line.MouseRightButtonUp += Line_MouseRightButtonUp;
            canvas.Children.Add(line);
        }



        #region LineActions

        //private void Line_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    canvas.Children.Remove((Line)sender);
        //}

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var draggableControl = sender as Shape;
            translate = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
            isDragging = true;
            clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
        }

        private void Line_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as Shape;
            draggable.ReleaseMouseCapture();
        }

        private void Line_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var draggableControl = sender as Shape;
                if (isDragging && draggableControl != null)
                {
                    Point currentPosition = e.GetPosition(this);
                    var transform = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                    transform.X = translate.X + (currentPosition.X - clickPosition.X);
                    transform.Y = translate.Y + (currentPosition.Y - clickPosition.Y);
                    draggableControl.RenderTransform = new TranslateTransform(transform.X, transform.Y);
                }
            }
            else if(e.RightButton == MouseButtonState.Pressed)
            {
                centre = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
                var rotatableControl = sender as Shape;
                if (isRotating && rotatableControl != null)
                {
                    Point currentPosition = e.GetPosition(this);
                    var transform = rotatableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                    rotatableControl.RenderTransform = new RotateTransform(Vector.AngleBetween(clickPosition - centre, currentPosition - centre), centre.X, centre.Y);
                }
                   
            }
        }

        private void Line_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            isRotating = false;
            var rotatableControl = sender as Shape;
            rotatableControl.ReleaseMouseCapture();
        }

        private void Line_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rotatableControl = sender as Shape;
            rotate = rotatableControl.RenderTransform as RotateTransform ?? new RotateTransform();
            isRotating = true;
            clickPosition = e.GetPosition(canvas);
            rotatableControl.CaptureMouse();
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Line)sender).Stroke = Brushes.Black;
        }

        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Line)sender).Stroke = Brushes.Red;
        }

        #endregion


    }
}
