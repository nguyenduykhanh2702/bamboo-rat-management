
using AutoMapper;

public class ExpenseService : IExpenseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    public ExpenseService(IUnitOfWork unitOfWork,
                         IMapper mapper,
                         IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;
    }
    public async Task<ExpenseDetailDto> AddAsync(CreateExpenseDto dto)
    {
        await _validationService.ValidateAsync(dto);
        var expense = _mapper.Map<Expense>(dto);
        await _unitOfWork.ExpenseRepository.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ExpenseDetailDto>(expense);

    }

    public async Task<ExpenseDetailDto> GetExpenseByIdAsync(Guid id)
    {
        var expense = await _unitOfWork.ExpenseRepository.GetByIdAsync(id);
        if (expense == null)
            throw new NotFoundException($"Không tìm thấy bản ghi chi phí với id: {id}");
        return _mapper.Map<ExpenseDetailDto>(expense);
    }
}