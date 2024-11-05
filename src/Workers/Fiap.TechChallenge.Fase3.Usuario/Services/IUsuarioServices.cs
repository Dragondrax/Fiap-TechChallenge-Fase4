using Fiap.TechChallenge.Fase1.Infraestructure.DTO;

namespace Fiap.TechChallenge.Fase3.Usuario.Services;

public interface IUsuarioServices
{
    Task<bool> AlterarUsuarioService(CriarAlterarUsuarioDTO usuarioDto);
    Task<bool> CriarUsuarioService(CriarAlterarUsuarioDTO usuarioDto);
    Task<bool> RemoverUsuarioService(Guid usuarioId);
}
