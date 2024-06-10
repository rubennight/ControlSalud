using Microsoft.Maui.Controls;

namespace ControlSalud
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            Navigation.PushAsync(new Navigation.AgregarPacienteView());
        }
    }

}
