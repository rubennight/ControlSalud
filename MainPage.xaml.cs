using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System.Threading.Tasks;
using ControlSalud.Entities;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControlSalud
{
    public partial class MainPage : ContentPage
    {
        private readonly BdLocalService bdLocalService;

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            bdLocalService = new BdLocalService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarPacientes();
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
            Navigation.PushAsync(new Navigation.AgregarPacienteView());
        }

        private void PacientesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && e.Item is Paciente selectedPaciente)
            {
                Navigation.PushAsync(new Navigation.AgregarPacienteView(selectedPaciente));
            }

        }
    }
}
