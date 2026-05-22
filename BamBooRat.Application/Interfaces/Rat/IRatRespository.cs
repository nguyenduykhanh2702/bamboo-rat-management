public interface IRatRespository
{
    IQueryable<Rat> Query();
    Task<Rat?> GetByIdAsync(Guid id);

    Task AddAsync(Rat rat);

    void Update(Rat rat);

    void Remove(Rat rat);
    Task<List<string>> GetDuplicateFieldsAsync(
     string name,
     string code,
     Guid? excludeId = null);
}