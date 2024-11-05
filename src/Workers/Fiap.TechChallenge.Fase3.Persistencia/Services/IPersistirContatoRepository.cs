using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;

namespace Fiap.TechChallenge.Fase3.Persistencia.Services
{
    public interface IPersistirContatoRepository
    {
        Task<bool> CadastrarContatoRepository(CriarAlterarContatoDTO contato);
        Task<bool> AlterarContatoRepository(CriarAlterarContatoDTO contato);
        Task<bool> RemoverContatoRepository(Guid contatoId);
    }
}
