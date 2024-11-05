using Fiap.TechChallenge.Fase1.Data.Repository;
using Fiap.TechChallenge.Fase1.Dominio.Entidades;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO;

namespace Fiap.TechChallenge.Fase3.Persistencia.Services
{
    public class PersistirUsuarioRepository(IUsuarioRepository usuarioRepository) : IPersistirUsuarioRepository
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public async Task<bool> CadastrarUsuarioRepository(CriarAlterarUsuarioDTO usuario)
        {
            try
            {
                var dadosUsuario = await _usuarioRepository.ObterPorEmailAsync(usuario.Email.ToLower());

                if (dadosUsuario != null)
                    return false;

                var novoUsuario = new Usuario(usuario.Nome, usuario.Email, usuario.Senha, usuario.Role);

                await _usuarioRepository.AdicionarAsync(novoUsuario);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AlterarUsuarioRepository(CriarAlterarUsuarioDTO usuario)
        {
            try
            {
                var dadosUsuario = await _usuarioRepository.ObterPorEmailAsync(usuario.Email.ToLower());

                if (dadosUsuario == null)
                    return false;

                dadosUsuario.AlterarUsuario(usuario.Nome, usuario.Email.ToLower(), usuario.Senha, usuario.Role);

                await _usuarioRepository.AtualizarAsync(dadosUsuario);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoverUsuarioRepository(Guid usuarioId)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);

                if (usuario == null)
                    return false;

                usuario.ExcluirUsuario();

                await _usuarioRepository.RemoverAsync(usuario);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
