using Fiap.TechChallenge.Fase1.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Fiap.TechChallenge.Fase1.Data.Seed
{
    internal static class ContatoTesteSeed
    {
        internal static void AdicionarContatoTeste(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contato>().HasData(new Contato("Nome Teste", 11, "994918888", "emailtestecontato@gmail.com"));
            modelBuilder.Entity<Contato>().HasData(new Contato("Nome Teste", 11, "994918888", "emailtestecontatoexcluir@gmail.com"));
        }
    }
}
