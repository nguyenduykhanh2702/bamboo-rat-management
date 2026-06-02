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

        CreateMap<ConFirmBirthDto, Breeding>();

        CreateMap<CageTransfer, CageTransferDto>()
            .ForMember(
                dest => dest.RatCode,
                opt => opt.MapFrom(src => src.Rat.Code))

            .ForMember(
                dest => dest.RatName,
                opt => opt.MapFrom(src => src.Rat.Name))

            .ForMember(
                dest => dest.FromCageName,
                opt => opt.MapFrom(src =>
                    src.FromCage != null
                        ? src.FromCage.Name
                        : null))

            .ForMember(
                dest => dest.ToCageName,
                opt => opt.MapFrom(src => src.ToCage.Name));

    }
}