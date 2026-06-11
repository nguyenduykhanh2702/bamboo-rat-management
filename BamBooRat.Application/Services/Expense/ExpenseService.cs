
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
    public async Task<ExpenseDetailDto> AddAsync(ExpenseRequestDto dto)
    {
        await _validationService.ValidateAsync(dto);
        var expense = _mapper.Map<Expense>(dto);
        await _unitOfWork.ExpenseRepository.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ExpenseDetailDto>(expense);

    }

    public async Task DeleteAsync(Guid id)
    {
        var expense = await _unitOfWork.ExpenseRepository.GetByIdAsync(id);
        if (expense == null)
            throw new NotFoundException($"Không tìm thấy bản ghi với id :{id}");
        _unitOfWork.ExpenseRepository.Remove(expense);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ExpenseDetailDto> GetExpenseByIdAsync(Guid id)
    {
        var expense = await _unitOfWork.ExpenseRepository.GetByIdAsync(id);
        if (expense == null)
            throw new NotFoundException($"Không tìm thấy bản ghi chi phí với id: {id}");
        return _mapper.Map<ExpenseDetailDto>(expense);
    }

    public async Task<PagedResult<ExpenseDetailDto>> GetExpensePagedResultAsync(ExpenseParams expenseParams)
    {
        var query = _unitOfWork.ExpenseRepository.Query();
        if (!string.IsNullOrEmpty(expenseParams.Search))
        {
            var search = expenseParams.Search.Trim().ToLower();
            query = query.Where(x => x.ItemName.ToLower().Contains(search));
        }
        if (expenseParams.FromDate.HasValue)
        {
            query = query.Where(x => x.ExpenseDate >= expenseParams.FromDate);
        }
        if (expenseParams.ToDate.HasValue)
        {
            query = query.Where(x => x.ExpenseDate <= expenseParams.ToDate);
        }
        var pageExpenses = await PaginationHelper.ToPagedResultAsync(query, expenseParams.PageNumber, expenseParams.PageSize);
        var expenseDto = _mapper.Map<List<ExpenseDetailDto>>(pageExpenses.Items);
        return new PagedResult<ExpenseDetailDto>
        {
            Items = expenseDto,
            TotalCount = pageExpenses.TotalCount,
            PageNumber = pageExpenses.PageNumber,
            PageSize = pageExpenses.PageSize
        };
    }

    public async Task UpdateAsync(Guid id, ExpenseRequestDto dto)
    {
        var expense = await _unitOfWork.ExpenseRepository.GetByIdAsync(id);
        if (expense == null)
            throw new NotFoundException($"Không tìm thấy bản ghi chi phí với id: {id}");
        await _validationService.ValidateAsync(dto);
        _mapper.Map(dto, expense);
        _unitOfWork.ExpenseRepository.Update(expense);
        await _unitOfWork.SaveChangesAsync();
    }
}