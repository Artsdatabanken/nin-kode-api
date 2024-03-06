/*
using AutoMapper;
using NiN3.Core.Models.DTOs.type;
using NiN3.Core.Models.DTOs.variabel;
using NiN3.Core.Models.DTOs;
using NiN3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiN3.Core.Models.Enums;

namespace NiN3.Infrastructure.Mapping.Profiles
{
    public class VersionProfile : Profile
    {
        public VersionProfile()
        {
            CreateMap<Hovedtypegruppe_Hovedoekosystem, HovedoekosystemDto>(MemberList.Destination)
                .ForMember(dest => dest.HovedoekosystemEnum, opt => opt.MapFrom(src => src.HovedoekosystemEnum))
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.HovedoekosystemEnum)))
                ;

            CreateMap<Grunntype, GrunntypeDto>(MemberList.Destination)
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => src.Navn))
                .ForMember(dest => dest.Kategori, opt => opt.MapFrom(src => "Grunntype"))
                .ForMember(dest => dest.Kode, opt => opt.MapFrom(x => new KodeDto { Id = x.Kode, Definisjon = "https://nin-kode-api.artsdatabanken.no/v3.0", Langkode = x.Langkode }))
                ;

            CreateMap<Hovedtype_Kartleggingsenhet, KartleggingsenhetDto>(MemberList.Destination)
                .ForMember(dest => dest.Kategori, opt => opt.MapFrom(src => "Kartleggingsenhet"))
                .ForMember(dest => dest.MaalestokkEnum, opt => opt.MapFrom(src => src.Kartleggingsenhet.Maalestokk))
                .ForMember(dest => dest.MaalestokkNavn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Kartleggingsenhet.Maalestokk)))
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => src.Kartleggingsenhet.Navn))
                .ForMember(dest => dest.Kode, opt => opt.MapFrom(x => new KodeDto { Id = x.Kartleggingsenhet.Kode, Definisjon = "https://nin-kode-api.artsdatabanken.no/v3.0", Langkode = x.Kartleggingsenhet.Langkode }))
                .ForMember(dest => dest.Grunntyper, opt => opt.MapFrom(src => src.Kartleggingsenhet.Grunntyper))
                ;

            CreateMap<Hovedtype, HovedtypeDto>(MemberList.Destination)
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => src.Navn))
                .ForMember(dest => dest.Kode, c => c.MapFrom(x => new KodeDto { Id = x.Kode, Definisjon = "https://nin-kode-api.artsdatabanken.no/v3.0", Langkode = x.Langkode }))
                .ForMember(dest => dest.ProsedyrekategoriEnum, opt => opt.MapFrom(src => src.Prosedyrekategori))
                .ForMember(dest => dest.ProsedyrekategoriNavn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Prosedyrekategori)))
                .ForMember(dest => dest.Grunntyper, opt => opt.MapFrom(src => src.Grunntyper))
                .ForMember(dest => dest.Kartleggingsenheter, opt => opt.MapFrom(src => src.Hovedtype_Kartleggingsenheter))
                ;
            CreateMap<Hovedtypegruppe, HovedtypegruppeDto>(MemberList.Destination)
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => src.Navn))
                .ForMember(dest => dest.Kode, c => c.MapFrom(x => new KodeDto { Id = x.Kode, Definisjon = "https://nin-kode-api.artsdatabanken.no/v3.0", Langkode = x.Langkode }))
                .ForMember(dest => dest.Kategori, opt => opt.MapFrom(src => "Hovedtypegruppe"))
                .ForMember(dest => dest.Typekategori3Enum, opt => opt.MapFrom(src => src.Typekategori3))
                .ForMember(dest => dest.Typekategori3Navn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Typekategori3)))
                .ForMember(dest => dest.Hovedoekosystemer, opt => opt.MapFrom(src => src.Hovedoekosystemer))
                .ForMember(dest => dest.Hovedtyper, opt => opt.MapFrom(src => src.Hovedtyper))
                ;

            CreateMap<NiN3.Core.Models.Type, TypeDto>(MemberList.Destination)
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Typekategori2)))
                .ForMember(dest => dest.Kode, c => c.MapFrom(x => new KodeDto { Id = x.Kode, Definisjon = "https://nin-kode-api.artsdatabanken.no/v3.0", Langkode = x.Langkode }))
                .ForMember(dest => dest.Kategori, opt => opt.MapFrom(src => "Type"))
                .ForMember(dest => dest.EcosystnivaaEnum, opt => opt.MapFrom(src => src.Ecosystnivaa))
                .ForMember(dest => dest.EcosystnivaaNavn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Ecosystnivaa)))
                .ForMember(dest => dest.TypekategoriEnum, opt => opt.MapFrom(src => src.Typekategori))
                .ForMember(dest => dest.TypekategoriNavn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Typekategori)))
                .ForMember(dest => dest.Typekategori2Enum, opt => opt.MapFrom(src => src.Typekategori2))
                .ForMember(dest => dest.Typekategori2Navn, opt => opt.MapFrom(src => EnumUtil.ToDescriptionBlankIfNull(src.Typekategori2)))
                .ForMember(dest => dest.Hovedtypegrupper, opt => opt.MapFrom(src => src.Hovedtypegrupper))
                ;
            ;
            CreateMap<Variabeltrinn, MaaleskalaDto>(MemberList.Destination)
                .ForMember(dest => dest.MaaleskalatypeEnum, opt => opt.MapFrom(src => src.MaaleskalatypeEnum))
                .ForMember(dest => dest.MaaleskalatypeNavn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.MaaleskalatypeEnum)))
                .ForMember(dest => dest.EnhetEnum, opt => opt.MapFrom(src => src.EnhetEnum))
                .ForMember(dest => dest.EnhetNavn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.EnhetEnum)))
                ;
            CreateMap<Trinn, TrinnDto>(MemberList.Destination)
                .ForMember(dest => dest.Verdi, opt => opt.MapFrom(src => src.Verdi))
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => src.Navn));
            CreateMap<VariabelnavnMaaleskala, MaaleskalaTrinnDto>(MemberList.Destination)
                .ForMember(dest => dest.MaaleskalaDto, opt => opt.MapFrom(src => src.Variabeltrinn))
                .ForMember(dest => dest.TrinnDto, opt => opt.MapFrom(src => src.Trinn)); ;

            CreateMap<Variabelnavn, VariabelnavnDto>(MemberList.Destination)
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => src.Navn))
                .ForMember(dest => dest.Kode, opt => opt.MapFrom(x => new KodeDto { Id = x.Kode, Definisjon = "https://nin-kode-api.artsdatabanken.no/v3.0", Langkode = x.Langkode }))
                .ForMember(dest => dest.Variabelkategori2Enum, opt => opt.MapFrom(src => src.Variabelkategori2))
                .ForMember(dest => dest.Variabelkategori2Navn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Variabelkategori2)))
                .ForMember(dest => dest.VariabeltypeEnum, opt => opt.MapFrom(src => src.Variabeltype))
                .ForMember(dest => dest.VariabeltypeNavn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Variabeltype)))
                .ForMember(dest => dest.VariabelgruppeEnum, opt => opt.MapFrom(src => src.Variabelgruppe))
                .ForMember(dest => dest.VariabelgruppeNavn, opt => opt.MapFrom(src => EnumUtil.ToDescriptionBlankIfNull(src.Variabelgruppe)))
                .ForMember(dest => dest.MaaleskalaTrinn, opt => opt.MapFrom(src => src.VariabelnavnMaaleskala))

                ;
            CreateMap<Variabel, VariabelDto>(MemberList.Destination)
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => src.Navn))
                .ForMember(dest => dest.Kode, opt => opt.MapFrom(x => new KodeDto { Id = x.Kode, Definisjon = "https://nin-kode-api.artsdatabanken.no/v3.0", Langkode = x.Langkode }))
                .ForMember(dest => dest.Kategori, opt => opt.MapFrom(src => "Variabel"))
                .ForMember(dest => dest.EcosystnivaaEnum, opt => opt.MapFrom(src => src.Ecosystnivaa))
                .ForMember(dest => dest.EcosystnivaaNavn, opt => opt.MapFrom(src => EnumUtil.ToDescription(src.Ecosystnivaa)))
                .ForMember(dest => dest.VariabelkategoriEnum, opt => opt.MapFrom(src => src.Variabelkategori))
                .ForMember(dest => dest.VariabelkategoriNavn, opt => opt.MapFrom(src => EnumUtil.ToDescriptionBlankIfNull(src.Variabelkategori)))
                .ForMember(dest => dest.Variabelnavn, opt => opt.MapFrom(src => src.Variabelnavn))

            ;
            CreateMap<Versjon, VersjonDto>(MemberList.Destination);
        }
    }
}
*/