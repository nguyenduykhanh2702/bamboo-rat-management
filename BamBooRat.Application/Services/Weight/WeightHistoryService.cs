
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class WeightHistoryService : IWeightHistoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public WeightHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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