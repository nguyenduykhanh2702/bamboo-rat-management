using AutoMapper;
using FluentValidation;

public class CageService : ICageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public CageService(IUnitOfWork unitOfWork,
     IMapper mapper,
     IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;

    }
    public async Task<CageDto> GetCageByIdAsync(Guid id)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Không tìm thấy chuồng với ID đã cho");
        return _mapper.Map<CageDto>(cage);
    }
    public async Task<CageDto> AddCageAsync(CreateCageDto cageDto)
    {
        // validate duplicate Name
        var errors = new List<ValidationError>();
        var exists = await _unitOfWork.CageRepository.ExistsByNameAsync(cageDto.Name);
        if (exists)
        {
            ValidationHelper.AddError(errors, "name", "Tên chuồng đã tồn tại");
        }

        ValidationHelper.ThrowIfAny(errors);
        await _validationService.ValidateAsync(cageDto);
        var cage = _mapper.Map<Cage>(cageDto);
        await _unitOfWork.CageRepository.AddAsync(cage);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CageDto>(cage);
    }
    public async Task UpdateAsync(Guid id, CreateCageDto cageDto)
    {
        // validate exists
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Không tìm thấy chuồng với ID đã cho");

        // validate input
        await _validationService.ValidateAsync(cageDto);

        // validate duplicate Name
        var errors = new List<ValidationError>();
        var exists = await _unitOfWork.CageRepository.ExistsByNameAsync(cageDto.Name, id);
        if (exists)
        {
            ValidationHelper.AddError(errors, CageFields.Name, ErrorMessages.DuplicateRatByName);
        }
        ValidationHelper.ThrowIfAny(errors);

        _mapper.Map(cageDto, cage);
        _unitOfWork.CageRepository.Update(cage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Không tìm thấy chuồng với ID đã cho");

        _unitOfWork.CageRepository.Remove(cage);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<PagedResult<CageDto>> GetPagedResultAsync(CageParams cageParams)
    {
        var query = _unitOfWork.CageRepository.Query().Where(x => x.IsActive);
        //  SEARCH
        if (!string.IsNullOrWhiteSpace(cageParams.Search))
        {
            var keyword = cageParams.Search.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(keyword));
        }

        //  FILTER
        if (cageParams.MinCapacity.HasValue)
        {
            query = query.Where(x => x.Capacity >= cageParams.MinCapacity.Value);
        }

        if (cageParams.MaxCapacity.HasValue)
        {
            query = query.Where(x => x.Capacity <= cageParams.MaxCapacity.Value);
        }

        // SORT
        query = CageQueryableExtensions.ApplySorting(query, cageParams.OrderBy);

        var pagedCages = await PaginationHelper.ToPagedResultAsync(query, cageParams.PageNumber, cageParams.PageSize);
        var cageDtos = _mapper.Map<List<CageDto>>(pagedCages.Items);
        return new PagedResult<CageDto>
        {
            Items = cageDtos,
            TotalCount = pagedCages.TotalCount,
            PageNumber = pagedCages.PageNumber,
            PageSize = pagedCages.PageSize
        };
    }
}

