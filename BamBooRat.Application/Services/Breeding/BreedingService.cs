
using System.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

public class BreedingService : IBreedingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    public BreedingService(IUnitOfWork unitOfWork,
                IMapper mapper,
                IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;
    }
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
            breeding.IsOffspringSurvived = dto.IsOffspringSurvived;
            breeding.BreedingStatus = BreedingStatus.Completed;
        }
        else
        {
            breeding.BreedingStatus = BreedingStatus.Failed;
        }
        _unitOfWork.BreedingRepository.Update(breeding);
        await _unitOfWork.SaveChangesAsync();
    }

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
        //  Save
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BreedingDto>(breeding);
    }

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

    public async Task SpreatBreedingAsync(Guid breedingId)
    {
        var breeding = await _unitOfWork.BreedingRepository.Query()
        .Include(x => x.Female)
        .Include(x => x.Male)
        .FirstOrDefaultAsync(x => x.Id == breedingId);

        if (breeding == null)
            throw new NotFoundException("Không tìm thấy quá trình phối với ID đã cho");

        var errors = new List<ValidationError>();
        if (breeding.BreedingStatus != BreedingStatus.Breeding)
            ValidationHelper.AddError(errors, "BreedingStatus", "Chỉ có thể tách khi đang trong quá trình phối");

        breeding.Female.CageId = breeding.FemaleOldCageId;

        var femaleOldCage = await _unitOfWork.CageRepository.GetByIdAsync(breeding.FemaleOldCageId);

        if (femaleOldCage == null)
            throw new NotFoundException("Không tìm thấy chuồng cũ của con cái");

        femaleOldCage.IsLocked = false;

        breeding.ActualSeparationDate = DateTime.UtcNow;

        breeding.BreedingStatus = BreedingStatus.Separated;

        _unitOfWork.BreedingRepository.Update(breeding);
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
            // Chưa biết có đẻ hay không
            if (lastBreeding.IsBirthSuccessful == null)
            {
                var days = (now - lastBreeding.StartDate).TotalDays;

                if (days < BreedingRules.FemaleCheckBirthDays)
                    ValidationHelper.AddError(errors, "Female", "Chưa đến thời điểm xác định có đẻ hay không");
            }
            //  Có đẻ
            if (lastBreeding.IsBirthSuccessful == true && lastBreeding.ActualBirthDate.HasValue)
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
                if (lastBreeding.IsOffspringSurvived == false)
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

    public Task CancelBreedingAsync(Guid breedingId)
    {
        throw new NotImplementedException();
    }
}