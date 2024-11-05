using Fiap.TechChallenge.Fase1.Dominio;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;

namespace Fiap.TechChallenge.Fase3.Contato.Services
{
    public class ContatoServices : IContatoServices
    {
        private readonly IContatoService _contatoService;
        public ContatoServices(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }
        public async Task<bool> AlterarContatoService(CriarAlterarContatoDTO contatoDto)
        {
            try
            {
                var validacao = new CriarContatoDTOValidator().Validate(contatoDto);

                if (!validacao.IsValid)
                {
                    return false;
                }

                var responseContato = await _contatoService.AlterarContato(contatoDto);

                if (responseContato.Sucesso)
                    return true;

                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CriarContatoService(CriarAlterarContatoDTO contatoDto)
        {
            try
            {
                var validacao = new CriarContatoDTOValidator().Validate(contatoDto);

                if (!validacao.IsValid)
                {
                    return false;
                }

                var responseContato = await _contatoService.SalvarContato(contatoDto);

                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public async Task<bool> RemoverContatoService(Guid contatoId)
        {
            try
            {
                var responseContato = await _contatoService.RemoverContato(contatoId);

                if (responseContato.Sucesso)
                    return true;

                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
