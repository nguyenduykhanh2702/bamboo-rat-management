
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

    public async Task<PagedResult<RatDto>> GetPagedResultAsync(PaginationParams paginationParams)
    {
        var query = _unitOfWork.RatRespository.Query();

        var pagedRats = await PaginationHelper.ToPagedResultAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
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
}