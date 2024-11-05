using Fiap.TechChallenge.Fase1.SharedKernel.Filas;
using RabbitMQ.Client;

namespace Fiap.TechChallenge.Fase3.RabbitMQ.GerenciamentoFilas
{
    public class GerenciamentoFilasRabbitMQ : IGerenciamentoFilasRabbitMQ
    {
        private string ExchangeString = Exchange.ValorExchange;
        private string ExchangeDeadLetterString = Exchange.ValorExchangeDeadLetter;

        public void CriarFilas(IModel channel)
        {
            DeclararExchange(channel);
            CriarFila(channel,
                      FilasPersistencia.CriarContato,
                      ExchangeString,
                      FilasPersistencia.CriarContatoDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasPersistencia.CriarUsuario,
                      ExchangeString,
                      FilasPersistencia.CriarUsuarioDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasPersistencia.AtualizarContato,
                      ExchangeString,
                      FilasPersistencia.AtualizarContatoDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasPersistencia.AtualizarUsuario,
                      ExchangeString,
                      FilasPersistencia.AtualizarUsuarioDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasPersistencia.ExcluirContato,
                      ExchangeString,
                      FilasPersistencia.ExcluirContatoDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasPersistencia.ExcluirUsuario,
                      ExchangeString,
                      FilasPersistencia.ExcluirUsuarioDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasContatos.CriarContatoService,
                      ExchangeString,
                      FilasContatos.CriarContatoServiceDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasContatos.AtualizarContatoService,
                      ExchangeString,
                      FilasContatos.AtualizarContatoServiceDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasContatos.DeletarContatoService,
                      ExchangeString,
                      FilasContatos.DeletarContatoServiceDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasUsuarios.CriarUsuarioService,
                      ExchangeString,
                      FilasUsuarios.CriarUsuarioServiceDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasUsuarios.AtualizarUsuarioService,
                      ExchangeString,
                      FilasUsuarios.AtualizarUsuarioServiceDeadLetter,
                      ExchangeDeadLetterString);

            CriarFila(channel,
                      FilasUsuarios.DeletarUsuarioService,
                      ExchangeString,
                      FilasUsuarios.DeletarUsuarioServiceDeadLetter,
                      ExchangeDeadLetterString);
        }
        private void CriarFila(IModel channel, string fila, string exchange, string filaDeadLetter, string exchangeDeadLetter)
        {
            //Cria Fila Normal
            var argsFila = ObterArgumentoDaFila(fila, exchangeDeadLetter);
            channel.QueueDeclare(queue: fila,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: argsFila);

            channel.QueueBind(fila, exchange, fila, null);

            //Cria Fila DeadLetter
            var argsDlq = ObterArgumentoDaFilaDeadLetter(filaDeadLetter, exchange);
            channel.QueueDeclare(filaDeadLetter, true, false, false, argsDlq);
            channel.QueueBind(filaDeadLetter, exchangeDeadLetter, fila, null);

        }
        private void DeclararExchange(IModel channel)
        {
            channel.ExchangeDeclare(ExchangeString, "direct", true, false);
            channel.ExchangeDeclare(ExchangeDeadLetterString, "direct", true, false);
        }
        private Dictionary<string, object> ObterArgumentoDaFila(string fila, string exchangeDeadLetter)
        {
            var args = new Dictionary<string, object>();

            args.Add("x-dead-letter-exchange", exchangeDeadLetter);
            args.Add("x-queue-mode", "lazy");

            return args;
        }

        private Dictionary<string, object> ObterArgumentoDaFilaDeadLetter(string fila, string exchange)
        {
            var argsDlq = new Dictionary<string, object>();

            argsDlq.Add("x-queue-mode", "lazy");
            argsDlq.Add("x-dead-letter-exchange", exchange);

            return argsDlq;
        }
    }
}
