using Microsoft.EntityFrameworkCore;
using WebApiEspera.Models;

namespace WebApiEspera.Contexto
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options)
            : base(options) => Database.EnsureCreated();
        
        public DbSet<Espera> Espera { get; set; }
        public DbSet<Atendimento> Atendimento { get; set; }
        public DbSet<Contagem> Contagem { get; set; }
    }
}
