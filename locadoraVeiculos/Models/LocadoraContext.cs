using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace locadoraVeiculos.Models
{
    public class LocadoraContext : DbContext
    {
        public LocadoraContext(DbContextOptions<LocadoraContext> options)
            : base(options)
        {
        }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Manutencao> Manutencoes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {          
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Locadora;trusted_Connection=True;TrustServerCertificate=true;");
            
        }


    }
}
