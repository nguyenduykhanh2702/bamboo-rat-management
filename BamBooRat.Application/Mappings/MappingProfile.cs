using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cage, CageDto>();
        CreateMap<CreateCageDto, Cage>();

        CreateMap<Rat, RatDto>();
        CreateMap<Rat, RatDetailDto>();
        CreateMap<Rat, CageSimpleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Cage.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Cage.Name));

        CreateMap<CreateRatDto, Rat>();
        CreateMap<UpdateRatDto, Rat>();

        CreateMap<Rat, CreateRatDto>();
        CreateMap<Rat, UpdateRatDto>();

        CreateMap<Breeding, BreedingDto>()
           .ForMember(dest => dest.MaleCode, opt => opt.MapFrom(src => src.Male.Code))
           .ForMember(dest => dest.FemaleCode, opt => opt.MapFrom(src => src.Female.Code))
           .ForMember(dest => dest.CageName, opt => opt.MapFrom(src => src.Cage.Name));
    }
}