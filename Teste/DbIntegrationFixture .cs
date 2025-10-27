using Cadastro_Pessoa.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class DbIntegrationFixture : IDisposable, IClassFixture<DbIntegrationFixture>
{
    public string ConnectionString { get; private set; }
    public DataContext Context { get; private set; }

    public DbIntegrationFixture()
    {
        // Gera uma string de conexão única para cada execução
        ConnectionString = $"Server=(localdb)\\mssqllocaldb;Database=CadastroPessoaTests_{Guid.NewGuid():N};Trusted_Connection=True;";

        // Configura o DbContext
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        Context = new DataContext(options);

        // Garante que o banco de dados exista
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}