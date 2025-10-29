using Cadastro_Pessoa.Controllers.v2;
using Cadastro_Pessoa.Models.DTO;
using Cadastro_Pessoa.Service.Interfaces.v2;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CadastroPessoa.Tests.Controllers
{
    public class PessoaControllerV2Tests
    {
        private readonly Mock<IPessoaServiceV2> _mockService;
        private readonly PessoaController _controller;

        public PessoaControllerV2Tests()
        {
            _mockService = new Mock<IPessoaServiceV2>();
            _controller = new PessoaController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOkComListaDePessoas()
        {
            var pessoas = new List<PessoaV2Dto> { new PessoaV2Dto { Id = 1, Nome = "Miguel" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(pessoas);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var lista = Assert.IsAssignableFrom<IEnumerable<PessoaV2Dto>>(okResult.Value);
            Assert.Single(lista);
        }


        [Fact]
        public async Task GetById_ComIdExistente_DeveRetornarOk()
        {
            var pessoa = new PessoaV2Dto { Id = 1, Nome = "João" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(pessoa);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<PessoaV2Dto>(okResult.Value);
            Assert.Equal("João", dto.Nome);
        }

        [Fact]
        public async Task GetById_ComIdInexistente_DeveRetornarNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(99))
                .ThrowsAsync(new KeyNotFoundException("Pessoa não encontrada"));

            var result = await _controller.GetById(99);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Pessoa não encontrada", notFound.Value);
        }

        [Fact]
        public async Task Create_ComDadosValidos_DeveRetornarOk()
        {
            var dto = new PessoaV2Dto { Id = 1, Nome = "Maria" };
            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(dto);

            var result = await _controller.Create(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pessoaCriada = Assert.IsType<PessoaV2Dto>(okResult.Value);
            Assert.Equal("Maria", pessoaCriada.Nome);
        }

        [Fact]
        public async Task Create_ComErroDeArgumento_DeveRetornarBadRequest()
        {
            var dto = new PessoaV2Dto { Nome = "" };
            _mockService.Setup(s => s.CreateAsync(dto))
                .ThrowsAsync(new ArgumentException("Nome inválido"));

            var result = await _controller.Create(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Nome inválido", badRequest.Value);
        }

        [Fact]
        public async Task Create_ModelStateInvalido_DeveRetornarBadRequest()
        {
            _controller.ModelState.AddModelError("Nome", "Campo obrigatório");
            var dto = new PessoaV2Dto();

            var result = await _controller.Create(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_ComDadosValidos_DeveRetornarOk()
        {
            var dto = new PessoaV2Dto { Id = 1, Nome = "Miguel Atualizado" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(dto);

            var result = await _controller.Update(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pessoaAtualizada = Assert.IsType<PessoaV2Dto>(okResult.Value);
            Assert.Equal("Miguel Atualizado", pessoaAtualizada.Nome);
        }

        [Fact]
        public async Task Update_ComIdInexistente_DeveRetornarNotFound()
        {
            var dto = new PessoaV2Dto { Nome = "Inexistente" };
            _mockService.Setup(s => s.UpdateAsync(99, dto))
                .ThrowsAsync(new KeyNotFoundException("Pessoa não encontrada"));

            var result = await _controller.Update(99, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Pessoa não encontrada", notFound.Value);
        }

        [Fact]
        public async Task Update_ComErroDeArgumento_DeveRetornarBadRequest()
        {
            var dto = new PessoaV2Dto { Nome = "Teste" };
            _mockService.Setup(s => s.UpdateAsync(1, dto))
                .ThrowsAsync(new ArgumentException("Dados inválidos"));

            var result = await _controller.Update(1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Dados inválidos", badRequest.Value);
        }

        [Fact]
        public async Task Update_ModelStateInvalido_DeveRetornarBadRequest()
        {
            _controller.ModelState.AddModelError("Nome", "Campo obrigatório");
            var dto = new PessoaV2Dto();

            var result = await _controller.Update(1, dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ComIdExistente_DeveRetornarNoContent()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ComIdInexistente_DeveRetornarNotFound()
        {
            _mockService.Setup(s => s.DeleteAsync(99))
                .ThrowsAsync(new KeyNotFoundException("Pessoa não encontrada"));

            var result = await _controller.Delete(99);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Pessoa não encontrada", notFound.Value);
        }
    }
}
