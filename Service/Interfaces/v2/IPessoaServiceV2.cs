using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;

namespace Cadastro_Pessoa.Service.Interfaces.v2;
public interface IPessoaServiceV2
{
    Task<IEnumerable<PessoaV2Dto>> GetAllAsync();
    Task<PessoaV2Dto> GetByIdAsync(int id);
    Task<PessoaV2Dto> CreateAsync(PessoaV2Dto dto);
    Task<PessoaV2Dto> UpdateAsync(int id, PessoaV2Dto dto);
    Task DeleteAsync(int id);
}

