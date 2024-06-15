using SQLite;

namespace ControlSalud.Entities
{
    [Table("paciente")]
    public class Paciente
    {
        [PrimaryKey, AutoIncrement, Column("id_paciente")]
        public int IdPaciente { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("apellido")]
        public string? Apellido { get; set; }

        [Column("sexo")]
        public string? Sexo {  get; set; }

        [Column("edad")]
        public int Edad { get; set; }

        [Column("peso")]
        public int Peso { get; set; }

        [Column("estatura")]
        public int Estatura { get; set; }

        [Column("nivel_actividad_fisica")]
        public string? NivelActividadFisica { get; set; }

        [Ignore]
        public string NombreCompleto => $"{Nombre.ToUpper()} {Apellido.ToUpper()}";
    }
}
