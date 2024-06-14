using ControlSalud.Entities;

namespace ControlSalud.Navigation
{
    public partial class AgregarPacienteView : ContentPage
    {
        private readonly BdLocalService bdLocalService;

        public AgregarPacienteView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            bdLocalService = new BdLocalService();
        }

        private async void OnGuardarPacienteClicked(object sender, EventArgs e)
        {
            try
            {
                if (Nombre.Text != null && Apellido.Text != null && Sexo.SelectedItem != null)
                {
                    var nuevoPaciente = new Paciente
                    {
                        Nombre = Nombre.Text,
                        Apellido = Apellido.Text,
                        Sexo = Sexo.SelectedItem.ToString(),
                    };

                    await bdLocalService.AgregarPaciente(nuevoPaciente);

                    var nuevoPacienteData = new PacienteData
                    {
                        IdPaciente = nuevoPaciente.IdPaciente,
                        Edad = (int)edadSlider.Value,
                        Peso = (int)pesoSlider.Value,
                        Estatura = (int)estaturaSlider.Value,
                        NivelActividadFisica = ActividadFisica.SelectedItem.ToString(),
                        Fecha = DateTime.Now.ToString("yyyy-MM-dd")
                    };
                    await DisplayAlert("Éxito", "Paciente y sus datos se guardaron correctamente.", "OK");
                    await bdLocalService.AgregarPacienteData(nuevoPacienteData);
                    await Navigation.PopAsync();

                } else
                {
                    await DisplayAlert("Error", "El nombre, apellido y sexo no pueden ir vacíos", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al guardar los datos: " + ex.Message, "OK");
            }

        }
    }
}
