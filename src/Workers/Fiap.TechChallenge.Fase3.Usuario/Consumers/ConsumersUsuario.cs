using Fiap.TechChallenge.Fase1.Infraestructure.DTO;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Usuario;
using Fiap.TechChallenge.Fase1.SharedKernel.Filas;
using Fiap.TechChallenge.Fase3.Usuario.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Fiap.TechChallenge.Fase3.Usuario.Consumers;

public class ConsumersUsuario(IUsuarioServices usuarioService) : IConsumersUsuario
{
    private readonly IUsuarioServices _usuarioService = usuarioService;

    public void ConsumerAtualizarUsuario(IModel channel)
    {
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var dadosMensagem = JsonSerializer.Deserialize<CriarAlterarUsuarioDTO>(message, options);

            var result = await _usuarioService.AlterarUsuarioService(dadosMensagem);

            if (result)
                channel.BasicAck(ea.DeliveryTag, false);
            else
                channel.BasicReject(ea.DeliveryTag, false);
        };

        channel.BasicConsume(queue: FilasUsuarios.AtualizarUsuarioService,
                             autoAck: false,
                             consumer: consumer);
    }

    public void ConsumerCriarUsuario(IModel channel)
    {
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var dadosMensagem = JsonSerializer.Deserialize<CriarAlterarUsuarioDTO>(message, options);

            var result = await _usuarioService.CriarUsuarioService(dadosMensagem);

            if (result)
                channel.BasicAck(ea.DeliveryTag, false);
            else
                channel.BasicReject(ea.DeliveryTag, false);
        };

        channel.BasicConsume(queue: FilasUsuarios.CriarUsuarioService,
                             autoAck: false,
                             consumer: consumer);
    }

    public void ConsumerDeletarUsuario(IModel channel)
    {
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var dadosMensagem = JsonSerializer.Deserialize<DeletarUsuarioDto>(message, options);

            var result = await _usuarioService.RemoverUsuarioService(dadosMensagem.Id);

            if (result)
                channel.BasicAck(ea.DeliveryTag, false);
            else
                channel.BasicReject(ea.DeliveryTag, false);
        };

        channel.BasicConsume(queue: FilasUsuarios.DeletarUsuarioService,
                             autoAck: false,
                             consumer: consumer);
    }
}

