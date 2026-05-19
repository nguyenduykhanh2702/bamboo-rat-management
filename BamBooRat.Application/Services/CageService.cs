using AutoMapper;

public class CageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    public async Task<List<CageDto>> GetAllCagesAsync()
    {
        var cages = await _unitOfWork.CageRepository.GetAllAsync();
        return _mapper.Map<List<CageDto>>(cages);
    }
    public async Task<CageDto> GetCageByIdAsync(Guid id)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Cage not found");
        return _mapper.Map<CageDto>(cage);
    }
    public async Task<CageDto> AddCageAsync(CreateCageDto cageDto)
    {
        var cage = _mapper.Map<Cage>(cageDto);
        await _unitOfWork.CageRepository.AddAsync(cage);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CageDto>(cage);
    }
    public async Task UpdateAsync(Guid id, CreateCageDto dto)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Cage not found");

        _mapper.Map(dto, cage);
        _unitOfWork.CageRepository.Update(cage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new Exception("Cage not found");

        _unitOfWork.CageRepository.Remove(cage);
        await _unitOfWork.SaveChangesAsync();
    }
}