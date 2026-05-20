public interface ICageRepository
{
    Task<List<Cage>> GetAllAsync();

    Task<Cage?> GetByIdAsync(Guid id);

    Task AddAsync(Cage cage);

    void Update(Cage cage);

    void Remove(Cage cage);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
}