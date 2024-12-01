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
            return new ConnectionFactory { HostName = "localhost", UserName = "techchallenge", Password = "techchallenge" };
        }
    }
}
