using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;

namespace Fiap.TechChallenge.Fase3.Contato.Services
{
    public interface IContatoServices
    {
        Task<bool> CriarContatoService(CriarAlterarContatoDTO contatoDto);
        Task<bool> AlterarContatoService(CriarAlterarContatoDTO contatoDto);
        Task<bool> RemoverContatoService(Guid contatoId);
    }
}
