using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
// I'm used to CommunityToolkit, so I'm using it here too
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfApp
{
    public record ColorOption(IGeoFactory Factory, string Label);

    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<ColorOption> ColorOptions { get; }

        [ObservableProperty]
        private ColorOption? _selectedColor;

        // Redefining CommunityToolkit generated method
        partial void OnSelectedColorChanged(ColorOption? value) => UpdateFigures();

        // Constructor Chaining, so that the XAML doesn't have to include arguments to call the viewmodel
        // But also so that we can still put different factories IEnumerable if we wanted to (eg. for testing)
        public MainViewModel() : this(GeoFactoryProvider.GetFactories()) { }

        // IEnumerable instead of List to be more flexible
        public MainViewModel(IEnumerable<IGeoFactory> factories)
        {
            var options = factories.Select(f => new ColorOption(f, f.GetType().Name.Replace("Factory", "")));   // Getting color name from type names 
            ColorOptions = new ObservableCollection<ColorOption>(options);
            SelectedColor = ColorOptions.FirstOrDefault();
        }

        public ObservableCollection<UIElement> Figures { get; } = new();

        private void UpdateFigures()
        {
            Figures.Clear();
            if (SelectedColor == null || SelectedColor.Factory == null) return;
            var factory = SelectedColor.Factory;

            Figures.Add(factory.CreateCircle().CreateUIElement());
            Figures.Add(factory.CreateSquare().CreateUIElement());
            Figures.Add(factory.CreateTriangle().CreateUIElement());
        }
    }
}
