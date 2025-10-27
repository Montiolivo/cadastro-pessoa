namespace Cadastro_Pessoa.Models.DTO;
public class PessoaV1Dto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Naturalidade { get; set; } = string.Empty;
    public string Nacionalidade { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
}


