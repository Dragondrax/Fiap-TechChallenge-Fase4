using RabbitMQ.Client;

namespace Fiap.TechChallenge.Fase1.SharedKernel.RabbitMQ
{
    public class ConfiguracoesRabbitMQ : IConfiguracoesRabbitMQ
    {
        public IConnection AbrirConexaoRabbitMQ()
        {
            return ObterConexaoRabbitMQ().CreateConnection();
        }
        private ConnectionFactory ObterConexaoRabbitMQ()
        {
            return new ConnectionFactory { HostName = "host.docker.internal", UserName = "techchallenge", Password = "techchallenge" };
        }
    }
}
