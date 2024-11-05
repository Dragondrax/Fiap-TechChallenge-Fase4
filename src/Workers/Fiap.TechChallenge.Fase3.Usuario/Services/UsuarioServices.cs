using Fiap.TechChallenge.Fase1.Aplicacao;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO;

namespace Fiap.TechChallenge.Fase3.Usuario.Services;

public class UsuarioServices : IUsuarioServices
{
    private readonly IUsuarioService _usuarioService;
    public UsuarioServices(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }
    public async Task<bool> AlterarUsuarioService(CriarAlterarUsuarioDTO usuarioDto)
    {
        try
        {
            var validacao = new CriarAlterarUsuarioDTOValidator().Validate(usuarioDto);

            if (!validacao.IsValid)
            {
                return false;
            }

            var responseContato = await _usuarioService.AlterarUsuario(usuarioDto);

            if (responseContato.Sucesso)
                return true;

            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> CriarUsuarioService(CriarAlterarUsuarioDTO usuarioDto)
    {
        try
        {
            var validacao = new CriarAlterarUsuarioDTOValidator().Validate(usuarioDto);

            if (!validacao.IsValid)
            {
                return false;
            }

            var responseContato = await _usuarioService.SalvarUsuario(usuarioDto);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> RemoverUsuarioService(Guid usuarioId)
    {
        try
        {
            var responseContato = await _usuarioService.RemoverUsuario(usuarioId);

            if (responseContato.Sucesso)
                return true;

            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

}
