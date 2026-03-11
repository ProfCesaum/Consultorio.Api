using Consultorio.Api.Services;
using System.ComponentModel.DataAnnotations;

namespace Consultorio.Api.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required, EmailAddress(ErrorMessage = "E-mail em formato invalido")]
        public string Email { get; set; }
        [Required, ValidadorCpfService]
        public string Cpf { get; set; }

    }
}
