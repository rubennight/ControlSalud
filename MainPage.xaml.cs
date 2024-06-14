using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace ControlSalud
{
    public partial class MainPage : ContentPage
    {
        private readonly BdLocalService bdLocalService; 

        public MainPage(BdLocalService bdService)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            bdLocalService = bdService;
            Task.Run(async () => await CargarPacientes());
        }

        private async Task CargarPacientes()
        {
            var pacientes = await bdLocalService.ObtenerPacientes();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (pacientes != null && pacientes.Count > 0)
                {
                    PacientesListView.ItemsSource = pacientes;
                    PacientesListView.IsVisible = true;
                    NoPacientesLabel.IsVisible = false;
                }
                else
                {
                    PacientesListView.IsVisible = false;
                    NoPacientesLabel.IsVisible = true;
                }
            });
        }
        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            Navigation.PushAsync(new Navigation.AgregarPacienteView(bdLocalService));
        }
    }

}
