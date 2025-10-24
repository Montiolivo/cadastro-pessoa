using Cadastro_Pessoa.Models;
using Microsoft.EntityFrameworkCore;

namespace Cadastro_Pessoa.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
            : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>()
            .HasIndex(p => p.CPF)
            .IsUnique(true);
    }
}
