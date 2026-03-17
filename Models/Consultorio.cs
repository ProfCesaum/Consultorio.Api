using System.ComponentModel.DataAnnotations;

namespace Consultorio.Api.Models
{
    public class Consultorio
    {
        public int Id { get; set; }
        [Required] public string Nome { get; set; }
        [Required] public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public int Numero { get; set; }
    }
}
