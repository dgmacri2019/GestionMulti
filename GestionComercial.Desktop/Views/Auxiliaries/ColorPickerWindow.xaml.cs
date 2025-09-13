using System.Windows;
using System.Windows.Media;

namespace GestionComercial.Desktop.Views.Auxiliaries
{
    /// <summary>
    /// Lógica de interacción para ColorPickerWindow.xaml
    /// </summary>
    public partial class ColorPickerWindow : Window
    {
        public string SelectedColorHex { get; private set; }

        public ColorPickerWindow(string initialColor = "#FFFFFF")
        {
            InitializeComponent();

            // Inicializar sliders con color inicial
            if (initialColor.StartsWith("#") && (initialColor.Length == 7 || initialColor.Length == 9))
            {
                var color = (Color)ColorConverter.ConvertFromString(initialColor);
                sliderR.Value = color.R;
                sliderG.Value = color.G;
                sliderB.Value = color.B;
            }

            UpdatePreview();
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            int r = (int)sliderR.Value;
            int g = (int)sliderG.Value;
            int b = (int)sliderB.Value;

            lblR.Text = r.ToString();
            lblG.Text = g.ToString();
            lblB.Text = b.ToString();

            var color = Color.FromRgb((byte)r, (byte)g, (byte)b);
            previewColor.Background = new SolidColorBrush(color);

            SelectedColorHex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
