using Cadastro_Pessoa.Models;

namespace CadastroPessoa.Tests.Mocks;
public static class PessoaMock
{
    public static List<Pessoa> GetPessoas()
    {
        return new List<Pessoa>
            {
                new Pessoa
                {
                    Id = 1,
                    Nome = "João Silva",
                    Sexo = "Masculino",
                    Email = "joao.silva@email.com",
                    DataNascimento = new DateTime(1990, 5, 20),
                    Naturalidade = "São Paulo",
                    Nacionalidade = "Brasileiro",
                    CPF = "12345678901",
                    DataCadastro = DateTime.Now.AddMonths(-12),
                    DataAtualizacao = DateTime.Now.AddDays(-1)
                },
                new Pessoa
                {
                    Id = 2,
                    Nome = "Maria Oliveira",
                    Sexo = "Feminino",
                    Email = "maria.oliveira@email.com",
                    DataNascimento = new DateTime(1985, 8, 15),
                    Naturalidade = "Rio de Janeiro",
                    Nacionalidade = "Brasileira",
                    CPF = "23456789012",
                    DataCadastro = DateTime.Now.AddMonths(-10),
                    DataAtualizacao = DateTime.Now.AddDays(-2)
                },
                new Pessoa
                {
                    Id = 3,
                    Nome = "Carlos Pereira",
                    Sexo = "Masculino",
                    Email = "carlos.pereira@email.com",
                    DataNascimento = new DateTime(1995, 12, 30),
                    Naturalidade = "Belo Horizonte",
                    Nacionalidade = "Brasileiro",
                    CPF = "34567890123",
                    DataCadastro = DateTime.Now.AddMonths(-6),
                    DataAtualizacao = DateTime.Now.AddDays(-3)
                }
            };
    }
}

