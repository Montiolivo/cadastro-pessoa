using System.ComponentModel.DataAnnotations;

namespace Cadastro_Pessoa.Models.DTO;
public class PessoaV2Dto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    public string Sexo { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }

    public string Naturalidade { get; set; } = string.Empty;

    public string Nacionalidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF é obrigatório")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos numéricos")]
    public string CPF { get; set; } = string.Empty;

    [Required(ErrorMessage = "Endereço é obrigatório")]
    public Endereco Endereco { get; set; } = new Endereco();
}

