using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSalud.Entities
{
    [Table("PacienteData")]
    public class PacienteData
    {
        [PrimaryKey] 
        [AutoIncrement]
        [Column("id_paciente_data")]
        public int IdPacienteData { get; set; }

        [Column("id_paciente")]
        public int IdPaciente {  get; set; }

        [Column("edad")]
        public int Edad {  get; set; }

        [Column("peso")]
        public int Peso { get; set; }

        [Column("estatura")]
        public int Estatura {  get; set; }

        [Column("nivel_actividad_fisica")]
        public string? NivelActividadFisica { get; set; }
        
        [Column("fecha")]
        public string? Fecha { get; set; }
    }
}
