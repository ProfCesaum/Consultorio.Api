using System.Text.Json.Serialization;

namespace Consultorio.Api.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        [JsonIgnore]
        public Paciente? Paciente { get; set; }
        public int MedicoId { get; set; }

        public Medico? Medico { get; set; }
        public DateTime DataHora { get; set; }
        public string Obs { get; set; }

    }
}
