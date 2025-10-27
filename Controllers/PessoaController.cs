using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;
using Cadastro_Pessoa.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro_Pessoa.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaService _service;

        public PessoaController(IPessoaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaV1Dto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaV1Dto>> GetById(int id)
        {
            try { return Ok(await _service.GetByIdAsync(id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
        }

        [HttpPost]
        public async Task<ActionResult<PessoaV1Dto>> Create(PessoaV1Dto pessoa)
        {
            try { return Ok(await _service.CreateAsync(pessoa)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PessoaV1Dto>> Update(int id, PessoaV1Dto pessoa)
        {
            try { return Ok(await _service.UpdateAsync(id, pessoa)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try { await _service.DeleteAsync(id); return NoContent(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
        }
    }
}
