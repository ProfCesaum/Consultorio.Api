using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Consultorio.Api.Services;

public class ValidadorCpfService : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var cpf = value as string;

        if (string.IsNullOrWhiteSpace(cpf))
            return new ValidationResult("CPF é obrigatório.");

        // Remove todos os caracteres que não sejam números (pontos, traços, espaços)
        // Exemplo: "123.456.789-01" vira "12345678901"
        cpf = Regex.Replace(cpf, "[^0-9]", "");

        if (cpf.Length != 11)
            return new ValidationResult("CPF deve conter 11 dígitos.");

        return ValidationResult.Success;
    }
}