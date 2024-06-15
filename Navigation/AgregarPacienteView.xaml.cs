using ControlSalud.Entities;
using Microsoft.Maui.Controls.Compatibility;
using System;

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

            // Add event listeners
            edadSlider.ValueChanged += (s, e) => updateStats();
            pesoSlider.ValueChanged += (s, e) => updateStats();
            estaturaSlider.ValueChanged += (s, e) => updateStats();
            Sexo.SelectedIndexChanged += (s, e) => updateStats();
            ActividadFisica.SelectedIndexChanged += (s, e) => updateStats();

            // Initial update of stats
            updateStats();
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
                }
                else
                {
                    await DisplayAlert("Error", "El nombre, apellido y sexo no pueden ir vacíos", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al guardar los datos: " + ex.Message, "OK");
            }
        }

        private void updateStats()
        {
            double peso = pesoSlider.Value;
            double estatura = estaturaSlider.Value / 100; // Convert cm to meters
            int edad = (int)edadSlider.Value;
            int sexo = Sexo.SelectedIndex == 0 ? 1 : 0; // Assuming 0 is male and 1 is female

            double imc = getIMC(peso, estatura);
            double porcentajeGrasa = getPorcentajeGrasaCorporal(imc, edad, sexo);
            double pesoIdeal = getPesoIdeal((int)estaturaSlider.Value, sexo);

            // Ensure that ActividadFisica.SelectedItem is not null
            if (ActividadFisica.SelectedItem != null)
            {
                double tdee = getTDEE(estaturaSlider.Value, peso, edad, ActividadFisica.SelectedItem.ToString());
                txtTdee.Text = tdee.ToString("F2") + " Kcal";
            }
            else
            {
                txtTdee.Text = "N/A";
            }

            txtImc.Text = imc.ToString("F2");
            txtGrasa.Text = porcentajeGrasa.ToString("F2") + " %";
            txtPesoIdeal.Text = pesoIdeal.ToString("F2") + " Kg";
        }

        private double getIMC(double peso, double estatura)
        {
            return peso / (estatura * estatura);
        }

        private double getPorcentajeGrasaCorporal(double imc, int edad, int sexo)
        {
            return 1.2 * imc + 0.23 * edad - 10.8 * sexo - 5.4;
        }

        private double getPesoIdeal(int estatura, int sexo)
        {
            double factor = sexo == 0 ? 2.5 : 4;
            return estatura - 100 - ((estatura - 150) / factor);
        }

        private double getTDEE(double altura, double peso, int edad, string nivelActividad)
        {
            double bmr = (altura * 6.25) + (peso * 9.99) - (edad * 4.92) - 161;

            switch (nivelActividad)
            {
                case "Sedentario: 0 a 30 min. a la semana":
                    return bmr * 1.2;
                case "Poco activo: 1 a 2 horas a la semana":
                    return bmr * 1.375;
                case "Moderadamente Activo: 2 a 3 horas a la semana":
                    return bmr * 1.55;
                case "Activo: 3 a 4 horas a la semana":
                    return bmr * 1.725;
                case "Muy Activo: 4 a 5 horas a la semana":
                    return bmr * 1.9;
                default:
                    return bmr;
            }
        }
    }
}
