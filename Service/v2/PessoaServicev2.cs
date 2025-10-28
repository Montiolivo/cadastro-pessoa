using AutoMapper;
using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;
using Cadastro_Pessoa.Service.Interfaces.v2;
using Cadastro_Pessoa.Services;
using Microsoft.EntityFrameworkCore;

namespace Cadastro_Pessoa.Service.v2;
public class PessoaServiceV2 : IPessoaServiceV2
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PessoaServiceV2(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PessoaV2Dto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<PessoaV2Dto>>(await _context.Pessoas.Include(p => p.Endereco).ToListAsync());


    public async Task<PessoaV2Dto> GetByIdAsync(int id)
    {
        var pessoa = await _context.Pessoas
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pessoa == null)
            throw new KeyNotFoundException("Pessoa não encontrada.");

        return _mapper.Map<PessoaV2Dto>(pessoa);
    }


    public async Task<PessoaV2Dto> CreateAsync(PessoaV2Dto dto)
    {
        if (dto.Endereco == null)
            throw new ArgumentException("Endereço é obrigatório.");

        PessoaValidator.ValidarPessoa(new Pessoa { Nome = dto.Nome, CPF = dto.CPF, DataNascimento = dto.DataNascimento }, _context);

        var pessoa = _mapper.Map<Pessoa>(dto);
        pessoa.DataCadastro = DateTime.UtcNow;
        pessoa.DataAtualizacao = DateTime.UtcNow;

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return _mapper.Map<PessoaV2Dto>(pessoa);
    }


    public async Task<PessoaV2Dto> UpdateAsync(int id, PessoaV2Dto dto)
    {
        var pessoa = await _context.Pessoas
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pessoa == null)
            throw new KeyNotFoundException("Pessoa não encontrada.");

        if (dto.Endereco == null)
            throw new ArgumentException("Endereço é obrigatório.");

        PessoaValidator.ValidarPessoa(new Pessoa { Nome = dto.Nome, CPF = dto.CPF, DataNascimento = dto.DataNascimento }, _context, id);

        _mapper.Map(dto, pessoa);
        pessoa.DataAtualizacao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<PessoaV2Dto>(pessoa);
    }


    public async Task DeleteAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null)
            throw new KeyNotFoundException("Pessoa não encontrada.");

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
    }
}

