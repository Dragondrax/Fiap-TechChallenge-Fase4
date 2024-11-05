using RabbitMQ.Client;

namespace Fiap.TechChallenge.Fase3.RabbitMQ.GerenciamentoFilas
{
    public interface IGerenciamentoFilasRabbitMQ
    {
        void CriarFilas(IModel channel);
    }
}
