using Fiap.TechChallenge.Fase1.Infraestructure.DTO;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Usuario;
using Fiap.TechChallenge.Fase1.Infraestructure.Enum;
using Fiap.TechChallenge.Fase1.Integration.Tests;
using Fiap.TechChallenge.Fase1.Integration.Tests.Infra;
using Fiap.TechChallenge.Fase1.Integration.Tests.Model;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Fiap.TechChallenge.Fase1.Integration.Test
{
    [Collection("Test Integration")]
    public class UsuarioIntegrationTest
    {
        private readonly FiapTechChallengeWebApplicationFactory _app;
        private DockerFixture _dockerFixture;
        private const string _emailUsuarioTeste = "emailtesteusuario@gmail.com";

        public UsuarioIntegrationTest()
        {
            _app = new FiapTechChallengeWebApplicationFactory();
            _dockerFixture = new DockerFixture();
        }

        /// <summary>
        /// Primeiro Teste realiza a autenticação com um usuario padrão definido também dentro das secrets do CI do Github ACtions e configurado aqui na API
        /// </summary>
        [Fact]
        public async void Post_Efetua_Login_Com_Sucesso()
        {
            // Arrange
            using var client = _app.CreateClient();

            var usuario = new AutenticarUsuarioDTO
            {
                Email = _app._email,
                Senha = _app._senha
            };

            // Act
            var resultado = await client.PostAsJsonAsync("/api/Usuario/Autenticar", usuario);

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }

        /// <summary>
        /// Faz autenticação e busca um usuário padrão para retorno positivo e teste de integração
        /// </summary>
        [Fact]
        public async void Post_Busca_Usuario_Com_Sucesso()
        {
            // Arrange
            using var client = await _app.GetClientWithAccessTokenAsync();

            var usuario = new BuscarUsuarioDTO
            {
                Email = _app._email
            };

            // Act
            var resultado = await client.PostAsJsonAsync("/api/Usuario/BuscarUsuario", usuario);
            var aux = resultado.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }

        [Fact]
        public async void Post_Salvar_Novo_Usuario()
        {
            // Arrange
            using var client = await _app.GetClientWithAccessTokenAsync();
            var email = "emailtesteusuario2@gmail.com";

            CriarAlterarUsuarioDTO criarUsuarioDTO = new()
            {
                Email = email,
                Nome = "Nome Teste",
                Role = (Roles)1,
                Senha = "senha123456"
            };

            BuscarUsuarioDTO buscarUsuario = new BuscarUsuarioDTO()
            {
                Email = email
            };

            // Act
            var resultado = await client.PostAsJsonAsync("/api/Usuario/CriarUsuario", criarUsuarioDTO);

            await Task.Delay(60000);

            var usuarioRetornado = await client.PostAsJsonAsync("/api/Usuario/BuscarUsuario", buscarUsuario);
            usuarioRetornado.EnsureSuccessStatusCode();

            var model = await usuarioRetornado.Content.ReadFromJsonAsync<ResponseModelTeste>();

            // Verificação da propriedade Objeto como string
            var usuarioEncontradoJson = model.Objeto.GetRawText();

            // Desserializar para UsuarioDTO com case-insensitive options
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var usuarioEncontrado = JsonSerializer.Deserialize<UsuarioDTO>(usuarioEncontradoJson, options);

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
            Assert.Equal(HttpStatusCode.OK, usuarioRetornado.StatusCode);
            Assert.Equal(criarUsuarioDTO.Email, usuarioEncontrado.Email);
        }

        [Fact]
        public async void Put_Alterar_Usuario()
        {
            // Arrange
            using var client = await _app.GetClientWithAccessTokenAsync();

            // Buscar usuário
            var usuario = new BuscarUsuarioDTO
            {
                Email = _emailUsuarioTeste
            };

            var usuarioRetornado = await client.PostAsJsonAsync("/api/Usuario/BuscarUsuario", usuario);
            usuarioRetornado.EnsureSuccessStatusCode();

            var model = await usuarioRetornado.Content.ReadFromJsonAsync<ResponseModelTeste>();

            // Verificação da propriedade Objeto como string
            var usuarioEncontradoJson = model.Objeto.GetRawText();

            // Deserializar para UsuarioDTO com case-insensitive options
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var usuarioEncontrado = JsonSerializer.Deserialize<UsuarioDTO>(usuarioEncontradoJson, options);

            // Assert se o usuário foi encontrado
            Assert.NotNull(usuarioEncontrado);
            Assert.NotEqual(Guid.Empty, usuarioEncontrado.Id);

            CriarAlterarUsuarioDTO alterarUsuarioDTO = new()
            {
                Email = usuarioEncontrado.Email,
                Nome = "Nome Teste Alterado",
                Role = (Roles)1,
                Senha = "senha12345678"
            };

            // Act
            var resultado = await client.PutAsJsonAsync("/api/Usuario/AlterarUsuario", alterarUsuarioDTO);

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);

            await Task.Delay(60000);

            usuarioRetornado = await client.PostAsJsonAsync("/api/Usuario/BuscarUsuario", usuario);
            usuarioRetornado.EnsureSuccessStatusCode();

            model = await usuarioRetornado.Content.ReadFromJsonAsync<ResponseModelTeste>();

            // Verificação da propriedade Objeto como string
            usuarioEncontradoJson = model.Objeto.GetRawText();

            // Deserializar para UsuarioDTO com case-insensitive options
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            usuarioEncontrado = JsonSerializer.Deserialize<UsuarioDTO>(usuarioEncontradoJson, options);

            // Assert se o usuário foi encontrado
            Assert.NotNull(usuarioEncontrado);
            Assert.NotEqual(Guid.Empty, usuarioEncontrado.Id);
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
            Assert.Equal(alterarUsuarioDTO.Nome, usuarioEncontrado.Nome);
        }

        [Fact]
        public async Task Delete_Usuario_Test()
        {
            using var client = await _app.GetClientWithAccessTokenAsync();
            var emailExcluir = "emailtesteusuarioexcluir@gmail.com";
            // Buscar usuário
            var usuario = new BuscarUsuarioDTO
            {
                Email = emailExcluir
            };

            var usuarioRetornado = await client.PostAsJsonAsync("/api/Usuario/BuscarUsuario", usuario);
            usuarioRetornado.EnsureSuccessStatusCode();

            var model = await usuarioRetornado.Content.ReadFromJsonAsync<ResponseModelTeste>();

            // Verificação da propriedade Objeto como string
            var usuarioEncontradoJson = model.Objeto.GetRawText();

            // Desserializar para UsuarioDTO com case-insensitive options
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var usuarioEncontrado = JsonSerializer.Deserialize<UsuarioDTO>(usuarioEncontradoJson, options);

            // Assert se o usuário foi encontrado
            Assert.NotNull(usuarioEncontrado);
            Assert.NotEqual(Guid.Empty, usuarioEncontrado.Id);

            // Excluir usuário
            var resultado = await client.DeleteAsync($"/api/Usuario/RemoverUsuario?id={usuarioEncontrado.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);

            await Task.Delay(60000);


            // Buscar usuário
            usuario = new BuscarUsuarioDTO
            {
                Email = emailExcluir
            };

            usuarioRetornado = await client.PostAsJsonAsync("/api/Usuario/BuscarUsuario", usuario);
            model = await usuarioRetornado.Content.ReadFromJsonAsync<ResponseModelTeste>();

            // Verificação da propriedade Objeto como string
            usuarioEncontradoJson = model.Objeto.GetRawText();

            // Deserializar para UsuarioDTO com case-insensitive options
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            usuarioEncontrado = JsonSerializer.Deserialize<UsuarioDTO>(usuarioEncontradoJson, options);

            // Assert se o usuário não foi encontrado
            Assert.Null(usuarioEncontrado);
        }
    }
}