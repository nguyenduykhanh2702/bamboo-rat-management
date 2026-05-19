using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cage, CageDto>();
        CreateMap<CreateCageDto, Cage>();
    }
}