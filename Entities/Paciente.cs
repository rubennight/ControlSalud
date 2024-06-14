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
    }
}
