using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class BreedingService : IBreedingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    private readonly ICageTransferService _cageTransferService;
    public BreedingService(IUnitOfWork unitOfWork,
                IMapper mapper,
                IValidationService validationService,
                ICageTransferService cageTransferService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;
        _cageTransferService = cageTransferService;
    }
    /// <summary>
    /// Xác nhận kết quả mang thai của quá trình phối. 
    /// Khi xác nhận có mang thai, quá trình phối sẽ được đánh dấu là "Đã mang thai". 
    /// Khi xác nhận không mang thai, quá trình phối sẽ được đánh dấu là "Thất bại".
    /// Quá trình phối chỉ có thể xác nhận kết quả mang thai khi đã tách (đạt đến ngày tách hoặc đã thực hiện tách thủ công).
    /// </summary>
    /// <param name="breedingId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task ConfirmPregnancyAsync(Guid breedingId, ConfirmPregnancyDto dto)
    {
        var breeding = await _unitOfWork.BreedingRepository.Query()
                        .FirstOrDefaultAsync(x => x.Id == breedingId);
        if (breeding == null)
            throw new NotFoundException("Không tìm thấy quá trình phối");

        var errors = new List<ValidationError>();
        if (breeding.BreedingStatus != BreedingStatus.Separated)
        {
            ValidationHelper.AddError(errors, "BreedingStatus", "Chỉ có thể xác nhận có đẻ khi đã tách");
            ValidationHelper.ThrowIfAny(errors);
        }
        // Xác nhận dúi cái đã mang thai
        if (dto.IsPregnant)
        {
            breeding.BreedingStatus = BreedingStatus.Pregnant;
        }
        else
        {
            breeding.BreedingStatus = BreedingStatus.Failed;
        }

        _unitOfWork.BreedingRepository.Update(breeding);
        await _unitOfWork.SaveChangesAsync();
    }
    /// <summary>
    /// Xác nhận kết quả sinh sản của quá trình phối. 
    /// Khi xác nhận có đẻ, quá trình phối sẽ được cập nhật các thông tin về ngày sinh thực tế, 
    /// số lượng con đẻ và được đánh dấu là "Đã đẻ thành công".
    /// </summary>
    /// <param name="breedingId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task ConFirmBirthAsync(Guid breedingId, ConFirmBirthDto dto)
    {
        var breeding = await _unitOfWork.BreedingRepository.Query().
                        FirstOrDefaultAsync(x => x.Id == breedingId);
        if (breeding == null)
            throw new NotFoundException("Không tìm thấy quá trình phối");

        var errors = new List<ValidationError>();
        if (dto.IsBirthSuccessful)
        {
            if (!dto.ActualBirthDate.HasValue)
                ValidationHelper.AddError(errors, "ActualBirthDate", "Ngày sinh thực tế là bắt buộc khi xác nhận có đẻ");
            if (!dto.OffspringCount.HasValue)
                ValidationHelper.AddError(errors, "OffspringCount", "Số lượng con đẻ là bắt buộc khi xác nhận có đẻ");
            if (dto.OffspringCount <= 0)
                ValidationHelper.AddError(errors, "OffspringCount", "Số lượng con đẻ phải lớn hơn 0");
        }
        ValidationHelper.ThrowIfAny(errors);
        breeding.IsBirthSuccessful = dto.IsBirthSuccessful;
        if (dto.IsBirthSuccessful)
        {
            breeding.ActualBirthDate = dto.ActualBirthDate;
            breeding.OffspringCount = dto.OffspringCount;
            breeding.BreedingStatus = BreedingStatus.BirthSuccessful;
        }
        else
        {
            breeding.BreedingStatus = BreedingStatus.Failed;
        }
        breeding.Notes = dto.Notes;
        _unitOfWork.BreedingRepository.Update(breeding);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Tạo quá trình phối mới giữa con đực và con cái. Quá trình này sẽ thực hiện các bước sau:
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<BreedingDto> CreateBreedingAsync(CreateBreedingDto dto)
    {
        await _validationService.ValidateAsync(dto);
        var now = DateTime.UtcNow;
        var male = await _unitOfWork.RatRespository.GetByIdAsync(dto.MaleId);

        var female = await _unitOfWork.RatRespository.GetByIdAsync(dto.FemaleId);


        if (male == null || female == null)
            throw new NotFoundException("Không tìm thấy dúi");

        //  Chuồng của con đực
        var maleCage = await _unitOfWork.CageRepository.GetByIdAsync(male.CageId);
        if (maleCage == null)
            throw new NotFoundException("Không tìm thấy chuồng con đực");

        //  Chuồng cũ của con cái
        var femaleOldCage = await _unitOfWork.CageRepository.GetByIdAsync(female.CageId);
        if (femaleOldCage == null)
            throw new NotFoundException("Không tìm thấy chuồng con cái");

        // Validate input
        await ValidateBreeding(female, male, now, femaleOldCage);

        var expectedBirthDate = dto.StartDate.AddDays(BreedingRules.PregnancyDays);

        var expectedSeparationDate = dto.StartDate.AddDays(BreedingRules.SeparationDays);
        // 3. Tạo Breeding
        var breeding = new Breeding
        {
            MaleId = male.Id,
            FemaleId = female.Id,

            CageId = maleCage.Id,
            FemaleOldCageId = femaleOldCage.Id,

            ExpectedBirthDate = expectedBirthDate,
            ExpectedSeparationDate = expectedSeparationDate,

            StartDate = dto.StartDate,
            CreatedDate = now,
            BreedingStatus = BreedingStatus.Breeding,
            IsBirthSuccessful = null,
            IsOffspringSurvived = null,
            ActualBirthDate = null,
            OffspringCount = null
        };

        femaleOldCage.IsLocked = true;
        // chuyển con cái sang cage phối(chuồng của con đực)
        female.CageId = maleCage.Id;

        await _unitOfWork.BreedingRepository.AddAsync(breeding);

        await _cageTransferService.CreateAsync(female.Id, femaleOldCage.Id, maleCage.Id, now, TransferReason.Breeding);
        //  Save
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BreedingDto>(breeding);
    }

    /// <summary>
    /// Lấy thông tin chi tiết của một quá trình phối dựa trên ID. Thông tin trả về sẽ bao gồm:
    /// </summary>
    /// <param name="id"></param>
    /// <returns> </returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<BreedingDto> GetBreedingByIdAsync(Guid id)
    {
        var breeding = await _unitOfWork.BreedingRepository.Query().Where(x => x.Id == id).Select(x => new BreedingDto
        {
            Id = x.Id,
            MaleId = x.MaleId,
            MaleCode = x.Male.Code,

            CageId = x.CageId,
            CageName = x.Cage.Name,

            FemaleId = x.FemaleId,
            FemaleOldCageId = x.FemaleOldCageId,
            FemaleCode = x.Female.Code,

            StartDate = x.StartDate,
            ExpectedBirthDate = x.ExpectedBirthDate,
            ExpectedSeparationDate = x.ExpectedSeparationDate,

            BreedingStatus = x.BreedingStatus,

            IsBirthSuccessful = x.IsBirthSuccessful,
            IsOffspringSurvived = x.IsOffspringSurvived,
            ActualBirthDate = x.ActualBirthDate,
            OffspringCount = x.OffspringCount

        }).FirstOrDefaultAsync();
        if (breeding == null)
            throw new NotFoundException("Không tìm thấy quá trình phối với ID đã cho");
        return breeding;
    }

    /// <summary>
    /// Tách quá trình phối khi đã đạt đến ngày tách hoặc khi muốn tách thủ công trước thời điểm tách dự kiến. 
    /// Khi tách quá trình phối, con cái sẽ được chuyển về chuồng cũ và quá trình phối sẽ được đánh dấu là đã tách.
    /// Quá trình phối chỉ có thể tách khi đang trong trạng thái phối và chưa xác nhận kết quả mang thai.
    /// </summary>
    /// <param name="breedingId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task SpreatBreedingAsync(Guid breedingId)
    {
        var breeding = await _unitOfWork.BreedingRepository.Query()
        .Include(x => x.Female)
        .Include(x => x.Male).AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == breedingId);

        if (breeding == null)
            throw new NotFoundException("Không tìm thấy quá trình phối với ID đã cho");

        var errors = new List<ValidationError>();
        if (breeding.BreedingStatus != BreedingStatus.Breeding)
            ValidationHelper.AddError(errors, "BreedingStatus", "Chỉ có thể tách khi đang trong quá trình phối");

        var femaleOldCage = await _unitOfWork.CageRepository.GetByIdAsync(breeding.FemaleOldCageId);

        if (femaleOldCage == null)
            throw new NotFoundException("Không tìm thấy chuồng cũ của con cái");


        breeding.Female.CageId = breeding.FemaleOldCageId;

        femaleOldCage.IsLocked = false;

        breeding.ActualSeparationDate = DateTime.UtcNow;

        breeding.BreedingStatus = BreedingStatus.Separated;

        _unitOfWork.BreedingRepository.Update(breeding);

        // add to cage transfer log
        await _cageTransferService.CreateAsync(breeding.FemaleId,
                                                breeding.CageId,
                                                breeding.FemaleOldCageId,
                                                DateTime.UtcNow,
                                                TransferReason.Separation);
        await _unitOfWork.SaveChangesAsync();

    }

    private async Task ValidateBreeding(Rat female, Rat male, DateTime now, Cage femaleOldCage)
    {
        now = DateTime.UtcNow;
        var errors = new List<ValidationError>();

        if (male.Gender != Gender.Male)
            ValidationHelper.AddError(errors, "Male", "Phải là dúi đực");

        if (female.Gender != Gender.Female)
            ValidationHelper.AddError(errors, "Female", "Phải là dúi cái");

        if (male.Id == female.Id)
            ValidationHelper.AddError(errors, "Breeding", "Không thể phối cùng 1 con");

        if (male.Weight < 1.3)
            ValidationHelper.AddError(errors, "Weight", "Con đực cần phối không được dưới 1.3 kg");

        if (female.Weight < 1.3)
            ValidationHelper.AddError(errors, "Weight", "Con cái cần phối không được dưới 1.3 kg");

        if (femaleOldCage.IsLocked)
            ValidationHelper.AddError(errors, "FemaleCage", "Chuồng con cái đang bị khóa");

        var isMaleBusy = await _unitOfWork.BreedingRepository.Query()
            .AnyAsync(b => b.MaleId == male.Id
                        && b.BreedingStatus == BreedingStatus.Breeding);

        if (isMaleBusy)
            ValidationHelper.AddError(errors, "Male", "Con đực đang trong quá trình phối");

        var activeBreeding = await _unitOfWork.BreedingRepository.Query()
            .AnyAsync(b => b.FemaleId == female.Id
                        && b.BreedingStatus == BreedingStatus.Breeding);

        if (activeBreeding)
            ValidationHelper.AddError(errors, "Female", "Con cái đang trong quá trình phối");

        var lastMaleBreeding = await _unitOfWork.BreedingRepository.Query()
            .Where(b => b.MaleId == male.Id)
            .OrderByDescending(b => b.StartDate)
            .FirstOrDefaultAsync();

        if (lastMaleBreeding != null)
        {
            var days = (now - lastMaleBreeding.StartDate).TotalDays;

            if (days < BreedingRules.MaleRestDays)
            {
                ValidationHelper.AddError(
                    errors,
                    "Male",
                    $"Con đực cần nghỉ ít nhất {BreedingRules.MaleRestDays} ngày"
                );
            }
        }

        var lastBreeding = await _unitOfWork.BreedingRepository.Query()
            .Where(b => b.FemaleId == female.Id)
            .OrderByDescending(b => b.StartDate)
            .FirstOrDefaultAsync();

        if (lastBreeding != null)
        {
            // // Chưa biết có đẻ hay không
            // if (lastBreeding.IsBirthSuccessful == null)
            // {
            //     var days = (now - lastBreeding.StartDate).TotalDays;

            //     if (days < BreedingRules.FemaleCheckBirthDays)
            //         ValidationHelper.AddError(errors, "Female", "Chưa đến thời điểm xác định có đẻ hay không");
            // }
            //  Có đẻ
            if (lastBreeding.BreedingStatus == BreedingStatus.BirthSuccessful && lastBreeding.ActualBirthDate.HasValue)
            {
                var days = (now - lastBreeding.ActualBirthDate.Value).TotalDays;

                // Đẻ + nuôi con OK 
                if (lastBreeding.IsOffspringSurvived == true)
                {
                    var restDays = BreedingRules.FemaleCareDays + BreedingRules.FemaleRecoveryDays;

                    if (days < restDays)
                        ValidationHelper.AddError(errors, "Female", "Con cái chưa nghỉ đủ sau sinh");
                }

                //  Đẻ nhưng con chết nghỉ ngắn (15 ngày)
                if (lastBreeding.BreedingStatus == BreedingStatus.OffspringDead)
                {
                    if (days < BreedingRules.FemaleRetryAfterLossDays)
                        ValidationHelper.AddError(errors, "Female", "Chưa đủ thời gian phối lại sau khi mất con");
                }
            }
        }

        //  Validate giới hạn số lần sinh thành công trong năm
        var startOfYear = new DateTime(now.Year, 1, 1);
        var endOfYear = new DateTime(now.Year + 1, 1, 1);

        var successCount = await _unitOfWork.BreedingRepository.Query()
            .CountAsync(b =>
                b.FemaleId == female.Id &&
                b.IsBirthSuccessful == true &&
                b.IsOffspringSurvived == true &&
                b.StartDate >= startOfYear &&
                b.StartDate < endOfYear);

        if (successCount >= BreedingRules.MaxBreedingPerYear) // = 3
            ValidationHelper.AddError(errors, "Female", "Đã đạt tối đa 3 lần sinh thành công trong năm");

        //  Throw nếu có lỗi
        if (errors.Any())
            ValidationHelper.ThrowIfAny(errors);

    }

    /// <summary>
    /// Hủy bỏ một quá trình phối đang diễn ra. Khi hủy bỏ quá trình phối, 
    /// con cái sẽ được chuyển về chuồng cũ và quá trình phối sẽ được đánh dấu là đã hủy.
    /// </summary>
    /// <param name="breedingId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task CancelBreedingAsync(Guid breedingId)
    {
        var breeding = await _unitOfWork.BreedingRepository.Query().
                Where(x => x.Id == breedingId).Include(x => x.Female)
                        .FirstOrDefaultAsync();

        if (breeding == null)
            throw new NotFoundException("Không tìm thấy quá trình phối với ID đã cho");

        var femaleOldCage = await _unitOfWork.CageRepository.GetByIdAsync(breeding.FemaleOldCageId);

        if (femaleOldCage == null)
            throw new NotFoundException("Không tìm thấy chuồng cũ của con cái");

        var errors = new List<ValidationError>();
        if (breeding.BreedingStatus != BreedingStatus.Breeding)
            ValidationHelper.AddError(errors, "BreedingStatus", "Chỉ có thể hủy khi đang trong quá trình phối"); femaleOldCage.Id = breeding.FemaleOldCageId;
        ValidationHelper.ThrowIfAny(errors);

        breeding.Female.CageId = breeding.FemaleOldCageId;
        femaleOldCage.IsLocked = false;
        breeding.BreedingStatus = BreedingStatus.Cancelled;

        _unitOfWork.BreedingRepository.Update(breeding);
        await _unitOfWork.SaveChangesAsync();

    }
    /// <summary>
    /// Cập nhật tình trạng sống sót của con non sau khi đã xác nhận đẻ thành công.
    /// </summary>
    /// <param name="breedingId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateOffSpringStatusAsync(Guid breedingId, UpdateOffSpringStatusDto dto)
    {
        var breeding = await _unitOfWork.BreedingRepository.Query().Where(x => x.Id == breedingId)
                        .FirstOrDefaultAsync();
        if (breeding == null)
            throw new NotFoundException("Không tìm thấy quá trình phối với ID đã cho");

        var errors = new List<ValidationError>();

        if (breeding.BreedingStatus != BreedingStatus.BirthSuccessful)
            ValidationHelper.AddError(errors, "BreedingStatus", "Chỉ có thể cập nhật tình trạng con non khi đã xác nhận đẻ thành công");
        ValidationHelper.ThrowIfAny(errors);
        if (dto.IsOffspringSurvived)
        {
            breeding.BreedingStatus = BreedingStatus.OffspringAlive;
        }
        else
        {
            breeding.BreedingStatus = BreedingStatus.OffspringDead;
        }

        _unitOfWork.BreedingRepository.Update(breeding);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedResult<BreedingDto>> GetBreedingsAsync(BreedingParams breedingParams)
    {
        var query = _unitOfWork.BreedingRepository.Query()
                    .Include(x => x.Cage)
                    .Include(x => x.Female)
                    .Include(x => x.Male).AsNoTracking();

        if (!string.IsNullOrEmpty(breedingParams.Search))
        {
            var keyword = breedingParams.Search.Trim().ToLower();
            query = query.Where(x =>
                    x.Female.Code.ToLower().Contains(keyword) ||
                    x.Male.Code.ToLower().Contains(keyword) ||
                    x.Cage.Name.ToLower().Contains(keyword));
        }
        if (breedingParams.Status.HasValue)
        {
            query = query.Where(x => x.BreedingStatus == breedingParams.Status.Value);
        }
        if (breedingParams.FromDate.HasValue)
        {
            query = query.Where(x =>
                x.StartDate >= breedingParams.FromDate.Value);
        }
        if (breedingParams.ToDate.HasValue)
        {
            query = query.Where(x =>
                x.StartDate <= breedingParams.ToDate.Value);
        }
        var pageBreeding = await PaginationHelper.ToPagedResultAsync(query, breedingParams.PageNumber, breedingParams.PageSize);

        var breedingDto = _mapper.Map<List<BreedingDto>>(pageBreeding.Items);
        return new PagedResult<BreedingDto>
        {
            Items = breedingDto,
            TotalCount = pageBreeding.TotalCount,
            PageNumber = pageBreeding.PageNumber,
            PageSize = pageBreeding.PageSize
        };
    }
}