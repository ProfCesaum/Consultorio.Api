namespace Consultorio.Api.Models
{
    public class ViaCepResponse
    {
        // CEP no formato 00000-000
        public string cep { get; set; } = string.Empty;

        // Nome da rua, avenida, praça, etc.
        public string logradouro { get; set; } = string.Empty;

        // Complemento do endereço (pode vir vazio)
        public string complemento { get; set; } = string.Empty;

        // Nome do bairro
        public string bairro { get; set; } = string.Empty;

        // Nome da cidade
        public string localidade { get; set; } = string.Empty;

        // Sigla do estado (UF) - Ex: SP, RJ, MG
        public string uf { get; set; } = string.Empty;

        // Código IBGE do município
        public string ibge { get; set; } = string.Empty;

        // Código GIA (Guia de Informação e Apuração do ICMS) - usado em SP
        public string gia { get; set; } = string.Empty;

        // Código DDD da região
        public string ddd { get; set; } = string.Empty;

        // Código SIAFI do município
        public string siafi { get; set; } = string.Empty;
    }
}
