using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using System.Windows;
using System.Windows.Input;

namespace GestionComercial.Desktop.Views.Searchs
{
    /// <summary>
    /// Lógica de interacción para ClientSearchWindows.xaml
    /// </summary>
    public partial class ClientSearchWindows : Window
    {
        private List<ClientViewModel> _allClients;

        public ClientViewModel? SelectedClient { get; private set; }

        public ClientSearchWindows(string searchText = "")
        {
            InitializeComponent();

            // Cargar todos los clientes desde el cache
            _allClients = ClientCache.Instance.GetAll();

            // Llenar grilla
            dgClients.ItemsSource = _allClients;

            // Si vino texto de búsqueda, lo aplico
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                txtSearch.Text = searchText;
                FilterClients(searchText);
            }
            txtSearch.Focus();
            txtSearch.CaretIndex = txtSearch.Text.Length;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            SelectedClient = dgClients.SelectedItem as ClientViewModel;
            if (SelectedClient != null)
                DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            FilterClients(txtSearch.Text);
        }

        private void dgClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BtnOk_Click(sender, e);
        }


        private void FilterClients(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                dgClients.ItemsSource = _allClients;
            else
                dgClients.ItemsSource = _allClients
            .Where(a =>
                (a.OptionalCode ?? "").ToLower().Contains(text.ToLower()) ||
                (a.FantasyName ?? "").ToLower().Contains(text.ToLower()) ||
                (a.BusinessName ?? "").ToLower().Contains(text.ToLower()) ||
                (a.DocumentNumber ?? "").ToLower().Contains(text.ToLower())
            )
            .ToList();
        }

    }
}
