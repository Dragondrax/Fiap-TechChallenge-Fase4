using Fiap.TechChallenge.Fase1.Infraestructure.DTO;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Usuario;
using Fiap.TechChallenge.Fase1.SharedKernel.Filas;
using Fiap.TechChallenge.Fase3.Persistencia.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Fiap.TechChallenge.Fase3.Persistencia.Consumers
{
    public class ConsumersPersistencia : IConsumerPersistencia
    {
        private readonly IPersistirContatoRepository _persistirContatoRepository;
        private readonly IPersistirUsuarioRepository _persistirUsuarioRepository;
        public ConsumersPersistencia(IPersistirContatoRepository persistirContatoRepository, IPersistirUsuarioRepository persistirUsuarioRepository)
        {
            _persistirContatoRepository = persistirContatoRepository;
            _persistirUsuarioRepository = persistirUsuarioRepository;
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

                var result = await _persistirContatoRepository.AlterarContatoRepository(dadosMensagem);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasPersistencia.AtualizarContato,
                                 autoAck: false,
                                 consumer: consumer);
        }

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

                var result = await _persistirUsuarioRepository.AlterarUsuarioRepository(dadosMensagem);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasPersistencia.AtualizarUsuario,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public void ConsumerCadastrarContato(IModel channel)
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

                var result = await _persistirContatoRepository.CadastrarContatoRepository(dadosMensagem);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasPersistencia.CriarContato,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public void ConsumerCadastrarUsuario(IModel channel)
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

                var result = await _persistirUsuarioRepository.CadastrarUsuarioRepository(dadosMensagem);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasPersistencia.CriarUsuario,
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

                var result = await _persistirContatoRepository.RemoverContatoRepository(dadosMensagem.Id);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasPersistencia.ExcluirContato,
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

                var result = await _persistirUsuarioRepository.RemoverUsuarioRepository(dadosMensagem.Id);

                if (result)
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                    channel.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: FilasPersistencia.ExcluirUsuario,
                                 autoAck: false,
                                 consumer: consumer);
        }
    }
}
