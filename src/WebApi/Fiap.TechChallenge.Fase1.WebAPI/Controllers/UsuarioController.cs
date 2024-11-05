using Fiap.TechChallenge.Fase1.Aplicacao;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Usuario;
using Fiap.TechChallenge.Fase1.SharedKernel;
using Fiap.TechChallenge.Fase1.SharedKernel.Filas;
using Fiap.TechChallenge.Fase1.SharedKernel.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.TechChallenge.Fase1.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPublicaMensagemNaFila _publicarMensagem;

        public UsuarioController(IUsuarioService usuarioService, IPublicaMensagemNaFila publicarMensagem)
        {
            _usuarioService = usuarioService;
            _publicarMensagem = publicarMensagem;
        }

        [HttpPost("CriarUsuario")]
        public async Task<IActionResult> SalvarNovoUsuario(CriarAlterarUsuarioDTO usuario)
        {
            var resultado = await _publicarMensagem.PublicarMensagem(FilasUsuarios.CriarUsuarioService, Exchange.ValorExchange, usuario);
            if (resultado)
                return Ok(resultado);
            else
                return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
        }

        [HttpPost("Autenticar")]
        [AllowAnonymous]
        public async Task<IActionResult> Autenticar(AutenticarUsuarioDTO usuario)
        {
            var resultado = await _usuarioService.AutenticarUsuario(usuario);
            if (resultado.Sucesso == true)
                return Ok(resultado);
            else if (resultado.Sucesso == false && resultado.Objeto is null && resultado.Mensagem.Any(x => String.IsNullOrEmpty(x)))
                return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
            else
                return BadRequest(resultado);
        }

        [HttpPost("BuscarUsuario")]
        public async Task<IActionResult> BuscarUsuario(BuscarUsuarioDTO usuario)
        {
            var resultado = await _usuarioService.BuscarUsuarioPorEmail(usuario);
            if (resultado.Sucesso == true)
                return Ok(resultado);
            else if (resultado.Sucesso == false && resultado.Objeto is null && resultado.Mensagem.Any(x => String.IsNullOrEmpty(x)))
                return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
            else
                return BadRequest(resultado);
        }

        [HttpPut("AlterarUsuario")]
        public async Task<IActionResult> AlterarUsuario(CriarAlterarUsuarioDTO usuario)
        {
            try
            {
                var resultado = await _publicarMensagem.PublicarMensagem(FilasUsuarios.AtualizarUsuarioService, Exchange.ValorExchange, usuario);
                if (resultado)
                    return Ok(resultado);
                else
                    return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
            }
            catch(Exception ex)
            {
                return null;
            }   

        }

        [HttpDelete("RemoverUsuario")]
        public async Task<IActionResult> RemoverUsuario(Guid id)
        {
            var deletarUsuarioDto = new DeletarUsuarioDto()
            {
                Id = id
            };

            var resultado = await _publicarMensagem.PublicarMensagem(FilasUsuarios.DeletarUsuarioService, Exchange.ValorExchange, deletarUsuarioDto);
            if (resultado)
                return Ok(resultado);
            else
                return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
        }
    }
}
