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
    public async Task CreateAsync(Guid RatId, Guid sourceCageId, Guid destinationCageId, DateTime transferDate)
    {
        var transfer = new CageTransfer
        {
            RatId = RatId,
            FromCageId = sourceCageId,
            ToCageId = destinationCageId,
            TransferDate = transferDate
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
        var search = string.Empty;
        if (!string.IsNullOrEmpty(cageTransferParams.FromCageName))
        {
            search = cageTransferParams.FromCageName.Trim().ToLower();
            query = query.Where(x => x.FromCage.Name.ToLower().Contains(search));
        }

        if (!string.IsNullOrEmpty(cageTransferParams.ToCageName))
        {
            search = cageTransferParams.ToCageName.Trim().ToLower();
            query = query.Where(x => x.ToCage.Name.ToLower().Contains(search));
        }
        if (!string.IsNullOrEmpty(cageTransferParams.RatName))
        {
            search = cageTransferParams.RatName.Trim().ToLower();
            query = query.Where(x => x.Rat.Name.ToLower().Contains(search));
        }
        if (!string.IsNullOrEmpty(cageTransferParams.RatCode))
        {
            search = cageTransferParams.RatCode.Trim().ToLower();
            query = query.Where(x => x.Rat.Code.ToLower().Contains(search));
        }
        if (cageTransferParams.FromDate.HasValue)
        {
            query = query.Where(x => x.TransferDate >= cageTransferParams.FromDate.Value);
        }

        if (cageTransferParams.ToDate.HasValue)
        {
            query = query.Where(x => x.TransferDate <= cageTransferParams.ToDate.Value);
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