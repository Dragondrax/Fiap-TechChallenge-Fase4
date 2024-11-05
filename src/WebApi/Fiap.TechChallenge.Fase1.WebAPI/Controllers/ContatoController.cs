using Fiap.TechChallenge.Fase1.Dominio;
using Fiap.TechChallenge.Fase1.Infraestructure.DTO.Contato;
using Fiap.TechChallenge.Fase1.SharedKernel;
using Fiap.TechChallenge.Fase1.SharedKernel.Filas;
using Fiap.TechChallenge.Fase1.SharedKernel.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.TechChallenge.Fase1.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContatoController(IContatoService contatoService, IPublicaMensagemNaFila publicarMensagem) : ControllerBase
{
    private readonly IContatoService _contatoService = contatoService;
    private readonly IPublicaMensagemNaFila _publicarMensagem = publicarMensagem;

    [HttpPost("CriarContato")]
    public async Task<IActionResult> SalvarNovoContato([FromBody] CriarAlterarContatoDTO contatoDTO)
    {
        var resultado  = await _publicarMensagem.PublicarMensagem(FilasContatos.CriarContatoService, Exchange.ValorExchange, contatoDTO);
        if (resultado)
            return Ok(resultado);
        else
            return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
    }


    [HttpGet("FiltrarPorDDD/{DDD}")]
    public async Task<IActionResult> BuscarContatosPorDDD(int DDD)
    {
        var resultado = await _contatoService.BuscarContatosPorDDD(DDD);

        if (resultado.Sucesso)
            return Ok(resultado);
        else if (resultado.Sucesso == false && resultado.Objeto is null && resultado.Mensagem.Any(x => string.IsNullOrEmpty(x)))
            return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
        else if (resultado.Sucesso == false && resultado.Objeto is null && resultado.Mensagem.Contains("Ops, o ddd informado não existe!"))
            return BadRequest(resultado);
        else
            return NotFound(resultado);  
    }

    [HttpPost("BuscarContatoPorEmail")]
    public async Task<IActionResult> BuscarContatoPorEmail(BuscarContatoDTO contatoDTO)
    {
        var resultado = await _contatoService.BuscarContatoPorEmail(contatoDTO);

        if (resultado.Sucesso)
            return Ok(resultado);
        else if (resultado.Sucesso == false && resultado.Objeto is null && resultado.Mensagem.Any(x => string.IsNullOrEmpty(x)))
            return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
        else if (resultado.Sucesso == false && resultado.Objeto is null && resultado.Mensagem.Contains("Ops, email não foi encontrado em nosso banco de dados!"))
            return NotFound(resultado);
        else 
            return BadRequest(resultado);
    }

    [HttpPut("AlterarContato")]
    public async Task<IActionResult> AtualizarContato([FromBody] CriarAlterarContatoDTO contatoDTO)
    {
        var resultado = await _publicarMensagem.PublicarMensagem(FilasContatos.AtualizarContatoService, Exchange.ValorExchange, contatoDTO);
        if (resultado)
            return Ok(resultado);
        else
            return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
    }

    [HttpDelete("RemoverContato")]
    public async Task<IActionResult> RemoverContato(Guid id)
    {
        var apagarContatoDto = new DeletarContatoDto
        {
            Id = id
        };

        var resultado = await _publicarMensagem.PublicarMensagem(FilasContatos.DeletarContatoService, Exchange.ValorExchange, apagarContatoDto);
        if (resultado)
            return Ok(resultado);
        else
            return StatusCode(500, MensagemErroGenerico.MENSAGEM_ERRO_500);
    }
}
