
using AutoMapper;

public class HealthRecordService : IHealthRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    public HealthRecordService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }
    public async Task<HealthRecordDetailDto> CreateAsync(CreateHealthRecordDto dto)
    {
        await _validationService.ValidateAsync(dto);

        var health = _mapper.Map<HealthRecord>(dto);

        await _unitOfWork.HealthRecordRepository.AddAsync(health);

        await _unitOfWork.SaveChangesAsync();


        return _mapper.Map<HealthRecordDetailDto>(health);
    }

    public async Task DeleteAsync(Guid id)
    {
        var health = await _unitOfWork.HealthRecordRepository.GetByIdAsync(id);
        if (health == null)
            throw new NotFoundException($"Không tìm thấy bản ghi sức khỏe với id: {id}");
        _unitOfWork.HealthRecordRepository.Remove(health);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedResult<HealthRecordDto>> GetAllAsync(HealthRecordParams healthParams)
    {
        var query = _unitOfWork.HealthRecordRepository.Query().Select(x => new HealthRecordDto
        {
            Id = x.Id,
            RatId = x.RatId,
            RecordDate = x.RecordDate,
            RatCode = x.Rat.Code,
            RatName = x.Rat.Name,
            Diagnosis = x.Diagnosis,
            Treatment = x.Treatment,
        });
        if (!string.IsNullOrEmpty(healthParams.Search))
        {
            var search = healthParams.Search.Trim().ToLower();
            query = query.Where(h => h.RatCode.ToLower().Contains(search) || h.RatName.ToLower().Contains(search));
        }

        if (healthParams.RecordDate.HasValue)
            query = query.Where(h => h.RecordDate <= healthParams.RecordDate.Value);

        var healthPage = await PaginationHelper.ToPagedResultAsync(query, healthParams.PageNumber, healthParams.PageSize);

        return new PagedResult<HealthRecordDto>
        {
            Items = healthPage.Items.ToList(),
            TotalCount = healthPage.TotalCount,
            PageNumber = healthPage.PageNumber,
            PageSize = healthPage.PageSize
        };
    }

    public async Task<HealthRecordDetailDto> GetByIdAsync(Guid id)
    {
        var health = await _unitOfWork.HealthRecordRepository.GetByIdAsync(id);
        if (health == null)
            throw new NotFoundException($"Không tìm thấy bản ghi sức khỏe với id: {id}");
        return _mapper.Map<HealthRecordDetailDto>(health);
    }

    public async Task UpdateAsync(Guid id, UpdateHealthRecordDto dto)
    {
        await _validationService.ValidateAsync(dto);
        var health = await _unitOfWork.HealthRecordRepository.GetByIdAsync(id);
        if (health == null)
            throw new NotFoundException($"Không tìm thấy bản ghi sức khỏe với id: {id}");

        _mapper.Map(dto, health);
        _unitOfWork.HealthRecordRepository.Update(health);
        await _unitOfWork.SaveChangesAsync();
    }

}