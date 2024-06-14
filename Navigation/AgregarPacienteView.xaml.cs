using ControlSalud.Entities;

namespace ControlSalud.Navigation;

public partial class AgregarPacienteView : ContentPage
{
	private readonly BdLocalService bdLocalService;
	public AgregarPacienteView(BdLocalService bdService)
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);

		bdLocalService = bdService;
	}

	private async void OnGuardarPacienteClicked(object sender, EventArgs e)
	{
		var nuevoPaciente = new Paciente
		{
			Nombre = Nombre.Text,
			Apellido = Apellido.Text,
			Sexo = Sexo.SelectedItem.ToString(),
		};

		await bdLocalService.ActualizarPaciente(nuevoPaciente);

		var nuevoPacienteData = new PacienteData
		{
			IdPaciente = nuevoPaciente.IdPaciente,
			Edad = (int)edadSlider.Value,
			Peso = (int)pesoSlider.Value,
			Estatura = (int)estaturaSlider.Value,
			NivelActividadFisica = ActividadFisica.SelectedItem.ToString(),
			Fecha = DateTime.Now.ToString("yyyy-MM-dd")
		};

		await bdLocalService.AgregarPacienteData(nuevoPacienteData);

		await Navigation.PopAsync();
	}
}