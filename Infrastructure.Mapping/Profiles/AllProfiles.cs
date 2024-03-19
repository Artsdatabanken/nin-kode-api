using AutoMapper;
using Microsoft.Extensions.Configuration;
using NiN3.Core.Models;
using NiN3.Core.Models.DTOs;
using NiN3.Core.Models.Enums;
using static System.Net.WebRequestMethods;

namespace NiN3.Infrastructure.Mapping.Profiles
{
    //Løsning: automapper
    public class AllProfiles : Profile
    {
        public readonly MapperConfiguration _root_url;
        public readonly IConfiguration _configuration;
        /*
        public AllProfiles()
        {
        }*/
        public AllProfiles(/*IConfiguration configuration*/)
        {
            // ex : // .ForMember(dest => dest.FullName, opt => opt.ResolveUsing(src => src.Name + " Monroe"))
            // q: example of automapper with custom mapping
            //CreateMap<Models.>
            // todo: se custom mappings: https://medium.com/knowledge-pills/how-to-use-automapper-in-c-6f949402be05
            //CreateMap<Versjon, VersjonDto>();
            //var _root_url = root_url;
            //_configuration = configuration;
            var _root_url = "https://nin-kode-api.artsdatabanken.no/v3.0";//configuration["root_url"];
            //CreateMap<Versjon, VersjonDto>()

                //q: accept empty list for typer
            /*
            CreateMap<Versjon, VersjonDto>()
                    .ForMember(dest => dest.Typer, opt => opt.MapFrom(src => src.Typer ?? new ICollection<Type>()));*/
            /*
            CreateMap<NiN3.Core.Models.Type, TypeDto>()
                .ForMember(dest => dest.Kategori, opt => opt.MapFrom(src => "Type"))
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Ecosystnivaa) + " " +
                EnumUtil.ToDescription(src.Typekategori) + " " +
                EnumUtil.ToDescriptionBlankIfNull(src.Typekategori2)))
                .ForPath(dest => dest.Kode.Id, opt => opt.MapFrom(src => src.Kode))
                .ForPath(dest => dest.Kode.Definisjon, opt => opt.MapFrom(src => _root_url + "/typer/hentkode/" + src.Kode)); 
            */
            /*
            CreateMap<NiN3.Core.Models.Hovedtypegruppe, HovedtypegruppeDto>()
                .ForMember(dest => dest.Kategori, opt => opt.MapFrom(src => "Hovedtypegruppe"))
                .ForPath(dest => dest.Kode.Id, opt => opt.MapFrom(src => src.Kode))
                .ForPath(dest => dest.Kode.Definisjon, opt => opt.MapFrom(src => _root_url + "/typer/hentkode/" + src.Kode));*/
            /* CreateMap<NiN3.Core.Models.Hovedtype, HovedtypeDto>()
                 .ForPath(dest => dest.Kode.Id, opt => opt.MapFrom(src => src.Kode))
                 .ForPath(dest => dest.Kode.Definisjon, opt => opt.MapFrom(src => _root_url + "/typer/hentkode/" + src.Kode));
            */
            // config.GetValue<string>("root_url")

            /*CreateMap<NiN3.Core.Models.Type, KodeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Kode));*/
        }
    }
}
