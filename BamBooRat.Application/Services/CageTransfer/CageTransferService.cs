using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class CageTransferService : ICageTransferService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CageTransferService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task CreateAsync(Guid RatId, Guid sourceCageId, Guid destinationCageId, DateTime transferDate, TransferReason reason)
    {
        var transfer = new CageTransfer
        {
            RatId = RatId,
            FromCageId = sourceCageId,
            ToCageId = destinationCageId,
            TransferDate = transferDate,
            Reason = reason,
        };
        await _unitOfWork.CageTransferRespository.AddAsync(transfer);
    }

    public async Task DeleteAsync(Guid id)
    {
        var query = await _unitOfWork.CageTransferRespository.GetByIdAsync(id);
        if (query == null)
        {
            throw new NotFoundException("Cage transfer not found");
        }
        _unitOfWork.CageTransferRespository.Remove(query);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedResult<CageTransferDto>> GetAllCageTransferAsync(CageTransferParams cageTransferParams)
    {
        var query = _unitOfWork.CageTransferRespository.Query();

        if (!string.IsNullOrEmpty(cageTransferParams.Search))
        {
            var searchTerm = cageTransferParams.Search.Trim().ToLower();
            query = query.Where(x =>
                x.FromCage.Name.ToLower().Contains(searchTerm) ||
                x.ToCage.Name.ToLower().Contains(searchTerm) ||
                x.Rat.Name.ToLower().Contains(searchTerm) ||
                x.Rat.Code.ToLower().Contains(searchTerm));
        }
        query = query.OrderByDescending(x => x.TransferDate);

        var pageCageTransfer = await PaginationHelper.ToPagedResultAsync(query, cageTransferParams.PageNumber, cageTransferParams.PageSize);
        return new PagedResult<CageTransferDto>
        {
            Items = _mapper.Map<List<CageTransferDto>>(pageCageTransfer.Items),
            TotalCount = pageCageTransfer.TotalCount,
            PageNumber = pageCageTransfer.PageNumber,
            PageSize = pageCageTransfer.PageSize
        };
    }

    public async Task<CageTransferDetailDto> GetByIdAsync(Guid id)
    {
        var query = await _unitOfWork.CageTransferRespository.Query().Where(x => x.Id == id).Select(x => new CageTransferDetailDto
        {
            Id = x.Id,
            RatId = x.RatId,
            TransferDate = x.TransferDate,
            RatName = x.Rat.Name,
            RatCode = x.Rat.Code,
            FromCageName = x.FromCage.Name,
            ToCageName = x.ToCage.Name,
            Reason = x.Reason,
            Note = x.Note
        }).FirstOrDefaultAsync();

        if (query == null)
        {
            throw new NotFoundException("Cage transfer not found");
        }
        return query;
    }
}