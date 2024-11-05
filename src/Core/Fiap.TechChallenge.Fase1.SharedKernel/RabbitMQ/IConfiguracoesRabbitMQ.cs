using RabbitMQ.Client;

namespace Fiap.TechChallenge.Fase1.SharedKernel.RabbitMQ
{
    public interface IConfiguracoesRabbitMQ
    {
        IConnection AbrirConexaoRabbitMQ();
    }
}
