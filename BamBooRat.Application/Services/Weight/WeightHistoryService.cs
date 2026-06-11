
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class WeightHistoryService : IWeightHistoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    public WeightHistoryService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }

    public async Task<WeightHistoryDto> CreateWeightHistoryParams(CreateWeightHistoryDto weightHistoryDto)
    {
        var rat = await _unitOfWork.RatRespository.GetByIdAsync(weightHistoryDto.RatId);
        if (rat == null)
        {
            throw new NotFoundException($"Không tìm thấy bản ghi Rat với id : {weightHistoryDto.RatId}");
        }
        await _validationService.ValidateAsync(weightHistoryDto);

        var weightHistory = new WeightHistory
        {
            RatId = weightHistoryDto.RatId,
            RecordedDate = weightHistoryDto.RecordedDate,
            Weight = weightHistoryDto.Weight
        };
        await _unitOfWork.WeightHistoryRepository.AddAsync(weightHistory);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<WeightHistoryDto>(weightHistory);
    }

    public async Task<PagedResult<WeightHistoryDto>> GetWeightHistoryPageResult(WeightHistoryParams weightHistoryParams)
    {
        var query = _unitOfWork.WeightHistoryRepository.Query().Include(x => x.Rat).AsNoTracking();
        if (!string.IsNullOrEmpty(weightHistoryParams.Search))
        {
            var search = weightHistoryParams.Search.Trim().ToLower();
            query = query.Where(x => x.Rat.Name.ToLower().Contains(search) || x.Rat.Code.ToLower().Contains(search));
        }
        query = query.OrderByDescending(x => x.RecordedDate);

        var pagedWeightHistory = await PaginationHelper.ToPagedResultAsync(query, weightHistoryParams.PageNumber, weightHistoryParams.PageSize);

        var weightHistoryDto = _mapper.Map<List<WeightHistoryDto>>(pagedWeightHistory.Items);
        return new PagedResult<WeightHistoryDto>
        {
            Items = weightHistoryDto,
            TotalCount = pagedWeightHistory.TotalCount,
            PageNumber = pagedWeightHistory.PageNumber,
            PageSize = pagedWeightHistory.PageSize
        };
    }
}