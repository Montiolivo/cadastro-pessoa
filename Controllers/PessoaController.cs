using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cadastro_Pessoa.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{
    private readonly DataContext _context;

    public PessoaController(DataContext context)
    {
        _context = context;
    }

    // GET api/pessoas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetAll()
    {
        return await _context.Pessoas.ToListAsync();
    }

    // GET api/pessoas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Pessoa>> GetById(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
            return NotFound();

        return pessoa;
    }

    // POST api/pessoas
    [HttpPost]
    public async Task<ActionResult<Pessoa>> Create(Pessoa pessoa)
    {
        try
        {
            // Validações adicionais
            if (!ValidarEmail(pessoa.Email))
                return BadRequest("E-mail inválido");

            if (!ValidarCPF(pessoa.CPF))
                return BadRequest("CPF inválido");

            if (await CPFJaExiste(pessoa.CPF))
                return BadRequest("CPF já cadastrado");

            if (DateTime.Now.Year - pessoa.DataNascimento.Year > 150)
                return BadRequest("Data de nascimento inválida");

            pessoa.DataCadastro = DateTime.UtcNow;
            pessoa.DataAtualizacao = DateTime.UtcNow;

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, pessoa);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao criar pessoa: {ex.Message}");
        }
    }

    // PUT api/pessoas/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Pessoa>> Update(int id, Pessoa pessoaAtualizada)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
            return NotFound();

        try
        {
            pessoa.Nome = pessoaAtualizada.Nome;
            pessoa.Sexo = pessoaAtualizada.Sexo;
            pessoa.Email = pessoaAtualizada.Email;
            pessoa.DataNascimento = pessoaAtualizada.DataNascimento;
            pessoa.Naturalidade = pessoaAtualizada.Naturalidade;
            pessoa.Nacionalidade = pessoaAtualizada.Nacionalidade;
            pessoa.CPF = pessoaAtualizada.CPF;
            pessoa.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(pessoa);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao atualizar pessoa: {ex.Message}");
        }
    }

    // DELETE api/pessoas/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
            return NotFound();

        try
        {
            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao deletar pessoa: {ex.Message}");
        }
    }

    private bool ValidarEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return true; // Email é opcional

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
    }

    private bool ValidarCPF(string cpf)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(cpf, @"^\d{11}$"))
            return false;

        int[] digitos = cpf.Select(d => int.Parse(d.ToString())).ToArray();

        int soma1 = 0;
        for (int i = 0; i < 9; i++)
            soma1 += digitos[i] * (10 - i);

        int digito1 = soma1 % 11 < 2 ? 0 : 11 - (soma1 % 11);

        int soma2 = 0;
        for (int i = 0; i < 10; i++)
            soma2 += digitos[i] * (11 - i);

        int digito2 = soma2 % 11 < 2 ? 0 : 11 - (soma2 % 11);

        return digito1 == digitos[9] && digito2 == digitos[10];
    }

    private async Task<bool> CPFJaExiste(string cpf)
    {
        return await _context.Pessoas.AnyAsync(p => p.CPF == cpf);
    }
}

