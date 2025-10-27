using System.ComponentModel.DataAnnotations;

namespace Cadastro_Pessoa.Models
{
    public class Endereco
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório")]
        public string Logradouro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número é obrigatório")]
        public string Numero { get; set; } = string.Empty;

        public string Complemento { get; set; } = string.Empty;

        [Required(ErrorMessage = "O bairro é obrigatório")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório")]
        [StringLength(2, ErrorMessage = "O estado deve conter 2 letras")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "CEP deve conter 8 dígitos numéricos")]
        public string CEP { get; set; } = string.Empty;
    }
}
