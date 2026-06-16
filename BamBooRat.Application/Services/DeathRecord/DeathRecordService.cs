
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class DeathRecordService : IDeathRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBreedingService _breedingService;

    public DeathRecordService(IUnitOfWork unitOfWork,
                              IMapper mapper,
                              ICageTransferService cageTransferService,
                              IBreedingService breedingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _breedingService = breedingService;
    }
    public async Task<DeathRecordDto> AddDeathRecordAsync(
    DeathRecordRequestDto dto)
    {
        var rat = await _unitOfWork.RatRespository
            .GetByIdAsync(dto.RatId);

        if (rat == null)
            throw new NotFoundException(
                $"Rat với id {dto.RatId} không tìm thấy.");

        var errors = new List<ValidationError>();

        if (rat.Status == RatStatus.Dead)
        {
            ValidationHelper.AddError(
                errors,
                "ratId",
                $"Rat với id {dto.RatId} đã có bản ghi tử vong.");
        }

        if (errors.Any())
            ValidationHelper.ThrowIfAny(errors);

        var breeding = await _unitOfWork.BreedingRepository
            .GetActiveBreedingByRatIdAsync(dto.RatId);

        if (breeding != null)
        {
            await _breedingService.CancelBreedingBecauseRatDiedAsync(
                breeding,
                rat.Id);
        }

        var deathRecord = new DeathRecord
        {
            RatId = dto.RatId,
            CageId = rat.CageId,
            DeathDate = dto.DeathDate,
            Cause = dto.Cause,
            Note = dto.Note
        };

        await _unitOfWork.DeathRecordRepository
            .AddDeathRecordAsync(deathRecord);

        rat.Status = RatStatus.Dead;

        rat.CageId = null;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DeathRecordDto>(deathRecord);
    }

    public async Task<PagedResult<DeathRecordDto>> GetAllDeathRecordsAsync(DeathRecordParams deathRecordParams)
    {
        var query = _unitOfWork.DeathRecordRepository.Query().
        Include(x => x.Rat).
        Include(x => x.Cage).
        Select(x => new DeathRecordDto
        {
            Id = x.Id,
            RatId = x.RatId,
            RatCode = x.Rat.Code,
            RatName = x.Rat.Name,
            // CageName = x.Cage != null ? x.Cage.Name : "NULL"
            CageName = x.Cage != null ? x.Cage.Name : null,
            DeathDate = x.DeathDate
        });
        if (!string.IsNullOrEmpty(deathRecordParams.Search))
        {
            string keyword = deathRecordParams.Search.Trim().ToLower();
            query = query.Where(x =>
                            x.RatCode != null && x.RatCode.ToLower().Contains(keyword) ||
                            x.CageName != null && x.CageName.ToLower().Contains(keyword));
        }
        if (deathRecordParams.FromDate.HasValue)
        {
            query = query.Where(x => x.DeathDate >= deathRecordParams.FromDate);
        }
        if (deathRecordParams.ToDate.HasValue)
        {
            query = query.Where(x => x.DeathDate <= deathRecordParams.ToDate);
        }
        query = query.OrderByDescending(x => x.DeathDate);
        var deathRecordPages = await PaginationHelper.ToPagedResultAsync(query, deathRecordParams.PageNumber,
                                                             deathRecordParams.PageSize);
        return new PagedResult<DeathRecordDto>
        {
            Items = deathRecordPages.Items.ToList(),
            TotalCount = deathRecordPages.TotalCount,
            PageNumber = deathRecordParams.PageNumber,
            PageSize = deathRecordParams.PageSize

        };
    }

    public async Task<DeathRecordDto> GetDeathRecordByIdAsync(Guid id)
    {
        var deathRecord = await _unitOfWork.DeathRecordRepository.Query().
                                Include(x => x.Rat).
                                Include(x => x.Cage).
                                Where(x => x.Id == id).
                                Select(x => new DeathRecordDto
                                {
                                    Id = x.Id,
                                    RatId = x.RatId,
                                    RatCode = x.Rat.Code,
                                    RatName = x.Rat.Name,
                                    CageName = x.Cage!.Name
                                }).FirstOrDefaultAsync();

        if (deathRecord == null)
            throw new NotFoundException(
                $"Bản ghi tử vong với id {id} không tìm thấy.");
        return deathRecord;
    }
}