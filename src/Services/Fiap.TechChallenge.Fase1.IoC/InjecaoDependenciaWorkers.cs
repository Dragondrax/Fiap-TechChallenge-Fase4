using Fiap.TechChallenge.Fase1.Aplicacao;
using Fiap.TechChallenge.Fase1.Aplicacao.DDDRegiao;
using Fiap.TechChallenge.Fase1.Data.Context;
using Fiap.TechChallenge.Fase1.Data.Repository;
using Fiap.TechChallenge.Fase1.Dominio;
using Fiap.TechChallenge.Fase1.SharedKernel.RabbitMQ;
using Fiap.TechChallenge.Fase3.RabbitMQ.GerenciamentoFilas;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.TechChallenge.Fase1.IoC
{
    public static class InjecaoDependenciaWorkers
    {
        public static IServiceCollection ResolverDependencia(this IServiceCollection services)
        {
            services.AddSingleton<ContextTechChallenge>();

            services.AddSingleton<IUsuarioRepository, UsuarioRepository>();
            services.AddSingleton<IUsuarioService, UsuarioService>();
            services.AddSingleton<IContatoRepository, ContatoRepository>();
            services.AddSingleton<IContatoService, ContatoService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IDDDRegiaoService, DDDRegiaoService>();
            services.AddSingleton<IDDDRegiaoRepository, DDDRegiaoRepository>();
            services.AddSingleton<IPublicaMensagemNaFila, PublicaMensagemNaFila>();
            services.AddSingleton<IGerenciamentoFilasRabbitMQ, GerenciamentoFilasRabbitMQ>();
            services.AddSingleton<IConfiguracoesRabbitMQ, ConfiguracoesRabbitMQ>();

            return services;
        }
    }
}
