
using System.Text.Json;
using System.Text;

namespace Fiap.TechChallenge.Fase1.SharedKernel.RabbitMQ
{
    public class PublicaMensagemNaFila : IPublicaMensagemNaFila
    {
        private readonly IConfiguracoesRabbitMQ _configuracaoRabbit;
        public PublicaMensagemNaFila(IConfiguracoesRabbitMQ configuracaoRabbit)
        {
            _configuracaoRabbit = configuracaoRabbit;
        }
        public async Task<bool> PublicarMensagem(string fila, string exchange, object messageBody)
        {
            var conn = await Task.FromResult(_configuracaoRabbit.AbrirConexaoRabbitMQ());
            var model = conn.CreateModel();
            var properties = model.CreateBasicProperties();
            var messageBodyEncoding = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageBody));
            model.BasicPublish(exchange, fila, false, properties, messageBodyEncoding);
            return true;
        }
    }
}
