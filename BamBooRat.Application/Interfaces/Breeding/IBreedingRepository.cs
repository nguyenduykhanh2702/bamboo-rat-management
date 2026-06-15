public interface IBreedingRepository
{
    IQueryable<Breeding> Query();
    Task<Breeding?> GetByIdAsync(Guid id);

    Task AddAsync(Breeding breeding);

    void Update(Breeding breeding);

    void Remove(Breeding breeding);
    Task<Breeding?> GetActiveBreedingByRatIdAsync(Guid ratId);
}