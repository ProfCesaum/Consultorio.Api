using System.Text.Json.Serialization;

namespace Consultorio.Api.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
        public int ConsultorioId { get; set; }

        public Consultorio? Consultorio { get; set; }
    }
}
