using RabbitMQ.Client;

namespace Fiap.TechChallenge.Fase3.Persistencia.Consumers
{
    public interface IConsumerPersistencia
    {
        public void ConsumerCadastrarUsuario(IModel channel);
        public void ConsumerCadastrarContato(IModel channel);
        public void ConsumerAtualizarUsuario(IModel channel);
        public void ConsumerAtualizarContato(IModel channel);
        public void ConsumerDeletarUsuario(IModel channel);
        public void ConsumerDeletarContato(IModel channel);
    }
}
