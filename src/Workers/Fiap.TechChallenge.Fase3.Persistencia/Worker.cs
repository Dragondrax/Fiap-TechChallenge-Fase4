using Fiap.TechChallenge.Fase1.SharedKernel.RabbitMQ;
using Fiap.TechChallenge.Fase3.Persistencia.Consumers;
using Fiap.TechChallenge.Fase3.RabbitMQ.GerenciamentoFilas;

namespace Fiap.TechChallenge.Fase3.Persistencia
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguracoesRabbitMQ _configuracoesRabbit;
        private readonly IGerenciamentoFilasRabbitMQ _gerenciamentoFila;
        private readonly IConsumerPersistencia _consumer;

        public Worker(ILogger<Worker> logger,
                      IConfiguracoesRabbitMQ configuracoesRabbit,
                      IGerenciamentoFilasRabbitMQ gerenciamentoFila,
                      IConsumerPersistencia consumer)
        {
            _logger = logger;
            _configuracoesRabbit = configuracoesRabbit;
            _consumer = consumer;
            _gerenciamentoFila = gerenciamentoFila;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = _configuracoesRabbit.AbrirConexaoRabbitMQ();
            var channel = connection.CreateModel();
            channel.BasicQos(0, 1, false);

            _gerenciamentoFila.CriarFilas(channel);
            _consumer.ConsumerCadastrarContato(channel);
            _consumer.ConsumerAtualizarContato(channel);
            _consumer.ConsumerDeletarContato(channel);
            _consumer.ConsumerCadastrarUsuario(channel);
            _consumer.ConsumerAtualizarUsuario(channel);
            _consumer.ConsumerDeletarUsuario(channel);
        }
    }
}
