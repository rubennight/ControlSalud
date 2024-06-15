using ControlSalud.Entities;

namespace ControlSalud.Navigation
{
    public partial class AgregarPacienteView : ContentPage
    {
        private readonly BdLocalService bdLocalService;

        Paciente px;

        public AgregarPacienteView() : this(null)
        {

        }

        public AgregarPacienteView(Paciente paciente)
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

            if (paciente!=null)
            {
                loadPaciente(paciente);
            }

            px = paciente;


            // Initial update of stats
            updateStats();
        }

        private async void OnGuardarPacienteClicked(object sender, EventArgs e)
        {
            try
            {
                if (Nombre.Text != null && Apellido.Text != null && Sexo.SelectedItem != null)
                {

                    if (px!=null)
                    {

                        var pax= new Paciente
                        {
                            IdPaciente = px.IdPaciente,
                            Nombre = Nombre.Text,
                            Apellido = Apellido.Text,
                            Sexo = Sexo.SelectedItem.ToString(),
                            Edad = (int)edadSlider.Value,
                            Peso = (int)pesoSlider.Value,
                            Estatura = (int)estaturaSlider.Value,
                            NivelActividadFisica = ActividadFisica.SelectedItem.ToString()
                        };
                        await bdLocalService.ActualizarPaciente(pax);

                        await DisplayAlert("Éxito", "Paciente actualizado correctamente.", "OK");
                    } 
                    else
                    {
                        var nuevoPaciente = new Paciente
                        {
                            Nombre = Nombre.Text,
                            Apellido = Apellido.Text,
                            Sexo = Sexo.SelectedItem.ToString(),
                            Edad = (int)edadSlider.Value,
                            Peso = (int)pesoSlider.Value,
                            Estatura = (int)estaturaSlider.Value,
                            NivelActividadFisica = ActividadFisica.SelectedItem.ToString()
                        };

                        await bdLocalService.AgregarPaciente(nuevoPaciente);

                        await DisplayAlert("Éxito", "Paciente registrado correctamente.", "OK");

                    }

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

            txtImc.Text = getIMCDescripcion(imc);
            txtGrasa.Text = getGCDescripcion(porcentajeGrasa,sexo);
            txtPesoIdeal.Text = pesoIdeal.ToString("F2") + " Kg";
        }


        private void loadPaciente(Paciente px)
        {
            Nombre.Text = px.Nombre.ToString();
            Apellido.Text = px.Apellido.ToString();
            Sexo.SelectedIndex = px.Sexo == "Masculino" ? 0 : 1;

            switch (px.NivelActividadFisica)
            {
                case "Sedentario: 0 a 30 min.a la semana":
                    ActividadFisica.SelectedIndex = 0;
                    break;
                case "Poco activo: 1 a 2 horas a la semana":
                    ActividadFisica.SelectedIndex = 1;
                    break;
                case "Moderadamente Activo: 2 a 3 horas a la semana":
                    ActividadFisica.SelectedIndex = 2;
                    break;
                case "Activo: 3 a 4 horas a la semana":
                    ActividadFisica.SelectedIndex = 3;
                    break;
                case "Muy Activo: 4 a 5 horas a la semana":
                    ActividadFisica.SelectedIndex = 4;
                    break;
            }

            edadSlider.Value = (int)px.Edad;
            pesoSlider.Value = (int)px.Peso;
            estaturaSlider.Value = (int)px.Estatura;



        }

        private double getIMC(double peso, double estatura)
        {
            return peso / (estatura * estatura);           
        }

        private string getIMCDescripcion(double imc)
        {
            string analisis;

            switch (imc)
            {
                case < 18.5:
                    analisis = "Bajo peso";
                    break;

                case >= 18.5 and < 25.0:
                    analisis = "Peso normal";
                    break;

                case >= 25 and < 30:
                    analisis = "Sobrepeso";
                    break;

                case >= 30 and < 35:
                    analisis = "Obesidad Clase I";
                    break;

                case >= 35 and < 40:
                    analisis = "Obesidad Clase II";
                    break;

                case > 40:
                    analisis = "Obesidad Clase III";
                    break;

                default:
                    analisis = "";
                    break;
            }


            return $"{imc:F2} ({analisis})";
        }

        private double getPorcentajeGrasaCorporal(double imc, int edad, int sexo)
        {
            return 1.2 * imc + 0.23 * edad - 10.8 * sexo - 5.4;
        }

        private string getGCDescripcion(double porcentajeGrasa, int sexo)
        {
            string descripcion = porcentajeGrasa switch
            {
                _ when sexo == 1 && porcentajeGrasa >= 10 && porcentajeGrasa <= 13 => "Grasa esencial",
                _ when sexo == 0 && porcentajeGrasa >= 2 && porcentajeGrasa <= 5 => "Grasa esencial",
                _ when sexo == 1 && porcentajeGrasa >= 14 && porcentajeGrasa <= 20 => "Atletas",
                _ when sexo == 0 && porcentajeGrasa >= 6 && porcentajeGrasa <= 13 => "Atletas",
                _ when sexo == 1 && porcentajeGrasa >= 21 && porcentajeGrasa <= 24 => "Fitness",
                _ when sexo == 0 && porcentajeGrasa >= 14 && porcentajeGrasa <= 17 => "Fitness",
                _ when sexo == 1 && porcentajeGrasa >= 25 && porcentajeGrasa <= 31 => "Aceptable",
                _ when sexo == 0 && porcentajeGrasa >= 18 && porcentajeGrasa <= 24 => "Aceptable",
                _ when sexo == 1 && porcentajeGrasa > 32 => "Obesidad",
                _ when sexo == 0 && porcentajeGrasa > 25 => "Obesidad",
                _ => "Clasificación desconocida"
            };

            return $"{porcentajeGrasa:F2}% ({descripcion})";

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
