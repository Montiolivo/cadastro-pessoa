using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

public class AuthControllerTests : IDisposable
{
    private readonly DataContext _context;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "AuthControllerTests")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new DataContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _context.Usuarios.Add(new Usuario { Username = "admin", Password = "123456" });
        _context.SaveChanges();

        _controller = new AuthController(_context);
    }

    [Fact]
    public void Login_ComCredenciaisValidas_DeveRetornarToken()
    {
        var result = _controller.Login(new Usuario { Username = "admin", Password = "123456" });
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenObj = Assert.IsAssignableFrom<dynamic>(okResult.Value);
        Assert.NotNull(tokenObj.Token.ToString());
        Assert.True(tokenObj.Token.ToString().Length > 10);
    }

    [Fact]
    public void Login_ComCredenciaisInvalidas_DeveRetornarUnauthorized()
    {
        var result = _controller.Login(new Usuario { Username = "erro", Password = "teste" });

        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Usuário ou senha inválidos", unauthorizedResult.Value);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
