using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;

namespace Cadastro_Pessoa.Mapping;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Pessoa, PessoaV1Dto>().ReverseMap();
        CreateMap<Pessoa, PessoaV2Dto>().ReverseMap();
    }
}

