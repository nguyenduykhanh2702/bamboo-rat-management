
using AutoMapper;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;

public class RatService : IRatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    private readonly IMapper _mapper;
    public RatService(IUnitOfWork unitOfWork,
                    IMapper mapper,
                   IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;

    }

    public async Task<RatDto> CreateAsync(CreateRatDto createRatDto)
    {
        // validate duplicate Name and Code
        var duplicates = await _unitOfWork.RatRespository.GetDuplicateFieldsAsync(createRatDto.Name, createRatDto.Code);

        var errors = new List<ValidationError>();

        if (duplicates.Contains(RatFields.Name))
        {
            ValidationHelper.AddError(errors, RatFields.Name, ErrorMessages.DuplicateRatByName);
        }

        if (duplicates.Contains(RatFields.Code))
        {
            ValidationHelper.AddError(errors, RatFields.Code, ErrorMessages.DuplicateRatByCode);
        }

        ValidationHelper.ThrowIfAny(errors);
        await _validationService.ValidateAsync(createRatDto);

        var rat = _mapper.Map<Rat>(createRatDto);
        rat.Status = RatStatus.Alive; // Set default status
        await _unitOfWork.RatRespository.AddAsync(rat);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RatDto>(rat);
    }

    public async Task DeleteAsync(Guid id)
    {
        var rat = await _unitOfWork.RatRespository.GetByIdAsync(id);
        if (rat == null)
            throw new NotFoundException("Không tìm thấy dúi với ID đã cho");
        _unitOfWork.RatRespository.Remove(rat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedResult<RatDto>> GetPagedResultAsync(RatParams ratParams)
    {
        var query = _unitOfWork.RatRespository.Query();
        // FILTER
        if (!string.IsNullOrWhiteSpace(ratParams.Code))
        {
            var code = ratParams.Code.Trim().ToUpper();
            query = query.Where(r => r.Code.ToLower().Contains(code));
        }
        if (ratParams.Status != null)
        {
            query = query.Where(x => x.Status == ratParams.Status);
        }
        if (ratParams.Type != null)
        {
            query = query.Where(x => x.Type == ratParams.Type);
        }
        if (ratParams.Gender != null)
        {
            query = query.Where(x => x.Gender == ratParams.Gender);
        }
        // SORT
        query = RatQueryableExtensions.ApplySorting(query, ratParams.OrderBy);
        var pagedRats = await PaginationHelper.ToPagedResultAsync(query, ratParams.PageNumber, ratParams.PageSize);
        var ratDtos = _mapper.Map<List<RatDto>>(pagedRats.Items);
        return new PagedResult<RatDto>
        {
            Items = ratDtos,
            TotalCount = pagedRats.TotalCount,
            PageNumber = pagedRats.PageNumber,
            PageSize = pagedRats.PageSize
        };
    }

    public async Task<RatDetailDto> GetRatByIdAsync(Guid id)
    {
        var rat = await _unitOfWork.RatRespository.Query().Where(r => r.Id == id).
        Select(x => new RatDetailDto
        {
            Id = x.Id,
            Name = x.Name,
            Code = x.Code,
            Description = x.Description,
            Type = x.Type,
            Status = x.Status,
            Gender = x.Gender,
            DateOfBirth = x.DateOfBirth,
            Age = x.Age,
            Cage = new CageSimpleDto
            {
                Id = x.Cage.Id,
                Name = x.Cage.Name
            }
        }).FirstOrDefaultAsync();
        if (rat == null)
            throw new NotFoundException("Không tìm thấy dúi với ID đã cho");
        return rat;
    }

    public async Task UpdateAsync(Guid id, UpdateRatDto ratDto)
    {
        // validate if rat exists
        var rat = await _unitOfWork.RatRespository.GetByIdAsync(id);
        if (rat == null)
            throw new NotFoundException("Không tìm thấy dúi với ID đã cho");

        // validate input
        await _validationService.ValidateAsync(ratDto);

        // validate duplicate Name and Code
        var duplicates = await _unitOfWork.RatRespository.GetDuplicateFieldsAsync(ratDto.Name, ratDto.Code, id);

        var errors = new List<ValidationError>();

        if (duplicates.Contains(RatFields.Name))
        {
            ValidationHelper.AddError(errors, RatFields.Name, ErrorMessages.DuplicateRatByName);
        }

        if (duplicates.Contains(RatFields.Code))
        {
            ValidationHelper.AddError(errors, RatFields.Code, ErrorMessages.DuplicateRatByCode);
        }

        ValidationHelper.ThrowIfAny(errors);

        _mapper.Map(ratDto, rat);
        _unitOfWork.RatRespository.Update(rat);
        await _unitOfWork.SaveChangesAsync();

    }
}