using RabbitMQ.Client;

namespace Fiap.TechChallenge.Fase3.Usuario.Consumers;

public interface IConsumersUsuario
{
    public void ConsumerCriarUsuario(IModel channel);
    public void ConsumerAtualizarUsuario(IModel channel);
    public void ConsumerDeletarUsuario(IModel channel);
}
