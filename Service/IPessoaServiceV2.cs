using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;

namespace Cadastro_Pessoa.Services.v2;
public interface IPessoaServiceV2
{
    Task<IEnumerable<Pessoa>> GetAllAsync();
    Task<Pessoa> GetByIdAsync(int id);
    Task<Pessoa> CreateAsync(PessoaV2Dto dto);
    Task<Pessoa> UpdateAsync(int id, PessoaV2Dto dto);
    Task DeleteAsync(int id);
}

