using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

public class CageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCageDto> _validator;

    private readonly IMapper _mapper;
    public CageService(IUnitOfWork unitOfWork,
     IMapper mapper,
     IValidator<CreateCageDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;

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
        var exists = await _unitOfWork.CageRepository.ExistsByNameAsync(cageDto.Name);
        if (exists)
            ThrowDuplicateName();
        await ValidateAsync(cageDto);
        var cage = _mapper.Map<Cage>(cageDto);
        await _unitOfWork.CageRepository.AddAsync(cage);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CageDto>(cage);
    }
    public async Task UpdateAsync(Guid id, CreateCageDto cageDto)
    {
        // validate duplicate Name
        var exists = await _unitOfWork.CageRepository.ExistsByNameAsync(cageDto.Name, id);
        if (exists)
            ThrowDuplicateName();

        await ValidateAsync(cageDto);

        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Không tìm thấy chuồng với ID đã cho");

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
    public async Task<PagedResult<CageDto>> GetPagedResultAsync(PaginationParams paginationParams)
    {
        var query = _unitOfWork.CageRepository.Query();
        //  SEARCH
        if (!string.IsNullOrWhiteSpace(paginationParams.Search))
        {
            var keyword = paginationParams.Search.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(keyword));
        }

        //  FILTER
        if (paginationParams.MinCapacity.HasValue)
        {
            query = query.Where(x => x.Capacity >= paginationParams.MinCapacity.Value);
        }

        if (paginationParams.MaxCapacity.HasValue)
        {
            query = query.Where(x => x.Capacity <= paginationParams.MaxCapacity.Value);
        }

        // SORT
        query = ApplySorting(query, paginationParams.OrderBy);

        var pagedCages = await PaginationHelper.ToPagedResultAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        var cageDtos = _mapper.Map<List<CageDto>>(pagedCages.Items);
        return new PagedResult<CageDto>
        {
            Items = cageDtos,
            TotalCount = pagedCages.TotalCount,
            PageNumber = pagedCages.PageNumber,
            PageSize = pagedCages.PageSize
        };
    }

    private async Task ValidateAsync(CreateCageDto dto)
    {
        var result = await _validator.ValidateAsync(dto);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => new ValidationError
            {
                Field = e.PropertyName.ToLower(),
                Message = e.ErrorMessage
            }).ToList();

            throw new ValidationExceptionCustom(errors);
        }
    }
    private void ThrowDuplicateName()
    {
        throw new ValidationExceptionCustom(new List<ValidationError>
        {
            new ValidationError
            {
                Field = "name",
                Message = "Tên chuồng đã tồn tại."
            }
        });
    }

    private IQueryable<Cage> ApplySorting(IQueryable<Cage> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query.OrderBy(x => x.Name);

        return orderBy.ToLower() switch
        {
            "name" => query.OrderBy(x => x.Name),
            "name desc" => query.OrderByDescending(x => x.Name),
            "capacity" => query.OrderBy(x => x.Capacity),
            "capacity desc" => query.OrderByDescending(x => x.Capacity),
            "createddate" => query.OrderBy(x => x.CreatedDate),
            "createddate desc" => query.OrderByDescending(x => x.CreatedDate),
            _ => query.OrderBy(x => x.Name)
        };
    }
}

