using Fiap.TechChallenge.Fase1.Infraestructure.DTO;

namespace Fiap.TechChallenge.Fase3.Persistencia.Services
{
    public interface IPersistirUsuarioRepository
    {
        Task<bool> CadastrarUsuarioRepository(CriarAlterarUsuarioDTO usuario);
        Task<bool> AlterarUsuarioRepository(CriarAlterarUsuarioDTO usuario);
        Task<bool> RemoverUsuarioRepository(Guid usuarioId);
    }
}
