using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;
using Fiap.TechChallenge.Fase1.SharedKernel.Filas;
using Fiap.TechChallenge.Fase3.Contato.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Fiap.TechChallenge.Fase3.Contato.Consumers
{
    public class ConsumersContato : IConsumersContato
    {
        private readonly IContatoServices _contatoService;
        public ConsumersContato(IContatoServices contatoService)
        {
            _contatoService = contatoService;
        }
        public void ConsumerAtualizarContato(IModel channel)
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
                var dadosMensagem = JsonSerializer.Deserialize<CriarAlterarContatoDTO>(message, options);

                var result = await _contatoService.AlterarContatoService(dadosMensagem);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasContatos.AtualizarContatoService,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public void ConsumerCriarContato(IModel channel)
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
                var dadosMensagem = JsonSerializer.Deserialize<CriarAlterarContatoDTO>(message, options);

                var result = await _contatoService.CriarContatoService(dadosMensagem);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasContatos.CriarContatoService,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public void ConsumerDeletarContato(IModel channel)
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
                var dadosMensagem = JsonSerializer.Deserialize<DeletarContatoDto>(message, options);

                var result = await _contatoService.RemoverContatoService(dadosMensagem.Id);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasContatos.DeletarContatoService,
                                 autoAck: false,
                                 consumer: consumer);
        }
    }
}
