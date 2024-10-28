using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Componentes;
using System.Text.RegularExpressions;

namespace ControleFazenda.App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<FormaPagamento, FormaPagamentoVM>().ReverseMap();
            CreateMap<Caixa, CaixaVM>().ReverseMap();
            CreateMap<FluxoCaixa, FluxoCaixaVM>().ReverseMap();
            CreateMap<Colaborador, ColaboradorVM>().ReverseMap();
            CreateMap<Fornecedor, FornecedorVM>().ReverseMap();
            CreateMap<Endereco, EnderecoVM>().ReverseMap();
            CreateMap<Recibo, ReciboVM>().ReverseMap();
            CreateMap<NFe, NFeVM>().ReverseMap();
            CreateMap<Vale, ValeVM>().ReverseMap();
            CreateMap<Malote, MaloteVM>().ReverseMap();
            CreateMap<Diaria, DiariaVM>().ReverseMap();
            CreateMap<Diarista, DiaristaVM>().ReverseMap();
        }
    }
}
