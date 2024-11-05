using Fiap.TechChallenge.Fase1.Dominio.Model;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;
using Fiap.TechChallenge.Fase1.Integration.Tests.Infra;
using Fiap.TechChallenge.Fase1.Integration.Tests.Model;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Fiap.TechChallenge.Fase1.Integration.Tests;

[Collection("Test Integration")]
public class ContatoIntegrationTest
{
    private readonly FiapTechChallengeWebApplicationFactory _app;
    private DockerFixture _dockerFixture; 
    private const string _emailContatoTeste = "emailtestecontato@gmail.com";
    private const string _emailContatoTesteExcluir = "emailtestecontatoexcluir@gmail.com";
    private readonly IHost _host;

    public ContatoIntegrationTest()
    {
        _app = new FiapTechChallengeWebApplicationFactory();
        _dockerFixture = new DockerFixture();
    }

    [Fact]
    public async void Post_Criar_Novo_Contato()
    {
        // Arrange
        using var client = await _app.GetClientWithAccessTokenAsync();
        var email = "emailtestecontato2@gmail.com";

        CriarAlterarContatoDTO criarContato = new()
        {
            Email = email,
            Nome = "Nome Teste",
            Telefone = "994918888",
            DDD = 11,
        };

        BuscarContatoDTO buscarContato = new BuscarContatoDTO()
        {
            Email = email
        };

        // Act
        var resultado = await client.PostAsJsonAsync("/api/Contato/CriarContato", criarContato);

        await Task.Delay(60000);

        var resultadoBuscaEmail = await client.PostAsJsonAsync("/api/Contato/BuscarContatoPorEmail", buscarContato);
        var model = await resultadoBuscaEmail.Content.ReadFromJsonAsync<ResponseModelTeste>();
        var contatoEncontradoJson = model.Objeto.GetRawText();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var contatoEncontrado = JsonSerializer.Deserialize<ResponseBuscarContato>(contatoEncontradoJson, options);

        // Assert
        Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        Assert.Equal(criarContato.Email, contatoEncontrado.Contato.Email);
    }

    [Fact]
    public async void Get_Filtrar_Por_DDD()
    {
        // Arrange
        using var client = await _app.GetClientWithAccessTokenAsync();

        // Act
        var resultado = await client.GetAsync($"/api/Contato/FiltrarPorDDD/{11}");

        // Arrange
        Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
    }

    [Fact]
    public async void Post_Buscar_Contato_Por_Email()
    {
        // Arrange
        using var client = await _app.GetClientWithAccessTokenAsync();

        BuscarContatoDTO buscarContato = new()
        {
            Email = _emailContatoTeste
        };

        // Act
        var resultado = await client.PostAsJsonAsync("/api/Contato/BuscarContatoPorEmail", buscarContato);
        var model = await resultado.Content.ReadFromJsonAsync<ResponseModelTeste>();
        var contatoEncontradoJson = model.Objeto.GetRawText();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var contatoEncontrado = JsonSerializer.Deserialize<ResponseBuscarContato>(contatoEncontradoJson, options);


        // Arrange
        Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        //Assert.Equal(criarContato.Email, contatoEncontrado.Contato.Email);

    }

    [Fact]
    public async Task Put_Alterar_Contato()
    {
        // Arrange
        using var client = await _app.GetClientWithAccessTokenAsync();

        var buscarContato = new BuscarContatoDTO
        {
            Email = _emailContatoTeste
        };

        var resultadoBuscar = await client.PostAsJsonAsync("/api/Contato/BuscarContatoPorEmail", buscarContato);
        resultadoBuscar.EnsureSuccessStatusCode();

        var modelBuscar = await resultadoBuscar.Content.ReadFromJsonAsync<ResponseModelTeste>();
        var contatoBuscadoJson = modelBuscar.Objeto.GetRawText();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var contatoBuscado = JsonSerializer.Deserialize<ResponseBuscarContato>(contatoBuscadoJson, options);

        Assert.NotNull(contatoBuscado);
        //Assert.Equal(criarContato.Email, contatoBuscado.Contato.Email);

        var alterarContato = new CriarAlterarContatoDTO
        {
            Email = _emailContatoTeste,
            Nome = "Nome Teste Alterado",
            Telefone = "999999999",
            DDD = 12,
        };

        // Act - Alterar o contato
        var resultadoAlterar = await client.PutAsJsonAsync("/api/Contato/AlterarContato", alterarContato);
        Assert.Equal(HttpStatusCode.OK, resultadoAlterar.StatusCode);

        await Task.Delay(60000);

        var resultadoBuscaEmail = await client.PostAsJsonAsync("/api/Contato/BuscarContatoPorEmail", buscarContato);
        var model = await resultadoBuscaEmail.Content.ReadFromJsonAsync<ResponseModelTeste>();
        var contatoEncontradoJson = model.Objeto.GetRawText();

        var contatoEncontrado = JsonSerializer.Deserialize<ResponseBuscarContato>(contatoEncontradoJson, options);

        // Assert
        Assert.Equal(HttpStatusCode.OK, resultadoAlterar.StatusCode);
        Assert.Equal(alterarContato.Nome, contatoEncontrado.Contato.Nome);
    }

    [Fact]
    public async void Delete_Remover_Contato()
    {
        // Arrange
        using var client = await _app.GetClientWithAccessTokenAsync();

        BuscarContatoDTO buscarContato = new()
        {
            Email = _emailContatoTesteExcluir
        };

        var resultadoBuscar = await client.PostAsJsonAsync("/api/Contato/BuscarContatoPorEmail", buscarContato);
        var model = await resultadoBuscar.Content.ReadFromJsonAsync<ResponseModelTeste>();

        // Verificação da propriedade Objeto como string
        var contatoEncontradoJson = model.Objeto.GetRawText();

        // Desserializar para UsuarioDTO com case-insensitive options
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var contatoEncontrado = JsonSerializer.Deserialize<ResponseBuscarContato>(contatoEncontradoJson, options);

        // Act
        var resultado = await client.DeleteAsync($"/api/Contato/RemoverContato?id={contatoEncontrado.Contato.Id}");

        // Arrange
        Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
    }
}
