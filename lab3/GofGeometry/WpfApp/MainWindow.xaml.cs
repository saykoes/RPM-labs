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
        private IGeoFactory _currentFactory;

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
                    _currentFactory = new RedFactory();
                    break;
                // Green
                case 1:
                    _currentFactory = new GreenFactory();
                    break;
                // Blue
                case 2:
                    _currentFactory = new BlueFactory();
                    break;
                // Cyan
                case 3:
                    _currentFactory = new CyanFactory();
                    break;
                // Magenta
                case 4:
                    _currentFactory = new MagentaFactory();
                    break;
                // Yellow
                case 5:
                    _currentFactory = new YellowFactory();
                    break;
                // blacK
                case 6:
                    _currentFactory = new BlackFactory();
                    break;
                default:
                    return;
            }

            FiguresPanel.Children.Clear();

            FiguresPanel.Children.Add(_currentFactory.CreateCircle().CreateUIElement());
            FiguresPanel.Children.Add(_currentFactory.CreateSquare().CreateUIElement());
            FiguresPanel.Children.Add(_currentFactory.CreateTriangle().CreateUIElement());
        }
        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiguresUpdate();
        }
    }
}
