using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using System.Text.RegularExpressions;

namespace Cadastro_Pessoa.Services;
public static class PessoaValidator
{
    public static void ValidarPessoa(Pessoa pessoa, DataContext context, int? idAtual = null)
    {
        if (!string.IsNullOrEmpty(pessoa.Email) && !Regex.IsMatch(pessoa.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            throw new ArgumentException("E-mail inválido.");

        if (!ValidarCPF(pessoa.CPF))
            throw new ArgumentException("CPF inválido.");

        if (context.Pessoas.Any(p => p.CPF == pessoa.CPF && p.Id != idAtual))
            throw new ArgumentException("CPF já cadastrado.");

        if (DateTime.Now.Year - pessoa.DataNascimento.Year > 150)
            throw new ArgumentException("Data de nascimento inválida.");
    }

    public static bool ValidarCPF(string cpf)
    {
        if (!Regex.IsMatch(cpf, @"^\d{11}$")) return false;

        int[] digitos = cpf.Select(d => int.Parse(d.ToString())).ToArray();

        int soma1 = 0;
        for (int i = 0; i < 9; i++) soma1 += digitos[i] * (10 - i);
        int digito1 = soma1 % 11 < 2 ? 0 : 11 - (soma1 % 11);

        int soma2 = 0;
        for (int i = 0; i < 10; i++) soma2 += digitos[i] * (11 - i);
        int digito2 = soma2 % 11 < 2 ? 0 : 11 - (soma2 % 11);

        return digito1 == digitos[9] && digito2 == digitos[10];
    }
}
