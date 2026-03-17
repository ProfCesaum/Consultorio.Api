using Consultorio.Api.Models;

namespace Consultorio.Api.Services;

public class ViaCepService
{
    private readonly HttpClient _httpClient;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
    }

    public async Task<Models.ViaCepResponse> ObterEnderecoPorCepAsync(string cep)
    {
        var endereco = await _httpClient.GetFromJsonAsync<Models.ViaCepResponse>($"{cep}/json/");
        return endereco;
    }
}
