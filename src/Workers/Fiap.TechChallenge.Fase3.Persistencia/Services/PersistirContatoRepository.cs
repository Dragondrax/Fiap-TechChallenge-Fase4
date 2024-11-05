using Fiap.TechChallenge.Fase1.Dominio;
using Fiap.TechChallenge.Fase1.Dominio.Entidades;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;

namespace Fiap.TechChallenge.Fase3.Persistencia.Services
{
    public class PersistirContatoRepository(IContatoRepository contatoRepository) : IPersistirContatoRepository
    {
        private readonly IContatoRepository _contatoRepository = contatoRepository;

        public async Task<bool> CadastrarContatoRepository(CriarAlterarContatoDTO contato)
        {
            try
            {
                var dadosContato = await _contatoRepository.ObterPorEmailAsync(contato.Email.ToLower());

                if (dadosContato != null)
                    return false;

                var novoContato = new Contato(contato.Nome, contato.DDD, contato.Telefone, contato.Email);

                await _contatoRepository.AdicionarAsync(novoContato);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AlterarContatoRepository(CriarAlterarContatoDTO contato)
        {
            try
            {
                var dadosContrato = await _contatoRepository.ObterPorEmailAsync(contato.Email.ToLower());

                if (dadosContrato == null)
                    return false;

                dadosContrato.AlterarContato(contato.Nome, contato.DDD, contato.Telefone, contato.Email);

                await _contatoRepository.AtualizarAsync(dadosContrato);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoverContatoRepository(Guid contatoId)
        {
            try
            {
                var contato = await _contatoRepository.ObterPorIdAsync(contatoId);

                if (contato == null)
                    return false;

                contato.ExcluirContato();

                await _contatoRepository.RemoverAsync(contato);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
