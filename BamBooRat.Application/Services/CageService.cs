using AutoMapper;
using FluentValidation;

public class CageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCageDto> _validator;

    private readonly IMapper _mapper;
    public CageService(IUnitOfWork unitOfWork,
     IMapper mapper,
     IValidator<CreateCageDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;

    }
    public async Task<List<CageDto>> GetAllCagesAsync()
    {
        var cages = await _unitOfWork.CageRepository.GetAllAsync();
        return _mapper.Map<List<CageDto>>(cages);
    }
    public async Task<CageDto> GetCageByIdAsync(Guid id)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Không tìm thấy chuồng với ID đã cho");
        return _mapper.Map<CageDto>(cage);
    }
    public async Task<CageDto> AddCageAsync(CreateCageDto cageDto)
    {
        // validate duplicate Name
        var exists = await _unitOfWork.CageRepository.ExistsByNameAsync(cageDto.Name);
        if (exists)
            ThrowDuplicateName();
        await ValidateAsync(cageDto);
        var cage = _mapper.Map<Cage>(cageDto);
        await _unitOfWork.CageRepository.AddAsync(cage);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CageDto>(cage);
    }
    public async Task UpdateAsync(Guid id, CreateCageDto cageDto)
    {
        // validate duplicate Name
        var exists = await _unitOfWork.CageRepository.ExistsByNameAsync(cageDto.Name, id);
        if (exists)
            ThrowDuplicateName();

        await ValidateAsync(cageDto);

        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Không tìm thấy chuồng với ID đã cho");

        _mapper.Map(cageDto, cage);
        _unitOfWork.CageRepository.Update(cage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var cage = await _unitOfWork.CageRepository.GetByIdAsync(id);
        if (cage == null)
            throw new NotFoundException("Không tìm thấy chuồng với ID đã cho");

        _unitOfWork.CageRepository.Remove(cage);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task ValidateAsync(CreateCageDto dto)
    {
        var result = await _validator.ValidateAsync(dto);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => new ValidationError
            {
                Field = e.PropertyName.ToLower(),
                Message = e.ErrorMessage
            }).ToList();

            throw new ValidationExceptionCustom(errors);
        }
    }
    private void ThrowDuplicateName()
    {
        throw new ValidationExceptionCustom(new List<ValidationError>
        {
            new ValidationError
            {
                Field = "name",
                Message = "Tên chuồng đã tồn tại."
            }
        });
    }
}

