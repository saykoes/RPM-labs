using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        private CircleCreator _currentCircleCreator;
        private SquareCreator _currentSquareCreator;
        private TriangleCreator _currentTriangleCreator;

        public MainWindow()
        {
            InitializeComponent();
            FiguresUpdate();
        }

        private void FiguresUpdate()
        {
            switch (ColorComboBox.SelectedIndex)
            {
                // Red
                case 0:
                    _currentCircleCreator = new RedCircleCreator();
                    _currentSquareCreator = new RedSquareCreator();
                    _currentTriangleCreator = new RedTriangleCreator();
                    break;
                // Green
                case 1:
                    _currentCircleCreator = new GreenCircleCreator();
                    _currentSquareCreator = new GreenSquareCreator();
                    _currentTriangleCreator = new GreenTriangleCreator();
                    break;
                // Blue
                case 2:
                    _currentCircleCreator = new BlueCircleCreator();
                    _currentSquareCreator = new BlueSquareCreator();
                    _currentTriangleCreator = new BlueTriangleCreator();
                    break;
                // Cyan
                case 3:
                    _currentCircleCreator = new CyanCircleCreator();
                    _currentSquareCreator = new CyanSquareCreator();
                    _currentTriangleCreator = new CyanTriangleCreator();
                    break;
                // Magenta
                case 4:
                    _currentCircleCreator = new MagentaCircleCreator();
                    _currentSquareCreator = new MagentaSquareCreator();
                    _currentTriangleCreator = new MagentaTriangleCreator();
                    break;
                // Yellow
                case 5:
                    _currentCircleCreator = new YellowCircleCreator();
                    _currentSquareCreator = new YellowSquareCreator();
                    _currentTriangleCreator = new YellowTriangleCreator();
                    break;
                // blacK
                case 6:
                    _currentCircleCreator = new BlackCircleCreator();
                    _currentSquareCreator = new BlackSquareCreator();
                    _currentTriangleCreator = new BlackTriangleCreator();
                    break;
                default:
                    return;
            }

            FiguresPanel.Children.Clear();

            FiguresPanel.Children.Add(_currentCircleCreator.CreateCircle().CreateUIElement());
            FiguresPanel.Children.Add(_currentSquareCreator.CreateSquare().CreateUIElement());
            FiguresPanel.Children.Add(_currentTriangleCreator.CreateTriangle().CreateUIElement());
        }
        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiguresUpdate();
        }
    }
}
